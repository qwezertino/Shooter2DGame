using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RaycastWeaponVisual : MonoBehaviour
{
    [SerializeField] private RaycastWeapon _weapon;
    private const string ATTACK = "Attack";
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _weapon.OnAttack += OnWeaponShot;
    }
    private void OnWeaponShot(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
