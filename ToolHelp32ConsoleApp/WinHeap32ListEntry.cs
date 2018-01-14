using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
public struct WinHeap32ListEntry:Toolhelp32.IEntry {
	[DllImport("kernel32.dll")]
	public static extern bool Heap32ListNext(Toolhelp32.Snapshot snap, ref WinHeap32ListEntry entry);

	public bool TryMoveNext(Toolhelp32.Snapshot snap, out Toolhelp32.IEntry entry) {
		var x = new WinHeap32ListEntry { dwSize=(UIntPtr)Marshal.SizeOf(typeof(WinHeap32ListEntry)) };
		var b = Heap32ListNext(snap, ref x);
		entry=x;
		return b;
	}

	public UIntPtr dwSize; // SIZE_T dwSize;
	public uint th32ProcessID;
	public UIntPtr th32HeapID; // ULONG_PTR th32HeapID;
	public uint dwFlags;
}
