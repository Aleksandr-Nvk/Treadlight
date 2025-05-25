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
        TileManager.RemoveAll();
        CubeManager.DestroyCube(true);
        UIManager.HidePlayButton();
        UIManager.ShowPauseButton();
        TileManager.GenerateStartGrid();
        TileManager.StartGeneration();
        CubeManager.SpawnCube();
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
        UIManager.ShowGameOverMenu();
        UIManager.HidePauseButton();
        TileManager.StopGeneration();
        InputManager.DisableInput();
    }

    public void RestartSession()
    {
        UIManager.HideGameOverMenu();
        TileManager.StopGeneration();
        InputManager.DisableInput();

        StartSession();
    }

    public void CancelSession()
    {
        // when exited from pause menu
    }
}