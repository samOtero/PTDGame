using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaptureDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public IntVariable PauseState;
    public UnitRuntimeCollection HoveringUnits;
    public Canvas buttonCanvas;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }
    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition = originalPosition;
        var captured = GetCaptureUnit();
        if (captured != null && captured.canCapture()) {
            captured.doCapture();
        }
        PauseState.Value -= 1;
    }

    public Unit GetCaptureUnit() {
        if (HoveringUnits.Items.Count > 0) {
            return HoveringUnits.Items[0];
        }

        return null;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        PauseState.Value += 1;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / buttonCanvas.scaleFactor;
    }
}
