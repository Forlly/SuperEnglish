using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Views;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText ;
    [SerializeField] private float _timeRemaining = 120;
    [SerializeField] private bool _timerIsRunning = false;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TMP_Text _totalPoints;
    [SerializeField] private TMP_Text _guessedWords;
    [SerializeField] private Button _restartButton;
    private void Start()
    {
        _timerIsRunning = true;
        _restartButton.onClick.AddListener(RestartLevel);
    }
    void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                _timerText.text = $"{(int)_timeRemaining / 60} : {(int)_timeRemaining % 60}";
            }
            else
            {
                Debug.Log("Time has run out!");
                TimeIsOver();
            }
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TimeIsOver()
    {
        _timeRemaining = 0;
        _timerIsRunning = false;
        _endPanel.SetActive(true);
        _totalPoints.text = $"Total points: {WordChecker.Instance.GetPoints().ToString()}";
        _guessedWords.text = $"Guessed words: {WordChecker.Instance.GetCountOfGuessedWords().ToString()}";
    }
}
