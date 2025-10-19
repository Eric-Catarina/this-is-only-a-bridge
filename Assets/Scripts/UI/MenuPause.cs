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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        HandleCursorForScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleCursorForScene(scene);

        if (scene.name == "Creditos" && Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void HandleCursorForScene(Scene scene)
    {
        bool isMenuScene = scene.name == "Main_Menu" || scene.name == "Creditos";
        
        if (isMenuScene)
        {
            UnlockCursor();
            if (eventSystem != null) eventSystem.SetActive(true);
        }
        else
        {
            LockCursor();
            if (eventSystem != null) eventSystem.SetActive(true);
        }
    }

    void Update()
    {
        bool isMenuScene = SceneManager.GetActiveScene().name == "Main_Menu" || SceneManager.GetActiveScene().name == "Creditos";
        if (isMenuScene) return;

        if (Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            TogglePause();
        }
    }

    public void TogglePause()
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
}