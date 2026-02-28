using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnRestartScene(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        {
            foreach (GameObject joystick in MenuPause.menuPauseInstancec.joysticks)
            {
                joystick.SetActive(false);
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        foreach (GameObject joystick in MenuPause.menuPauseInstancec.joysticks)
        {
            joystick.SetActive(true);
        }
#endif
        if (SceneManager.GetActiveScene().name == "Creditos")
        {
            Destroy(gameObject);
        }

        if (context.started)
        {
            RestartScene();
        }
    }

    // Carrega próxima cena na lista
    public void LoadNextScene()
    {
        if (LevelDeathManager.Instance != null)
            LevelDeathManager.Instance.MarkLevelPassed();

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentIndex + 1);
    }

    public void SkipLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentIndex + 1);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Reinicia a cena atual
    public void RestartScene()
    {
        if (LevelDeathManager.Instance != null)
            LevelDeathManager.Instance.RegisterDeath();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Sai do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
}
