using UnityEngine;

public class SpringPlatformMechanism : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float speedThreshold = 5f; // Quão rápido ele precisa passar
    [SerializeField] private float launchForce = 15f;   // Força da ejeção

    [Header("Configurações da Plataforma (Visual)")]
    [Tooltip("Arraste o objeto filho 'PlataformSpring' aqui")]
    [SerializeField] private Transform springPlatform;
    [SerializeField] private float compressedLocalY = 0.1f; // Altura dela abaixada
    [SerializeField] private float extendedLocalY = 1.0f;   // Altura dela estourada
    [SerializeField] private float springPopSpeed = 15f;    // Velocidade que a mola sobe

    private bool isTriggered = false;
    private Vector3 initialLocalPos;

    void Start()
    {
        if (springPlatform != null)
        {
            initialLocalPos = springPlatform.localPosition;

            // Força a plataforma vermelha a começar "abaixada" e comprimida
            springPlatform.localPosition = new Vector3(initialLocalPos.x, compressedLocalY, initialLocalPos.z);
        }
    }

    void Update()
    {
        // Se o jogador ativou a armadilha, a mola sobe violentamente até a altura estendida
        if (isTriggered && springPlatform != null)
        {
            Vector3 targetPos = new Vector3(initialLocalPos.x, extendedLocalY, initialLocalPos.z);
            springPlatform.localPosition = Vector3.Lerp(springPlatform.localPosition, targetPos, Time.deltaTime * springPopSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se a mola não disparou e se o objeto tem a tag Player
        if (!isTriggered && other.CompareTag(playerTag))
        {
            // 'attachedRigidbody' é mágico: ele acha o Rigidbody do carro 
            // mesmo que o colisor que encostou seja o da roda ou do para-choque
            Rigidbody playerRb = other.attachedRigidbody;

            if (playerRb != null)
            {
                // Aqui nós pegamos a velocidade de movimento REAL do carro no mundo
                float playerSpeed = playerRb.linearVelocity.magnitude;

                Debug.Log("Velocidade do carro ao passar: " + playerSpeed);

                if (playerSpeed > speedThreshold)
                {
                    ActivateSpring(playerRb);
                }
            }
        }
    }

    private void ActivateSpring(Rigidbody playerRb)
    {
        isTriggered = true;
        // VelocityChange aplica a força diretamente, ignorando se o carro pesa 1kg ou 2 toneladas
        playerRb.AddForce(transform.up * launchForce, ForceMode.VelocityChange);
        Debug.Log("Mola ativada! Jogador ejetado.");
    }
}