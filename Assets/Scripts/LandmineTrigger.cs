using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LandmineTrigger : MonoBehaviour
{
    [Header("Configuração da Mina")]
    [Tooltip("Tag do objeto que pode ativar a mina. Certifique-se que seu carro tenha esta tag.")]
    [SerializeField] private string playerTag = "Player";
    
    [Tooltip("Velocidade máxima em Km/h para passar em segurança.")]
    [SerializeField] private float safeSpeedKmh = 5f;

    [Header("Efeitos da Explosão")]
    [Tooltip("A força do impulso aplicado ao objeto na explosão.")]
    [SerializeField] private float explosionForce = 700f;

    [Tooltip("O raio da explosão. Afeta Rigidbodies dentro desta área.")]
    [SerializeField] private float explosionRadius = 5f;

    [Tooltip("Modificador que adiciona uma força vertical para lançar o objeto para cima.")]
    [SerializeField] private float upwardsModifier = 2.0f;

    [Header("Feedback Visual e Sonoro (Opcional)")]
    [Tooltip("Prefab do sistema de partículas para instanciar na explosão.")]
    [SerializeField] private GameObject explosionVFX;
    
    [Tooltip("Som da explosão.")]
    [SerializeField] private AudioClip explosionSFX;

    private bool hasExploded = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded || !other.CompareTag(playerTag))
        {
            return;
        }

        Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
        if (targetRigidbody == null)
        {
            Debug.LogWarning("O objeto com a tag 'Player' não possui Rigidbody. A mina não pode funcionar.", other);
            return;
        }

        float speedMps = targetRigidbody.linearVelocity.magnitude;
        float speedKmh = speedMps * 3.6f;

        if (speedKmh >= safeSpeedKmh)
        {
            Detonate(targetRigidbody);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    private void Detonate(Rigidbody target)
    {
        hasExploded = true;
        
        target.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);

        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }
        
        if (explosionSFX != null)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
        }

        Destroy(gameObject);
    }
}