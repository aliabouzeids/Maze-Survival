using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class Movement_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool up;
    [SerializeField] bool down;
    [SerializeField] bool left;
    [SerializeField] bool right;

    OnScreenButton onScreenButton;

    void Awake()
    {
        onScreenButton = gameObject.GetComponent<OnScreenButton>();
        if (onScreenButton == null)
            onScreenButton = gameObject.AddComponent<OnScreenButton>();

        if (up) onScreenButton.controlPath = "<Keyboard>/w";
        if (down) onScreenButton.controlPath = "<Keyboard>/s";
        if (left) onScreenButton.controlPath = "<Keyboard>/a";
        if (right) onScreenButton.controlPath = "<Keyboard>/d";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onScreenButton.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onScreenButton.OnPointerUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onScreenButton.OnPointerUp(eventData);
    }

    public void OnPointerCancel(PointerEventData eventData)
    {
        onScreenButton.OnPointerUp(eventData);
    }

}