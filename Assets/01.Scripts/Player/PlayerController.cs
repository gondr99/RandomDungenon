using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerAnimator _playerAnimator;
    
    private void Awake()
    {
        _playerAnimator = transform.Find("Visual").GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _playerAnimator.SetSpriter(0);
        }
        
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _playerAnimator.SetSpriter(1);
        }
        
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _playerAnimator.SetSpriter(2);
        }
    }
    
}
