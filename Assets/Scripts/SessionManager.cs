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
        UIManager.ShowSoundToggle();
        
        InputManager.DisableInput();
        TileManager.StopGeneration();
        TileManager.ClearGrid();
        CubeManager.DestroyCube(true);
        
        StartPreviewSession();
    }
    
    private void EndSession()
    {
        UIManager.ShowGameOverMenu();
        UIManager.HidePauseButton();
        
        InputManager.DisableInput();
        TileManager.StopGeneration();
    }
}