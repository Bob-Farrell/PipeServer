using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAScience {

	public class Win32 {

#if !PocketPC && !WindowsCE
		[Flags]
		private enum KeyStates {
			None = 0,
			Down = 1,
			Toggled = 2
			}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		private static KeyStates GetKeyState(Keys key) {
			KeyStates state = KeyStates.None;

			short retVal = GetKeyState((int)key);

			//If the high-order bit is 1, the key is down
			//otherwise, it is up.
			if ((retVal & 0x8000) == 0x8000)
				state |= KeyStates.Down;

			//If the low-order bit is 1, the key is toggled.
			if ((retVal & 1) == 1)
				state |= KeyStates.Toggled;

			return state;
			}

		public static bool IsKeyDown(Keys key) {
			return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
			}

		public static bool IsKeyToggled(Keys key) {
			return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
			}
#endif

		public struct RECT {
			public int left;
			public int top;
			public int right;
			public int bottom;
			}

		public struct NOTIFYICONDATA {
			public int cbSize;
			public IntPtr hWnd;
			public uint uID;
			public uint uFlags;
			public uint uCallbackMessage;
			public IntPtr hIcon;
			}

		public struct COPYDATASTRUCT {
			public IntPtr dwData;
			public Int32 cbData;
			public IntPtr lpData;
			}

		public struct MemoryStatus {
			public uint Length;
			public uint MemoryLoad;
			public uint TotalPhysical;
			public uint AvailablePhysical;
			public uint TotalPageFile;
			public uint AvailablePageFile;
			public uint TotalVirtual;
			public uint AvailableVirtual;
			}

		[DllImport("kernel32.dll")]
		public static extern void GlobalMemoryStatus(out MemoryStatus stat);

		public enum Platform {
			X86,
			X64,
			Unknown
			}

		public const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
		public const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
		public const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
		public const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

		[StructLayout(LayoutKind.Sequential)]
		public struct SYSTEM_INFO {
			public ushort wProcessorArchitecture;
			public ushort wReserved;
			public uint dwPageSize;
			public IntPtr lpMinimumApplicationAddress;
			public IntPtr lpMaximumApplicationAddress;
			public UIntPtr dwActiveProcessorMask;
			public uint dwNumberOfProcessors;
			public uint dwProcessorType;
			public uint dwAllocationGranularity;
			public ushort wProcessorLevel;
			public ushort wProcessorRevision;
			};

		// public const int WM_NOTIFY_TRAY = WM_USER;
		// Property tags from Gdiplusimaging.h
		public const int PropertyTagTypeASCII = 2;
		public const int PropertyTagTypeByte = 1;
		public const int PropertyTagTypeLong = 4;
		public const int PropertyTagTypeRational = 5;
		public const int PropertyTagTypeShort = 3;
		public const int PropertyTagTypeSLONG = 9;
		public const int PropertyTagTypeSRational = 10;
		public const int PropertyTagTypeUndefined = 7;


		public const int NIF_ICON = 0x02; 
		public const int NIF_MESSAGE = 0x01;
		public const int NIF_TIP = 0x04;

		// public const uint WM_COPYDATA = 0x004A;

		// public const int SW_MINIMIZED = 6;

		public const int SW_HIDE = 0;
		public const int SW_SHOWNORMAL = 1;
		public const int SW_NORMAL = 1;
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_SHOWMAXIMIZED = 3;
		public const int SW_MAXIMIZE = 3;
		public const int SW_SHOWNOACTIVATE = 4;
		public const int SW_SHOW = 5;
		public const int SW_MINIMIZE = 6;
		public const int SW_SHOWMINNOACTIVE = 7;
		public const int SW_SHOWNA = 8;
		public const int SW_RESTORE = 9;
		public const int SW_SHOWDEFAULT = 10;
		public const int SW_FORCEMINIMIZE = 11;
		public const int SW_MAX = 11;

		public const int WM_CLOSE = 0x0010;

		public const int WM_ACTIVATE = 0x0006;
		public const int WM_MOUSEMOVE = 0x0200;
		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_RBUTTONDOWN = 0x0204;
		public const int WM_MOUSEHOVER = 0x02A1;
		public const int WM_MOUSELEAVE = 0x02A3;
		public const int WM_MOUSEACTIVATE = 0x0021;

		public const int ODBC_ADD_DSN = 1;	// User DSN
		public const int ODBC_CONFIG_DSN = 2;
		public const int ODBC_REMOVE_DSN = 3;
		public const int ODBC_ADD_SYS_DSN = 4;	// System DSN
		public const int ODBC_CONFIG_SYS_DSN = 5;
		public const int ODBC_REMOVE_SYS_DSN = 6;

		// Scroll messages, e.g. SendMessage(textBox1.Handle,WM_VSCROLL,(IntPtr)SB_PAGEUP,IntPtr.Zero);
		public const int WM_SCROLL = 276; // Horizontal scroll
		public const int WM_VSCROLL = 277; // Vertical scroll
		public const int SB_LINEUP = 0; // Scrolls one line up
		public const int SB_LINELEFT = 0;// Scrolls one cell left
		public const int SB_LINEDOWN = 1; // Scrolls one line down
		public const int SB_LINERIGHT = 1;// Scrolls one cell right
		public const int SB_PAGEUP = 2; // Scrolls one page up
		public const int SB_PAGELEFT = 2;// Scrolls one page left
		public const int SB_PAGEDOWN = 3; // Scrolls one page down
		public const int SB_PAGERIGTH = 3; // Scrolls one page right
		public const int SB_PAGETOP = 6; // Scrolls to the upper left
		public const int SB_LEFT = 6; // Scrolls to the left
		public const int SB_PAGEBOTTOM = 7; // Scrolls to the upper right
		public const int SB_RIGHT = 7; // Scrolls to the right
		public const int SB_ENDSCROLL = 8; // Ends scroll

		public const int HWND_BROADCAST = 0xffff;
		// public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

		// SetWindowPos flags
		public const uint SWP_NOSIZE = 0x0001;
		public const uint SWP_NOMOVE = 0x0002;
		public const uint SWP_NOZORDER = 0x0004;
		public const uint SWP_NOREDRAW = 0x0008;
		public const uint SWP_NOACTIVATE = 0x0010;
		public const uint SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
		public const uint SWP_SHOWWINDOW = 0x0040;
		public const uint SWP_HIDEWINDOW = 0x0080;
		public const uint SWP_NOCOPYBITS = 0x0100;
		public const uint SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
		public const uint SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

		public const uint SWP_DRAWFRAME = 0x0020;
		public const uint SWP_NOREPOSITION = 0x0200;

		public const uint SWP_DEFERERASE = 0x2000;
		public const uint SWP_ASYNCWINDOWPOS = 0x4000;

		public static readonly IntPtr HWND_TOP = new IntPtr(0);
		public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

		public const uint GW_HWNDFIRST = 0;
		public const uint GW_HWNDLAST = 1;
		public const uint GW_HWNDNEXT = 2;
		public const uint GW_HWNDPREV = 3;
		public const uint GW_OWNER = 4;
		public const uint GW_CHILD = 5;
		public const uint GW_ENABLEDPOPUP = 6;

		//static readonly int GWL_WNDPROC = -4;
		//static readonly int GWL_HINSTANCE = -6;
		//static readonly int GWL_HWNDPARENT = -8;
		//static readonly int GWL_STYLE = -16;
		//static readonly int GWL_EXSTYLE = -20;
		//static readonly int GWL_USERDATA = -21;
		//static readonly int GWL_ID = -12;

		public enum WindowLongFlags : int
			{
			GWL_EXSTYLE = -20,
			GWLP_HINSTANCE = -6,
			GWLP_HWNDPARENT = -8,
			GWL_ID = -12,
			GWL_STYLE = -16,
			GWL_USERDATA = -21,
			GWL_WNDPROC = -4,
			DWLP_USER = 0x8,
			DWLP_MSGRESULT = 0x0,
			DWLP_DLGPROC = 0x4
			}

		public const long WS_SYSMENU = 0x00080000L;
		public const long WS_MAXIMIZEBOX = 0x00010000L;
		public const long WS_MINIMIZEBOX = 0x00020000L;
		public const long WS_CAPTION = 0x00C00000L;
		public const long WS_DISABLED = 0x08000000L;

		[Flags]
		public enum WindowStyles : uint
			{
			WS_OVERLAPPED = 0x00000000,
			WS_POPUP = 0x80000000,
			WS_CHILD = 0x40000000,
			WS_MINIMIZE = 0x20000000,
			WS_VISIBLE = 0x10000000,
			WS_DISABLED = 0x08000000,
			WS_CLIPSIBLINGS = 0x04000000,
			WS_CLIPCHILDREN = 0x02000000,
			WS_MAXIMIZE = 0x01000000,
			WS_BORDER = 0x00800000,
			WS_DLGFRAME = 0x00400000,
			WS_VSCROLL = 0x00200000,
			WS_HSCROLL = 0x00100000,
			WS_SYSMENU = 0x00080000,
			WS_THICKFRAME = 0x00040000,
			WS_GROUP = 0x00020000,
			WS_TABSTOP = 0x00010000,

			WS_MINIMIZEBOX = 0x00020000,
			WS_MAXIMIZEBOX = 0x00010000,

			WS_CAPTION = WS_BORDER | WS_DLGFRAME,
			WS_TILED = WS_OVERLAPPED,
			WS_ICONIC = WS_MINIMIZE,
			WS_SIZEBOX = WS_THICKFRAME,
			WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

			WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
			WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
			WS_CHILDWINDOW = WS_CHILD,

			//Extended Window Styles

			WS_EX_DLGMODALFRAME = 0x00000001,
			WS_EX_NOPARENTNOTIFY = 0x00000004,
			WS_EX_TOPMOST = 0x00000008,
			WS_EX_ACCEPTFILES = 0x00000010,
			WS_EX_TRANSPARENT = 0x00000020,

			//#if(WINVER >= 0x0400)

			WS_EX_MDICHILD = 0x00000040,
			WS_EX_TOOLWINDOW = 0x00000080,
			WS_EX_WINDOWEDGE = 0x00000100,
			WS_EX_CLIENTEDGE = 0x00000200,
			WS_EX_CONTEXTHELP = 0x00000400,

			WS_EX_RIGHT = 0x00001000,
			WS_EX_LEFT = 0x00000000,
			WS_EX_RTLREADING = 0x00002000,
			WS_EX_LTRREADING = 0x00000000,
			WS_EX_LEFTSCROLLBAR = 0x00004000,
			WS_EX_RIGHTSCROLLBAR = 0x00000000,

			WS_EX_CONTROLPARENT = 0x00010000,
			WS_EX_STATICEDGE = 0x00020000,
			WS_EX_APPWINDOW = 0x00040000,

			WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
			WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
			//#endif /* WINVER >= 0x0400 */

			//#if(WIN32WINNT >= 0x0500)

			WS_EX_LAYERED = 0x00080000,
			//#endif /* WIN32WINNT >= 0x0500 */

			//#if(WINVER >= 0x0500)

			WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
			WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
										  //#endif /* WINVER >= 0x0500 */

			//#if(WIN32WINNT >= 0x0500)

			WS_EX_COMPOSITED = 0x02000000,
			WS_EX_NOACTIVATE = 0x08000000
			//#endif /* WIN32WINNT >= 0x0500 */

			}

		//// For GetWindowLongPtr
		//public enum GWL
		//	{
		//	GWL_WNDPROC =    (-4),
		//	GWL_HINSTANCE =  (-6),
		//	GWL_HWNDPARENT = (-8),
		//	GWL_STYLE =      (-16),
		//	GWL_EXSTYLE =    (-20),
		//	GWL_USERDATA =   (-21),
		//	GWL_ID =     (-12)
		//	}

		//public const int HWND_TOP        ((HWND)0)
		//public const int HWND_BOTTOM     ((HWND)1)	// use (IntPtr)1
		//public const int HWND_TOPMOST    ((HWND)-1)
		//public const int HWND_NOTOPMOST  ((HWND)-2)

		// An enumerated type for the control messages
		// sent to the handler routine.
		public enum CtrlTypes {
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT,
			CTRL_CLOSE_EVENT,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT
			}

		// A delegate type to be used as the handler routine 
		// for SetConsoleCtrlHandler.
		public delegate bool HandlerRoutine(CtrlTypes CtrlType);

		[Flags]
		public enum SendMessageTimeoutFlags : uint {
			SMTO_NORMAL = 0x0000,
			SMTO_BLOCK = 0x0001,
			SMTO_ABORTIFHUNG = 0x0002,
			SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
			}


	// Flags for playing sounds.  
	// e.g. if reading the sound from a filename, you need to specify SND_FILENAME
	// To play the sound asynchronously specify SND_ASYNC
		[Flags]
		public enum PlaySoundFlags:int {
			SND_SYNC = 0x0000,  // play synchronously (default)
			SND_ASYNC = 0x0001,  // play asynchronously
			SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found
			SND_MEMORY = 0x0004,  // pszSound points to a memory file
			SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound
			SND_NOSTOP = 0x0010,  // don't stop any currently playing sound
			SND_NOWAIT = 0x00002000, // don't wait if the driver is busy
			SND_ALIAS = 0x00010000, // name is a registry alias
			SND_ALIAS_ID = 0x00110000, // alias is a predefined ID
			SND_FILENAME = 0x00020000, // name is file name
			SND_RESOURCE = 0x00040004  // name is resource name or atom
			};

#if PocketPC || WindowsCE

		[DllImport("coredll.dll", EntryPoint="MessageBeep", SetLastError=true)]
		public static extern void MessageBeep(int type);

		// Play a sound
		// e.g. PlaySound("\\Windows\\Alarm1.wav", IntPtr.Zero,SoundFlags.SND_FILENAME);
		[DllImport("coredll.dll", SetLastError = true,CallingConvention=CallingConvention.Winapi)]
		public static extern bool PlaySound(string szSound,IntPtr hMod,PlaySoundFlags sf);

		[DllImport("coredll.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(String className, String windowName);
		[DllImport("coredll.dll", SetLastError = true)]
		public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

		public static void HideWindow(IntPtr hWnd) {
			// Call like HideWindow(form.Handle);
			ShowWindow(hWnd, SW_MINIMIZE);
			}
		//[DllImport("coredll.dll")]
		//public static extern int ValidateRect(int hwnd, ref RECT lpRect);

		[DllImport("coredll.dll")]
		public static extern int ValidateRect(IntPtr hwnd, IntPtr nullPointer);

		// Registry?
		[DllImport("coredll.dll")]
		public static extern UInt32 GlobalAddAtom( String lpString );
		[DllImport("coredll.dll")]
		public static extern UInt32 GlobalDeleteAtom( UInt32 nAtom );

		[DllImport("coredll.dll")]
		internal static extern int Shell_NotifyIcon(int dwMessage, ref NOTIFYICONDATA pnid);

		//[DllImport("coredll.dll")]
		//protected static extern short GetAsyncKeyState

		//// Power status
		//[DllImport("coredll")]
		//public static extern uint GetSystemPowerStatusEx

		//[DllImport("coredll")]
		//public static extern uint GetSystemPowerStatusEx2

		//// System Reset

		//[DllImport("Coredll.dll")]
		//public extern static uint KernelIoControl

#else
		[DllImport("kernel32.dll")]
		internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32.dll")]
		internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

		public static Boolean Is64BitOS() {
			//bool is64BitProcess = (IntPtr.Size == 8);
			return (IntPtr.Size == 8) || IsWow64();
			}

		public static Boolean Is32BitProcessOn64BitOS() {
			//bool is64BitProcess = (IntPtr.Size == 8);
			return IsWow64();
			}

		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(
			[In] IntPtr hProcess,
			[Out] out bool wow64Process
		);

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsWow64() {
			//if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
			//    Environment.OSVersion.Version.Major >= 6) {
			if (Environment.OSVersion.Version >= new Version(5, 1, 2600, 0)) {
				using (Process p = Process.GetCurrentProcess()) {
					bool retVal;
					return IsWow64Process(p.Handle, out retVal) ? retVal : false;
					}
				}
			else {
				return false;
				}
			}

		public static SYSTEM_INFO GetSystemInfo() {
			SYSTEM_INFO sysInfo = new SYSTEM_INFO();

			if (Environment.OSVersion.Version >= new Version(5, 1, 2600, 0)) {
				//if (System.Environment.OSVersion.Version.Major > 5 ||
				//    (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1)) {
				GetNativeSystemInfo(ref sysInfo);
				}
			else {
				GetSystemInfo(ref sysInfo);
				}

			return sysInfo;
			}
		public static Platform GetPlatform() {
			SYSTEM_INFO sysInfo = new SYSTEM_INFO();

			if( Environment.OSVersion.Version >= new Version(5, 1, 2600, 0) ) {
			//if (System.Environment.OSVersion.Version.Major > 5 ||
			//    (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1)) {
				GetNativeSystemInfo(ref sysInfo);
				}
			else {
				GetSystemInfo(ref sysInfo);
				}

			switch (sysInfo.wProcessorArchitecture) {
				case PROCESSOR_ARCHITECTURE_IA64:
				case PROCESSOR_ARCHITECTURE_AMD64:
					return Platform.X64;

				case PROCESSOR_ARCHITECTURE_INTEL:
					return Platform.X86;

				default:
					return Platform.Unknown;
				}
			}
		
		[DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        public static extern bool PlaySound(string szSound, IntPtr hMod, PlaySoundFlags flags);

		//[DllImport("user32", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
		public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam) {
			return SendMessage(hWnd, (UInt32)wMsg, wParam, lParam);
			}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

		[DllImport("user32")]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam,
			SendMessageTimeoutFlags fuFlags, uint uTimeout, out IntPtr lpdwResult);

		//[DllImport("user32")]
		public static uint RegisterWindowMessageA(string message) {
			return RegisterWindowMessage(message);
			}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint RegisterWindowMessage(string lpString);

		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr FindWindow(string className, string windowName);

		[DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent,IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		//[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		//public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);

		[DllImport("user32.dll")]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetWindow(IntPtr hwnd, uint wCmd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetNextWindow(IntPtr hWnd, uint wCmd);

		//[DllImport("user32", SetLastError = true)]
		//public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		public static bool SetForegroundWindowSafe(IntPtr hWnd) {
			return SetForegroundWindow(new HandleRef(new object(), hWnd));
			}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(HandleRef hWnd);

		[DllImport("user32")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32")]
		public static extern int ShowWindowAsync(IntPtr hWnd, int swCommand);

		[DllImport("user32.dll")]
		public static extern int ShowWindow(IntPtr hWnd, int swCommand);

		//[DllImport("user32")]
		//public static extern int SetWindowPos(IntPtr hwnd,IntPtr hWndInsertAfter,int x,int y,int cx,int cy,uint wFlags);

		public static bool SetWindowPosSafe(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags) {
			return SetWindowPos(new HandleRef(new object(), hWnd), new HandleRef(new object(), hWndInsertAfter),x,y,cx,cy,flags);
			}

		[DllImport("user32")]
		public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter,int x, int y, int cx, int cy, uint flags);

		[DllImport("user32")]
		public static extern bool SetWindowPos(IntPtr hWnd,IntPtr hWndInsertAfter,int x, int y, int cx, int cy, uint flags);

		[DllImport("user32",SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd,out RECT rect); 

		[DllImport("user32")]
		public static extern bool MoveWindow(IntPtr hwnd,int x,int y,int width,int height,bool repaint);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetWindowTextLength(IntPtr hWnd);


		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		// This static method is required because Win32 does not support
		// GetWindowLongPtr directly
		public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
			{
			if (IntPtr.Size == 8)
				return GetWindowLongPtr64(hWnd, nIndex);
			else
				return GetWindowLongPtr32(hWnd, nIndex);
			}

		////[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		////private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
		//public static IntPtr GetWindowLongPtr(HandleRef hWnd, int nIndex)
		//	{
		//	if (IntPtr.Size == 8)
		//		return GetWindowLongPtr64(hWnd, nIndex);
		//	else
		//		return GetWindowLongPtr32(hWnd, nIndex);
		//	}

		//[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		//private static extern IntPtr GetWindowLongPtr32(HandleRef hWnd, int nIndex);

		//[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		//private static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

		// This static method is required because legacy OSes do not support
		// SetWindowLongPtr
		public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
			{
			if (IntPtr.Size == 8)
				return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			else
				return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
			}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32")]
		public static extern int ValidateRect(IntPtr hwnd, IntPtr nullPointer);

		// Declare the SetConsoleCtrlHandler function as external and receiving a delegate.
		[DllImport("Kernel32")]
		public static extern IntPtr GetConsoleWindow();

		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler,bool Add);


		[DllImport("shell32",EntryPoint = "FindExecutable")]
		public static extern long FindExecutableA(
		  string lpFile,string lpDirectory,StringBuilder lpResult);


		//[DllImport("user32.dll")]
		//public static extern int ValidateRect(int hwnd, ref RECT lpRect);

		// public bool AddUserDSN(string DSName, string DBPath)
		//{
		//  return SQLConfigDataSource((IntPtr)0, 1, 
		//     "Microsoft Access Driver (*.MDB)\0",
		//     "DSN=" + DSName + "\0Uid=Admin\0pwd=\0DBQ=" + DBPath + "\0");
		//}
		[DllImport("ODBCCP32.DLL", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, ExactSpelling = true)]
		static extern bool SQLConfigDataSource(IntPtr hwndParent, int fRequest, string lpszDriver, string lpszAttributes);

		private static string FileDSNAttributes(string DSN, string DBPath, string description) {
			return "DSN=" + DSN + "\0DBQ=" + DBPath + "\0Description=" + description + "\0";
			}
		private static string Access2007DriverString() {
			return "Microsoft Access Driver (*.mdb, *.accdb)\0";
			}
		private static string AccessDriverString() {
			return "Microsoft Access Driver (*.mdb)\0";
			}
		public static bool AddAccdbFileDSN(string DSN, string DBPath, string description, bool systemDSN) {
			string errMsg;
			return AddAccdbFileDSN(DSN, DBPath, description, systemDSN, out errMsg);
			}
		public static bool AddAccdbFileDSN(string DSN, string DBPath, string description, bool systemDSN, out string errMsg) {
			errMsg = "";
			bool ok = false;

			try {
				ok = SQLConfigDataSource(IntPtr.Zero, systemDSN ? ODBC_ADD_SYS_DSN : ODBC_ADD_DSN,
					Access2007DriverString(), FileDSNAttributes(DSN, DBPath, description));
				}
			catch(Exception ex) {
				errMsg = ex.Message;
				}

			if (!ok && (string.Compare(Path.GetExtension(DBPath), ".mdb", true) == 0)) {
				try {
					ok = SQLConfigDataSource(IntPtr.Zero, systemDSN ? ODBC_ADD_SYS_DSN : ODBC_ADD_DSN,
						AccessDriverString(), FileDSNAttributes(DSN, DBPath, description));
					}
				catch(Exception ex) {
					errMsg = ex.Message;
					}
				}

			return ok;
			//String attributes = "DSN=" + DSN + "\0DBQ=" + DBPath + "\0Description=" + description + "\0";
			//return SQLConfigDataSource(IntPtr.Zero, systemDSN ? ODBC_ADD_SYS_DSN : ODBC_ADD_DSN,
			// "Microsoft Access Driver (*.mdb, *.accdb)\0", attributes);

			//// "DSN=" + DSName + "\0Uid=Admin\0pwd=\0DBQ=" + DBPath + "\0");
			}
		public static bool RemoveAccdbFileDSN(string DSN, string DBPath, string description, bool systemDSN) {
			bool ok = false;

			try {
				ok = SQLConfigDataSource(IntPtr.Zero, systemDSN ? ODBC_REMOVE_SYS_DSN : ODBC_REMOVE_DSN,
					Access2007DriverString(), FileDSNAttributes(DSN, DBPath, description));
				}
			catch {
				}

			if (!ok && (string.Compare(Path.GetExtension(DBPath), ".mdb", true) == 0)) {
				try {
					ok = SQLConfigDataSource(IntPtr.Zero, systemDSN ? ODBC_ADD_SYS_DSN : ODBC_ADD_DSN,
						AccessDriverString(), FileDSNAttributes(DSN, DBPath, description));
					}
				catch {
					}
				}

			return ok;
			}

		public static string FindExecutable(string filename) {
			// The file must exist
			if (!File.Exists(filename))
				return "";

			StringBuilder objResultBuffer = new StringBuilder(1024);
			long result = FindExecutableA(filename, string.Empty, objResultBuffer);

			if (result >= 32)
				return objResultBuffer.ToString();
			else
				return "";
			}

		public static void PostMessageToConsoleWindow(uint msg,IntPtr wparam,IntPtr lparam) {
			try {
				IntPtr hWnd = GetConsoleWindow();
				if (hWnd == IntPtr.Zero)
					return;

				PostMessage(hWnd,msg,wparam,lparam);
				}
			catch {
				}
			}

		public static void MoveConsoleWindow(int x,int y) {
			try {
				IntPtr hWnd = GetConsoleWindow();
				if (hWnd == IntPtr.Zero)
					return;

				RECT r;
				if (!GetWindowRect(hWnd,out r))
					return;

				MoveWindow(hWnd,x,y,r.right - r.left,r.bottom - r.top,true);
				}
			catch {
				}
			}

		public static bool GetConsoleWindowPos(out int x,out int y,out int width,out int height) {
			x = 0;
			y = 0;
			width = 0;
			height = 0;

			try {
				IntPtr hWnd = GetConsoleWindow();
				if (hWnd == IntPtr.Zero)
					return false;

				RECT r;
				if (!GetWindowRect(hWnd,out r))
					return false;
				if (y <= -32000)
					return false;

				x = r.left;
				y = r.top;
				width = r.right - r.left;
				height = r.bottom - r.top;

				return true;
				}
			catch {
				return false;
				}
			}

#endif
		}
	}
