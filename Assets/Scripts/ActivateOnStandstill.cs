using UnityEngine;

public class ActivateOnStandstill : MonoBehaviour
{
    [Header("Configuração do Gatilho")]
    [Tooltip("O GameObject que será ativado.")]
    [SerializeField] private GameObject targetObjectToActivate;

    [Tooltip("O tempo em segundos que o jogador precisa ficar parado para ativar o objeto.")]
    [SerializeField] private float requiredStandstillTime = 5f;

    [Header("Sensibilidade")]
    [Tooltip("A velocidade máxima para ser considerado 'parado'. Ajuda a ignorar pequenos deslizes da física.")]
    [SerializeField] private float velocityThreshold = 0.1f;

    [Tooltip("Tag do objeto do jogador (o carro).")]
    [SerializeField] private string playerTag = "Player";
    
    private Rigidbody playerRigidbody;
    private float standstillTimer = 0f;
    private bool hasBeenTriggered = false;

    private void Awake()
    {
        if (targetObjectToActivate == null)
        {
            Debug.LogError("O 'Target Object To Activate' não foi atribuído neste script! Desativando componente.", this);
            enabled = false;
            return;
        }

        targetObjectToActivate.SetActive(false);
    }

    private void Start()
    {
        GameObject playerObject = gameObject;
        if (playerObject != null)
        {
            playerRigidbody = playerObject.GetComponent<Rigidbody>();
        }

        if (playerRigidbody == null)
        {
            Debug.LogError($"Não foi possível encontrar um Rigidbody no objeto com a tag '{playerTag}'. Desativando componente.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (hasBeenTriggered || playerRigidbody == null)
        {
            return;
        }

        if (playerRigidbody.linearVelocity.magnitude < velocityThreshold)
        {
            standstillTimer += Time.deltaTime;

            if (standstillTimer >= requiredStandstillTime)
            {
                ActivateTarget();
            }
        }
        else
        {
            standstillTimer = 0f;
        }
    }

    private void ActivateTarget()
    {
        targetObjectToActivate.SetActive(true);
        hasBeenTriggered = true;
        
        enabled = false;
    }
}