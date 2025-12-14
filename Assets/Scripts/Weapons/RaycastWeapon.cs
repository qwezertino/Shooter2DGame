using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ActionGame.Utils;

public class RaycastWeapon : BaseWeapon
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _muzzleFlash;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask _hitLayers = -1;
    [SerializeField] private bool _debug = false;

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

        // Raycast который ИГНОРИРУЕТ триггеры (PolygonCollider), но попадает в обычные коллайдеры (BoxCollider)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false; // НЕ попадать в триггеры
        contactFilter.SetLayerMask(_hitLayers);
        contactFilter.useLayerMask = true;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        int hitCount = Physics2D.Raycast(_firePoint.position, _firePoint.right, contactFilter, hits, _weaponData.range);

        RaycastHit2D hitInfo = hitCount > 0 ? hits[0] : default;

        if (_debug)
        {
            Utils.RaycastLogHits(hits, hitCount, _weaponData?.weaponName ?? "Unknown Weapon");

            // Визуализация raycast в Scene
            Utils.RaycastDrawLine(
                _firePoint.position,
                _firePoint.right,
                _weaponData.range,
                hitInfo.collider != null,
                hitInfo.point
            );
        }

        if (hitInfo)
        {
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