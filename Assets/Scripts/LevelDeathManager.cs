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
    private string levelTimeKey;
    private const string totalDeathsKey = "TotalDeaths";
    private const string totalTimeKey = "TotalTimePlayed";

    private int deathsThisLevel = 0;
    private int deathsTotal = 0;
    private bool levelPassed = false;
    private float levelStartTime = 0f;

    private void Awake()
    {
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
        InitKeysForScene(SceneManager.GetActiveScene().name);
        LoadValuesFromPrefs();

        if (skipLevelButton != null)
            skipLevelButton.onClick.AddListener(OnSkipLevelButtonPressed);

        levelStartTime = Time.time;

        UpdateUI();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Creditos")
        {
            Destroy(gameObject);
        }

        if (!hudFather.activeSelf && SceneManager.GetActiveScene().name != "Main_Menu")
            hudFather.SetActive(true);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;

        if (skipLevelButton != null)
            skipLevelButton.onClick.RemoveListener(OnSkipLevelButtonPressed);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitKeysForScene(scene.name);
        LoadValuesFromPrefs();
        levelStartTime = Time.time;
        UpdateUI();
    }

    private void InitKeysForScene(string sceneName)
    {
        levelDeathsKey = $"Deaths_Level_{sceneName}";
        levelPassedKey = $"LevelPassed_{sceneName}";
        levelTimeKey = $"LevelTime_{sceneName}";
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

        float timeTaken = Time.time - levelStartTime;
        float totalTime = PlayerPrefs.GetFloat(totalTimeKey, 0f);
        totalTime += timeTaken;

        PlayerPrefs.SetInt(levelPassedKey, 1);
        PlayerPrefs.SetFloat(levelTimeKey, timeTaken);
        PlayerPrefs.SetFloat(totalTimeKey, totalTime);
        PlayerPrefs.Save();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (deathsThisLevelText != null)
            deathsThisLevelText.text = "Level deaths: " + deathsThisLevel;

        if (deathsTotalText != null)
            deathsTotalText.text = "Total deaths: " + deathsTotal;

        if (messageText != null)
            messageText.gameObject.SetActive(false);
        if (skipLevelButton != null)
            skipLevelButton.gameObject.SetActive(false);

        if (levelPassed)
        {
            if (messageText != null)
            {
                messageText.text = "You already passed this level.";
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
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt($"LevelSkipped_{sceneName}", 1);
        PlayerPrefs.Save();
        GameManager.Instance.SkipLevel();
    }

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
