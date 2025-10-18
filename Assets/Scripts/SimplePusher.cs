using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePusher : MonoBehaviour
{
    [Header("Configuração do Empurrão")]
    [Tooltip("O intervalo (Mínimo e Máximo) da força a ser aplicada aleatoriamente.")]
    [SerializeField] private Vector2 pushForceRange = new Vector2(0f, 5f);

    [Tooltip("O eixo local do empurrão. A direção será aleatoriamente positiva ou negativa ao longo deste eixo.")]
    [SerializeField] private Vector3 localPushAxis = Vector3.right;

    [Tooltip("O tipo de força a ser aplicada (Force para contínuo, Impulse para súbito).")]
    [SerializeField] private ForceMode forceMode = ForceMode.Force;

    [Header("Ativação")]
    [Tooltip("Se marcado, aplica a força continuamente a cada frame de física.")]
    [SerializeField] private bool pushContinuously = false;
    
    [Tooltip("Se marcado, aplica um único empurrão quando a cena começa.")]
    [SerializeField] private bool pushOnStart = false;

    private Rigidbody rb;
    private int randomSign;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {

        if (pushOnStart)
        {
            ApplyPush();
        }

        randomSign = (Random.value < 0.5f) ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (pushContinuously)
        {
            ApplyPush();
        }
    }
    
    public void ApplyPush()
    {
        if (rb == null) return;
        
        float randomForce = Random.Range(pushForceRange.x, pushForceRange.y);
        
        Vector3 randomizedLocalDirection = localPushAxis.normalized * randomSign;

        Vector3 worldPushDirection = transform.TransformDirection(randomizedLocalDirection);
        
        rb.AddForce(worldPushDirection * randomForce, forceMode);
    }
}   