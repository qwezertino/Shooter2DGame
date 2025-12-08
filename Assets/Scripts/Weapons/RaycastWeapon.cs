using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastWeapon : BaseWeapon
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _muzzleFlash;

    private void Start()
    {
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }

    protected override IEnumerator CoroutineAttack()
    {
        if (_weaponData == null)
        {
            Debug.LogError($"WeaponData not assigned to {gameObject.name}!");
            yield break;
        }

        _lineRenderer.positionCount = 2;
        RaycastHit2D hitInfo = Physics2D.Raycast(_firePoint.position, _firePoint.right, _weaponData.range);

        if (hitInfo)
        {
            // Debug.Log($"{_weaponData.weaponName} hit: {hitInfo.transform.name} for {_weaponData.damage} damage");

            // TODO: Добавить нанесение урона когда будет система здоровья
            // IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            // damageable?.TakeDamage(_weaponData.damage);

            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, _firePoint.position + _firePoint.right * _weaponData.range);
        }

        _lineRenderer.enabled = true;
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_weaponData.muzzleFlashDuration);
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }
}