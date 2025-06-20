﻿using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action OnTopRightSwipe;
    public Action OnTopLeftSwipe;
    public Action OnBottomRightSwipe;
    public Action OnBottomLeftSwipe;

    private CubeInputSystem _inputSystem;

    private const float SwipeThreshold = 50;
    
    public void Awake()
    {
        _inputSystem = new CubeInputSystem();
    }

    private Vector2? _startTapPosition;
    private void Update()
    {
        var isTapDown = _inputSystem.Player.Tap.IsPressed();
        var currentTapPosition = _inputSystem.Player.Drag.ReadValue<Vector2>();
        if (isTapDown)
        {
            _startTapPosition ??= currentTapPosition;
            if (_startTapPosition is not null &&
                Vector2.Distance(_startTapPosition.Value, currentTapPosition) > SwipeThreshold)
            {
                InvokeSwipe(_startTapPosition.Value, currentTapPosition);
                _inputSystem.Player.Tap.Reset();
                _startTapPosition = null;
            }
        }
    }

    public void EnableInput() => _inputSystem.Player.Enable();
    public void DisableInput() => _inputSystem.Player.Disable();
    
    private void InvokeSwipe(Vector2 startTapPosition, Vector2 endTapPosition)
    {
        var diff = endTapPosition - startTapPosition;
        switch (diff.x)
        {
            case >= 0 when diff.y >= 0:
                OnTopRightSwipe?.Invoke();
                break;
            case >= 0 when diff.y < 0:
                OnBottomRightSwipe?.Invoke();
                break;
            case < 0 when diff.y >= 0:
                OnTopLeftSwipe?.Invoke();
                break;
            case < 0 when diff.y < 0:
                OnBottomLeftSwipe?.Invoke();
                break;
        }
    }
}