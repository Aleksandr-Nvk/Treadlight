using Tiles;
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
        CubeManager.DestroyCube(true);
        CubeManager.SpawnCube();
        InputManager.EnableInput();
        TileManager.GenerateStartGrid();
        TileManager.StartGeneration();
    }

    public void PauseSession()
    {
        UIManager.HidePauseButton();
        UIManager.ShowPauseMenu();
        InputManager.DisableInput();
        TileManager.StopGeneration();
    }

    public void ResumeSession()
    {
        UIManager.ShowPauseButton();
        UIManager.HidePauseMenu();
        InputManager.EnableInput();
        TileManager.StartGeneration();
    }

    public void RestartSession()
    {
        UIManager.HideGameOverMenu();
        InputManager.EnableInput();
        TileManager.ClearGrid();

        StartSession();
    }

    public void CancelSession()
    {
        UIManager.HidePauseMenu();
        UIManager.HideGameOverMenu();
        UIManager.ShowPlayButton();
        UIManager.ShowSettingsButton();
        InputManager.DisableInput();
        TileManager.StopGeneration();
        TileManager.ClearGrid();
        CubeManager.DestroyCube(true);
    }
    
    private void EndSession()
    {
        UIManager.ShowGameOverMenu();
        UIManager.HidePauseButton();
        InputManager.DisableInput();
        TileManager.StopGeneration();
    }
}