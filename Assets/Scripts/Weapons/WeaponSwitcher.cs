using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private List<BaseWeapon> _weaponSlots = new List<BaseWeapon>(3);

    [Header("Settings")]
    [SerializeField] private int _startingWeaponIndex = 0;

    private int _currentWeaponIndex = -1;
    private BaseWeapon _currentWeapon;

    private void Start()
    {
        foreach (var weapon in _weaponSlots)
        {
            if (weapon != null)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        SwitchToWeapon(_startingWeaponIndex);

        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnWeaponSlotChanged += GameInput_OnWeaponSlotChanged;
        }
    }

    private void GameInput_OnWeaponSlotChanged(object sender, WeaponSlotEventArgs e)
    {
        SwitchToWeapon(e.SlotIndex - 1);
    }

    public void SwitchToWeapon(int index)
    {
        if (index < 0 || index >= _weaponSlots.Count)
        {
            Debug.LogWarning($"[WeaponSwitcher] Invalid weapon index: {index}");
            return;
        }

        // Проверяем что оружие есть в слоте
        if (_weaponSlots[index] == null)
        {
            Debug.LogWarning($"[WeaponSwitcher] No weapon in slot {index}");
            return;
        }

        if (_currentWeaponIndex == index)
        {
            return;
        }

        if (_currentWeapon != null)
        {
            _currentWeapon.ResetState();
            _currentWeapon.gameObject.SetActive(false);
        }

        _currentWeaponIndex = index;
        _currentWeapon = _weaponSlots[index];
        _currentWeapon.gameObject.SetActive(true);

        if (ActiveWeapon.Instance != null)
        {
            ActiveWeapon.Instance.SetCurrentWeapon(_currentWeapon);
        }

        Debug.Log($"[WeaponSwitcher] Switched to weapon: {_currentWeapon.name}");
    }

    public BaseWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public int GetCurrentWeaponIndex()
    {
        return _currentWeaponIndex;
    }
}
