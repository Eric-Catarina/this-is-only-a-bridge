using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Parâmetros de Força")]
    [SerializeField] private float motorTorque = 2000f;
    [SerializeField] private float brakeTorque = 2000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float steeringRange = 30f;
    [SerializeField] private float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] private float centreOfGravityOffset = -1f;

    [Header("Parâmetros de Atrito")]
    [Tooltip("Força de frenagem aplicada quando não há aceleração, simulando atrito e resistência do ar.")]
    [SerializeField] private float rollingResistance = 100f;

    [Header("Áudio")]
    [SerializeField] private AudioSource engineAudio;

    private WheelController[] wheels;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelController>();
        engineAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Vector3 centerOfMass = rb.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rb.centerOfMass = centerOfMass;
    }

    private void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

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

            if (Mathf.Abs(verticalInput) > 0.001f)
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
                wheel.WheelCollider.brakeTorque = rollingResistance;
            }

        }

        // Detecta se o carro está acelerando
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



        // Add gravity
        rb.AddForce(Physics.gravity * rb.mass);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sky"))
        {
            GameManager.Instance.RestartScene();
        }
    }
}