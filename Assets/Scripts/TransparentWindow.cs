using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    
    const int GwlExstyle = -20;
    
    const int WsExLayered = 0x00080000;

    
    static readonly IntPtr HwndTopmost = new IntPtr(-1);
    
    const uint LwaColorKey = 0x00000001;
    
    
    public const int WM_RBUTTONUP = 0x0205;
    public const int WM_COMMAND = 0x0111;
    public const int WM_USER = 0x0400;

    public const uint TPM_RIGHTBUTTON = 0x0002;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if !UNITY_EDITOR
        IntPtr hWnd = Interop.GetActiveWindow();
        
        Interop.MARGINS margins = new Interop.MARGINS { cxLeftWidth = -1 };
        Interop.DwmExtendFrameIntoClientArea(hWnd, ref margins);

        int exStyle = Interop.GetWindowLong(hWnd, GwlExstyle);
        exStyle |= WsExLayered;
        Interop.SetWindowLong(hWnd, GwlExstyle, exStyle);

        Interop.SetLayeredWindowAttributes(hWnd, 0, 0, LwaColorKey);

        Interop.SetWindowPos(hWnd, HwndTopmost, 0, 0, 0, 0, 0);
#endif //!UNITY_EDITOR
    }
}
