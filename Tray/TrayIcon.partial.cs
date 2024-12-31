using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.SWP;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.WinRT.WinRT;
using TerraFX.Interop.Windows;
using Windows.UI.Xaml;

namespace XamlIslandsNet9.Tray
{
	public partial class TrayIcon
	{
		private unsafe HWND CreateWindow(string Icon)
		{
			fixed (char* lpszClassName = "SystemTrayIconWindowClass" + Id.ToString())
			fixed (char* lpWindowName = Id.ToString())
			{
				WNDCLASSEXW wndClass;

				wndClass.cbSize = (uint)sizeof(WNDCLASSEXW);
				wndClass.style = 0;
				wndClass.lpfnWndProc = &TrayIcon.WndProc;
				wndClass.hInstance = GetModuleHandle(null);
				wndClass.hIcon = LoadIcon(Icon);
				wndClass.hCursor = GetCursor();
				wndClass.lpszClassName = lpszClassName;

				RegisterClassEx(&wndClass);
				return CreateWindowEx(0, lpszClassName, lpWindowName, 0, 0, 0, 0, 0, HWND.NULL, HMENU.NULL, wndClass.hInstance, null);
			}
		}

		private unsafe HCURSOR LoadIcon(string Icon)
		{
			fixed (char* iconPath = Path.Combine(AppContext.BaseDirectory, Icon))
			{
				return (HCURSOR)LoadImage(HINSTANCE.NULL, iconPath, 1, 0, 0, 0x00000010 | 0x00000020);
			}
		}

		private unsafe HCURSOR GetCursor()
		{
			fixed (char* cursor = "#32512")
			{
				return LoadCursor(HINSTANCE.NULL, cursor);
			}
		}

		[UnmanagedCallersOnly]
		public static unsafe LRESULT WndProc(HWND hWnd, uint message, WPARAM wParam, LPARAM lParam)
		{
			if (message == 0x8000 + Id)
			{
				if (lParam == WM_LBUTTONDOWN)
				{
					LeftClicked?.Invoke(null, EventArgs.Empty);
				}
				else if (lParam == WM_RBUTTONDOWN)
				{
					RightClicked?.Invoke(null, EventArgs.Empty);
				}
			}

			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
}
