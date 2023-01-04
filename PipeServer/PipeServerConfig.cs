using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAScience;

namespace ExindaPipeServer {
	internal class PipeServerConfig:AppConfig {
		public ExtFormState MainFormState = new();
		public string PipeName = "exindapipe";
		public bool AutoStart = false;
		public string PipeConfigsDefinition = "pipename=exinda;autostart=false";

		public void SetPipeConfigsDefinition(List<OnePipeConfig> configs)
			{
			List<string> configStrings = new List<string>();
			foreach (OnePipeConfig config in configs)
				configStrings.Add(config.ToString());
			PipeConfigsDefinition = string.Join("|", configStrings.ToArray());
			}

		public List<OnePipeConfig> GetPipeConfigs() {
			List<OnePipeConfig> configs = new List<OnePipeConfig>();

			string[] entries = PipeConfigsDefinition.Split(new char[] { '|' });
			foreach (string entry in entries) {
				OnePipeConfig? config = OnePipeConfig.FromString(entry);
				if (config != null)
					configs.Add(config);
				}
			return configs;

			//= new OnePipeConfig[] { new OnePipeConfig() { AutoStart = false, PipeName = "exinda" } };
			}

		public static String ConfigFile {
			get {
				String path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "IAScience");
				path = Path.Combine(path, "PipeServer");
				IAScience.Paths.CreatePath(path);
				return Path.Combine(path, "PipeServer.xml");
				}
			}
		public PipeServerConfig()
			: base(ConfigFile)
			{
			}

		}
	public class OnePipeConfig {
		public string PipeName { get; set; } = "exindapipe";
		public bool AutoStart { get; set; } = false;
		public override string ToString()
			{
			return $"pipename={PipeName};autostart={AutoStart}";
			}
		public static OnePipeConfig? FromString(string str)
			{
			OnePipeConfig config = new OnePipeConfig();

			string[] entries = str.Split(new char[] { ';' });
			foreach (string s in entries) {
				if (IAScience.Utility.ParseConfigLine(s, out string paramName, out string paramVal)) {
					if (paramName == "pipename")
						config.PipeName= paramVal;
					else if (paramName=="autostart")
						config.AutoStart = paramVal.ToLower().Equals("true");
					}
				}
			return config;
			}
		}
	}
