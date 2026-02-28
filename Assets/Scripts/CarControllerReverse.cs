using UnityEngine;
using UnityEngine.InputSystem;

public class CarControllerReverse : MonoBehaviour
{
    [Header("Parâmetros de Força")]
    [SerializeField] private float motorTorque = 2000f;
    [SerializeField] private float brakeTorque = 2000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float steeringRange = 30f;
    [SerializeField] private float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] private float centreOfGravityOffset = -1f;
    float verticalInput;
    float horizontalInput;

    private WheelController[] wheels;
    private Rigidbody rb;

    public PlayerInput playerInput;

    [SerializeField] private AudioSource engineAudio;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelController>();
        engineAudio = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Vector3 centerOfMass = rb.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rb.centerOfMass = centerOfMass;
        if (playerInput != null && MenuPause.menuPauseInstancec != null)
        {
            // Remove antes de adicionar para garantir que não haja duplicatas
            playerInput.actions["OnPause"].started -= MenuPause.menuPauseInstancec.OnPause;
            playerInput.actions["OnPause"].started += MenuPause.menuPauseInstancec.OnPause;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVec = context.ReadValue<Vector2>() * -1;
        horizontalInput = inputVec.x;
        verticalInput = inputVec.y;
        Debug.Log("Input recebido: " + inputVec);

        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(forwardSpeed));

        float currentMotorTorque = Mathf.Lerp(motorTorque, 0f, speedFactor);
        float currentSteerAngle = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = horizontalInput * currentSteerAngle;
            }

            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                bool isBraking = (forwardSpeed > 0.1f && verticalInput < -0.1f) || (forwardSpeed < -0.1f && verticalInput > 0.1f);

                if (isBraking)
                {
                    wheel.WheelCollider.motorTorque = 0f;
                    wheel.WheelCollider.brakeTorque = Mathf.Abs(verticalInput) * brakeTorque;
                }
                else
                {
                    if (wheel.motorized)
                    {
                        wheel.WheelCollider.motorTorque = verticalInput * currentMotorTorque;
                    }
                    wheel.WheelCollider.brakeTorque = 0f;
                }
            }
            else
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = 0f;
            }
        }
        bool currentlyAccelerating = false;

        foreach (var wheel in wheels)
        {
            // ... (código que já está aí)

            if (Mathf.Abs(verticalInput) > 0.001f && wheel.motorized)
            {
                currentlyAccelerating = true;
            }
        }

        // Toca ou para o som
        if (currentlyAccelerating && !engineAudio.isPlaying)
        {
            engineAudio.Play();
        }
        else if (!currentlyAccelerating && engineAudio.isPlaying)
        {
            engineAudio.Stop();
        }


    }
    /*public void ChamarPause(InputAction.CallbackContext context)
    {
        // Se o botão acabou de ser apertado
        if (context.started)
        {
            // Usa o seu Singleton para acessar o menu de qualquer lugar!
            if (MenuPause.menuPauseInstancec != null)
            {
                MenuPause.menuPauseInstancec.TogglePause();
            }
        }
    }*/
    private void OnDestroy()
    {
        if (MenuPause.menuPauseInstancec != null && playerInput != null)
        {
            InputAction actionPause = playerInput.actions.FindAction("OnPause");

            if (actionPause != null)
            {
                // Remove o link quando a cena reseta e esse carro morre
                actionPause.started -= MenuPause.menuPauseInstancec.OnPause;
            }
        }
    }
    private void OnDisable()
    {
        // Quando o carro for destruído ou desativado (no Reset), limpamos o evento
        if (playerInput != null && MenuPause.menuPauseInstancec != null)
        {
            playerInput.actions["OnPause"].started -= MenuPause.menuPauseInstancec.OnPause;
        }
    }
}