using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.IO;

public class DeathSummaryUI : MonoBehaviour
{
    [Header("Referências de UI")]
    public TextMeshProUGUI summaryText;
    public Button resetButton;

    private void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetAllDeaths);

        ShowDeathSummary();
    }

    private void OnDestroy()
    {
        if (resetButton != null)
            resetButton.onClick.RemoveListener(ResetAllDeaths);
    }

    private void OnEnable()
    {
        ShowDeathSummary();
    }

    private void ShowDeathSummary()
    {
        StringBuilder sb = new StringBuilder();

        int totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        float totalTime = PlayerPrefs.GetFloat("TotalTimePlayed", 0f);

        sb.AppendLine("<size=140%><b>Resumo de Progresso</b></size>\n");
        sb.AppendLine("<b>Por fase:</b>");

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(path);
            if (sceneName == "Main_Menu" || sceneName == "Creditos")
                continue;

            int deaths = PlayerPrefs.GetInt($"Deaths_Level_{sceneName}", 0);
            bool passed = PlayerPrefs.GetInt($"LevelPassed_{sceneName}", 0) == 1;
            bool skipped = PlayerPrefs.GetInt($"LevelSkipped_{sceneName}", 0) == 1;
            float time = PlayerPrefs.GetFloat($"LevelTime_{sceneName}", 0f);

            string status = "";
            if (passed)
                status = $" <color=#5EFF5E>(Completed)</color> <size=80%>({FormatTime(time)})</size>";
            else if (skipped)
                status = " <color=#FFD65E>(Git gud)</color>";

            sb.AppendLine($"• <b>{sceneName}</b>: {deaths}{status}");
        }

        sb.AppendLine("\n────────────────────────────");
        sb.AppendLine($"<b>Total deaths:</b> {totalDeaths}");
        sb.AppendLine($"<b>Total time played:</b> {FormatTime(totalTime)}");

        summaryText.text = sb.ToString();
    }

    private string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{mins:00}:{secs:00}";
    }

    private void ResetAllDeaths()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(path);
            if (sceneName == "Main_Menu" || sceneName == "Creditos")
                continue;

            PlayerPrefs.DeleteKey($"Deaths_Level_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelPassed_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelSkipped_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelTime_{sceneName}");
        }

        PlayerPrefs.DeleteKey("TotalDeaths");
        PlayerPrefs.DeleteKey("TotalTimePlayed");
        PlayerPrefs.Save();

        ShowDeathSummary();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
