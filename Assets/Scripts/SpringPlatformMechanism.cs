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

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se a mola já estourou e se quem pisou foi o player
        if (!isTriggered && collision.gameObject.CompareTag(playerTag))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Verifica a velocidade de impacto
                float playerSpeed = collision.relativeVelocity.magnitude;

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

        // Usamos 'transform.up' para jogar o player na direção que a prancha está apontando, e não no Y global
        playerRb.AddForce(transform.up * launchForce, ForceMode.Impulse);

        Debug.Log("Mola ativada! Jogador ejetado.");
    }
}