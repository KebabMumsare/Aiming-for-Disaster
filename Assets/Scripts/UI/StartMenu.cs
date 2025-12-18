using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartMenu : MonoBehaviour
{
    [SerializeField] public VideoPlayer videoPlayer;

    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // When the video finishes, start the game
        StartGame();
    }

    public void StartGame()
    {
        // Loads the next scene in the build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
