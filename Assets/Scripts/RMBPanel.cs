using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RMBPanel : MonoBehaviour
{
    public RectTransform panel;
    
    public void ShowPanelAtMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        
        Vector2 panelSize = panel.rect.size;
        
        RectTransform canvasRect = panel.parent as RectTransform;
        Vector2 canvasSize = canvasRect.rect.size;
        
        float clampedX = Mathf.Clamp(mousePos.x, 0, canvasSize.x - panelSize.x);
        float clampedY = Mathf.Clamp(mousePos.y, 0, canvasSize.y - panelSize.y);
        panel.anchoredPosition = new Vector2(clampedX, clampedY);
        
        panel.gameObject.SetActive(true);
    }
    
    public void HidePanel()
    {
        panel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HidePanel();
        }
    }
}
