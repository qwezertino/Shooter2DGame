using System;
using System.Collections;
using UnityEngine;
public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float _attackRate;
    private bool _canAttack = true;
    public event EventHandler OnAttack;
    private IEnumerator AttackCDRoutine()
    {
        if (_canAttack)
        {
            _canAttack = false;
            OnAttack?.Invoke(this, EventArgs.Empty);
            StartCoroutine(CoroutineAttack());
            yield return new WaitForSeconds(_attackRate);
            _canAttack = true;
        }
    }
    protected abstract IEnumerator CoroutineAttack();

    public void Attack()
    {
        StartCoroutine(AttackCDRoutine());
    }
}