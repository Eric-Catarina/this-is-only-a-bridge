using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePusher : MonoBehaviour
{
    [Header("Configuração do Empurrão")]
    [Tooltip("A magnitude da força a ser aplicada.")]
    [SerializeField] private float pushForce = 50f;

    [Tooltip("A direção do empurrão no espaço local do objeto. (1, 0, 0) para a direita, (-1, 0, 0) para a esquerda.")]
    [SerializeField] private Vector3 localPushDirection = Vector3.right;

    [Tooltip("O tipo de força a ser aplicada (Force para contínuo, Impulse para súbito).")]
    [SerializeField] private ForceMode forceMode = ForceMode.Force;

    [Header("Ativação")]
    [Tooltip("Se marcado, aplica a força continuamente a cada frame de física.")]
    [SerializeField] private bool pushContinuously = false;
    
    [Tooltip("Se marcado, aplica um único empurrão quando a cena começa.")]
    [SerializeField] private bool pushOnStart = false;

    private Rigidbody rb;

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
        
        Vector3 worldPushDirection = transform.TransformDirection(localPushDirection.normalized);
        rb.AddForce(worldPushDirection * pushForce, forceMode);
    }
}