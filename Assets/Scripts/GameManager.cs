using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configurações de Cena")]
    [Tooltip("Lista de cenas do jogo. Use o nome exato das cenas adicionadas no Build Settings.")]
    public string[] sceneNames;
    public string forceNextScene;

    int nextSceneIndex;

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
        nextSceneIndex = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    // Carrega próxima cena na lista
    public void LoadNextScene()
    {
        if (forceNextScene != "")
        {
            SceneManager.LoadScene(forceNextScene);
            return;
        }
        SceneManager.LoadScene(sceneNames[nextSceneIndex]);
        nextSceneIndex++;
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Reinicia a cena atual
    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Sai do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
}
