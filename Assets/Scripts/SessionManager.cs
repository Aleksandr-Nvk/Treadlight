using System;
using Cube;
using Tiles;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private TileManager TileManager;
    [SerializeField] private CubeManager CubeManager;
    [SerializeField] private InputManager InputManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private CameraFollower CameraFollower;
    [SerializeField] private ScoreManager ScoreManager;
    
    private void Awake()
    {
        CubeManager.OnCubeDestroyed += EndSession;
    }

    private void Start()
    {
        StartPreviewSession();
    }

    public void StartPreviewSession()
    {
        UIManager.ShowPlayButton();
        UIManager.ShowSoundToggle();
        UIManager.ShowInfoButton();
        UIManager.ShowScoreCounter();
        UIManager.ShowHighestScoreText();
        UIManager.SetScoreText(ScoreManager.LoadHighestScore());
        
        TileManager.GenerateStartGrid();
        TileManager.StartGeneration();
        CameraFollower.EnableCameraIdle();
    }

    public void StartSession()
    {
        UIManager.HidePlayButton();
        UIManager.HideSoundToggle();
        UIManager.HideInfoButton();
        UIManager.ShowPauseButton();
        UIManager.HideHighestScoreText();
        ScoreManager.ResetScore();

        CubeManager.DestroyCube(true);
        CubeManager.SpawnCube();
        InputManager.EnableInput();
        TileManager.ClearGrid();
        TileManager.GenerateStartGrid();
        TileManager.StartGeneration();
        CameraFollower.DisableCameraIdle();
    }

    public void PauseSession()
    {
        UIManager.HidePauseButton();
        UIManager.ShowPauseMenu();
        
        InputManager.DisableInput();
        TileManager.StopGeneration();
        CameraFollower.ZoomOut();
    }

    public void ResumeSession()
    {
        UIManager.ShowPauseButton();
        UIManager.HidePauseMenu();
        
        InputManager.EnableInput();
        TileManager.StartGeneration();
        CameraFollower.ZoomIn();
    }

    public void RestartSession()
    {
        UIManager.HideGameOverMenu();
        
        InputManager.EnableInput();
        TileManager.ClearGrid();
        ScoreManager.ResetScore();

        StartSession();
    }

    public void CancelSession()
    {
        UIManager.HidePauseMenu();
        UIManager.HideGameOverMenu();
        UIManager.ShowPlayButton();
        UIManager.ShowSoundToggle();
        
        InputManager.DisableInput();
        TileManager.StopGeneration();
        TileManager.ClearGrid();
        CubeManager.DestroyCube(true);
        ScoreManager.ResetScore();

        StartPreviewSession();
    }
    
    public void EndSession()
    {
        UIManager.ShowGameOverMenu();
        UIManager.HidePauseButton();
        ScoreManager.SaveHighestScore();
        ScoreManager.ResetScore();

        InputManager.DisableInput();
        TileManager.StopGeneration();
    }
    
    // for demonstration
    public void OpenGitHub() => Application.OpenURL("https://github.com/Aleksandr-Nvk");
}