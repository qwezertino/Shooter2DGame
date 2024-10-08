using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolVisual : MonoBehaviour
{
    [SerializeField] private Pistol _pistol;
    private const string ATTACK = "Attack";
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _pistol.OnAttack += OnPistolShot;
    }

    private void OnPistolShot(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
