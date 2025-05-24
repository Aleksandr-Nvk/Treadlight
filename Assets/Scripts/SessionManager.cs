using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private TileManager TileManager;
    [SerializeField] private CubeManager CubeManager;
    [SerializeField] private InputManager InputManager;
    
    public void StartSession()
    {
        CubeManager.SpawnCube();
        TileManager.StartGeneration();
        InputManager.EnableInput();
    }
}