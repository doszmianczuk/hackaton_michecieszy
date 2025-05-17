using System;
using UnityEngine;

public class JokePanel : MonoBehaviour
{
    public Transform blob;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (blob != null)
        {
            Vector2 blobPos = Camera.main.WorldToScreenPoint(blob.position);
            blobPos += new Vector2(0, 150);
        
            Vector2 panelSize = rectTransform.rect.size;
        
            RectTransform canvasRect = rectTransform.parent as RectTransform;
            Vector2 canvasSize = canvasRect.rect.size;
        
            float clampedX = Mathf.Clamp(blobPos.x, 0, canvasSize.x - panelSize.x);
            float clampedY = Mathf.Clamp(blobPos.y, 0, canvasSize.y - panelSize.y);
            rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}
