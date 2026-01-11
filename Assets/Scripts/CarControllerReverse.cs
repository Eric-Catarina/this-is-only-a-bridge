using UnityEngine;

public class CarControllerReverse : MonoBehaviour
{
    [Header("Parâmetros de Força")]
    [SerializeField] private float motorTorque = 2000f;
    [SerializeField] private float brakeTorque = 2000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float steeringRange = 30f;
    [SerializeField] private float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] private float centreOfGravityOffset = -1f;

    private WheelController[] wheel;
    private Rigidbody rb;
    private Vector2 currentInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheel = GetComponentsInChildren<WheelController>();
    }

    private void Start()
    {
        Vector3 centerOfMass = rb.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rb.centerOfMass = centerOfMass;
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onPlayerMoveInput += OnMoveInput;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerMoveInput -= OnMoveInput;
    }

    private void OnMoveInput(Vector2 input)
    {
        currentInput = input;
    }

    private void FixedUpdate()
    {
        // Nota: A lógica original inverte os inputs (-Input), mantivemos a inversão aqui
        float verticalInput = -currentInput.y;
        float horizontalInput = -currentInput.x;

        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(forwardSpeed));

        float currentMotorTorque = Mathf.Lerp(motorTorque, 0f, speedFactor);
        float currentSteerAngle = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        foreach (var wheel in wheel)
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
    }
}