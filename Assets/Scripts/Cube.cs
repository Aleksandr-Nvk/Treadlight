using UnityEngine;

public class Cube : MonoBehaviour
{
    private CubeInputManager _inputManager;

    public void Init(CubeInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _inputManager.OnTopRightSwipe += () => { };
        _inputManager.OnTopLeftSwipe += () => { };
        _inputManager.OnBottomRightSwipe += () => { };
        _inputManager.OnBottomLeftSwipe += () => { };
    }
}
