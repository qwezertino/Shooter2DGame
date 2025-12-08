using System;
using System.Collections;
using UnityEngine;
using Weapons.Enums;
using Weapons.Interfaces;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData _weaponData;
    private bool _canAttack = true;
    private bool _isFiring = false;

    public event EventHandler OnAttack;
    public WeaponData WeaponData => _weaponData;

    public void Attack()
    {
        if (!_isFiring)
        {
            StartCoroutine(SingleShotRoutine());
        }
    }

    public void StartFiring()
    {
        if (!_isFiring && _weaponData != null)
        {
            _isFiring = true;

            if (_weaponData.fireMode == FireMode.Single)
            {
                StartCoroutine(SingleShotRoutine());
                _isFiring = false;
            }
            else
            {
                StartCoroutine(ContinuousFireRoutine());
            }
        }
    }

    public void StopFiring()
    {
        _isFiring = false;
    }

    public void ResetState()
    {
        _isFiring = false;
        _canAttack = true;
        StopAllCoroutines();
    }

    private IEnumerator SingleShotRoutine()
    {
        if (_canAttack && _weaponData != null)
        {
            _canAttack = false;
            OnAttack?.Invoke(this, EventArgs.Empty);
            StartCoroutine(CoroutineAttack());
            yield return new WaitForSeconds(_weaponData.attackRate);
            _canAttack = true;
        }
    }

    private IEnumerator ContinuousFireRoutine()
    {
        while (_isFiring)
        {
            if (_canAttack && _weaponData != null)
            {
                _canAttack = false;
                OnAttack?.Invoke(this, EventArgs.Empty);
                StartCoroutine(CoroutineAttack());
                yield return new WaitForSeconds(_weaponData.attackRate);
                _canAttack = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    protected abstract IEnumerator CoroutineAttack();
}