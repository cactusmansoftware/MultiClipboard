using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MultiClipboard
{
	static class Program
	{

		private static IntPtr _hookID = IntPtr.Zero;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            // Start off with form invisible.  NotifyIcon will be present.
			Form1 f1 = new Form1();
            f1.Visible = false;
			KeyInterceptor interceptor = new KeyInterceptor(f1);

			_hookID = interceptor.SetHook(interceptor.HookCallback);
			Application.Run();
			UnhookWindowsHookEx(_hookID);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);
	}
}
