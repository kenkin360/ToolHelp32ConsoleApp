using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CustomFormatter:IFormatProvider, ICustomFormatter {
	String ICustomFormatter.Format(String format, object arg, IFormatProvider formatProvider) {
		var type = arg.GetType();
		var fields = type.GetFields();
		var q = fields.Select(x => String.Format("{0}:{1}", x.Name, x.GetValue(arg)));
		return String.Format("{{{0}}}", String.Join(", ", q.ToArray()));
	}

	object IFormatProvider.GetFormat(Type formatType) {
		return typeof(ICustomFormatter)!=formatType ? null : this;
	}
}

class Program {
	static void EnumerateProcessesTwice() {
		var provider = new CustomFormatter { };

		var entries = Toolhelp32.TakeSnapshot<WinProcessEntry>(Toolhelp32.SnapProcess, 0);

		foreach(var x in entries) {
			Debug.WriteLine(String.Format(provider, "1 .. {0}", x));
		}

		// Snapshot disposed and created again
		foreach(var x in entries) {
			Debug.WriteLine(String.Format(provider, "2 .. {0}", x));
		}
	}

	static void EnumerateThreads() {
		var provider = new CustomFormatter { };

		foreach(var x in Toolhelp32.TakeSnapshot<WinThreadEntry>(Toolhelp32.SnapThread, 0)) {
			Debug.WriteLine(String.Format(provider, ".. {0}", x));
		}
	}

	static void EnumerateModules() {
		var provider = new CustomFormatter { };

		foreach(var x in Toolhelp32.TakeSnapshot<WinModuleEntry>(Toolhelp32.SnapModule, 0)) {
			Debug.WriteLine(String.Format(provider, ".. {0}", x));
		}
	}

	static void EnumerateHeap32List() {
		var provider = new CustomFormatter { };
		var id = Process.GetCurrentProcess().Id;

		foreach(var x in Toolhelp32.TakeSnapshot<WinHeap32ListEntry>(Toolhelp32.SnapHeapList, id)) {
			Debug.WriteLine(String.Format(provider, ".. {0}", x));
		}
	}

	static void WrapWrite(String title, Action testMethod) {
		Debug.WriteLine(String.Format("--- begin {0} --- ", title));
		testMethod();
		Debug.WriteLine(String.Format("--- end {0} --- ", title));
		Debugger.Break();
	}

	static void Main(string[] args) {
		Console.WriteLine("Note the output is in the debug output window");

		WrapWrite("processes", EnumerateProcessesTwice);
		WrapWrite("threads", EnumerateThreads);
		WrapWrite("modules", EnumerateModules);
		WrapWrite("heap32lists", EnumerateHeap32List);
	}
}
