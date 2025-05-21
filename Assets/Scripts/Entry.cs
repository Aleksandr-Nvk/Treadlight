using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField]
    private Cube Cube;
    
    [SerializeField]
    private CubeInputManager CubeInputManager;
    
    private void Awake()
    {
        CubeInputManager.Init();
        Cube.Init(CubeInputManager);
    }
}
