using System;
using UnityEngine;

public class CubeInputManager : MonoBehaviour
{
    private const float SwipeThreshold = 50; // minimum swipe length in pixels

    private CubeInputSystem _inputSystem;

    public Action OnTopRightSwipe;
    public Action OnTopLeftSwipe;
    public Action OnBottomRightSwipe;
    public Action OnBottomLeftSwipe;

    public void Init()
    {
        _inputSystem = new CubeInputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Enable();
    }

    private Vector2? _startTapPosition;
    private void Update()
    {
        var isTapDown = _inputSystem.Player.Tap.IsPressed();;
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
    
    private void OnDisable()
    {
        _inputSystem.Player.Disable();
    }
    
    private void InvokeSwipe(Vector2 startTapPosition, Vector2 endTapPosition)
    {
        var diff = endTapPosition - startTapPosition;
        switch (diff.x)
        {
            case >= 0 when diff.y >= 0:
                OnTopRightSwipe.Invoke();
                break;
            case >= 0 when diff.y < 0:
                OnBottomRightSwipe.Invoke();
                break;
            case < 0 when diff.y >= 0:
                OnTopLeftSwipe.Invoke();
                break;
            case < 0 when diff.y < 0:
                OnBottomLeftSwipe.Invoke();
                break;
        }
    }
}