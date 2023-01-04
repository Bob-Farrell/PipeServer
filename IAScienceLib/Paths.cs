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
#pragma warning disable CS8603
#pragma warning disable CS8604

	public class Paths {
		private static Char[] _PathSeparators = new Char[] { '/', '\\', ' ', '\t' };
		public static String AppendToFilename(String filepath, String appendText) {
			return Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath) + appendText + Path.GetExtension(filepath));
			}
		public static String PrependToFilename(String filepath, String prependText) {
			return Path.Combine(Path.GetDirectoryName(filepath), prependText + Path.GetFileName(filepath));
			}
		public static String ChangeExtension(String filepath, String newExt) {
			return Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath) + newExt);
			}
		public static String DeepestFolder(String folder) {
			// Return the last folder in the path, with no path delimiters
			if (folder == "") {
				folder = Directory.GetCurrentDirectory();
				if (folder == "")
					return "";
				}

			int lastone = folder.Length - 1;

			// Ignore trailing delimeter
			if ((folder[lastone] == '\\') || (folder[lastone] == '/'))
				lastone--;

			if (lastone <= 0)
				return "";

			if (folder[lastone] == ':')
				return "";

			int savelast = lastone;

			while ((lastone >= 0) && (folder[lastone] != '\\') && (folder[lastone] != '/'))
				lastone--;

			if (lastone < 0)	// found no path delimiters
				return folder.Substring(0, savelast + 1);
			else
				return folder.Substring(lastone + 1, savelast - lastone);
			}
		public static String TopFolder(String folder) {
			// Return the first folder in the path, with no path delimiters
			if (folder == "")
				return "";

			Char[] delims = new Char[2] { '\\', '/' };
			String[] a = folder.Split(delims);
			foreach(String s in a) {
				if (s != "")
					return s;
				}

			return "";
			}
		public static String ParentPath(String folder, int levels) {
			for (int n = 0; n < levels; n++)
				folder = ParentPath(folder);

			return folder;
			}
		public static String ParentPath(String folder)
			{
			// CF doesn't have Path.Parent
			// If a path is returned it will have a trailing delimiter
			// Returns null if folder is a top level path, e.g.
			// F:\			on a PC
			// \\Server1	on a server
			// \		on a Pocket PC

			if (folder == "") {
				folder = Directory.GetCurrentDirectory();
				if (folder == "")
					return "";
				}

			int lastone = folder.Length - 1;

			// Ignore trailing delimeter
			if ((folder[lastone] == '\\') || (folder[lastone] == '/'))
				lastone--;

			if (lastone == 0)
				return null;

			while ((lastone >= 0) && (folder[lastone] != '\\') && (folder[lastone] != '/'))
				lastone--;

			//while ((lastone >= 0) && ((folder[lastone] == '\\') || (folder[lastone] == '/')))
			//    lastone--;

			if (lastone < 0)	// found no path delimiters
				return null;
			else if (lastone == 0)	// path starts with a delimiter but is not a server path
				return folder.Substring(0, 1);	// i.e. "\" or "/"
			else if (lastone == 1)	// either the root of a server path or garbage
				return null;
			else
				return folder.Substring(0, lastone + 1);
			}
		/// <summary>
		/// Deprecated - use Paths.GetDirectoryName
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static String SafeGetDirectory(String filename) {
			return String.IsNullOrEmpty(filename) ? "" : Path.GetDirectoryName(filename);
			}
		public static String GetDirectoryName(String filename) {
			return String.IsNullOrEmpty(filename) ? "" : Path.GetDirectoryName(filename);
			}
		public static String GetFileName(String filename) {
			return String.IsNullOrEmpty(filename) ? "" : Path.GetFileName(filename);
			}
		public static bool DirectoryExists(String folder) {
			if (String.IsNullOrEmpty(folder))
				return false;

			try {
				if ((folder.Length > 1) && folder.EndsWith("\\"))
					folder = folder.Substring(0, folder.Length - 1);
				return Directory.Exists(folder);
				}
			catch {
				return false;
				}
			}
		public static bool FileExists(String filename) {
			if (filename == "")
				return false;

			try {
				return File.Exists(filename);
				}
			catch {
				return false;
				}
			}
		public static bool CreatePath(String path) {	
			// Return false if the path is empty
			// Else return true if the path exists or can be created
			if (path == "")
				return false;

			if (Directory.Exists(path))
				return true;

			try {
				Directory.CreateDirectory(path);
				return Directory.Exists(path);
				}
			catch {
				return false;
				}
			}
		public static bool IsRootPath(String path) {
			return 	path == Path.GetPathRoot(path);
			}
		public static bool SameFolder(String folder1,String folder2) {
			if ((folder1.Length == 0) || (folder2.Length == 0))
				return (folder1 == folder2);

			char c = folder1[folder1.Length-1];
			if ((c == '\\') || (c == '/') || (c == ':'))
				folder1 = folder1.Substring(0,folder1.Length-1);

			c = folder2[folder2.Length-1];
			if ((c == '\\') || (c == '/') || (c == ':'))
				folder2 = folder2.Substring(0,folder2.Length-1);

			return (String.Compare(folder1, folder2, true) == 0);
			}
		public static bool IsDeepestFolder(String path,String folder) {
			return String.Compare(DeepestFolder(path), folder) == 0;
			}
		public static String ExtensionWithoutDot(String filename) {
			String ext = Path.GetExtension(filename);
			if (ext == "")
				return "";
			return ext[0] == '.' ? Utility.SafeSubstring(ext,1) : ext;
			}
		public static String DeepestPathThatExists(String fileOrFolder) {
			try {
				if (Directory.Exists(fileOrFolder))
					return fileOrFolder;
				}
			catch {
				}
			
			fileOrFolder = ParentPath(fileOrFolder);
			String parentPath = ParentPath(fileOrFolder);

			do {
				if (Directory.Exists(fileOrFolder))
					return fileOrFolder;
				fileOrFolder = parentPath;
				parentPath = ParentPath(fileOrFolder);
				}
			while (parentPath != fileOrFolder);

			return "";
			}
		public static String Combine(String p1, String p2) {
			p1 = p1.Trim(_PathSeparators);
			p2 = p2.Trim(_PathSeparators);
			if (p1.Length == 0)
				return p2;
			else if (p2.Length == 0)
				return p1;
			else
				return Path.Combine(p1, p2);
			}
		public static String Combine(String p1, String p2, String p3) {
			return Paths.Combine(Path.Combine(p1, p2), p3);
			}
		public static String Combine(String p1, String p2, String p3,String p4) {
			return Paths.Combine(Paths.Combine(Paths.Combine(p1, p2), p3), p4);
			}
		public static String FindApplicationFile(String filename, int upLevels) {
			// filename could be "config\test.txt" in which case we search for <app path>\config\test.txt
			// and <parent of app path>\config\test.txt

			String appPath = Utility.ApplicationPath();

			for (int iteration = 0; iteration <= upLevels; iteration++) {
				String appFile = Path.Combine(appPath, filename);
				if (File.Exists(appFile))
					return appFile;
				appPath = Paths.ParentPath(appPath);
				}

			return "";
			}
		public static String FindApplicationFolder(String folderName, int upLevels) {
			// folderName could be "config" in which case we search for a folder <app path>\config
			// and <parent of app path>\config etc

			String appPath = Utility.ApplicationPath();

			for (int iteration = 0; iteration <= upLevels; iteration++) {
				if (String.IsNullOrEmpty(appPath))
					return "";

				String appFolder = Path.Combine(appPath, folderName);
				if (Directory.Exists(appFolder))
					return appFolder;
				appPath = Paths.ParentPath(appPath);
				}

			return "";
			}
		}
#pragma warning restore CS8603
#pragma warning restore CS8604

	}
