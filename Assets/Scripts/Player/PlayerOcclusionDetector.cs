using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обнаруживает объекты между камерой и игроком и делает их полупрозрачными.
/// Прикрепите этот компонент к камере или к игроку.
/// </summary>
public class PlayerOcclusionDetector : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Трансформ игрока. Если пусто - будет найден автоматически")]
    [SerializeField] private Transform _playerTransform;

    [Tooltip("Камера для проверки. Если пусто - используется Camera.main")]
    [SerializeField] private Camera _camera;

    [Header("Detection Settings")]
    [Tooltip("Искать все объекты со SpriteFadeOcclusion в сцене")]
    [SerializeField] private bool _findAllOccludableObjects = true;

    [Tooltip("Частота проверки (в секундах). 0 = каждый кадр")]
    [SerializeField] private float _checkInterval = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool _showDebugRays = false;

    private HashSet<SpriteFadeOcclusion> _fadedObjects = new HashSet<SpriteFadeOcclusion>();
    private SpriteFadeOcclusion[] _allOccludableObjects;
    private float _lastCheckTime;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if (_playerTransform == null)
        {
            // Пытаемся найти игрока
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else if (Player.Instance != null)
            {
                _playerTransform = Player.Instance.transform;
            }
        }

        if (_playerTransform == null)
        {
            Debug.LogError("[PlayerOcclusionDetector] Player transform не найден!");
            enabled = false;
            return;
        }

        // Находим все объекты со SpriteFadeOcclusion в сцене
        if (_findAllOccludableObjects)
        {
            _allOccludableObjects = FindObjectsByType<SpriteFadeOcclusion>(FindObjectsSortMode.None);
            Debug.Log($"[PlayerOcclusionDetector] Найдено {_allOccludableObjects.Length} объектов с SpriteFadeOcclusion");
        }
    }

    private void Start()
    {
        if (_findAllOccludableObjects && (_allOccludableObjects == null || _allOccludableObjects.Length == 0))
        {
            _allOccludableObjects = FindObjectsByType<SpriteFadeOcclusion>(FindObjectsSortMode.None);
        }
    }

    private void Update()
    {
        if (_checkInterval > 0)
        {
            if (Time.time - _lastCheckTime >= _checkInterval)
            {
                CheckForOcclusion();
                _lastCheckTime = Time.time;
            }
        }
        else
        {
            CheckForOcclusion();
        }
    }

    private void CheckForOcclusion()
    {
        if (_playerTransform == null || _camera == null)
            return;

        if (_allOccludableObjects == null || _allOccludableObjects.Length == 0)
            return;

        Vector3 playerPosition = _playerTransform.position;

        // Создаем новый HashSet для текущего кадра
        HashSet<SpriteFadeOcclusion> currentlyOccludedObjects = new HashSet<SpriteFadeOcclusion>();

        // Проверяем все объекты со SpriteFadeOcclusion
        foreach (SpriteFadeOcclusion occludableObject in _allOccludableObjects)
        {
            if (occludableObject == null)
                continue;

            // Пропускаем если это сам игрок
            if (occludableObject.transform == _playerTransform ||
                occludableObject.transform.IsChildOf(_playerTransform))
                continue;

            // Проверяем перекрывает ли спрайт игрока
            if (IsSpriteOccludingPlayer(occludableObject, playerPosition))
            {
                currentlyOccludedObjects.Add(occludableObject);

                // Если объект еще не был затухшим - затухаем его
                if (!_fadedObjects.Contains(occludableObject))
                {
                    occludableObject.FadeOut();
                }
            }
        }

        // Восстанавливаем объекты, которые больше не закрывают игрока
        foreach (SpriteFadeOcclusion fadedObject in _fadedObjects)
        {
            if (fadedObject != null && !currentlyOccludedObjects.Contains(fadedObject))
            {
                fadedObject.FadeIn();
            }
        }

        // Обновляем список затухших объектов
        _fadedObjects = currentlyOccludedObjects;

        // Debug визуализация
        if (_showDebugRays)
        {
            Debug.DrawLine(_camera.transform.position, playerPosition, Color.yellow);
        }
    }

    /// <summary>
    /// Проверяет перекрывает ли спрайт объекта позицию игрока визуально
    /// </summary>
    private bool IsSpriteOccludingPlayer(SpriteFadeOcclusion occludableObject, Vector3 playerPos)
    {
        SpriteRenderer spriteRenderer = occludableObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || !spriteRenderer.enabled)
            return false;

        // Проверяем что объект находится "впереди" игрока (ниже по Y)
        float objectPivotY = occludableObject.transform.position.y;
        if (objectPivotY >= playerPos.y)
            return false;

        // Пытаемся использовать PolygonCollider2D для точной проверки
        PolygonCollider2D polygonCollider = occludableObject.GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            // Используем OverlapPoint для точной проверки по контуру спрайта
            return polygonCollider.OverlapPoint(playerPos);
        }

        // Fallback: если нет PolygonCollider2D, используем стандартный Bounds
        Bounds spriteBounds = spriteRenderer.bounds;
        bool isInsideX = playerPos.x >= spriteBounds.min.x && playerPos.x <= spriteBounds.max.x;
        bool isInsideY = playerPos.y >= spriteBounds.min.y && playerPos.y <= spriteBounds.max.y;

        return isInsideX && isInsideY;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showDebugRays || _playerTransform == null)
            return;

        // Рисуем линию к игроку
        if (_camera != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_camera.transform.position, _playerTransform.position);
        }

        // Рисуем bounds всех объектов с SpriteFadeOcclusion
        if (_allOccludableObjects != null)
        {
            foreach (var obj in _allOccludableObjects)
            {
                if (obj == null) continue;

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null && sr.enabled)
                {
                    Gizmos.color = _fadedObjects.Contains(obj) ? Color.red : Color.green;
                    Gizmos.DrawWireCube(sr.bounds.center, sr.bounds.size);
                }
            }
        }

        // Рисуем позицию игрока
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_playerTransform.position, 0.3f);
    }
}
