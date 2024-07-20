using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeVisual : MonoBehaviour
{
    [SerializeField] private Axe _axe;
    private const string ATTACK = "Attack";
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _axe.OnAxeSwing += Axe_OnAxeSwing;
    }

    private void Axe_OnAxeSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
