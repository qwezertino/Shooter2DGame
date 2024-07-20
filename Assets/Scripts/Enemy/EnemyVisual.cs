using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator _animator;
    private bool _moving;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _animator.SetBool("IsMoving", _moving);
    }
    public void SetMoving(bool moving)
    {
        _moving = moving;
    }
}
