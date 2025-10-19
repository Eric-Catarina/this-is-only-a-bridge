using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject menuObject;
    public GameObject eventSystem;

    public static MenuPause Instance;

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

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Creditos")
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Main_Menu")
            return;
        else if (!eventSystem.activeSelf)
            eventSystem.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            ControlCanvas();
        }
    }

    public void ControlCanvas()
    {
        if (menuObject.activeSelf)
        {
            Time.timeScale = 1f;
            menuObject.SetActive(false);
            LevelDeathManager.Instance.skipLevelButton.Select();
        }
        else
        {
            Time.timeScale = 0f;
            menuObject.SetActive(true);
        }
    }

    public void Restart()
    {
        GameManager.Instance.RestartScene();
        Time.timeScale = 1f;
        menuObject.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
