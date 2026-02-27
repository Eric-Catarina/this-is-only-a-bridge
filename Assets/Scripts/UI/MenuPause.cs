
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MenuPause : MonoBehaviour
{
    public GameObject menuObject;
    //public GameObject eventSystem;
    public GameObject[] joysticks;
    public GameObject pauseButton;
    [SerializeField] GameObject restartButton;
    public static MenuPause menuPauseInstancec;

    private void Awake()
    {
        if (menuPauseInstancec == null)
        {
            menuPauseInstancec = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
#if UNITY_EDITOR
        pauseButton.SetActive(false);
        restartButton.SetActive(false);
        foreach (GameObject joystick in joysticks)
        {
            joystick.SetActive(false);
        }
#elif UNITY_ANDROID || UNITY_IOS
        pauseButton.SetActive(true);
        restartButton.SetActive(true);
        foreach (GameObject joystick in joysticks) 
        {
            joystick.SetActive(true);
        }
#endif
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //menuObject.SetActive(true);

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

        GameObject canvasObj = GameObject.Find("Canvas_MenuPause");
        if (canvasObj != null)
        {
            // Se 'Options' for filho direto do Canvas
            Transform optionsTransform = canvasObj.transform.Find("Options");
            if (optionsTransform != null)
            {
                menuObject = optionsTransform.gameObject;
                menuObject.SetActive(false); // Começa escondido
            }
        }

        if (scene.name == "Creditos" && menuPauseInstancec == this)
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
            //if (eventSystem != null) eventSystem.SetActive(true);
        }
        else
        {
            LockCursor();
            //if (eventSystem != null) eventSystem.SetActive(true);
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        bool isMenuScene = SceneManager.GetActiveScene().name == "Main_Menu" || SceneManager.GetActiveScene().name == "Creditos";
        if (isMenuScene) return;
        if (context.started)
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
        menuObject.SetActive(true);
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        UnlockCursor();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        menuObject.SetActive(false);
        #if UNITY_EDITOR
                pauseButton.SetActive(false);
        #elif UNITY_ANDROID || UNITY_IOS
            pauseButton.SetActive(false);
        #endif
        LockCursor();
    }

    public void Restart()
    {
        ResumeGame(); 
        GameManager.gameManager.RestartScene();
    }

    public void Quit()
    {
        GameManager.gameManager.QuitGame();
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