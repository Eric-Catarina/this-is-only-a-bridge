using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelEndTrigger : MonoBehaviour
{
    [Header("Configuração do Gatilho")]
    [Tooltip("Tag do objeto que pode ativar o gatilho. Certifique-se que seu carro tenha esta tag.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Nome da cena específica para carregar. Se deixado em branco, carregará a próxima cena da lista do GameManager.")]
    [SerializeField] private string specificSceneName;

    [Tooltip("Garante que o gatilho seja ativado apenas uma vez.")]
    [SerializeField] private bool triggerOnce = true;

    private bool hasBeenTriggered = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && hasBeenTriggered)
        {
            return;
        }

        if (other.CompareTag(playerTag))
        {
            hasBeenTriggered = true;
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance não foi encontrado. O gatilho não pode funcionar.");
            return;
        }
GameManager gameManager = GameManager.Instance;
        gameManager.LoadNextScene();
    }
}