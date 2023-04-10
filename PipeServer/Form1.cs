

using ExindaPipeServer;
using IAScience;
//using IAScienceLib;

namespace PipeServerApp {
	public partial class Form1 : ExtForm {
		PipeServer? m_PipeServer1;
		PipeClient? m_PipeClient;
		bool m_debug = false;
		PipeServerConfig m_Config;

		public Form1()
			{
			InitializeComponent();

			m_Config = new PipeServerConfig();
			LoadConfig();

			SetProfileValue(ref m_Config.MainFormState);

			this.Load += Form1_Load;
			this.FormClosing += Form1_FormClosing1;
			}

		int m_CloseTimerTicks = 0;
		bool m_CloseFormCalled = false;

		private void LoadConfig()
			{
			pipeNameTextBox.Text = String.IsNullOrEmpty(m_Config.PipeName) ? "exindapipe" : m_Config.PipeName;
			autoStartCheckBox.Checked = m_Config.AutoStart;
			//startServerButton.Text = m_Config.AutoStart ? "Stop server" : "Start server";

			autoStartCheckBox.CheckedChanged += AutoStartCheckBox_CheckedChanged;
			startServerButton.Click += StartServerButton_Click;

			bool b = Directory.Exists("C:\\Users\\bobfarrell") || Directory.Exists("C:\\Users\\bob.farrell");
            openClientButton.Visible = closeClientButton.Visible = writeClientButton.Visible = false;// b;
			}
		private void SaveConfig()
			{
			if (!String.IsNullOrEmpty(pipeNameTextBox.Text))
				m_Config.PipeName = pipeNameTextBox.Text;
			m_Config.AutoStart = autoStartCheckBox.Checked;
			m_Config.WriteKeysToConfig();
			}
		private void Form1_FormClosing1(object? sender, FormClosingEventArgs e)
			{
			if (m_CloseFormCalled) {
				SaveConfig();
				}
			else { // The first time close is called, see if the server is running. If it is, ask it to close and try to wait for that to happen before closing the form
				m_CloseFormCalled = true;

				if ((m_PipeServer1 != null) && m_PipeServer1.IsRunning) {
					m_PipeServer1.Close();

					if (e.CloseReason == CloseReason.UserClosing) {
						e.Cancel = true;
						m_CloseTimerTicks = 0;
						new System.Windows.Forms.Timer() { Enabled = true, Interval = 100 }.Tick += CloseTimer_Tick;
						}
					}
				else
					SaveConfig();
				}
			}

		private void CloseTimer_Tick(object? sender, EventArgs e)
			{
			if (m_CloseTimerTicks++ == 20) // allow a couple of seconds, after that just close
				this.Close();
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

		private void AutoStartCheckBox_CheckedChanged(object? sender, EventArgs e)
			{
			m_Config.AutoStart = autoStartCheckBox.Checked;
			m_Config.WriteKeysToConfig();
			}

		private void Form1_Load(object? sender, EventArgs e)
			{
			string pipeName = String.IsNullOrEmpty(pipeNameTextBox.Text) ? "exindapipe" : pipeNameTextBox.Text;
			m_PipeServer1 = new PipeServer(pipeName, m_debug);
			m_PipeServer1.MessageReceived += PipeServer_MessageReceived;
			m_PipeServer1.OutputEvent += PipeServer1_OutputEvent;
			m_PipeServer1.ConnectedEvent += PipeServer_ConnectedEvent;
			m_PipeServer1.WaitingEvent += PipeServer_WaitingEvent;
			m_PipeServer1.DisconnectedEvent += PipeServer_DisconnectedEvent;
			m_PipeServer1.ClosedEvent += PipeServer_ClosedEvent;

			this.FormClosing += Form1_FormClosing;

			if (m_Config.AutoStart)
				StartPipeServer();
			}

		private void PipeServer_ClosedEvent(object? sender, EventArgs e)
			{
			if (m_CloseFormCalled) {
				//this.Close();
				this.Invoke(() => this.Close());
				}
			else {
				Invokes.SetControlText(startServerButton, "Start");
				Invokes.EnableControl(pipeNameTextBox, true);
				string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
				Output($"Pipe \"{pipeName}\" closed");
				}
			}

		void Output(string text)
			{
			Invokes.AppendTextbox(outputTextBox, text, true, true);
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

		private void PipeServer1_OutputEvent(object? sender, PipeMessageEventArgs e)
			{
			string pipeName = sender != null ? ((PipeServer)sender).PipeName : "unknown";
			Output($"{pipeName} : {e.Message}");
			}

		//private void PipeClient_OutputEvent(object? sender, PipeMessageEventArgs e)
		//	{
		//	Output(e.Message);
		//	}

		private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
			{
			m_PipeServer1?.Close();
			//m_PipeServer2?.Close();
			m_PipeClient?.Close();
			}

		private void PipeServer_MessageReceived(object? sender, PipeMessageEventArgs e)
			{
			Invokes.AppendTextbox(outputTextBox, e.Message, true, true);
			}

		private void OpenClientButton_Click(object sender, EventArgs e)
			{
			if ((m_PipeServer1 != null) && (m_PipeServer1.IsRunning)) {
				m_PipeClient = new PipeClient(m_PipeServer1.PipeName);
				m_PipeClient.Start();
				}
			else
				Output("You need to start the server before running the test client");
			}

		int m_TestCount = 0;
		private void WriteClientButton_Click(object sender, EventArgs e)
			{
			m_PipeClient?.Write($"Test {++m_TestCount}");
			}

		private void CloseClientButton_Click(object sender, EventArgs e)
			{
			m_PipeClient?.Close();
			m_PipeClient = null;
			}

		private void clearButton_Click(object sender, EventArgs e)
			{
			outputTextBox.Clear();
			}

		private void copyButton_Click(object sender, EventArgs e)
			{
			Clipboard.SetText(outputTextBox.Text);
			}
		}
	}