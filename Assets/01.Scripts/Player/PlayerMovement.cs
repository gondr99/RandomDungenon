using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _visualTrm;

    [SerializeField] private float _speed;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementInput;
    private PlayerAnimator _playerAnimator;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = transform.Find("Visual").GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        _movementInput = _inputReader.Movement;
    }

    private void FixedUpdate()
    {
        Move();
        CheckFlip();
    }

    private void Move()
    {
        _rigidbody2D.velocity = _movementInput * _speed;
        _playerAnimator.SetMovement(_movementInput.sqrMagnitude > 0.01f);
    }

    private void CheckFlip()
    {
        if (Mathf.Abs(_movementInput.x) > 0)
        {
            Vector3 localScale = _visualTrm.localScale;
            if ( (_movementInput.x < 0 && localScale.x > 0)   || (_movementInput.x > 0) && localScale.x < 0 )
            {
                localScale.x *= -1;
            }
            _visualTrm.localScale = localScale;
        }
    }
}
