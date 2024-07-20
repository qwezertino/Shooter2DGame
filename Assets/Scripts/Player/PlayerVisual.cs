using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private const string IS_MOVING = "IsMoving";

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isMoving = PlayerMovement.Instance.IsMoving();
        _animator.SetBool(IS_MOVING, isMoving);
        AdjustFacingDirection();
    }

    private void AdjustFacingDirection()
    {
        Vector2 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();
        if (mousePos.x < playerPosition.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}
