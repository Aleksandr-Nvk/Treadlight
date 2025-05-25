using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private TileManager TileManager;
    [SerializeField] private CubeManager CubeManager;
    [SerializeField] private InputManager InputManager;
    [SerializeField] private UIManager UIManager;
    
    private void Awake()
    {
        CubeManager.OnCubeDestroyed += EndSession;
    }

    public void StartSession()
    {
        UIManager.HidePlayButton();
        UIManager.ShowPauseButton();
        CubeManager.SpawnCube();
        TileManager.GenerateStartGrid();
        TileManager.StartGeneration();
        InputManager.EnableInput();
    }

    public void PauseSession()
    {
        UIManager.HidePauseButton();
        UIManager.ShowPauseMenu();
        TileManager.StopGeneration();
        InputManager.DisableInput();
    }

    public void ResumeSession()
    {
        UIManager.ShowPauseButton();
        UIManager.HidePauseMenu();
        TileManager.StartGeneration();
        InputManager.EnableInput();
    }

    public void EndSession()
    {
        // UIManager.ShowPlayButton(); should be ShowRestartButton
        UIManager.HidePauseButton();
        TileManager.StopGeneration();
        InputManager.DisableInput();
    }
}