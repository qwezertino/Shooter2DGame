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

    private float _currentSpread = 0f;
    private float _lastShotTime = 0f;

    private void Start()
    {
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        if (_weaponData != null && Time.time - _lastShotTime > 0.1f)
        {
            _currentSpread = Mathf.MoveTowards(
                _currentSpread,
                _weaponData.minSpread,
                _weaponData.spreadRecoveryRate * Time.deltaTime
            );
        }
    }

    protected override IEnumerator CoroutineAttack()
    {
        if (_weaponData == null)
        {
            Debug.LogError($"WeaponData not assigned to {gameObject.name}!");
            yield break;
        }

        _lineRenderer.positionCount = 2;

        float spreadAngle = UnityEngine.Random.Range(-_currentSpread, _currentSpread);
        Vector2 spreadDirection = Quaternion.Euler(0, 0, spreadAngle) * _firePoint.right;

        _currentSpread = Mathf.Min(_currentSpread + _weaponData.spreadIncreasePerShot, _weaponData.maxSpread);
        _lastShotTime = Time.time;

        // Raycast that IGNORES triggers (PolygonCollider) but hits regular colliders (BoxCollider)
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false; // Do NOT hit triggers
        contactFilter.SetLayerMask(_hitLayers);
        contactFilter.useLayerMask = true;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        int hitCount = Physics2D.Raycast(_firePoint.position, spreadDirection, contactFilter, hits, _weaponData.range);

        RaycastHit2D hitInfo = hitCount > 0 ? hits[0] : default;

        if (_debug)
        {
            Utils.RaycastLogHits(hits, hitCount, _weaponData?.weaponName ?? "Unknown Weapon");
            Debug.Log($"Current Spread: {_currentSpread:F2}°");

            // Raycast visualization in Scene
            Utils.RaycastDrawLine(
                _firePoint.position,
                spreadDirection,
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
            _lineRenderer.SetPosition(1, (Vector2)_firePoint.position + spreadDirection * _weaponData.range);
        }

        _lineRenderer.enabled = true;
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_weaponData.muzzleFlashDuration);
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }
}