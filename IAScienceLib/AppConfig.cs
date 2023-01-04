using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.IO;
using IAScience;

namespace IAScience {
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8625
	public class AppConfig {
		// Define these here in case I ever want to change the delimeter used for storing string arrays
		private static Char[] StringArrayDelimeter = new Char[1] { '|' };
		private static String StringArrayJoiner = "|";

		private XmlNamespaceManager XmlNamespaces = null;
		private string XmlNamespacePrefix = "IAScience:";
		// Name of the Configuration Section to be written to in the Config file
		protected string ConfigSectionName = "";
		protected string ConfigFilename = "";

		private string EncryptFieldList = "";
		private string EncryptKey = "";

		public AppConfig() {
			}
		public AppConfig(String configFile) {
			ReadKeysFromConfig(configFile);
			}
		//public AppConfig(String devroot, String appName, String configFileName) {
		//    ReadKeysFromConfig(StandardConfigFileName(devroot,appName,configFileName,true));
		//    }
		public string LoadedSourceFile() {
			return ConfigFilename;
			}
		public static String StandardAppDataFolder(String devroot, String appName, Boolean createPath) {
#if PocketPC || WindowsCE
			String path = "\\My Documents";
#else
			String path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), devroot);
#endif
			path = Path.Combine(path, appName);
			if (createPath)
				Paths.CreatePath(path);

			return path;
			}
		public static String StandardConfigFileName(String devroot, String appName, String configFileName, Boolean createPath) {
			// e.g. StandardConfigFileName("IAScience","OSRReports","Config.xml")
//#if PocketPC || WindowsCE
//            String path = "\\My Documents";
//#else
//            String path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), devroot);
//#endif
//            path = Path.Combine(path, appName);
//            if (createPath)
//                Paths.CreatePath(path);

			String path = StandardAppDataFolder(devroot, appName, createPath);
			return Path.Combine(path, configFileName);
			}
		//object ICloneable.Clone() {
		//    // make memberwise copy
		//    return this.MemberwiseClone();
		//    }

		// Binding Flags constant to be reused for all Reflection access methods.
		public const BindingFlags MemberAccess =
			BindingFlags.Public | BindingFlags.NonPublic |
			BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

		/// <summary>
		/// Converts a type to string if possible. This method supports an optional culture generically on any value.
		/// It calls the ToString() method on common types and uses a type converter on all other objects
		/// if available
		/// </summary>
		/// <param name="RawValue">The Value or Object to convert to a string</param>
		/// <param name="Culture">Culture for numeric and DateTime values</param>
		/// <returns>string</returns>
		public static string TypedValueToString(object RawValue, CultureInfo Culture)
			{
			if (RawValue == null)
				return null;

			Type ValueType = RawValue.GetType();
			string Return = null;

			if (ValueType == typeof(string))
				Return = RawValue.ToString();
			else if (ValueType == typeof(string[]))
				Return = String.Join(StringArrayJoiner, (String[])RawValue);
			else if (ValueType == typeof(Int32[])) {
				Return = "";
				Int32[] srcValues = (Int32[]) RawValue;
				for (int n = 0; n < srcValues.Length - 1; n++)
					Return += srcValues[n].ToString() + StringArrayJoiner;
				if (srcValues.Length > 0)
					Return += srcValues[srcValues.Length - 1].ToString();
				}
			else if (ValueType == typeof(int) || ValueType == typeof(decimal) ||
				ValueType == typeof(double) || ValueType == typeof(float))
				Return = string.Format(Culture.NumberFormat, "{0}", RawValue);
			else if (ValueType == typeof(DateTime))
				Return = string.Format(Culture.DateTimeFormat, "{0}", RawValue);
			else if (ValueType == typeof(bool))
				Return = RawValue.ToString();
			else if (ValueType == typeof(byte))
				Return = RawValue.ToString();
			else if (ValueType.IsEnum)
				Return = RawValue.ToString();
			else if (ValueType == typeof(System.Drawing.Color)) {
				System.Drawing.Color c = (System.Drawing.Color)RawValue;
				Return = c.R.ToString() + ", " + c.G.ToString() + ", " + c.B.ToString();
				}
			else {
#if PocketPC || WindowsCE
				Return = RawValue.ToString();
#else
				// Any type that supports a type converter
				System.ComponentModel.TypeConverter converter =
					System.ComponentModel.TypeDescriptor.GetConverter(ValueType);
				if (converter != null && converter.CanConvertTo(typeof(string)))
					Return = converter.ConvertToString(null, Culture, RawValue);
				else
					// Last resort - just call ToString() on unknown type
					Return = RawValue.ToString();
#endif
				}
			return Return;
			}
		/// <summary>
		/// Turns a string into a typed value. Useful for auto-conversion routines
		/// like form variable or XML parsers.
		/// <seealso>Class wwUtils</seealso>
		/// </summary>
		/// <param name="SourceString">
		/// The string to convert from
		/// </param>
		/// <param name="TargetType">
		/// The type to convert to
		/// </param>
		/// <param name="Culture">
		/// Culture used for numeric and datetime values.
		/// </param>
		/// <returns>object. Throws exception if it cannot be converted.</returns>
		public static object StringToTypedValue(string SourceString, Type TargetType, CultureInfo Culture)
			{
			object Result = null;

			if (TargetType == typeof(string))
				Result = SourceString;
			else if (TargetType == typeof(string[])) {
				if (SourceString == "")
					Result = new String[0];
				else
					Result = SourceString.Split(StringArrayDelimeter);
				}
			else if (TargetType == typeof(Int32[])) {
				if (SourceString == "")
					Result = new Int32[0];
				else {
					String[] a = SourceString.Split(StringArrayDelimeter);
					Int32[] i = new Int32[a.Length];
					for (int n = 0; n < a.Length; n++)
						i[n] = IAScience.Utility.IsIntegerString(a[n]) ? System.Convert.ToInt32(a[n]) : 0;

					Result = i;
					}
				}
			else if (TargetType == typeof(int))
				Result = int.Parse(SourceString, System.Globalization.NumberStyles.Integer, Culture.NumberFormat);
			else if (TargetType == typeof(byte))
				Result = Convert.ToByte(SourceString);
			else if (TargetType == typeof(decimal))
				Result = Decimal.Parse(SourceString, System.Globalization.NumberStyles.Any, Culture.NumberFormat);
			else if (TargetType == typeof(double))
				Result = Double.Parse(SourceString, System.Globalization.NumberStyles.Any, Culture.NumberFormat);
			else if (TargetType == typeof(bool)) {
				if (SourceString.ToLower() == "true" || SourceString.ToLower() == "on" || SourceString == "1")
					Result = true;
				else
					Result = false;
				}
			else if (TargetType == typeof(DateTime))
				Result = Convert.ToDateTime(SourceString, Culture.DateTimeFormat);
			else if (TargetType.IsEnum)
				Result = Enum.Parse(TargetType, SourceString,false);
#if !(ConsoleApp || WebApp)
			else if (TargetType == typeof(ExtFormState))
				Result = ExtFormState.FromString(SourceString);
#endif
			else if (TargetType == typeof(ArrayList)) {
				}
			else if (TargetType == typeof(System.Drawing.Color)) {
				String[] a = IAScience.Utility.SplitAndRemoveEmpties(SourceString, ',');
				if (a.Length == 3) {
					int r, g, b;
					if (!IAScience.Utility.IsIntegerString(a[0], out r))
						r = 255;
					if (!IAScience.Utility.IsIntegerString(a[1], out g))
						g = 255;
					if (!IAScience.Utility.IsIntegerString(a[2], out b))
						b = 255;
					Result = System.Drawing.Color.FromArgb(r, g, b);
					}
				else
					Result = System.Drawing.Color.Blue;
				}
			else {
#if PocketPC || WindowsCE
				System.Diagnostics.Debug.Assert(false, "Type Conversion not handled in StringToTypedValue\r\n" +
													TargetType.Name + " " + SourceString);
					throw (new Exception("Type Conversion not handled in StringToTypedValue"));
#else
				System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(TargetType);
				if (converter != null && converter.CanConvertFrom(typeof(string)))
					try {
						Result = converter.ConvertFromString(null, Culture, SourceString);
						}
					catch {
						Result = null;
						}
				else {
					System.Diagnostics.Debug.Assert(false, "Type Conversion not handled in StringToTypedValue\r\n" +
													TargetType.Name + " " + SourceString);
					throw (new Exception("Type Conversion not handled in StringToTypedValue"));
					}
#endif
				}

			return Result;
			}

		/// <summary>
		/// Sets the value of a field or property via Reflection. This method alws 
		/// for using '.' syntax to specify objects multiple levels down.
		/// 
		/// wwUtils.SetPropertyEx(this,"Invoice.LineItemsCount",10)
		/// 
		/// which would be equivalent of:
		/// 
		/// this.Invoice.LineItemsCount = 10;
		/// </summary>
		/// <param name="Object Parent">
		/// Object to set the property on.
		/// </param>
		/// <param name="String Property">
		/// Property to set. Can be an object hierarchy with . syntax.
		/// </param>
		/// <param name="Object Value">
		/// Value to set the property to
		/// </param>
		public static object SetPropertyEx(object Parent, string Property, object Value)
			{
			Type Type = Parent.GetType();
			MemberInfo Member = null;

			// *** no more .s - we got our final object
			int lnAt = Property.IndexOf(".");
			if (lnAt < 0) {
				Member = Type.GetMember(Property, MemberAccess)[0];
				if (Member.MemberType == MemberTypes.Property) {

					((PropertyInfo)Member).SetValue(Parent, Value, null);
					return null;
					}
				else {
					((FieldInfo)Member).SetValue(Parent, Value);
					return null;
					}
				}

			// *** Walk the . syntax
			string Main = Property.Substring(0, lnAt);
			string Subs = Property.Substring(lnAt + 1);
			Member = Type.GetMember(Main, MemberAccess)[0];

			object Sub;
			if (Member.MemberType == MemberTypes.Property)
				Sub = ((PropertyInfo)Member).GetValue(Parent, null);
			else
				Sub = ((FieldInfo)Member).GetValue(Parent);

			// *** Recurse until we get the lowest ref
			SetPropertyEx(Sub, Subs, Value);
			return null;
			}

		/// <summary>
		/// Pass list of fields to encrypt
		/// </summary>
		/// <param name="EncryptFields"></param> e.g. "field1,field2"
		/// <param name="EncryptKey"></param>
		public void SetEncryptFields(string EncryptFields, string EncryptKey)
			{
			this.EncryptFieldList = "," + EncryptFields.ToLower() + ",";
			this.EncryptKey = EncryptKey;
			}

		/// <summary>
		/// Used to load up the default namespace reference and prefix
		/// information. This is required so that SelectSingleNode can
		/// find info in 2.0 or later config files that include a namespace
		/// on the root element definition.
		/// </summary>
		/// <param name="Dom"></param>
		private void GetXmlNamespaceInfo(XmlDocument Dom)
			{
			// *** Load up the Namespaces object so we can 
			// *** reference the appropriate default namespace
			if (Dom.DocumentElement.NamespaceURI == null || Dom.DocumentElement.NamespaceURI == "") {
				this.XmlNamespaces = null;
				this.XmlNamespacePrefix = "";
				}
			else {
				if (Dom.DocumentElement.Prefix == null || Dom.DocumentElement.Prefix == "")
					this.XmlNamespacePrefix = "IAScience";
				else
					this.XmlNamespacePrefix = Dom.DocumentElement.Prefix;

				XmlNamespaces = new XmlNamespaceManager(Dom.NameTable);
				XmlNamespaces.AddNamespace(this.XmlNamespacePrefix, Dom.DocumentElement.NamespaceURI);

				this.XmlNamespacePrefix += ":";
				}
			}

		/// <summary>
		/// Creates a Configuration section and also creates a ConfigSections section for new 
		/// non appSettings sections.
		/// </summary>
		/// <param name="Dom"></param>
		/// <param name="ConfigSection"></param>
		/// <returns></returns>
		private XmlNode CreateConfigSection(XmlDocument Dom, string ConfigSection)
			{
			// *** Create the actual section first and attach to document
			XmlNode AppSettingsNode = Dom.CreateNode(XmlNodeType.Element,
				ConfigSection, Dom.DocumentElement.NamespaceURI);

			XmlNode Parent = Dom.DocumentElement.AppendChild(AppSettingsNode);

			// *** Now check and make sure that the section header exists
			if (ConfigSection != "appSettings") {
				XmlNode ConfigSectionHeader = Dom.DocumentElement.SelectSingleNode(this.XmlNamespacePrefix + "configSections",
					this.XmlNamespaces);
				if (ConfigSectionHeader == null) {
					// *** Create the node and attributes and write it
					XmlNode ConfigSectionNode = Dom.CreateNode(XmlNodeType.Element,
						"configSections", Dom.DocumentElement.NamespaceURI);

					// *** Insert as first element in DOM
					ConfigSectionHeader = Dom.DocumentElement.InsertBefore(ConfigSectionNode,
						Dom.DocumentElement.ChildNodes[0]);
					}

				// *** Check for the Section
				XmlNode Section = ConfigSectionHeader.SelectSingleNode(this.XmlNamespacePrefix + "section[@name='" + ConfigSection + "']",
					this.XmlNamespaces);

				if (Section == null) {
					Section = Dom.CreateNode(XmlNodeType.Element, "section",
						null);

					XmlAttribute Attr = Dom.CreateAttribute("name");
					Attr.Value = ConfigSection;
					XmlAttribute Attr2 = Dom.CreateAttribute("type");
					Attr2.Value = "System.Configuration.NameValueSectionHandler,System,Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
					Section.Attributes.Append(Attr);
					Section.Attributes.Append(Attr2);
					ConfigSectionHeader.AppendChild(Section);
					}
				}

			return Parent;
			}

		/// <summary>
		/// Writes all of the configuration file properties to a specified 
		/// configuration file.
		/// 
		/// The format written is in standard .Config file format, but this method 
		/// allows writing out to a custom .Config file.
		/// <seealso>Class wwAppConfiguration</seealso>
		/// </summary>
		/// <param name="String Filename">
		/// Name of the file to write out
		/// </param>
		/// <returns>Void</returns>
		/// <example>
		/// // *** Overridden constructor
		/// public WebStoreConfig() : base(false)
		/// {
		///    this.SetEnryption("ConnectionString,MailPassword,
		///                      MerchantPassword","WebStorePassword");
		/// 
		///    // *** Use a custom Config file
		///    this.ReadKeysFromConfig(@"d:\projects\wwWebStore\MyConfig.config");
		/// }
		/// </example>
		public void Reload() {
			if (ConfigFilename != "")
				ReadKeysFromConfig(ConfigFilename);
			}

		public virtual bool WriteKeysToConfig()
			{
			if (ConfigFilename != "") {
				if (!Directory.Exists(Path.GetDirectoryName(ConfigFilename))) {
					try {
						Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilename));
						}
					catch {
						return false;
						}
					}
				return WriteKeysToConfig(ConfigFilename);
				}
			else
				return false;
			}

		//public static Object GetCopy(Object src) {
		//    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		//    MemoryStream ms = new MemoryStream();
		//    formatter.Serialize(ms,src);
		//    ms.Seek(0,System.IO.SeekOrigin.Begin);
		//    return formatter.Deserialize(ms);
		//    }


		/// <summary>
		/// Returns indented xml
		/// </summary>
		public static String FormatXml(String XML) {
			String result = "";

			MemoryStream memStream = new MemoryStream();
			XmlTextWriter textWriter = new XmlTextWriter(memStream, Encoding.Unicode);
			XmlDocument xmlDoc = new XmlDocument();

			try {
				// Load the XmlDocument with the XML.
				xmlDoc.LoadXml(XML);

				textWriter.Formatting = Formatting.Indented;

				// Write the XML into a formatting XmlTextWriter
				xmlDoc.WriteContentTo(textWriter);
				textWriter.Flush();
				memStream.Flush();

				// Have to rewind the MemoryStream in order to read
				// its contents.
				memStream.Position = 0;

				// Read MemoryStream contents into a StreamReader.
				StreamReader SR = new StreamReader(memStream);

				// Extract the text from the StreamReader.
				String FormattedXML = SR.ReadToEnd();

				result = FormattedXML;
				}
			catch (XmlException) {
				}

			memStream.Close();
			textWriter.Close();

			return result;
			}

		public virtual bool WriteKeysToConfig(string Filename)
			{
			// *** Load the config file into DOM parser
			XmlDocument Dom = new XmlDocument();
			// string Namespace = null;

			try {
				Dom.Load(Filename);
				}
			catch {
				// *** Can't load the file - create an empty document
				string Xml =
					@"<?xml version='1.0'?>
<configuration>
</configuration>";

				Dom.LoadXml(Xml);
				}

			// *** Load up the Namespaces object so we can 
			// *** reference the appropriate default namespace
			this.GetXmlNamespaceInfo(Dom);

			// *** Parse through each of hte properties of the properties
			Type typeWebConfig = this.GetType();
			MemberInfo[] Fields = typeWebConfig.GetMembers();

			foreach (MemberInfo Field in Fields) {
				// *** If we can't find the key - write it out to the document
				string Value = null;
				object RawValue = null;
				if (Field.MemberType == MemberTypes.Field)
					RawValue = ((FieldInfo)Field).GetValue(this);
				else if (Field.MemberType == MemberTypes.Property)
					RawValue = ((PropertyInfo)Field).GetValue(this, null);
				else
					continue; // not a property or field

				Value = TypedValueToString(RawValue, CultureInfo.InvariantCulture);

				// *** Encrypt the field if in list
				if (this.EncryptFieldList.IndexOf("," + Field.Name.ToLower() + ",") > -1)
					Value = Encrypt.EncryptString(Value, this.EncryptKey);

				string ConfigSection = "appSettings";
				if (ConfigSectionName != "")
					ConfigSection = ConfigSectionName;

				XmlNode Node = Dom.DocumentElement.SelectSingleNode(
					this.XmlNamespacePrefix + ConfigSection + "/" +
					this.XmlNamespacePrefix + "add[@key='" + Field.Name + "']", XmlNamespaces);

				if (Node == null) {
					// *** Create the node and attributes and write it
					Node = Dom.CreateNode(XmlNodeType.Element, "add", Dom.DocumentElement.NamespaceURI);

					XmlAttribute Attr2 = Dom.CreateAttribute("key");
					Attr2.Value = Field.Name;
					XmlAttribute Attr = Dom.CreateAttribute("value");
					Attr.Value = Value;

					Node.Attributes.Append(Attr2);
					Node.Attributes.Append(Attr);

					XmlNode Parent = Dom.DocumentElement.SelectSingleNode(
						this.XmlNamespacePrefix + ConfigSection, this.XmlNamespaces);

					if (Parent == null)
						Parent = this.CreateConfigSection(Dom, ConfigSection);

					Parent.AppendChild(Node);
					}
				else {
					// *** just write the value into the attribute
					Node.Attributes.GetNamedItem("value").Value = Value;
					}


				string XML = Node.OuterXml;

				} // for each

			try {
				if (!Directory.Exists(Path.GetDirectoryName(Filename)))
					Directory.CreateDirectory(Path.GetDirectoryName(Filename));

				////Dom.Save(Filename);

				//String formattedXml = FormatXml(Dom.InnerXml);
				//String[] lines = formattedXml.Split(new Char[] { '\r', '\n' });
				//List<String> list = new List<String>();
				//foreach (String line in lines)
				//    if (line.Trim().Length > 0)
				//        list.Add(line);
				//IAScience.Files.WriteLinesToFile(Filename, list.ToArray(), false);

				////IAScience.Files.WriteLineToFile(Filename, Dom.InnerXml.Replace("><", ">" + Environment.NewLine + "<"), false);
#if PocketPC
				IAScience.Files.WriteLineToFile(Filename, Dom.InnerXml, false);
#else
				Dom.Save(Filename);
#endif
				}
			catch {
				return false;
				}

			return true;
			}

		/// <summary>
		/// Returns a single value from the XML in a configuration file.
		/// </summary>
		/// <param name="Dom"></param>
		/// <param name="Key"></param>
		/// <param name="ConfigSection"></param>
		/// <returns></returns>
		protected string GetNamedValueFromXml(XmlDocument Dom, string Key, string ConfigSection)
			{
			XmlNode Node = Dom.DocumentElement.SelectSingleNode(
				this.XmlNamespacePrefix + ConfigSection + @"/" +
				this.XmlNamespacePrefix + "add[@key='" + Key + "']", this.XmlNamespaces);

			if (Node == null)
				return null;

			return Node.Attributes["value"].Value;
			}

		public Object GetValueByName(String fieldName) {
			try {
				Type typeWebConfig = this.GetType();
				MemberInfo[] Fields = typeWebConfig.GetMembers(BindingFlags.Public | BindingFlags.Instance);

				foreach (MemberInfo Member in Fields) {
					if (String.Compare(Member.Name,fieldName) == 0) {
						if (Member.MemberType == MemberTypes.Field) {
							FieldInfo Field = (FieldInfo)Member;
							return Field.GetValue(this);
							}
						else if (Member.MemberType == MemberTypes.Property) {
							PropertyInfo Property = (PropertyInfo)Member;
							return Property.GetValue(this,null);
							}
						else
							return null;
						}
					}
				}
			catch {
				}

			return null;
			}
		/// <summary>
		/// Version of ReadKeysFromConfig that reads and writes an external Config file
		/// that is not controlled through the ConfigurationSettings class.
		/// </summary>
		/// <param name="Filename">The filename to read from. If the file doesn't exist it is created if permissions are available</param>
		public virtual void ReadKeysFromConfig(string Filename)
			{
			Type typeWebConfig = this.GetType();
			MemberInfo[] Fields = typeWebConfig.GetMembers(BindingFlags.Public |
				BindingFlags.Instance);

			// *** Set a flag for missing fields
			// *** If we have any we'll need to write them out 
			bool MissingFields = false;

			XmlDocument Dom = new XmlDocument();

			try {
				Dom.Load(Filename);
				}
			catch {
				// *** Can't open or doesn't exist - so create it
				if (!this.WriteKeysToConfig(Filename))
					return;

				// *** Now load again
				Dom.Load(Filename);
				}

			// *** Retrieve XML Namespace information to assign default 
			// *** Namespace explicitly.
			this.GetXmlNamespaceInfo(Dom);

			// *** Save the configuration file
			this.ConfigFilename = Filename;

			string ConfigSection = this.ConfigSectionName;
			if (ConfigSection == "")
				ConfigSection = "appSettings";

			foreach (MemberInfo Member in Fields) {
				FieldInfo Field = null;
				PropertyInfo Property = null;
				Type FieldType = null;
				string TypeName = null;

				if (Member.MemberType == MemberTypes.Field) {
					Field = (FieldInfo)Member;
					FieldType = Field.FieldType;
					TypeName = Field.FieldType.Name.ToLower();
					}
				else if (Member.MemberType == MemberTypes.Property) {
					Property = (PropertyInfo)Member;
					FieldType = Property.PropertyType;
					TypeName = Property.PropertyType.Name.ToLower();
					}
				else
					continue;

				string Fieldname = Member.Name;

				XmlNode Section = Dom.DocumentElement.SelectSingleNode(this.XmlNamespacePrefix + ConfigSection, this.XmlNamespaces);
				if (Section == null) {
					//Section = this.CreateConfigSection(Dom, ConfigSectionName); // XXX
					Section = this.CreateConfigSection(Dom, ConfigSection);
					Dom.DocumentElement.AppendChild(Section);
					}

				string Value = this.GetNamedValueFromXml(Dom, Fieldname, ConfigSection);
				if (Value == null) {
					MissingFields = true;
					continue;
					}

				Fieldname = Fieldname.ToLower();

				// *** If we're encrypting decrypt any field that are encyrpted
				if (Value != "" && this.EncryptFieldList.IndexOf("," + Fieldname + ",") > -1)
				    Value = Encrypt.DecryptString(Value, this.EncryptKey);

				//this.SetPropertyFromString(FieldType,Fieldname,Value);

				// *** Assign the Property
				SetPropertyEx(this, Fieldname,
					StringToTypedValue(Value, FieldType, CultureInfo.InvariantCulture));
				}

			// *** We have to write any missing keys
			if (MissingFields)
				this.WriteKeysToConfig(Filename);

			}


		}
#pragma warning restore CS8600
#pragma warning restore CS8602
#pragma warning restore CS8603
#pragma warning restore CS8604
#pragma warning restore CS8625
	}
