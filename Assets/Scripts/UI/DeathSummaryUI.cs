using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.IO;

public class DeathSummaryUI : MonoBehaviour
{
    [Header("Referências de UI")]
    [Tooltip("Texto único para mostrar o resumo completo de mortes")]
    public TextMeshProUGUI summaryText;
    [Tooltip("Botão para resetar todo o histórico de mortes")]
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

        sb.AppendLine("<size=140%><b>Resumo de Mortes</b></size>\n");
        sb.AppendLine("<b>Por fase:</b>");

        bool hasAnyLevel = false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            // Ignora o menu principal
            if (sceneName == "Main_Menu")
                continue;
            if (sceneName == "Creditos")
                continue;

            string deathKey = $"Deaths_Level_{sceneName}";
            string passedKey = $"LevelPassed_{sceneName}";
            string skippedKey = $"LevelSkipped_{sceneName}";

            int levelDeaths = PlayerPrefs.GetInt(deathKey, 0);
            bool levelPassed = PlayerPrefs.GetInt(passedKey, 0) == 1;
            bool levelSkipped = PlayerPrefs.GetInt(skippedKey, 0) == 1;

            string status = "";
            if (levelPassed)
                status = " <color=#5EFF5E>(Completed)</color>"; // Verde
            else if (levelSkipped)
                status = " <color=#FFD65E>(Git gud)</color>"; // Amarelo

            sb.AppendLine($"• <b>{sceneName}</b>: {levelDeaths}{status}");
            hasAnyLevel = true;
        }

        if (!hasAnyLevel)
            sb.AppendLine("<i>Nenhuma fase encontrada!</i>");

        sb.AppendLine("\n────────────────────────────");
        sb.AppendLine($"<size=110%><b>Total de mortes:</b> {totalDeaths}</size>");

        if (summaryText != null)
            summaryText.text = sb.ToString();
    }

    private void ResetAllDeaths()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            if (sceneName == "Main_Menu")
                continue;

            PlayerPrefs.DeleteKey($"Deaths_Level_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelPassed_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelSkipped_{sceneName}");
        }

        PlayerPrefs.DeleteKey("TotalDeaths");
        PlayerPrefs.Save();

        ShowDeathSummary();
        Debug.Log("Histórico de mortes resetado!");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
