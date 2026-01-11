using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystickUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("UI References")]
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    [Header("Settings")]
    [SerializeField] private float deadZone = 0.1f;
    [SerializeField] private float handleRange = 1f;

    private Vector2 inputVector;
    private Vector2 initialPosition;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (joystickBackground != null)
        {
            joystickBackground.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (joystickBackground == null || joystickHandle == null) return;

        joystickBackground.gameObject.SetActive(true);
        
        // Posiciona o centro do joystick onde o dedo tocou
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, 
            eventData.position, 
            canvas.worldCamera, 
            out localPoint
        );

        joystickBackground.anchoredPosition = localPoint;
        joystickHandle.anchoredPosition = Vector2.zero;
        
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickBackground == null || joystickHandle == null) return;

        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, 
            eventData.position, 
            canvas.worldCamera, 
            out position))
        {
            // Calcula tamanho relativo para normalizar input
            Vector2 sizeDelta = joystickBackground.sizeDelta;
            inputVector = new Vector2(
                position.x / sizeDelta.x * 2f, 
                position.y / sizeDelta.y * 2f
            );

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move visualmente o Handle
            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * (sizeDelta.x / 2f) * handleRange, 
                inputVector.y * (sizeDelta.y / 2f) * handleRange
            );

            // Envia evento se passar da deadzone
            Vector2 output = (inputVector.magnitude < deadZone) ? Vector2.zero : inputVector;
            ActionsManager.Instance.onPlayerMoveInput?.Invoke(output);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        
        if (joystickBackground != null)
        {
            joystickBackground.gameObject.SetActive(false);
            joystickHandle.anchoredPosition = Vector2.zero;
        }

        ActionsManager.Instance.onPlayerMoveInput?.Invoke(Vector2.zero);
    }
}