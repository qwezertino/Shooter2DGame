using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    [SerializeField] private BaseWeapon _currentActiveWeapon;

    private void Awake()
    {
        Instance = this;

        if (_currentActiveWeapon != null)
        {
            if (!_currentActiveWeapon.gameObject.scene.IsValid())
            {
                Debug.LogError($"[ActiveWeapon] Weapon '{_currentActiveWeapon.name}' is a PREFAB, not a scene object! " +
                              $"Drag weapon from Hierarchy, not from Project folder!", this);
                _currentActiveWeapon = null;
            }
        }
        else
        {
            Debug.LogWarning("[ActiveWeapon] No weapon assigned in Inspector!", this);
        }
    }
    public void Attack()
    {
        _currentActiveWeapon.Attack();
    }

    public void StartFiring()
    {
        _currentActiveWeapon.StartFiring();
    }

    public void StopFiring()
    {
        _currentActiveWeapon.StopFiring();
    }

    // Метод для смены оружия из WeaponSwitcher
    public void SetCurrentWeapon(BaseWeapon weapon)
    {
        if (weapon != null && weapon.gameObject.scene.IsValid())
        {
            _currentActiveWeapon = weapon;
            Debug.Log($"[ActiveWeapon] Current weapon changed to: {weapon.name}");
        }
        else
        {
            Debug.LogError("[ActiveWeapon] Cannot set weapon - invalid weapon or prefab!", this);
        }
    }
}
