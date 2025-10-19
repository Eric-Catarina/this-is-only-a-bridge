using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioSource engineAudio;

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

        engineAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Creditos")
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.R) || Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            RestartScene();
            engineAudio.Play();
            engineAudio.Stop();
        }


    }

    // Carrega próxima cena na lista
    public void LoadNextScene()
    {
        if (LevelDeathManager.Instance != null)
            LevelDeathManager.Instance.MarkLevelPassed();

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentIndex + 1);
        engineAudio.Play();
        engineAudio.Stop();
    }

    public void SkipLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentIndex + 1);
        engineAudio.Play();
        engineAudio.Stop();
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        engineAudio.Play();
        engineAudio.Stop();
    }

    // Reinicia a cena atual
    public void RestartScene()
    {
        if (LevelDeathManager.Instance != null)
            LevelDeathManager.Instance.RegisterDeath();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        engineAudio.Play();
        engineAudio.Stop();
    }

    // Sai do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
}
