using UnityEngine;

/// <summary>
/// Делает спрайт полупрозрачным когда игрок находится за ним.
/// Используется для деревьев, стен и других препятствий.
/// </summary>
public class SpriteFadeOcclusion : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Прозрачность когда игрок за объектом (0 = полностью прозрачный, 1 = непрозрачный)")]
    [SerializeField][Range(0f, 1f)] private float _fadedAlpha = 0.3f;

    [Tooltip("Скорость изменения прозрачности")]
    [SerializeField] private float _fadeSpeed = 5f;

    [Tooltip("Применять к дочерним спрайтам (автоматически находит все SpriteRenderer в группе)")]
    [SerializeField] private bool _applyToChildren = true;

    private SpriteRenderer[] _spriteRenderers;
    private Color[] _originalColors;
    private Color[] _targetColors;
    private float _currentAlpha = 1f;
    private bool _isFaded = false;

    private void Awake()
    {
        InitializeSpriteRenderers();
    }

    private void InitializeSpriteRenderers()
    {
        if (_applyToChildren)
        {
            // Находим ВСЕ SpriteRenderer включая на самом объекте и всех дочерних
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

            if (_spriteRenderers.Length > 0)
            {
                Debug.Log($"[SpriteFadeOcclusion] Найдено {_spriteRenderers.Length} SpriteRenderer в группе {gameObject.name}");
            }
        }
        else
        {
            // Только на текущем объекте
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                _spriteRenderers = new SpriteRenderer[] { sr };
            }
            else
            {
                Debug.LogWarning($"[SpriteFadeOcclusion] SpriteRenderer не найден на {gameObject.name}. Попробуйте включить 'Apply To Children'");
            }
        }

        if (_spriteRenderers != null && _spriteRenderers.Length > 0)
        {
            _originalColors = new Color[_spriteRenderers.Length];
            _targetColors = new Color[_spriteRenderers.Length];

            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                if (_spriteRenderers[i] != null)
                {
                    _originalColors[i] = _spriteRenderers[i].color;
                    _targetColors[i] = _originalColors[i];
                }
            }
        }
        else
        {
            Debug.LogWarning($"[SpriteFadeOcclusion] Не найдено ни одного SpriteRenderer на {gameObject.name}!");
        }
    }

    private void Update()
    {
        UpdateFade();
    }

    private void UpdateFade()
    {
        if (_spriteRenderers == null || _originalColors == null)
            return;

        // Плавно меняем прозрачность
        float targetAlpha = _isFaded ? _fadedAlpha : 1f;
        _currentAlpha = Mathf.Lerp(_currentAlpha, targetAlpha, Time.deltaTime * _fadeSpeed);

        // Применяем к всем спрайтам
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            if (_spriteRenderers[i] != null)
            {
                Color color = _originalColors[i];
                color.a = _currentAlpha;
                _spriteRenderers[i].color = color;
            }
        }
    }

    /// <summary>
    /// Вызывается когда нужно сделать объект полупрозрачным
    /// </summary>
    public void FadeOut()
    {
        _isFaded = true;
    }

    /// <summary>
    /// Вызывается когда нужно вернуть нормальную прозрачность
    /// </summary>
    public void FadeIn()
    {
        _isFaded = false;
    }

    /// <summary>
    /// Устанавливает уровень затухания
    /// </summary>
    public void SetFadedAlpha(float alpha)
    {
        _fadedAlpha = Mathf.Clamp01(alpha);
    }
}
