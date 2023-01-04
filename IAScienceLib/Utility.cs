using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

#if (!WebApp)
	using System.Drawing;
#endif

#if (!WebApp && !ConsoleApp)
	using System.Windows.Forms;
#endif

namespace IAScience {
	#pragma warning disable CS8600
	#pragma warning disable CS8602
	#pragma warning disable CS8603
	#pragma warning disable CS8625
public class Utility {
		/// <summary>
		/// <para>Set true to only allow one object to be drawn</para>
		/// </summary>
		public static Version GetVersion() {
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			}

		public static double Root2 = 1.41421356237;

		private static Char[] m_TabCharArray = new Char[] { '\t' };
		private static Char[] m_SpaceCharArray = new Char[] { ' ' };
		private static Char[] m_ParseConfigLineChars = new Char[] { ' ', ';', '\t', '\n', };
		private static String[] m_FileSizeStrings = new String[] { "Bytes", "KB", "MB", "GB", "TB", "PB" };
		private static String[] m_ExcelFileExtensions = new String[] { ".xls", ".xlsx" };
		//public Utility()
		//    {
		//    }

		public static bool IsWindows() {
			return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
			//return System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString().ToLower().Contains("windows");
			}
		public static bool IsLinux() {
			return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
			//return System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString().ToLower().Contains("windows");
			}
		public static string OSName() {
			string os = System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString().ToLower();
			if (os.Contains("windows"))
				return "windows";
			else if (os.Contains("unix"))
				return "unix";
			else
				return "unknown";
			}

		public static void Trace(String s) {
#if (!PocketPC && !WindowsCE)
			System.Diagnostics.Trace.WriteLine(s);
#endif
			}
		public static void Trace(int n) {
#if (!PocketPC && !WindowsCE)
			System.Diagnostics.Trace.WriteLine(n.ToString());
#endif
			}

#if (!WebApp)
		public static void Trace(Rectangle r) {
#if (!PocketPC && !WindowsCE)
			System.Diagnostics.Trace.WriteLine("(" + r.X.ToString() + "," + r.Y.ToString() + ") ," + r.Width.ToString() + " x " + r.Height.ToString());
#endif
			}
#endif

#if (!WebApp)
		public static void Trace(Point p) {
#if (!PocketPC && !WindowsCE)
			System.Diagnostics.Trace.WriteLine("(" + p.X.ToString() + "," + p.Y.ToString() + ")");
#endif
			}
#endif

#if (!WebApp)
		public static void Trace(Size s) {
#if (!PocketPC && !WindowsCE)
			System.Diagnostics.Trace.WriteLine("(" + s.Width.ToString() + " x " + s.Height.ToString() + ")");
#endif
			}
#endif
		public static bool IsExcelExt(String ext) {
			ext = ext.ToLower();
			return Array.IndexOf(m_ExcelFileExtensions, ext) >= 0;
			}
		public static bool IsExcelFile(String filename) {
			return IsExcelExt(Path.GetExtension(filename));
			}
		public static bool IsCSVFile(String filename) {
			return Path.GetExtension(filename).ToLower() == ".csv";
			}
		public static bool IsTextFile(String filename) {
			return Path.GetExtension(filename).ToLower() == ".txt";
			}

#if (PocketPC || WindowsCE)
		public static void GetNearestScreenTopLeft(Rectangle windowRect, out int x, out int y) {
			x = 0;
			y = 0;
			}
		public static bool PointVisible(int x, int y) {
			Size s = Screen.PrimaryScreen.WorkingArea.Size;
			return (x >= 0) && (y >= 0) && (x < s.Width) && (y < s.Height);
			}
#endif

#if (!WebApp && !ConsoleApp && !PocketPC && !WindowsCE)
		// Drag/drop utility functions -------------------------------------------------
		public static String GetExcelFile(DragEventArgs e) {
			String[] files = GetExcelFiles(e);
			return files.Length > 0 ? files[0] : "";
			}
		public static bool ExcelFilesPresent(DragEventArgs e) {
			return ExcelFilesPresent(e, false);
			}
		public static bool ExcelFilesPresent(DragEventArgs e,bool firstFileOnly) {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop))
				return false;

			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			if (files == null)
				return false;

			if (firstFileOnly)
				return IsExcelFile(files[0]);

			foreach (String filename in files) {
				if (IsExcelFile(filename))
					return true;
				}
			return false;
			}
		public static String[] GetExcelFiles(DragEventArgs e) {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop))
				return new String[0];

			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			if (files == null)
				return new String[0];

			ArrayList filesList = new ArrayList();
			foreach (String filename in files) {
				if (IsExcelExt(Path.GetExtension(filename)))
					filesList.Add(filename);
				}
			
			return (String[])filesList.ToArray(typeof(String));			
			}
		// ------------------------------------------------------------------------------

		// Screen functions
		public static Rectangle CentreOnMainScreen(int width, int height)
			{	// Return a rectangle of the given size centred on the main screen
			Rectangle r = Screen.PrimaryScreen.Bounds;
			if (r.Width > width)
				r.Inflate((width - r.Width)/2,0);
			if (r.Height > height)
				r.Inflate(0,(height - r.Height) / 2);

			return r;
			}
		public static bool PointVisible(int x, int y) {
			Screen[] screens = Screen.AllScreens;
			for (int index = 0; index < screens.Length; index++) {
				if (screens[index].Bounds.Contains(x,y))
					return true;
				}
			return false;
			}
		public static bool TopLeftVisible(Rectangle windowRect) {
			return PointVisible(windowRect.Left, windowRect.Top);
			}
		public static void GetNearestScreenTopLeft(Rectangle windowRect, out int x, out int y) {
			x = 0;
			y = 0;

			int dSqMin = int.MaxValue;
			int minIndex = -1;

			Screen[] screens = Screen.AllScreens;

			for (int index = 0; index < screens.Length; index++) {
				int dx = windowRect.Left - screens[index].Bounds.Left;
				int dy = windowRect.Top - screens[index].Bounds.Top;
				int dSq = dx * dx + dy * dy;
				if (dSq < dSqMin) {
					dSqMin = dSq;
					minIndex = index;
					}
				}
			if (minIndex >= 0) {
				x = screens[minIndex].Bounds.Left;
				y = screens[minIndex].Bounds.Top;
				}
			}
		public static bool TitleBarVisible(Rectangle windowRect)
			{
			// titleBarCentre is the middle bit of the titlebar, i.e. where you can reasonably click on & drag
			// We see whether this rectangle intersects with any screen rectangles
			int titleBarHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
			Rectangle titleBarCentre = new Rectangle(windowRect.Left+25,windowRect.Top+5,windowRect.Width-105,titleBarHeight - 10);

			// Gets an array of all the screens connected to the system.
			Screen[] screens = Screen.AllScreens;
			for (int index = 0; index < screens.Length; index++) {
				if (screens[index].Bounds.IntersectsWith(titleBarCentre))
					return true;
				}
			return false;
			}

		static public Form FormFromHandle(IntPtr handle) {
			return handle == IntPtr.Zero ? null : (Form)Control.FromHandle(handle);
			}
#endif
		public static bool IsPrime(int n) {
			if (n % 2 == 0)
				return false;

			int maxDivisor = (int)(Math.Sqrt(n));
			for (int divisor = 3; divisor <= maxDivisor; divisor += 2) {
				//int x = n / divisor;
				//if (x * divisor == n)
				//    return false;
				if (n % divisor == 0)
				    return false;
				}
			return true;
			}
		public static bool IsBobsPC() {
			return Directory.Exists("F:\\Images\\Camera\\Isla") || Directory.Exists("C:\\temp\\BobsPC");
			}
		//static public Form MainApplicationForm() {
		//    return FormFromHandle(Process.GetCurrentProcess().MainWindowHandle);
		//    }
		public static String ApplicationPath() {	// Application folder, works for Windows & mobile apps
			//String appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			String appPath = AppDomain.CurrentDomain.BaseDirectory;
			// The return value starts with "file:\" if it's a local file in Windows
			if ((appPath.Length > 6) && (appPath.Substring(0, 6).ToLower() == "file:\\"))
				appPath = appPath.Substring(6);

			return appPath;
			}
		public static string VideoTimeString(long totalMilliseconds, bool includeZeroHours = false, bool includeMilliseconds = false)
			{
			long milliseconds = totalMilliseconds % 1000;
			long totalSeconds = (long)(totalMilliseconds / 1000);
			long seconds = totalSeconds % 60;
			long totalMinutes = (long)(totalSeconds / 60);
			long minutes = totalMinutes % 60;
			long hours = (long)(totalMinutes / 60);

			return (hours == 0 ? "" : (hours.ToString() + ":")) + minutes.ToString("00") + ":" + seconds.ToString("00") + (includeMilliseconds ? ("." + milliseconds.ToString("000")) : "");
			}

		public static bool StringContainsAllOf(String s, String[] strings, bool ignoreCase) {
			if (ignoreCase)
				s = s.ToLower();

			for (int n = 0; n < strings.Length; n++) {
				if (ignoreCase) {
					if (s.IndexOf(strings[n].ToLower()) < 0)
						return false;
					}
				else if (s.IndexOf(strings[n]) < 0)
					return false;
				}
			return true;
			}
		public static bool AllNumeric(String[] s) {
			for (int n = 0; n < s.Length; n++)
				if (!Utility.IsRealString(s[n]))
					return false;
			return true;
			}
		public static bool AllNonNumeric(String[] s) {
			for (int n = 0; n < s.Length; n++)
				if (Utility.IsRealString(s[n]))
					return false;
			return true;
			}
#if (!PocketPC && !WindowsCE)
		public static String RemoveDoubleSpaces(String s) {
			s = s.Trim();
			if (s == "")
				return "";
			String[] a = s.Split(m_SpaceCharArray, StringSplitOptions.RemoveEmptyEntries);
			return String.Join(" ", a);
			}
#endif

#if NoUnsafe
		public static void CopyBytes(byte[] src, int srcIndex, byte[] dst, int dstIndex, int count) {
			try {
				for (int n = 0; n < count; n++)
					dst[dstIndex++] = src[srcIndex++];
				}
			catch {
				}
			}
#else
		public static unsafe void CopyBytes(byte[] src, int srcIndex, byte[] dst, int dstIndex, int count) {
			if (src == null || srcIndex < 0 ||
				dst == null || dstIndex < 0 || count < 0) {
				throw new System.ArgumentException();
				}

			int srcLen = src.Length;
			int dstLen = dst.Length;
			if (srcLen - srcIndex < count || dstLen - dstIndex < count) {
				throw new System.ArgumentException();
				}

			// The following fixed statement pins the location of the src and dst objects
			// in memory so that they will not be moved by garbage collection.
			fixed (byte* pSrc = src, pDst = dst) {
				byte* ps = pSrc + srcIndex;
				byte* pd = pDst + dstIndex;

				// Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
				for (int i = 0; i < count / 4; i++) {
					*((int*)pd) = *((int*)ps);
					pd += 4;
					ps += 4;
					}

				// Complete the copy by moving any bytes that weren't moved in blocks of 4:
				for (int i = 0; i < count % 4; i++) {
					*pd = *ps;
					pd++;
					ps++;
					}
				}
			}
#endif
		public static int ClampValue(int value, int minValue, int maxValue) {
			if (value < minValue)
				return minValue;
			else if (value > maxValue)
				return maxValue;
			else
				return value;
			}
		public static double ClampValue(double value, double minValue, double maxValue) {
			if (value < minValue)
				return minValue;
			else if (value > maxValue)
				return maxValue;
			else
				return value;
			}
		public static int IndexOfMax(int[] intArray) {
			if (intArray.Length == 0)
				return -1;

			int indexOfMax = 0;
			for (int n = 1; n < intArray.Length; n++)
				if (intArray[n] > intArray[indexOfMax])
					indexOfMax = n;

			return indexOfMax;
			}
		private static double VerySmallDouble = 2.0e-015;
		private static double VerySmallNegDouble = -2.0e-015;
		public static bool NearZero(double d) {
			return (d < VerySmallDouble) && (d > VerySmallNegDouble);
			}
		#region StringFunctions
		public static string RandomString(int length) {
			string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			int count = characters.Length;
			Random random = new Random();
			StringBuilder sb = new StringBuilder();
			for (int n = 0; n < length; n++)
				sb.Append(characters[random.Next(count)]);

			return sb.ToString();
			}
		public static string UppercaseFirst(string s) {
			return string.IsNullOrEmpty(s) ? string.Empty : s.Length == 1 ? s.ToUpper() : char.ToUpper(s[0]) + s.Substring(1);
			}
		public static bool Contains(String srcString, String testString) {
			// CF doesn't have String.Contains
			return (srcString.ToLower().IndexOf(testString.ToLower()) >= 0);
			}
		public static String AddQuotes(String s) {
			return "\"" + s + "\"";
			}
		public static String SafeSubstring(String srcString, int startChar) {
			if (startChar < 0)
				startChar = 0;
			return srcString.Length <= startChar ? "" : srcString.Substring(startChar);
			}
		public static String SafeSubstring(String srcString, int startChar,int length) {
			if (length <= 0)
				return "";
			if (startChar < 0)
				startChar = 0;

			if (startChar >= srcString.Length)
				return "";

			if (srcString.Length - startChar <= length)
				return srcString.Substring(startChar);
			else
				return srcString.Substring(startChar, length);
			}
		public static String SafeRightString(String srcString, int length) {
			if (length <= 0)
				return "";
			return srcString.Length <= length ? srcString : srcString.Substring(srcString.Length - length, length);
			}
		public static String RepeatChar(Char c, int repeats)
			{
			String s = new String(c, repeats);
			return s;

			//String s = "";
			//for (int n = 0; n < repeats; n++)
			//    s += c;
			//return s;
			}
		public static bool IsIntegerChar(Char c)
			{
			return (c >= '0') && (c <= '9');
			}
		public static bool NotIntegerChar(Char c)
			{
			return (c < '0') || (c > '9');
			}
		public static bool IsPointChar(Char c)
			{
			return (c == '.');
			}
		public static bool IsIntegerString(String s, out int theInt) {
			if (IsIntegerString(s)) {
				//try {
					theInt = System.Convert.ToInt32(s);
				//    }
				//catch {
				//    theInt = int.MaxValue;
				//    }
				return true;
				}
			else {
				theInt = 0;
				return false;
				}
			}
		public static bool IsIntegerString(String s, out Int64 theInt) {
			if (IsIntegerString(s)) {
				theInt = System.Convert.ToInt64(s);
				return true;
				}
			else {
				theInt = 0;
				return false;
				}
			}
		public static bool IsSignChar(Char c)
			{
			return (c == '+') || (c == '-');
			}
		public static bool IsIntegerString(String s) {
			int length = s.Length;
			if (length == 0)
				return false;

			int index = 0;
			if (IsSignChar(s[0])) {
				if (length < 2)
					return false;
				index++;
				}

			while (index < length) {
				if (!Char.IsDigit(s[index]))
					return false;
				index++;
				}

			return true;
			}
		public static bool IsHexChar(Char c) {
			if (Char.IsDigit(c))
				return true;
			else
				return ((c >= 'a') && (c <= 'f')) || ((c >= 'A') && (c <= 'F'));
			}
		public static bool IsHexString(String s, out Int64 theInt) {
			if (IsHexString(s)) {
				theInt = Int64.Parse(s, System.Globalization.NumberStyles.HexNumber);
				return true;
				}
			else {
				theInt = 0;
				return false;
				}
			}
		public static bool IsHexString(String s) {
			int length = s.Length;
			if (length == 0)
				return false;

			int index = 0;
			if (IsSignChar(s[0])) {
				if (length < 2)
					return false;
				index++;
				}

			while (index < length) {
				if (!IsHexChar(s[index]))
					return false;
				index++;
				}

			return true;
			}
		public static bool IsRealStringOrPercent(String s) {
			int length = s.Length;
			if (length == 0)
				return false;

			if (s.EndsWith("%")) {
				if (length == 1)
					return false;
				else {
					s = s.Substring(0, length-1).Trim();
					length = s.Length;
					if (length == 0)
						return false;
					}
				}

			return IsRealString(s);
			}
		public static bool IsRealString(String s, out double d) {
			if (!IsRealString(s)) {
				d = 0;
				return false;
				}
			else {
				d = Convert.ToDouble(s);
				return true;
				}
			}
		public static bool IsRealString(String s) {
			int length = s.Length;
			if (length == 0)
				return false;

			Char c;
			int pointCount = 0, numCount = 0;
			int index = 0;

			c = s[index++];
			if (c == '.')
				pointCount = 1;
			else if (Char.IsDigit(c))
				numCount = 1;
			else if (!IsSignChar(c))
				return (false);

			// c = *s++;
			while (index < length) {
				c = s[index++];
				if (Char.IsDigit(c))
					numCount++;
				else if (c == '.')
					pointCount++;
				else
					return (false);
				}
			return ((pointCount < 2) && (numCount > 0));
			}
		public static String FloatString(double d, int sigDigs, int minDecs) {
			int i = sigDigs - (int) Math.Floor(Math.Log10(d)) - 1;
			if (i < minDecs) i = minDecs;
			return String.Format("{0:F" + i.ToString() + "}", d); 
			}
		public static int GetIntegerFromEndOfString(String s) {
			// Only works for positive integers, returns -1 if none found
			if (s.Length == 0)
				return -1;

			int n;
			String t = "";
			bool done = false;

			for (n = s.Length - 1; (n >= 0) && !done;  n--) {
				if (Char.IsDigit(s[n]))
					t = s[n] + t;
				else
					done = true;
				}

			return t == "" ? -1 : System.Convert.ToInt32(t);
			}

		public static int GetFirstIntegerFromString(String s) {
			int n,i;
			String t = "";

			for (n = 0; n < s.Length; n++) {
				if (Char.IsDigit(s[n]))
					t += s[n];
				else
					t += ' ';
				}
			String[] fields = t.Split(m_SpaceCharArray);
			for (n = 0; n < fields.Length; n++) {
				if (IsIntegerString(fields[n], out i))
					return i;
				}
			return 0;
			}
		public static int FieldCount(String s)
			{
			if (s.Length == 0)
				return 0;
			else
				return CharCount(s,'\t') + 1;
			}
		public static int FieldCount(String s, Char delim)
			{
			if (s.Length == 0)
				return 0;
			else
				return CharCount(s,delim) + 1;
			}
		public static int IndexOfField(String srcString, String fieldName, char delim) {
			String[] fields = srcString.Split(new Char[] { delim });
			for (int n = 0; n < fields.Length; n++) {
				if (String.Compare(fieldName, fields[n], true) == 0)
					return n;
				}
			return -1;
			}
		public static int IndexOfString(String findString, String[] findInList) {
			if (findInList == null)
				return -1;
			for (int n = 0; n < findInList.Length; n++) {
				if (String.Compare(findString, findInList[n], true) == 0)
					return n;
				}
			return -1;
			}
		public static int IndexOfString(String findString, List<String> findInList) {
			if (findInList == null)
				return -1;
			for (int n = 0; n < findInList.Count; n++) {
				if (String.Compare(findString, findInList[n], true) == 0)
					return n;
				}
			return -1;
			}
		public static int CharCount(String s, Char c)
			{
			int count = 0;
			for (int n = 0; n < s.Length; n++)
				if ( s[n] == c )
					count++;

			return count;
			}
		public static bool GetField(String s, int fieldNum, Char delim, out String fieldString)
			{
			String[] fields = s.Split(new Char[]{delim});
			if (fieldNum < fields.Length) {
				fieldString = fields[fieldNum];
				return true;
				}
			else {
				fieldString = "";
				return false;
				}
			}
		public static String GetField(String s, int fieldNum, Char delim) {	// will be blank if fieldNum'th field doesn't exist
			String fieldVal = "";
			GetField(s, fieldNum, delim, out fieldVal);
			return fieldVal;
			}
		public static String[] GetField(String[] s, int fieldNum, Char delim, String defaultValue) {
			Char[] delimArray = new Char[] { delim };
			String[] t = new String[s.Length];
			for (int n = 0; n < s.Length; n++) {
				String[] fields = s[n].Split(delimArray);
				t[n] = (fieldNum < fields.Length) ? fields[fieldNum] : defaultValue;
				}
			return t;
			}
		public static String[] GetStrings(String delimitedString, Char delim) {	// will be blank if fieldNum'th field doesn't exist
			if (delimitedString == "")
				return new String[0];
			else
				return delimitedString.Split(new Char[] { delim });
			}
		public static List<String> GetStringList(String delimitedString, Char delim) {
			String[] a = GetStrings(delimitedString, delim);
			List<String> list = new List<String>();
			foreach (String s in a)
				list.Add(s);

			return list;
			}
		public static void RemoveEmptyStringsFromEnd(List<String> stringList,bool trimStrings) {
			for (int n = stringList.Count - 1; n >= 0; n--) {
				string s = trimStrings ? stringList[n].Trim() : stringList[n];
				if (String.IsNullOrEmpty(s))
					stringList.RemoveAt(n);
				else
					return;
				}
			}
		public static bool AllFieldsEmpty(String s, Char delim) {
			String[] fields = s.Split(new Char[] { delim });
			for (int n = 0; n < fields.Length; n++)
				if (fields[n].Trim() != "")
					return false;

			return true;
			}
		public static bool AllStringsEmpty(String[] s, Char delim) {
			for (int n = 0; n < s.Length; n++)
				if (s[n].Trim() != "")
					return false;

			return true;
			}
		public static int NumericFieldCount(String s, Char delim) {
			int count = 0;
			String[] fields = s.Split(new Char[] { delim });
			for (int n = 0; n < fields.Length; n++)
				if (IsIntegerString(fields[n].Trim()))
					count++;

			return count;
			}

		public static Byte[] Append0Byte(Byte[] bytes) {
			Byte[] bytes0 = new byte[bytes.Length + 1];
			System.Buffer.BlockCopy(bytes, 0, bytes0, 0, bytes.Length);
			bytes0[bytes.Length] = 0;
			return bytes0;
			}
		public static Byte[] StringToByteArrayUTF8(String s) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetBytes(s);
			}
		public static Byte[] StringToByteArrayUTF8(String s, bool appendZero) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			Byte[] bytes = encoding.GetBytes(s);
			return appendZero ? Append0Byte(bytes) : bytes;
			}
		public static String StringFromByteArrayUTF8(Byte[] byteArray) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetString(byteArray, 0, byteArray.Length);
			}

		public static Byte[] StringToByteArrayASCII(String s) {
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetBytes(s);
			}
		public static Byte[] StringToByteArrayASCII(String s, bool appendZero) {
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			Byte[] bytes = encoding.GetBytes(s);
			return appendZero ? Append0Byte(bytes) : bytes;
			}
		public static String StringFromByteArrayASCII(Byte[] byteArray) {
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			return encoding.GetString(byteArray, 0, byteArray.Length);
			}

#endregion	// String Functions

		public static bool ParseConfigLine(String theLine,out String paramName,out String paramVal) {
			return ParseConfigLine(theLine,out paramName,out paramVal,false);
			}			
		public static bool ParseConfigLine(String theLine, out String paramName, out String paramVal,bool blankParamValueIsOk)
			{	// parse string of the form paramname=paramval
				// paramName & paramVal are only valid if true is returned
			paramName = "";
			paramVal = "";

			theLine = theLine.Trim(m_ParseConfigLineChars);	//new Char[]{' ',';','\t','\n',});
			
			int n = theLine.IndexOf('=');
			if (n < 1)
				return false;

			paramName = theLine.Substring(0,n).Trim();
			if (paramName.Length == 0)
				return false;

			int valueStringLength = theLine.Length - n - 1;
			if (valueStringLength < 1)
				return blankParamValueIsOk;

			paramVal = theLine.Substring(n+1,valueStringLength).Trim();
			return blankParamValueIsOk || (paramVal.Length > 0);
			}
		public static String PrefixLines(String s,String prefix) {
			String t = "";
			int offset = s.IndexOf("\n");
			int lastOffset = 0;

			while (offset >= 0) {
				t += prefix + s.Substring(lastOffset, offset - lastOffset + 1);
				lastOffset = offset + 1;
				if (lastOffset < s.Length)
					offset = s.IndexOf("\n",lastOffset);
				else
					offset = -1;
				}

			if (lastOffset < s.Length)
				t += prefix + s.Substring(lastOffset);

			return t;
			}
		public static String[] GetCommandLineEntries(String cmdline) {
			// e.g. "f:\test programs\test.exe" iadata="f:\temp\data files\data.txt"
			// would return two strings
			

			StringBuilder sb = new StringBuilder();
			ArrayList stringList = new ArrayList();
			string s;

			bool inQuotes = false;
			bool inEscape = false;

			foreach(Char c in cmdline) {
				if (inEscape) {
					sb.Append(GetEscapedChar(c));
					inEscape = false;
					}
				else if (c == '"') {
					inQuotes = !inQuotes;
					}
				else if (inQuotes) {
					sb.Append(c);
					}
				else if (c == ' ') {
					s = sb.ToString();
					if (s.Length > 0)
						stringList.Add(s);
					sb.Length = 0;
					}
				else
					sb.Append(c);
				}

			s = sb.ToString();
			if (s.Length > 0)
				stringList.Add(s);
			sb.Length = 0;

			return (string[])stringList.ToArray(typeof(string));
			}
		public static String RemoveQuotes(String s) {
			if ((s.Length > 1) && ((s[0] == '\"') && (s[s.Length - 1] == '\"')) || ((s[0] == '\'') && (s[s.Length - 1] == '\'')))
				return s.Substring(1, s.Length - 2);
			else
				return s;
			}
		public static string GetCommandLineArg(String[] args, string param) {
			foreach (String arg in args) {
				string paramName, paramValue;
				if (!Utility.ParseConfigLine(arg, out paramName, out paramValue))
					continue;
				if (String.Compare(paramName, param, true) == 0)
					return paramValue;
				}

			return "";
			}
		public static String[] ParseCSVLine(String line, Char delimiter) {
			StringBuilder sb = new StringBuilder();
			ArrayList list = new ArrayList();
			bool inQuotes = false;
			bool lastCharEndQuote = false;
			bool inEscape = false;

			foreach (Char c in line) {
				if (lastCharEndQuote) {
					lastCharEndQuote = false;
					if (c == '"') {	// 2 quotes in a row when we're already in quotes -> add a quote & we're still in quotes & move on to next char
						sb.Append('"');
						continue;
						}
					else {	// the end quote was just an end quote, so no longer in quotes & process this char as usual
						inQuotes = false;
						}
					}

				if (inEscape) {
					sb.Append(GetEscapedChar(c));
					inEscape = false;
					}
				else if (c == '\\')
					inEscape = true;
				else if (lastCharEndQuote) {
					if (c == '"')
						sb.Append('"');
					else
						inQuotes = false;
					lastCharEndQuote = false;
					}
				else if (c == '"') {
					if (inQuotes)
						lastCharEndQuote = true;
					else
						inQuotes = true;
					}
				else if (inQuotes) {
					sb.Append(c);
					}
				else if (c == delimiter) {
					list.Add(sb.ToString());
					sb.Length = 0;
					}
				else
					sb.Append(c);
				}

			// Add the last one
			list.Add(sb.ToString());

			return (String[])list.ToArray(typeof(String));
			}
		private static Char GetEscapedChar(Char c) {
			switch(c) {
				case 'n':return '\n';
				case 'r': return '\r';
				case 't': return '\t';
				case '"': return '"';
				case '\\': return '\\';
				default: return c;
				}
			}
		public static bool IsTextNumberString(String s, out String text, out int number) {
			text = "";
			number = 0;

			if (s.Length < 2)
				return false;
			else if (Char.IsDigit(s[0]) || !Char.IsDigit(s[s.Length - 1]))
				return false;
			else {
				int firstDigit = s.Length - 1;
				while (Char.IsDigit(s[firstDigit - 1]))
					firstDigit--;
				text = s.Substring(0, firstDigit);
				number = int.Parse(s.Substring(firstDigit));
				return true;
				}
			}
		public static bool IsNameNumberString(String s, out String name,out int number) {	
			// e.g. E210
			name = "";
			number = 0;

			if (s.Length < 2)
				return false;
			else if (Char.IsDigit(s[0]) || !Char.IsDigit(s[s.Length-1]))
				return false;
			else {
				int startChars = 1;
				int endDigits = 1;
				int index = 1;
				while (!Char.IsDigit(s[index++]))
					startChars++;

				index = s.Length-2;
				while (Char.IsDigit(s[index--]))
					endDigits++;

				if (startChars + endDigits == s.Length) {
					name = s.Substring(0, startChars);
					String numberString = s.Substring(startChars);
					number = System.Convert.ToInt32(numberString);
					return true;
					}
				else
					return false;
				}
			}
		public static bool IsNumberNameString(String s, out int number, out String name) {
			int integerCharCount;
			if (IsNumberNameString(s, out integerCharCount)) {
				number = System.Convert.ToInt32(s.Substring(0, integerCharCount));
				name = integerCharCount < s.Length ? s.Substring(integerCharCount) : "";
				return true;
				}
			else {
				number = 0;
				name = "";
				return false;
				}
			}
		public static  bool IsNumberNameString( String s, out int integerCharCount )
			{	// can be integer string like 692 or plus text like 692ab, not 4a7
				// integerCharCount is set to the number of integer chars at the start even if false is returned
			integerCharCount = 0;

			int length = s.Length;
			if (length == 0)
				return false;

			if ( !Char.IsDigit( s[0] ) ) {
				integerCharCount = 0;
				return false;
				}

			// find integer part of string
			int index = 1;
			while ((index < length) && Char.IsDigit(s[index]))
				index++;

			integerCharCount = index;

			// find non-integer part of string
			while (index < length)
				if (Char.IsDigit(s[index++]))
					return false;

			return true;
			}
		public static int DigitCountAtStart(String s) {
			if (String.IsNullOrEmpty(s))
				return 0;
			else {
				int index = 0;
				while ((index < s.Length) && Char.IsDigit(s[index]))
					index++;
				return index;
				}
			}
		public static bool ParseNumberNameString(String s, out int numVal, out String nameStr)
			{
			numVal = 0;
			nameStr = "";

			int numChars;
			if ( !IsNumberNameString( s, out numChars ) )
				return false;
			
			String numStr = s.Substring(0, numChars);
			numVal = Convert.ToInt32(numStr);
			nameStr = s.Substring(numChars, s.Length - numChars);
			return true;
			}
		public static String[] SplitAndRemoveEmpties(String s, char delimiter) {
			ArrayList list = new ArrayList();
			String[] a = s.Split(new Char[] { delimiter });
			for (int n = 0; n < a.Length; n++) {
				String t = a[n].Trim();
				if (t.Length > 0)
					list.Add(t);
				}
			return (String[])list.ToArray(typeof(String));
			}
		public static void MakeLower(String[] a) {
			for (int n = 0; n < a.Length; n++)
				a[n] = a[n].ToLower();
			}
		public static bool SameDay(DateTime t1,DateTime t2) {
			return (t1.Day == t2.Day) && (t1.Month == t2.Month) && (t1.Year == t2.Year);
			}
		public static String LocalMachineName() {
			// Mainly to remind me, so I don't have to spend ages finding it again
			return System.Net.Dns.GetHostName();
			}
		public static string ConvertToHexString(byte[] bytes) {
			return BitConverter.ToString(bytes).Replace("-", "");	// hex string
			}
		public static void SwapValues(ref int i, ref int j) {
			int temp = i;
			i = j;
			j = temp;
			}
#if !PocketPC && !WindowsCE
			// Not intended for Pocket PC use
		/// <summary>
		/// 0 if same version, -1 -> file1 older, 1 -> file1 newer
		/// </summary>
		public static int CompareFileVersions(String file1,String file2) {
			// return	-1 file1 older than file2
			//			0 same version
			//			1 file1 newer than file2

			if (!File.Exists(file1) || !File.Exists(file2))
				return 0;

			FileVersionInfo v1 = FileVersionInfo.GetVersionInfo(file1);
			FileVersionInfo v2 = FileVersionInfo.GetVersionInfo(file2);

			if (v1.FileMajorPart < v2.FileMajorPart)
				return -1;
			if (v1.FileMajorPart > v2.FileMajorPart)
				return 1;

			if (v1.FileMinorPart < v2.FileMinorPart)
				return -1;
			if (v1.FileMinorPart > v2.FileMinorPart)
				return 1;

			if (v1.FileBuildPart < v2.FileBuildPart)
				return -1;
			if (v1.FileBuildPart > v2.FileBuildPart)
				return 1;

			if (v1.FilePrivatePart < v2.FilePrivatePart)
				return -1;
			if (v1.FilePrivatePart > v2.FilePrivatePart)
				return 1;

			return 0;
			}
#endif
		public static String[] StringArrayFromArrayList(ArrayList list) {
#if PocketPC || WindowsCE
			String[] array = new String[list.Count];
			for (int n = 0; n < list.Count; n++)
				array[n] = (String)list[n];
			return array;
#else
			return (String[])list.ToArray(typeof(String));
#endif
			}
		public static void ArrayResize(ref String[] a, int newSize) {
#if PocketPC || WindowsCE
			String[] b = new String[newSize];
			int count = Math.Min(newSize, a.Length);
			if (count > 0)
				Array.Copy(a, b, count);
			a = b;
#else
			Array.Resize(ref a, newSize);
#endif
			}
		public static String[] RemoveBlanks(String[] a) {
			ArrayList list = new ArrayList();
			for (int n = 0; n < a.Length; n++)
				if (a[n] != "")
					list.Add(a[n]);
			return (String[]) list.ToArray(typeof(String));
			}
		//public static void ArrayResize(ref Array a,int newSize) {
		//    Array b = Array.CreateInstance (a.GetType().GetElementType(), newSize);
		//    int toCopy = Math.Min (a.Length, newSize);
		//    if (toCopy > 0)
		//        Array.Copy (a, b, toCopy);
		//    a = b;
		//    }
		public static void ArrayResize(ref String[,] srcArray, int rows, int cols) {
			String[,] newArray = new String[rows, cols];
			Array.Copy(srcArray, newArray,Math.Min(srcArray.Length,newArray.Length));
			srcArray = newArray;
			}

		public static String FileSizeString(long size) {
			if (size < 1024)
				return size.ToString() + " " + m_FileSizeStrings[0];

			double d = size;

			for (int n = 1; n < m_FileSizeStrings.Length; n++) {
				d /= 1024;
				if (d < 10)
					return d.ToString("0.00") + " " + m_FileSizeStrings[n];
				else if (d < 1024)
					return d.ToString("0.0") + " " + m_FileSizeStrings[n];
				}
			return d.ToString("0.0") + " " + m_FileSizeStrings[m_FileSizeStrings.Length-1];
			}
		public static String[] UniqueValues(String[] srcArray, bool ignoreCase) {
			ArrayList dstList = new ArrayList();

			if (ignoreCase) {
				ArrayList dstListLower = new ArrayList();
				foreach (String t in srcArray) {
					String tLower = t.ToLower();
					if (dstListLower.IndexOf(tLower) < 0) {
						dstListLower.Add(tLower);
						dstList.Add(t);
						}
					}
				}
			else {
				foreach (String t in srcArray) {
					if (dstList.IndexOf(t) < 0)
						dstList.Add(t);
					}
				}

			return StringArrayFromArrayList(dstList);
			}
		public static Dictionary<string,string> ParseCommandLine(string[] args) {
			Dictionary<string, string> p = new Dictionary<string, string>();

			//string[] args = Environment.GetCommandLineArgs();
			if (args.Length == 0)
				return p;

			p["cmd"] = args[0];

			for (int n = 1; n < args.Length; n++) { //string arg in args) {
				string arg = args[n];
				if (Utility.ParseConfigLine(arg, out string paramName, out string paramValue, true))
					p[paramName] = paramValue;
				else 
					p["arg" + n.ToString()] = arg;
				}

			return p;
			}
		}
#pragma warning restore CS8600
#pragma warning restore CS8602
#pragma warning restore CS8603
#pragma warning restore CS8625
	}
