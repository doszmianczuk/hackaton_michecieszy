using System;
using UnityEngine;
using UnityEngine.Events;

public class RightClickDetection : MonoBehaviour
{
    
    public UnityEvent onRightClick;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            onRightClick?.Invoke();
        }
    }
}
