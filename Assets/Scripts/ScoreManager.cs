using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private Cube.Cube Cube;
    
    private const string HighestScorePlayerPrefsKey = "HighestScore";

    private int _currentScore;
    private int _highestScore;

    private float _maxX;
    private float _maxZ;
    
    private void Awake()
    {
        Cube.OnMoved += _ =>
        {
            var currentCubePosition = Cube.transform.position;
            if (currentCubePosition.x - _maxX < 0.01f && currentCubePosition.z - _maxZ < 0.01f) return;
            
            _currentScore++;
            if (_currentScore > _highestScore)
            {
                UIManager.ShowHighestScoreText();
            }
                
            UIManager.SetScoreText(_currentScore);
            _maxX = currentCubePosition.x;
            _maxZ = currentCubePosition.z;
        };
    }

    private void Start()
    {
        _highestScore = LoadHighestScore();
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
        UIManager.SetScoreText(0);
    }
}