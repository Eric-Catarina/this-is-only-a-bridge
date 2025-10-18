using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configurações de Cena")]
    [Tooltip("Lista de cenas do jogo. Use o nome exato das cenas adicionadas no Build Settings.")]
    public string[] sceneNames;
    public string forceNextScene;

    int nextSceneIndex;

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
