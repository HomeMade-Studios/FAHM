using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WindowsInput;

namespace Autobuyer_Fifa_16 {
	public static class Keyboard {

		static InputSimulator input = new InputSimulator();

		public static void Write(string text) {
			Thread.Sleep(200);
			input.Keyboard.TextEntry(text);
		}

		public static void PressEnter() {
			Thread.Sleep(200);
			input.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
		}
	}
}
