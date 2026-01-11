using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemAdapter : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        ActionsManager.Instance.onPlayerMoveInput?.Invoke(input);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        ActionsManager.Instance.onPlayerMoveInput?.Invoke(Vector2.zero);
    }
}