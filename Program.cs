using System;
using System.Runtime.InteropServices;

using TerraFX.Interop.Windows;
using TerraFX.Interop.WinRT;

using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.SWP;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.WinRT.WinRT;

namespace XamlIslandsNet9
{
    internal class Program
    {
        static private App _xamlApp = null;

        static unsafe void Main(string[] args)
        {
            fixed (char* lpszClassName = "XamlIslandsClass")
            fixed (char* lpWindowName = "Xaml Islands Window") 
            {
                WNDCLASSW wc;
                wc.lpfnWndProc = &WndProc;
                wc.hInstance = GetModuleHandleW(null);
                wc.lpszClassName = lpszClassName;
                RegisterClassW(&wc);

				#region old code
				//CreateWindowExW(WS_EX_NOREDIRECTIONBITMAP, wc.lpszClassName, lpWindowName, WS_OVERLAPPEDWINDOW | WS_VISIBLE, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, HWND.NULL, HMENU.NULL, wc.hInstance, null);
				#endregion

				#region new code

				// Use WS_POPUP for a borderless window, and WS_EX_TOOLWINDOW to hide it from the taskbar
				CreateWindowExW(
					WS_EX_NOREDIRECTIONBITMAP, //| WS_EX_TOOLWINDOW,
					wc.lpszClassName,
					lpWindowName,
					WS_OVERLAPPEDWINDOW | WS_VISIBLE, // REPLACE WS_OVERLAPPEDWINDOW WITH WS_POPUP for borderless
					CW_USEDEFAULT,
					CW_USEDEFAULT,
					CW_USEDEFAULT,
					CW_USEDEFAULT,
					HWND.NULL,
					HMENU.NULL,
					wc.hInstance,
					null
				);

				#endregion






				MSG msg;
                while (GetMessageW(&msg, HWND.NULL, 0, 0))
                {
                    bool xamlSourceProcessedMessage = _xamlApp is not null && _xamlApp.PreTranslateMessage(&msg);
                    if (!xamlSourceProcessedMessage)
                    {
                        TranslateMessage(&msg);
                        DispatchMessageW(&msg);
                    }
                }
            }  
        }
		#region new code
		#endregion
		[UnmanagedCallersOnly]
        private static LRESULT WndProc(HWND hWnd, uint message, WPARAM wParam, LPARAM lParam)
        {
            switch (message)
            {
                case WM_CREATE:
                    OnWindowCreate(hWnd);

                    #region new code
                    unsafe
                    {
						nint style = GetWindowLongPtr(hWnd, GWL_STYLE);
						style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MAXIMIZEBOX | WS_MINIMIZEBOX); // Disable resizing and maximize/minimize
						SetWindowLongPtr(hWnd, GWL_STYLE, style);

						// so window does not appear in taskbar
						// disabled to test window
						/*nint exStyle = GetWindowLongPtr(hWnd, GWL_EXSTYLE);
						exStyle |= WS_EX_TOOLWINDOW; // Ensure it does not appear in the taskbar
						SetWindowLongPtr(hWnd, GWL_EXSTYLE, exStyle);*/

						// Force the window to update its appearance
						// Resize the window to 300x300 and keep it always on top
						SetWindowPos(hWnd, (HWND)HWND_TOPMOST, 0, 0, 300, 300,
									 SWP_NOMOVE | SWP_FRAMECHANGED);
					}
					#endregion
					break;
                case WM_SIZE:
                    _xamlApp?.OnResize(LOWORD(lParam), HIWORD(lParam));

                    break;
                case WM_SETTINGCHANGE:
                case WM_THEMECHANGED:
                    _xamlApp?.ProcessCoreWindowMessage(message, wParam, lParam);

                    break;
                case WM_SETFOCUS:
                    _xamlApp?.OnSetFocus();

                    break;
                case WM_DESTROY:
                    _xamlApp = null;
                    PostQuitMessage(0);
                    break;
                default:
                    return DefWindowProcW(hWnd, message, wParam, lParam);
            }
            return 0;
        }

        private static void OnWindowCreate(HWND hwnd)
        {
            RoInitialize(RO_INIT_TYPE.RO_INIT_SINGLETHREADED);
            _xamlApp = new(hwnd);
        }
    }
}
