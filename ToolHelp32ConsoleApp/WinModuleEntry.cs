using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
public struct WinModuleEntry:Toolhelp32.IEntry { // MODULEENTRY32
	[DllImport("kernel32.dll")]
	public static extern bool Module32Next(Toolhelp32.Snapshot snap, ref WinModuleEntry entry);

	public bool TryMoveNext(Toolhelp32.Snapshot snap, out Toolhelp32.IEntry entry) {
		var x = new WinModuleEntry { dwSize=Marshal.SizeOf(typeof(WinModuleEntry)) };
		var b = Module32Next(snap, ref x);
		entry=x;
		return b;
	}

	public int dwSize;
	public int th32ModuleID;
	public int th32ProcessID;
	public int GlblcntUsage;
	public int ProccntUsage;
	public IntPtr modBaseAddr;
	public int modBaseSize;
	public IntPtr hModule;
	//byte moduleName[256];
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string moduleName;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
	public string fileName;
	//byte fileName[260];
	//public const int sizeofModuleName = 256;
	//public const int sizeofFileName = 260;
}
