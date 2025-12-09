using UnityEngine;

/// <summary>
/// Динамически изменяет Order in Layer на основе Y-позиции объекта.
/// Объекты с меньшей Y-позицией будут отрисовываться поверх объектов с большей Y-позицией.
/// </summary>
public class YSortingOrder : MonoBehaviour
{
    [Header("Sorting Settings")]
    [Tooltip("Offset для Order in Layer. Используйте +1 для оружия, чтобы оно было поверх персонажа")]
    [SerializeField] private int _orderOffset = 0;

    [Tooltip("Частота обновления сортировки (в секундах). 0 = каждый кадр")]
    [SerializeField] private float _updateInterval = 0f;

    [Tooltip("Множитель для Y-позиции. Чем больше значение, тем больше разница между объектами")]
    [SerializeField] private float _positionMultiplier = 100f;

    [Tooltip("Инвертировать сортировку (объекты выше будут отрисовываться поверх)")]
    [SerializeField] private bool _invertSorting = false;

    [Header("Advanced Settings")]
    [Tooltip("Применять сортировку ко всем дочерним SpriteRenderer (для оружия с руками и магазином)")]
    [SerializeField] private bool _applyToChildren = false;

    [Tooltip("Статический режим - обновляется только один раз при старте (для деревьев и статичных объектов)")]
    [SerializeField] private bool _isStatic = false;

    private SpriteRenderer[] _spriteRenderers;
    private float _lastUpdateTime;
    private Transform _sortingTransform;

    private void Awake()
    {
        // Собираем все SpriteRenderer
        if (_applyToChildren)
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        }
        else
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                _spriteRenderers = new SpriteRenderer[] { sr };
            }
        }

        if (_spriteRenderers == null || _spriteRenderers.Length == 0)
        {
            Debug.LogError($"[YSortingOrder] SpriteRenderer не найден на {gameObject.name}!");
            enabled = false;
            return;
        }

        // По умолчанию используем свою позицию для сортировки
        _sortingTransform = transform;
    }

    private void Start()
    {
        UpdateSortingOrder();
    }

    private void LateUpdate()
    {
        // Статические объекты не обновляются после старта
        if (_isStatic)
        {
            return;
        }

        // Проверяем нужно ли обновлять сортировку
        if (_updateInterval > 0)
        {
            if (Time.time - _lastUpdateTime >= _updateInterval)
            {
                UpdateSortingOrder();
                _lastUpdateTime = Time.time;
            }
        }
        else
        {
            UpdateSortingOrder();
        }
    }

    private void UpdateSortingOrder()
    {
        // Проверяем что все инициализировано
        if (_spriteRenderers == null || _spriteRenderers.Length == 0 || _sortingTransform == null)
        {
            return;
        }

        // Вычисляем Order in Layer на основе Y-позиции
        float yPosition = _sortingTransform.position.y;

        // Инвертируем если нужно
        if (_invertSorting)
        {
            yPosition = -yPosition;
        }

        // Переводим позицию в целое число для Order in Layer
        int baseSortingOrder = Mathf.RoundToInt(-yPosition * _positionMultiplier) + _orderOffset;

        // Применяем к всем спрайтам
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            if (_spriteRenderers[i] != null)
            {
                // Для дочерних объектов добавляем небольшой offset чтобы сохранить их порядок
                _spriteRenderers[i].sortingOrder = baseSortingOrder + i;
            }
        }
    }

    /// <summary>
    /// Устанавливает трансформ, который будет использоваться для сортировки.
    /// Полезно если нужно сортировать оружие относительно позиции игрока.
    /// </summary>
    public void SetSortingTransform(Transform sortingTransform)
    {
        _sortingTransform = sortingTransform;
        UpdateSortingOrder();
    }

    /// <summary>
    /// Принудительно обновляет сортировку
    /// </summary>
    public void ForceUpdateSorting()
    {
        UpdateSortingOrder();
    }

    /// <summary>
    /// Устанавливает offset для Order in Layer
    /// </summary>
    public void SetOrderOffset(int offset)
    {
        _orderOffset = offset;
        UpdateSortingOrder();
    }
}
