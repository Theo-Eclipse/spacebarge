using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float returnSpeed = 500.0f;
    public Vector2 GetInput => joystickMagnitude > deadZone && dragging ? handle.anchoredPosition.normalized : Vector2.zero;
    [SerializeField] private RectTransform handle;
    [SerializeField] private CanvasGroup joystickColor;
    private RectTransform joystickBase;
    private bool dragging = false;
    private float radius => Mathf.Min(joystickBase.rect.width, joystickBase.rect.height)*0.5f;
    private float deadZone => radius * 0.05f;
    private float joystickMagnitude => handle.anchoredPosition.magnitude;

    void Start() 
    {
        joystickBase = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (!dragging)
            handle.anchoredPosition = Vector2.MoveTowards(handle.anchoredPosition, Vector2.zero, returnSpeed * Time.deltaTime);
        UpdateUiTransparency();
    }
    void UpdateUiTransparency()
    {
        joystickColor.alpha = Mathf.Clamp(joystickColor.alpha + Time.deltaTime * (dragging ? 2.0f : -1.5f), 0.3f, 0.85f);
    }
    public void OnDrag(PointerEventData data)
    {
        dragging = data.dragging;
        if (data.dragging)
            handle.anchoredPosition += data.delta;
        if (joystickMagnitude > radius)
            handle.anchoredPosition = handle.anchoredPosition.normalized * radius;
    }
    public void OnEndDrag(PointerEventData data)
    {
        dragging = false;
    }
}
