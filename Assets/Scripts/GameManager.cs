using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Creditos")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) || Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
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
