using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject menuObject;

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
        if (SceneManager.GetActiveScene().name == "Main_Menu")
            return;

        ControlCanvas();
    }

    void ControlCanvas()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.startButton.wasPressedThisFrame) // OU PRESSIONAR BOT�O 'START' NO JOYSTICK
        {
            if (menuObject.activeSelf)
            {
                Time.timeScale = 1f;
                menuObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                menuObject.SetActive(true);
            }
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
