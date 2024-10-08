using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pistol : BaseWeapon
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
    // public void Attack()
    // {
    //     StartCoroutine(AttackCDRoutine());
    // }
    // private IEnumerator AttackCDRoutine()
    // {
    //     if (_canFire)
    //     {
    //         _canFire = false;
    //         OnPistolShot?.Invoke(this, EventArgs.Empty);
    //         StartCoroutine(CoroutineAttack());
    //         yield return new WaitForSeconds(_fireRate);
    //         _canFire = true;
    //     }
    // }
    protected override IEnumerator CoroutineAttack()
    {
        _lineRenderer.positionCount = 2;
        RaycastHit2D _hitInfo = Physics2D.Raycast(_firePoint.position, _firePoint.right);
        if (_hitInfo)
        {

            Debug.Log(_hitInfo.transform.name);
            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, _hitInfo.point);
        }
        else
        {
            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, _firePoint.position + _firePoint.right * 100f);
        }
        _lineRenderer.enabled = true;
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }
}