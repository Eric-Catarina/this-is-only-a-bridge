using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Referências")]
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    [Header("Configuração")]
    [Tooltip("Distância máxima que o handle pode se mover.")]
    [SerializeField] private float moveRange = 100f;

    private Vector2 startPos;
    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        
        // Esconde o joystick inicialmente
        if (joystickBackground != null)
            joystickBackground.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (joystickBackground == null || joystickHandle == null) return;

        // Ativa e posiciona o joystick onde o jogador tocou
        joystickBackground.gameObject.SetActive(true);
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, 
            eventData.position, 
            parentCanvas.worldCamera, 
            out localPoint
        );

        joystickBackground.anchoredPosition = localPoint;
        joystickHandle.anchoredPosition = Vector2.zero;

        // Dispara evento inicial (zero)
        ActionsManager.Instance.onPlayerMoveInput?.Invoke(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickBackground == null || joystickHandle == null) return;

        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, 
            eventData.position, 
            parentCanvas.worldCamera, 
            out position))
        {
            // Calcula direção e limita pelo moveRange
            Vector2 direction = position;
            direction = Vector2.ClampMagnitude(direction, moveRange);

            joystickHandle.anchoredPosition = direction;

            // Normaliza para enviar valor entre -1 e 1
            Vector2 normalizedInput = direction / moveRange;
            ActionsManager.Instance.onPlayerMoveInput?.Invoke(normalizedInput);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (joystickBackground != null)
        {
            joystickBackground.gameObject.SetActive(false);
            joystickHandle.anchoredPosition = Vector2.zero;
        }

        // Reseta o movimento ao soltar
        ActionsManager.Instance.onPlayerMoveInput?.Invoke(Vector2.zero);
    }
}