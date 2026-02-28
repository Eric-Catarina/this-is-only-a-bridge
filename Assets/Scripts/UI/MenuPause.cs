
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
    private PlayerInput inputDoCarro; // referência mantida para desinscrever depois
    

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
        if (inputDoCarro != null)
        {
            inputDoCarro.onActionTriggered -= HandleGlobalInput;
            inputDoCarro = null;
        }
    }

    private void Start()
    {
        HandleCursorForScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inputDoCarro = FindFirstObjectByType<PlayerInput>();
        if (inputDoCarro != null)
        {
            inputDoCarro.onActionTriggered -= HandleGlobalInput;
            inputDoCarro.onActionTriggered += HandleGlobalInput;
        }

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
        if (menuObject == null) return;
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
        LevelDeathManager.Instance.restartText.SetActive(false);
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        UnlockCursor();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        menuObject.SetActive(false);
        LevelDeathManager.Instance.restartText.SetActive(true);
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
    private void OnDestroy()
    {
        if (inputDoCarro != null)
        {
            inputDoCarro.onActionTriggered -= HandleGlobalInput;
            inputDoCarro = null;
        }
    }
    private void HandleGlobalInput(InputAction.CallbackContext context)
    {
        // Evita chamadas depois do objeto ter sido destruído ou se não for a instância singleton ativa
        if (this == null || menuPauseInstancec != this) return;

        // Só reagir quando a ação iniciar
        if (!context.started) return;

        string actionName = context.action?.name;
        if (string.IsNullOrEmpty(actionName)) return;

        // Suporta nomes do InputSystem gerados/no projeto ("OnPause", "Pause", etc.)
        if (actionName == "OnPause" || actionName == "Pause")
        {
            // Não processar em cenas de menu
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Main_Menu" || sceneName == "Creditos") return;

            // Garantir que menuObject esteja disponível antes de alterar seu estado
            if (menuObject == null)
            {
                GameObject canvasObj = GameObject.Find("Canvas_MenuPause");
                if (canvasObj != null)
                {
                    Transform optionsTransform = canvasObj.transform.Find("Options");
                    if (optionsTransform != null)
                    {
                        menuObject = optionsTransform.gameObject;
                        menuObject.SetActive(false);
                    }
                }
            }

            if (menuObject != null)
            {
                TogglePause();
            }
            else
            {
                Debug.LogWarning("MenuPause: menuObject é null ao tentar alternar pause.");
            }
        }
        else if (actionName == "OnRestartScene" || actionName == "OnRestart" || actionName == "Restart")
        {
            // Reinicia cena a partir do atalho global
            Restart();
        }
    }
}