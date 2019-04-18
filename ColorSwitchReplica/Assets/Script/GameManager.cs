using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        BestScore = DataSaver.LoadBestScore();
    }

    #endregion
    
    public enum PlayState
    {
        Gaming,
        Pause,
        GameOver
    }
    
    [Header("Game State")]
    public PlayState State = PlayState.Gaming;

    public int BestScore { get; set; }

    /// <summary>
    /// Restart this Scene
    /// </summary>
    public IEnumerator Restart()
    {
        bool isPressed = false;
        while (!isPressed)
        {
            isPressed = Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump");
            yield return null;
        }
        yield return StartCoroutine(UIManager.Instance.GameOverAnimationEnd());
        State = PlayState.Gaming;
        SceneManager.LoadScene(0); // Now Scene
    }
    
    /// <summary>
    /// Game Quit
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR // Editor Play
        EditorApplication.isPlaying = false;
#elif PLATFORM_STANDALONE // Build Play
        Application.Quit();
#endif
    }
}
