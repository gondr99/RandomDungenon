using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _visualTrm;

    [SerializeField] private float _speed;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementInput;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
