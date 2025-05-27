using Tiles;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private Cube.Cube Cube;
    
    private const string HighestScorePlayerPrefsKey = "HighestScore";
    private const int StainScore = 5;

    private int _currentScore = 0;
    private int _highestScore;

    private float _maxX = 0;
    private float _maxZ = 0;
    
    private void Awake()
    {
        _highestScore = LoadHighestScore();
        Cube.OnMoved += tileType =>
        {
            var currentCubePosition = Cube.transform.parent.position;
            if (tileType == TileType.Stain)
            {
                _currentScore += StainScore;
                UIManager.SetScoreText(_currentScore);
            }

            if (currentCubePosition.x - _maxX < 0.01f && currentCubePosition.z - _maxZ < 0.01f) return;
            _maxX = currentCubePosition.x;
            _maxZ = currentCubePosition.z;
            
            _currentScore++;
            UIManager.SetScoreText(_currentScore);

            if (_currentScore > _highestScore)
            {
                UIManager.ShowHighestScoreText();
            }
        };
    }

    public int LoadHighestScore()
    {
        return PlayerPrefs.GetInt(HighestScorePlayerPrefsKey, 0);
    }

    public void SaveHighestScore()
    {
        if (_currentScore <= _highestScore) return;
        
        PlayerPrefs.SetInt(HighestScorePlayerPrefsKey, _currentScore);
    }

    public void ResetScore()
    {
        _currentScore = 0;
        _maxX = 0;
        _maxZ = 0;
        UIManager.SetScoreText(0);
    }
}