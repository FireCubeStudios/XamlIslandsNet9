using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.WinRT.WinRT;
using TerraFX.Interop.Windows;
using Windows.UI.Xaml.Controls;

namespace XamlIslandsNet9.Tray
{
	public unsafe partial class TrayIcon : IDisposable
	{
		private HWND windowHandle;
		private HICON notifyIconHandle;
		private NOTIFYICONDATAW notifyIconData; 

		public static event EventHandler LeftClicked;
		public static event EventHandler RightClicked;

		private static uint Id; // Used for callback to recieve clicked messages

		public TrayIcon(uint Id, string Icon, string ToolTip)
		{
			unsafe
			{
				TrayIcon.Id = Id;
				windowHandle = CreateWindow(Icon);
				notifyIconHandle = LoadIcon(Icon);
				NOTIFYICONDATAW notifyIconData = new NOTIFYICONDATAW
				{
					cbSize = (uint)sizeof(NOTIFYICONDATAW),
					hWnd = windowHandle,
					uID = Id,
					uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP | NIF_SHOWTIP,
					uCallbackMessage = WM_APP + Id,
					hIcon = notifyIconHandle,
					szTip = GetSzTip(ToolTip)
				};

				// Add the icon
				Shell_NotifyIcon(NIM_ADD, &notifyIconData);
			}
		}

		public unsafe NOTIFYICONDATAW._szTip_e__FixedBuffer GetSzTip(string toolTip)
		{
			// Create a char array of length 128, padding with '\0' if necessary
			char[] szTip = toolTip.PadRight(128, '\0').ToCharArray();

			// Create the fixed buffer and copy the values from the char array
			TerraFX.Interop.Windows.NOTIFYICONDATAW._szTip_e__FixedBuffer result = new TerraFX.Interop.Windows.NOTIFYICONDATAW._szTip_e__FixedBuffer();

			for (int i = 0; i < 128; i++)
				result[i] = szTip[i];

			return result;
		}

		public unsafe void Dispose()
		{
			fixed (NOTIFYICONDATAW* pNotifyIconData = &notifyIconData)
			{
				Shell_NotifyIcon(NIM_DELETE, pNotifyIconData);
				DestroyIcon(notifyIconHandle);
				DestroyWindow(windowHandle);
			}
		}
	}
}
