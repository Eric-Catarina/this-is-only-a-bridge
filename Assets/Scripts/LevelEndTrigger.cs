using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelEndTrigger : MonoBehaviour
{
    [Header("Configuração do Gatilho")]
    [Tooltip("Tag do objeto que pode ativar o gatilho. Certifique-se que seu carro tenha esta tag.")]
    [SerializeField] private string playerTag = "Player";

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
            GameManager.Instance.LoadNextScene();
        }
    }
}