using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    float startTimer;
    string currentScene;
    bool danielStarted = false;
    bool runningStarted = false;

    private void Awake()
    {
        startTimer = 0;
        danielStarted = false;
        currentScene = "IntroScene";
        SoundManager.Instance.PlayPlayerSound("GlassShatter");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        startTimer += Time.deltaTime;

        if (runningStarted == false && startTimer >= 0.5)
        {
            Debug.Log("Running started");
            SoundManager.Instance.PlayPlayerSound("Running");
            runningStarted = true;
        }

        if (danielStarted == false && startTimer >= 2)
        {
            Debug.Log("Daniel started");
            SoundManager.Instance.PlayEnemySound("starting");
            danielStarted = true;
        }

        if (currentScene == "IntroScene" && startTimer >= 5)
        {
            SceneManager.LoadScene("SoundTest");
            startTimer = 0;
        }
        else
        {
            return;
        }

    }

    public void LoadScene(string sceneName)
    {
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }
}
