using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MultiClipboard
{
	class KeyInterceptor
	{
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;

		private IntPtr _hookID = IntPtr.Zero;

		private Form1 _form;

		public KeyInterceptor(Form1 f1)
		{
			_form = f1;
		}

		public IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				_hookID = SetWindowsHookEx(WH_KEYBOARD_LL, proc,
							GetModuleHandle(curModule.ModuleName), 0);
				return _hookID;
			}
		}

		public delegate IntPtr LowLevelKeyboardProc(
			int nCode, IntPtr wParam, IntPtr lParam);

		public IntPtr HookCallback(
			int nCode, IntPtr wParam, IntPtr lParam)
		{
			IntPtr res = CallNextHookEx(_hookID, nCode, wParam, lParam);
			if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
			{
				if (Keys.Control == Control.ModifierKeys)
				{
					// We only care about this key -- or anything! -- if ctrl is pressed.
					int vkCode = Marshal.ReadInt32(lParam);
					processCtrlKey(vkCode);
				}
			}
			return res;
		}

		private void processCtrlKey(int vkCode)
		{
			if ((Keys)vkCode == Keys.D0)
			{
				_form.Switch = 0;
			}
			else if ((Keys)vkCode == Keys.D1)
			{
				_form.Switch = 1;
			}
			else if ((Keys)vkCode == Keys.D2)
			{
				_form.Switch = 2;
			}
			else if ((Keys)vkCode == Keys.D3)
			{
				_form.Switch = 3;
			}
			else if ((Keys)vkCode == Keys.D4)
			{
				_form.Switch = 4;
			}
			else if ((Keys)vkCode == Keys.D5)
			{
				_form.Switch = 5;
			}
			else if ((Keys)vkCode == Keys.D6)
			{
				_form.Switch = 6;
			}
			else if ((Keys)vkCode == Keys.D7)
			{
				_form.Switch = 7;
			}
			else if ((Keys)vkCode == Keys.D8)
			{
				_form.Switch = 8;
			}
			else if ((Keys)vkCode == Keys.D9)
			{
				_form.Switch = 9;
			}
			else if ((Keys)vkCode == Keys.V)
			{
				string text = _form.getClipboardText();
				if(text.Length > 0)
                    try
                    {
                        Clipboard.SetText(text);
                    }
                    catch (Exception e)
                    {
                        // Well, that didn't work.
                    }
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook,
			LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
			IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}
