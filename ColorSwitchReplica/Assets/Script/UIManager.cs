using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    [Header("Set UI Text")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _exitPanel;
    [Header("Value of point per score")]
    [SerializeField] private int _pointPerScore;
    
    //Set BESTSCORE TEXT

    enum AnimationParam
    {
        Close
    }

    private int _bestScore;
    private int _score;
    private bool _isPause = false;
    private Rigidbody2D _playerRigidbody2D;
    private Text _scoreResultText;
    private Text _bestResultText;
    private StringBuilder _stringBuilder = new StringBuilder();
    private Animator _gameOverAnimator;

    private void Start()
    {
        _gameOverPanel.SetActive(false);
        _exitPanel.SetActive(false);
        _playerRigidbody2D = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
        _gameOverAnimator = _gameOverPanel.GetComponentInChildren<Animator>();
        _scoreResultText = _gameOverPanel.GetComponentsInChildren<Text>()[2];
        _bestResultText = _gameOverPanel.GetComponentsInChildren<Text>()[3];

        _bestScore = GameManager.Instance.BestScore;
        _stringBuilder.Append("BEST : " + _bestScore);
        _bestScoreText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_isPause)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// Get Score
    /// </summary>
    public void ScoreUp()
    {
        _score += _pointPerScore;
        UpdateUi();
    }
    
    /// <summary>
    /// To Update Ui
    /// </summary>
    private void UpdateUi()
    {
        _stringBuilder.Append("SCORE : " + _score);
        _scoreText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
        _stringBuilder.Append("BEST : " + +_bestScore);
        _bestScoreText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
    }
    
    /// <summary>
    /// Game Over
    /// </summary>
    /// <param name="openDelay">Delay to open Game over Panel</param>
    public IEnumerator OpenGameOver(float openDelay)
    {
        yield return new WaitForSeconds(openDelay);
        
        // Now Score
        _stringBuilder.Append("SCORE : " + _score);
        _scoreResultText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
        
        // Best Score
        if (_score > _bestScore)
        {
            DataSaver.SaveData(_score);
            _bestScore = _score;
        }
        _stringBuilder.Append("BEST :" + _bestScore);
        _bestResultText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
       
        
        _gameOverPanel.gameObject.SetActive(true);
    }

    public void OnPauseButtonClick()
    {
        if (_isPause)
        {
            Play();
        }
        else
        {
            Pause();
        }
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.ExitGame();
    }

    public void OnResumeButtonClick()
    {
        Play();
    }

    public IEnumerator GameOverAnimationEnd()
    {
        _gameOverAnimator.SetTrigger(nameof(AnimationParam.Close));
        yield return new WaitForSeconds(_gameOverAnimator.GetCurrentAnimatorClipInfo(0).Length + .7f);
    }

    void Pause()
    {
        _exitPanel.SetActive(true);
        _isPause                   = true;
        GameManager.Instance.State = GameManager.PlayState.Pause;
        _playerRigidbody2D.velocity = Vector2.zero;
        _playerRigidbody2D.isKinematic = true;
    }

    void Play()
    {
        _exitPanel.SetActive(false);
        _isPause                   = false;
        GameManager.Instance.State = GameManager.PlayState.Gaming;
        _playerRigidbody2D.isKinematic = false;
    }
}
