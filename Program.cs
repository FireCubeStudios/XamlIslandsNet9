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
using Windows.UI.Xaml;
using System.Collections.Generic;
using System.Diagnostics;
using XamlIslandsNet9.Controls;
using XamlIslandsNet9.Pages;
using XamlIslandsNet9.Tray;

namespace XamlIslandsNet9
{
    internal class Program
    {
        public static App _xamlApp = null;
        public static Dictionary<HWND, XamlIslandWindow> Windows = new();
        private static XamlIslandWindow Window = null;

		static unsafe void Main(string[] args)
		{
			_xamlApp = new();

			Window = new XamlIslandWindow();
			Window.WindowFrame.Navigate(typeof(MainPage));

			var w2 = new XamlIslandWindow();
			w2.WindowFrame.Navigate(typeof(MainPage));

			var w3 = new XamlIslandWindow();
			w3.WindowFrame.Navigate(typeof(MainPage));

			var tray = new TrayIcon(1, "", "Xaml Islands Battery");

            TrayIcon.LeftClicked += (sender, e) =>
			{
              /*  if(_xamlApp is null)
				    _xamlApp = new();
				Window = new XamlIslandWindow();
                Window.WindowFrame.Navigate(typeof(MainPage));*/
            };

			MSG msg;
			while (GetMessageW(&msg, HWND.NULL, 0, 0))
			{
				//bool xamlSourceProcessedMessage = _xamlApp is not null && _xamlApp.PreTranslateMessage(&msg);
				bool xamlSourceProcessedMessage = Window is not null && Window.xamlInit && Window.PreTranslateMessage(&msg);
				if (!xamlSourceProcessedMessage)
				{
					TranslateMessage(&msg);
					DispatchMessageW(&msg);
				}

				/*if (Windows.TryGetValue(msg.hwnd, out var windowX))
				{
					bool xamlSourceProcessedMessage = windowX.xamlInit && windowX.PreTranslateMessage(&msg);
                    if (!xamlSourceProcessedMessage)
                    {
                        TranslateMessage(&msg);
                        DispatchMessageW(&msg);
                    }
				}*/
			 }
		}

		[UnmanagedCallersOnly]
        public static LRESULT WndProc(HWND hWnd, uint message, WPARAM wParam, LPARAM lParam)
        {
            XamlIslandWindow WindowX;
            Windows.TryGetValue(hWnd, out WindowX);
            switch (message)
            {
                case WM_CREATE:
                   // Window.OnWindowCreate(hWnd);
                   // this is now done in XamlIslandWindow
                    break;
                case WM_SIZE:
					WindowX?.OnResize(LOWORD(lParam), HIWORD(lParam));

                    break;
                case WM_SETTINGCHANGE:
                case WM_THEMECHANGED:
                    WindowX?.ProcessCoreWindowMessage(message, wParam, lParam);

                    break;
                case WM_SETFOCUS:
                    WindowX?.OnSetFocus();

                    break;
                case WM_DESTROY:
                    WindowX.Dispose();
                    WindowX = null;
                    if(Windows.Count == 0)
					{
						PostQuitMessage(0);
					}
                    break;
                default:
                    return DefWindowProcW(hWnd, message, wParam, lParam);
            }
            return 0;
        }
    }
}
