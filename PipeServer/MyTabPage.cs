using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExindaPipeServer;
using IAScience;

namespace PipeServerApp {
	public class MyTabPage : TabPage {
		public PipeServerControl PipeServerControl { get; private set; }
		public OnePipeConfig PipeConfig { get; private set; }

		public MyTabPage(PipeServerControl pipeServerControl, OnePipeConfig config) {
			PipeServerControl= pipeServerControl;
			PipeConfig = config;
			PipeServerControl.PipeNameChanged += PipeServerControl_PipeNameChanged;
			}

		private void PipeServerControl_PipeNameChanged(object? sender, PipeNameChangedEventArgs e)
			{
			Invokes.SetControlText(this, e.NewName);
			PipeConfig.PipeName = e.NewName;
			}
		}
	}
