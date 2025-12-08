using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [SerializeField] private float _movingSpeed = 5f;
    private Rigidbody2D _rigidbody;
    private float _minMovingSpeed = 0.1f;
    private bool _moving = false;
    private Vector2 inputVector = Vector2.zero;
    private void Awake()
    {
        Instance = this;
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        _rigidbody.linearVelocity = inputVector * _movingSpeed;

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _moving = true;
        }
        else
        {
            _moving = false;
        }
    }
    public bool IsMoving()
    {
        return _moving;
    }
}
