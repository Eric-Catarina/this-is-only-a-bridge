using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ActivateOnTrigger : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("O GameObject a ser ativado quando o gatilho for acionado.")]
    [SerializeField] private GameObject targetObjectToActivate;

    [Tooltip("Se especificado, apenas objetos com esta tag podem acionar o gatilho. Deixe em branco para permitir qualquer objeto.")]
    [SerializeField] private string triggeringTag = "Player";

    [Tooltip("Se verdadeiro, o gatilho funcionará apenas uma vez.")]
    [SerializeField] private bool triggerOnce = true;

    private bool hasBeenTriggered = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;

        if (targetObjectToActivate != null)
        {
            targetObjectToActivate.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && hasBeenTriggered)
        {
            return;
        }

        bool canTrigger = string.IsNullOrEmpty(triggeringTag) || other.CompareTag(triggeringTag);

        if (canTrigger)
        {
            if (targetObjectToActivate != null)
            {
                targetObjectToActivate.SetActive(true);
                hasBeenTriggered = true;
            }
            else
            {
                Debug.LogWarning("Target Object to Activate não está atribuído neste gatilho.", this);
            }
        }
    }
}