using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct WinThreadEntry:Toolhelp32.IEntry {
	[DllImport("kernel32.dll")]
	public static extern bool Thread32Next(Toolhelp32.Snapshot snap, ref WinThreadEntry entry);

	public bool TryMoveNext(Toolhelp32.Snapshot snap, out Toolhelp32.IEntry entry) {
		var x = new WinThreadEntry { dwSize=Marshal.SizeOf(typeof(WinThreadEntry)) };
		var b = Thread32Next(snap, ref x);
		entry=x;
		return b;
	}

	public int dwSize;
	public int cntUsage;
	public int th32ThreadID;
	public int th32OwnerProcessID;
	public int tpBasePri;
	public int tpDeltaPri;
	public int dwFlags;
}
