using IAScience;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExindaPipeServer;

namespace PipeServerApp {
	public partial class PipeServerControl : UserControl {
		public event EventHandler? ConfigChanged;
		public event EventHandler<PipeNameChangedEventArgs>? PipeNameChanged;
		public event EventHandler? PipeClosed;

		PipeServer? m_PipeServer1;
		PipeClient? m_PipeClient;
		bool m_debug = false;

		public PipeServer? PipeServer { get { return m_PipeServer1; } }

		public OnePipeConfig Config { get; private set; }

		//public string PipeName { get; private set; }
		//public bool AutoStart { get; set; } = false;
		public bool WaitingForClosed { get; set; } = false;
		public PipeServerControl(OnePipeConfig config)
			{
			InitializeComponent();
			Config= config;

			pipeNameTextBox.Text = String.IsNullOrEmpty(Config.PipeName) ? "exindapipe" : Config.PipeName;
			autoStartCheckBox.Checked = Config.AutoStart;
			//startServerButton.Text = m_Config.AutoStart ? "Stop server" : "Start server";

			autoStartCheckBox.CheckedChanged += AutoStartCheckBox_CheckedChanged;
			pipeNameTextBox.TextChanged += PipeNameTextBox_TextChanged;
			startServerButton.Click += StartServerButton_Click;

			bool b = false;// Directory.Exists("C:\\Users\\bobfarrell") || Directory.Exists("C:\\Users\\bob.farrell");
			openClientButton.Visible = closeClientButton.Visible = writeClientButton.Visible = b;

			openClientButton.Click += OpenClientButton_Click;
			writeClientButton.Click += WriteClientButton_Click;
			closeClientButton.Click += CloseClientButton_Click;
			clearButton.Click += ClearButton_Click;
			copyButton.Click += CopyButton_Click;

			this.Load += PipeServerControl_Load;
			}
		public bool StopServer(bool waitingToCloseForm)
			{
			if ((m_PipeServer1 == null) || !m_PipeServer1.IsRunning)
				return false;
			else {
				WaitingForClosed = waitingToCloseForm;
				m_PipeServer1?.Close();
				return true;
				}
			}
		private void PipeNameTextBox_TextChanged(object? sender, EventArgs e)
			{
			Config.PipeName = pipeNameTextBox.Text;
			PipeNameChanged?.Invoke(this, new PipeNameChangedEventArgs() { NewName = Config.PipeName });
			}

		private void ClearButton_Click(object? sender, EventArgs e)
			{
			outputTextBox.Clear();
			}

		private void CopyButton_Click(object? sender, EventArgs e)
			{
			Clipboard.SetText(outputTextBox.Text);
			}
		int m_TestCount = 0;
		private void WriteClientButton_Click(object? sender, EventArgs e)
			{
			m_PipeClient?.Write($"Test {++m_TestCount}");
			}

		private void CloseClientButton_Click(object? sender, EventArgs e)
			{
			m_PipeClient?.Close();
			m_PipeClient = null;
			}

		private void OpenClientButton_Click(object? sender, EventArgs e)
			{
			if ((m_PipeServer1 != null) && (m_PipeServer1.IsRunning)) {
				m_PipeClient = new PipeClient(m_PipeServer1.PipeName);
				m_PipeClient.Start();
				}
			else
				Output("You need to start the server before running the test client");
			}


		private void PipeServerControl_Load(object? sender, EventArgs e)
			{
			m_PipeServer1 = new PipeServer(Config.PipeName, m_debug);
			m_PipeServer1.MessageReceived += PipeServer_MessageReceived;
			m_PipeServer1.OutputEvent += PipeServer1_OutputEvent;
			m_PipeServer1.ConnectedEvent += PipeServer_ConnectedEvent;
			m_PipeServer1.WaitingEvent += PipeServer_WaitingEvent;
			m_PipeServer1.DisconnectedEvent += PipeServer_DisconnectedEvent;
			m_PipeServer1.ClosedEvent += PipeServer_ClosedEvent;

			//this.FormClosing += Form1_FormClosing;

			if (Config.AutoStart)
				StartPipeServer();
			}
		//public void Close(bool waitingToCloseForm = false)
		//	{
		//	WaitingForClosed = waitingToCloseForm;
		//	if ((m_PipeServer1 != null) && m_PipeServer1.IsRunning)
		//		m_PipeServer1?.Close();
		//	}
		private void PipeServer_MessageReceived(object? sender, PipeMessageEventArgs e)
			{
			Output(e.Message);
			}
		private void PipeServer1_OutputEvent(object? sender, PipeMessageEventArgs e)
			{
			string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
			Output($"{pipeName} : {e.Message}");
			}
		private void PipeServer_ClosedEvent(object? sender, EventArgs e)
			{
			if (!WaitingForClosed) {
				Invokes.SetControlText(startServerButton, "Start");
				Invokes.EnableControl(pipeNameTextBox, true);
				string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
				Output($"Pipe \"{pipeName}\" closed");
				}

			PipeClosed?.Invoke(this, EventArgs.Empty);
			}

		private void PipeServer_DisconnectedEvent(object? sender, EventArgs e)
			{
			string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
			Output($"Pipe \"{pipeName}\" disconnected");
			}

		private void PipeServer_WaitingEvent(object? sender, EventArgs e)
			{
			string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
			Output($"Pipe \"{pipeName}\" waiting for connection");
			Invokes.SetControlText(startServerButton, "Stop");
			Invokes.EnableControl(startServerButton, true);
			}

		private void PipeServer_ConnectedEvent(object? sender, EventArgs e)
			{
			string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
			Output($"Pipe \"{pipeName}\" connected");
			}

		void Output(string text)
			{
			Invokes.AppendTextbox(outputTextBox, text, true, true);
			}

		private void StartServerButton_Click(object? sender, EventArgs e)
			{
			if (m_PipeServer1 == null)
				return;

			if (m_PipeServer1.IsWaiting) {
				m_PipeServer1.Close();
				Invokes.SetControlText(startServerButton, "Stopping...");
				}
			else {
				StartPipeServer();
				}
			}
		private void StartPipeServer()
			{
			if (m_PipeServer1 == null)
				return;

			m_PipeServer1.PipeName = String.IsNullOrEmpty(pipeNameTextBox.Text) ? "exindapipe" : pipeNameTextBox.Text;
			m_PipeServer1.Start();
			Invokes.SetControlText(startServerButton, "Starting...");
			Invokes.EnableControl(pipeNameTextBox, false);
			}

		private void AutoStartCheckBox_CheckedChanged(object? sender, EventArgs e)
			{
			Config.AutoStart = autoStartCheckBox.Checked;
			ConfigChanged?.Invoke(this, EventArgs.Empty);
			//m_Config.WriteKeysToConfig();
			}

		}
	public class PipeNameChangedEventArgs : EventArgs {
		public string NewName { get; set; } = "";
		}
	}
