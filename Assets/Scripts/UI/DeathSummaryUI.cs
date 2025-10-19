using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;

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

        bool hasAnyDeath = false;

        // Percorre todas as cenas do Build Settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            string levelDeathsKey = $"Deaths_Level_{sceneName}";
            int levelDeaths = PlayerPrefs.GetInt(levelDeathsKey, 0);

            if (levelDeaths > 0)
            {
                hasAnyDeath = true;
                sb.AppendLine($"• <b>{sceneName}</b>: {levelDeaths} mortes");
            }
        }

        if (!hasAnyDeath)
            sb.AppendLine("<i>Nenhuma morte registrada ainda!</i>");

        sb.AppendLine("\n────────────────────────────");
        sb.AppendLine($"<size=110%><b>Total de mortes:</b> {totalDeaths}</size>");

        if (summaryText != null)
            summaryText.text = sb.ToString();
    }

    private void ResetAllDeaths()
    {
        // Apaga todas as chaves relacionadas
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            PlayerPrefs.DeleteKey($"Deaths_Level_{sceneName}");
            PlayerPrefs.DeleteKey($"LevelPassed_{sceneName}");
        }

        PlayerPrefs.DeleteKey("TotalDeaths");
        PlayerPrefs.Save();

        ShowDeathSummary();
        Debug.Log("Histórico de mortes resetado!");
    }
}
