using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using ActionGame.Utils;

/// <summary>
/// Player's Field of View (FOV) detector.
/// Determines which dynamic objects are within the visibility cone
/// and manages their visibility directly through SpriteRenderer.
/// </summary>
public class FOVDetector : MonoBehaviour
{
    [Header("FOV Settings")]
    [Tooltip("Use settings from Light2D component")]
    [SerializeField] private bool _useLightSettings = false;

    [Tooltip("Light2D component for synchronization (usually on PlayerSpotLight)")]
    [SerializeField] private Light2D _lightSource;

    [Tooltip("Point from which the cone is drawn (usually PlayerRotateWeapon). If empty - uses player position")]
    [SerializeField] private Transform _fovOriginPoint;

    [Tooltip("Visibility cone angle in degrees (ignored if Use Light Settings)")]
    [SerializeField] private float _fovAngle = 75f;

    [Tooltip("Maximum view distance (ignored if Use Light Settings)")]
    [SerializeField] private float _viewDistance = 15f;

    [Tooltip("Layer for dynamic objects (enemies, NPCs, etc.)")]
    [SerializeField] private LayerMask _dynamicObjectsLayer;

    [Header("Visibility")]
    [Tooltip("Fade effect speed")]
    [SerializeField] private float _fadeSpeed = 5f;

    [Tooltip("Transparency of hidden objects (0 = fully transparent)")]
    [SerializeField][Range(0f, 1f)] private float _hiddenAlpha = 0f;

    [Header("Performance")]
    [Tooltip("FOV check update frequency (seconds). 0.1-0.2 is optimal")]
    [SerializeField] private float _updateInterval = 0.15f;

    [Tooltip("Use Raycast to check for obstacles between player and object")]
    [SerializeField] private bool _useObstacleDetection = true;

    [Tooltip("Obstacle layers blocking view (walls, trees)")]
    [SerializeField] private LayerMask _obstacleLayer;

    private float _updateTimer;
    private HashSet<GameObject> _visibleObjects = new HashSet<GameObject>();
    private Dictionary<GameObject, SpriteRenderer[]> _objectRenderers = new Dictionary<GameObject, SpriteRenderer[]>();
    private Dictionary<SpriteRenderer, Color> _originalColors = new Dictionary<SpriteRenderer, Color>();
    private Dictionary<GameObject, float> _currentAlpha = new Dictionary<GameObject, float>();
    private Transform _playerTransform;
    private Vector3 _lookDirection;

    private void Awake()
    {
        _playerTransform = transform;

        // If origin point is not specified - use player position
        if (_fovOriginPoint == null)
        {
            _fovOriginPoint = transform;
        }

        // Synchronize with Light2D if specified
        UpdateSettingsFromLight();
    }

    private void Start()
    {
        // Check settings
        if (_dynamicObjectsLayer.value == 0)
        {
            Debug.LogError("[FOVDetector] Dynamic Objects Layer is not configured! Set it in Inspector.");
        }

        float currentFOV = GetCurrentFOVAngle();
        float currentDistance = GetCurrentViewDistance();
        string source = _useLightSettings && _lightSource != null ? "from Light2D" : "manual";

        Debug.Log($"[FOVDetector] Initialized ({source}). LayerMask value: {_dynamicObjectsLayer.value}, FOV: {currentFOV}°, Distance: {currentDistance}");
    }

    private void Update()
    {
        // Update settings from light if needed
        if (_useLightSettings && _lightSource != null)
        {
            UpdateSettingsFromLight();
        }

        // Use direction from weapon transform (PlayerRotateWeapon)
        if (_fovOriginPoint != null)
        {
            _lookDirection = _fovOriginPoint.right;
        }

        _updateTimer += Time.deltaTime;
        if (_updateTimer >= _updateInterval)
        {
            _updateTimer = 0f;
            DetectVisibleObjects();
        }

        // Update fade effect
        UpdateVisibilityFade();
    }

    /// <summary>
    /// Updates FOV settings from Light2D component
    /// </summary>
    private void UpdateSettingsFromLight()
    {
        if (_lightSource != null && _useLightSettings)
        {
            _viewDistance = _lightSource.pointLightOuterRadius;
            _fovAngle = _lightSource.pointLightInnerAngle;
        }
    }

    /// <summary>
    /// Gets current FOV angle (from light or manual)
    /// </summary>
    private float GetCurrentFOVAngle()
    {
        if (_useLightSettings && _lightSource != null)
        {
            return _lightSource.pointLightInnerAngle;
        }
        return _fovAngle;
    }

    /// <summary>
    /// Gets current view distance (from light or manual)
    /// </summary>
    private float GetCurrentViewDistance()
    {
        if (_useLightSettings && _lightSource != null)
        {
            return _lightSource.pointLightOuterRadius;
        }
        return _viewDistance;
    }

    [Header("Debug Info")]
    [SerializeField] private bool _showDetectionDebug = false;
    private int _debugFrameCounter = 0;

    /// <summary>
    /// Main logic for detecting visible objects
    /// </summary>
    private void DetectVisibleObjects()
    {
        HashSet<GameObject> currentlyVisible = new HashSet<GameObject>();

        float currentDistance = GetCurrentViewDistance();
        float currentFOV = GetCurrentFOVAngle();

        // Find all dynamic objects in radius from weapon point
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_fovOriginPoint.position, currentDistance, _dynamicObjectsLayer);

        // Debug every 60 frames (~1 sec)
        _debugFrameCounter++;
        if (_showDetectionDebug && _debugFrameCounter % 60 == 0)
        {
            Debug.Log($"[FOVDetector] Detected {colliders.Length} colliders on layer. Position: {_fovOriginPoint.position}, Direction: {_lookDirection}");
        }

        foreach (Collider2D collider in colliders)
        {
            GameObject targetObj = collider.gameObject;

            // Register object if not yet registered
            RegisterObject(targetObj);

            // Check if object is within the visibility cone
            if (Utils.IsInFieldOfView(_fovOriginPoint.position, _lookDirection, targetObj.transform.position, currentFOV))
            {
                // Optional: check if there are no obstacles
                if (_useObstacleDetection)
                {
                    if (!Utils.IsBlockedByObstacle(_fovOriginPoint.position, targetObj.transform.position, _obstacleLayer))
                    {
                        currentlyVisible.Add(targetObj);
                        if (_showDetectionDebug && _debugFrameCounter % 60 == 0)
                        {
                            Debug.Log($"[FOVDetector] Object {targetObj.name} VISIBLE (no obstacles)");
                        }
                    }
                    else if (_showDetectionDebug && _debugFrameCounter % 60 == 0)
                    {
                        Debug.Log($"[FOVDetector] Object {targetObj.name} blocked by obstacle");
                    }
                }
                else
                {
                    currentlyVisible.Add(targetObj);
                    if (_showDetectionDebug && _debugFrameCounter % 60 == 0)
                    {
                        Debug.Log($"[FOVDetector] Object {targetObj.name} VISIBLE");
                    }
                }
            }
            else if (_showDetectionDebug && _debugFrameCounter % 60 == 0)
            {
                Vector3 dirToTarget = (targetObj.transform.position - _fovOriginPoint.position).normalized;
                float angle = Vector3.Angle(_lookDirection, dirToTarget);
                Debug.Log($"[FOVDetector] Object {targetObj.name} outside FOV (angle: {angle:F1}°)");
            }
        }

        // Update objects visibility
        UpdateObjectVisibility(currentlyVisible);
    }

    /// <summary>
    /// Registers object and its sprites for visibility management
    /// </summary>
    private void RegisterObject(GameObject obj)
    {
        if (_objectRenderers.ContainsKey(obj)) return;

        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) return;

        _objectRenderers[obj] = renderers;
        _currentAlpha[obj] = 1f;

        // Save original colors
        foreach (var renderer in renderers)
        {
            if (!_originalColors.ContainsKey(renderer))
            {
                _originalColors[renderer] = renderer.color;
            }
        }
    }

    /// <summary>
    /// Updates fade effect for all objects
    /// </summary>
    private void UpdateVisibilityFade()
    {
        foreach (var kvp in _objectRenderers)
        {
            GameObject obj = kvp.Key;
            if (obj == null) continue;

            float targetAlpha = _visibleObjects.Contains(obj) ? 1f : _hiddenAlpha;

            if (Mathf.Abs(_currentAlpha[obj] - targetAlpha) > 0.01f)
            {
                _currentAlpha[obj] = Mathf.Lerp(_currentAlpha[obj], targetAlpha, Time.deltaTime * _fadeSpeed);

                // Apply alpha to all sprites
                foreach (var renderer in kvp.Value)
                {
                    if (renderer != null && _originalColors.ContainsKey(renderer))
                    {
                        Color color = _originalColors[renderer];
                        color.a = _originalColors[renderer].a * _currentAlpha[obj];
                        renderer.color = color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates objects visibility based on current FOV
    /// </summary>
    private void UpdateObjectVisibility(HashSet<GameObject> currentlyVisible)
    {
        _visibleObjects = currentlyVisible;
    }

    /// <summary>
    /// Force visibility update (for external calls)
    /// </summary>
    public void ForceUpdate()
    {
        DetectVisibleObjects();
    }

#if UNITY_EDITOR
    /// <summary>
    /// FOV visualization in editor
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 fovPos = _fovOriginPoint != null ? _fovOriginPoint.position : transform.position;
        float currentDistance = GetCurrentViewDistance();
        float currentFOV = GetCurrentFOVAngle();

        // Draw view radius
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Utils.DrawGizmosCircle(fovPos, currentDistance, 50);

        // Draw visibility cone
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Utils.DrawGizmosFOVCone(fovPos, _lookDirection, currentFOV, currentDistance, 20);

        // Draw lines to visible objects
        Gizmos.color = Color.green;
        foreach (var obj in _visibleObjects)
        {
            if (obj != null)
            {
                Gizmos.DrawLine(fovPos, obj.transform.position);
            }
        }
    }
#endif
}
