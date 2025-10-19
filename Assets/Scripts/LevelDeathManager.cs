using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelDeathManager : MonoBehaviour
{
    public static LevelDeathManager Instance;

    [Header("UI (atribua no Inspector - esses objetos serão filhos deste GameObject)")]
    public GameObject hudFather;
    public TextMeshProUGUI deathsThisLevelText;
    public TextMeshProUGUI deathsTotalText;
    public TextMeshProUGUI messageText;
    public Button skipLevelButton;
    public TextMeshProUGUI skipButtonText;

    [Header("Config")]
    public int deathsToOfferSkip = 10;

    private string levelDeathsKey;
    private string levelPassedKey;
    private const string totalDeathsKey = "TotalDeaths";

    private int deathsThisLevel = 0;
    private int deathsTotal = 0;
    private bool levelPassed = false;

    private void Awake()
    {
        // Singleton padrão
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Inicializa para a cena atual (Start roda uma vez; OnSceneLoaded cuida das trocas futuras)
        InitKeysForScene(SceneManager.GetActiveScene().name);
        LoadValuesFromPrefs();

        if (skipLevelButton != null)
        {
            skipLevelButton.onClick.AddListener(OnSkipLevelButtonPressed);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (!hudFather.activeSelf)
        {
            if(SceneManager.GetActiveScene().name != "Main_Menu")
                hudFather.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;

        if (skipLevelButton != null)
            skipLevelButton.onClick.RemoveListener(OnSkipLevelButtonPressed);
    }

    // Chamado sempre que uma cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Atualiza chaves e carrega valores da nova cena
        InitKeysForScene(scene.name);
        LoadValuesFromPrefs();
        UpdateUI();
    }

    private void InitKeysForScene(string sceneName)
    {
        levelDeathsKey = $"Deaths_Level_{sceneName}";
        levelPassedKey = $"LevelPassed_{sceneName}";
    }

    private void LoadValuesFromPrefs()
    {
        deathsThisLevel = PlayerPrefs.GetInt(levelDeathsKey, 0);
        deathsTotal = PlayerPrefs.GetInt(totalDeathsKey, 0);
        levelPassed = PlayerPrefs.GetInt(levelPassedKey, 0) == 1;
    }

    public void RegisterDeath()
    {
        deathsThisLevel++;
        deathsTotal++;

        PlayerPrefs.SetInt(levelDeathsKey, deathsThisLevel);
        PlayerPrefs.SetInt(totalDeathsKey, deathsTotal);
        PlayerPrefs.Save();

        UpdateUI();
    }

    public void MarkLevelPassed()
    {
        levelPassed = true;
        PlayerPrefs.SetInt(levelPassedKey, 1);
        PlayerPrefs.Save();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (deathsThisLevelText != null)
            deathsThisLevelText.text = "Level deaths: " + deathsThisLevel.ToString();

        if (deathsTotalText != null)
            deathsTotalText.text = "Total deaths: " + deathsTotal.ToString();

        // Oculta por padrão
        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (skipLevelButton != null)
            skipLevelButton.gameObject.SetActive(false);

        // Prioridade: se já passou -> mostrar "Você já passou essa fase" + botão "Pular a fase"
        if (levelPassed)
        {
            if (messageText != null)
            {
                messageText.text = "You alredy passed this level.";
                messageText.gameObject.SetActive(true);
            }

            if (skipLevelButton != null)
            {
                if (skipButtonText != null) skipButtonText.text = "Skip level";
                skipLevelButton.gameObject.SetActive(true);
                skipLevelButton.Select();
            }

            return;
        }

        // Se não passou e já acumulou mortes suficientes -> sugerir pular
        if (deathsThisLevel >= deathsToOfferSkip)
        {
            if (messageText != null)
            {
                messageText.text = "You can not DO IT? Skip this level NOOB!";
                messageText.gameObject.SetActive(true);
            }

            if (skipLevelButton != null)
            {
                if (skipButtonText != null) skipButtonText.text = "Skip level";
                skipLevelButton.gameObject.SetActive(true);
                skipLevelButton.Select();
            }
        }
    }

    private void OnSkipLevelButtonPressed()
    {
        //// Marca como passado e avança para a próxima cena (build index + 1)
        //PlayerPrefs.SetInt(levelPassedKey, 1);
        //PlayerPrefs.Save();

        //int current = SceneManager.GetActiveScene().buildIndex;
        //int next = current + 1;

        //if (next < SceneManager.sceneCountInBuildSettings)
        //{
        //    SceneManager.LoadScene(next);
        //}
        //else
        //{
        //    Debug.Log("Última cena — não há próxima cena nas Build Settings.");
        //    // opcional: carregar menu principal
        //    // SceneManager.LoadScene("Main_Menu");
        //}
        GameManager.Instance.SkipLevel();
    }

    // Métodos utilitários (debug / editor)
    public void ResetLevelDeaths()
    {
        deathsThisLevel = 0;
        PlayerPrefs.DeleteKey(levelDeathsKey);
        PlayerPrefs.Save();
        UpdateUI();
    }

    public void ResetTotalDeaths()
    {
        deathsTotal = 0;
        PlayerPrefs.DeleteKey(totalDeathsKey);
        PlayerPrefs.Save();
        UpdateUI();
    }
}
