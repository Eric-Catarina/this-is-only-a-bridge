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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        HandleCursorState(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleCursorState(scene);
    }
    
    void Update()
    {
        if (IsMenuScene())
            return;

        ControlCanvas();
    }

    private void HandleCursorState(Scene scene)
    {
        if (IsMenuScene())
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    void ControlCanvas()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            if (menuObject.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        menuObject.SetActive(true);
        UnlockCursor();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        menuObject.SetActive(false);
        LockCursor();
    }

    public void Restart()
    {
        ResumeGame();
        GameManager.Instance.RestartScene();
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private bool IsMenuScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        return currentScene == "Main_Menu" || currentScene == "Creditos";
    }
}