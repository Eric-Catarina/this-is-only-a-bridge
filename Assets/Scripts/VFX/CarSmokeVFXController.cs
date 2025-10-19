using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class CarSmokeVFXController : MonoBehaviour
{
    public float burstDuration;
    public VisualEffect[] burstSmokes;
    public VisualEffect[] loopSmokes;

    InputSystem_Actions inputActions;
    InputAction moveAction;

    bool isMoving;
    bool onBurst;
    float burstTimer;

    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        moveAction = inputActions.Player.SmokeVFX;

        moveAction.performed += PlayBurst;
        moveAction.canceled += StopAllVFX;

        burstTimer = 0;
        isMoving = false;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();

        moveAction.performed -= PlayBurst;
        moveAction.canceled -= StopAllVFX;
    }

    void Update()
    {
        if (isMoving)
        {
            burstTimer += Time.deltaTime;
        }

        if (onBurst && burstTimer > burstDuration)
        {
            PlayLoop();
        }
    }

    void PlayBurst(InputAction.CallbackContext context)
    {
        foreach (VisualEffect vfx in burstSmokes)
        {
            vfx.Play();
        }

        isMoving = true;
        onBurst = true;
    }

    void PlayLoop()
    {
        onBurst = false;

        foreach (VisualEffect vfx in burstSmokes)
        {
            vfx.Stop();
        }

        foreach (VisualEffect vfx in loopSmokes)
        {
            vfx.Play();
        }
    }

    void StopAllVFX(InputAction.CallbackContext context)
    {
        isMoving = false;
        burstTimer = 0;

        if (onBurst)
        {
            foreach (VisualEffect vfx in burstSmokes)
            {
                vfx.Stop();
            }
        }
        else
        {
            foreach (VisualEffect vfx in loopSmokes)
            {
                vfx.Stop();
            }
        }
    }
}
