using ExindaPipeServer;
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

namespace PipeServerApp {
	public partial class TabbedForm : ExtForm {
		PipeServerConfig m_Config;
		List<OnePipeConfig> m_PipeConfigs;

		public TabbedForm()
			{
			InitializeComponent();
			pipeServerTabs.TabPages.Clear();
			pipeServerTabs.MouseDown += PipeServerTabs_MouseDown;
			pipeServerTabs.TabIndexChanged += PipeServerTabs_TabIndexChanged;

			m_Config = new PipeServerConfig();
			m_PipeConfigs = m_Config.GetPipeConfigs();

			LoadConfig();

			SetProfileValue(ref m_Config.MainFormState);

			this.Load += Form1_Load;
			this.FormClosing += Form1_FormClosing1;
			}

		private void PipeServerTabs_TabIndexChanged(object? sender, EventArgs e)
			{
			Debug.WriteLine($"PipeServerTabs_TabIndexChanged, Tab index {pipeServerTabs.SelectedIndex}");
			}

		private void PipeServerTabs_MouseDown(object? sender, MouseEventArgs e)
			{
			Console.WriteLine($"PipeServerTabs_MouseDown {e.Location}");
			System.Diagnostics.Trace.WriteLine($"PipeServerTabs_MouseDown {e.Location}");
			Debug.WriteLine($"PipeServerTabs_MouseDown {e.Location}, Tab index {pipeServerTabs.SelectedIndex}");
			
			if (e.Button == MouseButtons.Middle) {
				}
			}

		int m_CloseTimerTicks = 0;
		bool m_CloseFormCalled = false;

		void AddTab(OnePipeConfig? config)
			{
			if (config != null) {
				PipeServerControl control= new PipeServerControl(config);
				control.ConfigChanged += PipeServerControl_ConfigChanged;
				control.PipeClosed += Control_PipeClosed;

				MyTabPage newPage = new MyTabPage(control, config);
				newPage.Text= config.PipeName;
				//control.Size = newPage.Size;
				newPage.Resize += NewPage_Resize;
				pipeServerTabs.TabPages.Add(newPage);
				newPage.Controls.Add(control);
				pipeServerTabs.SelectedTab= newPage;
				}
			}
		private bool NoServersRunning() {
			foreach (MyTabPage? page in pipeServerTabs.TabPages) {
				PipeServer? pipeServer = page == null ? null : page.PipeServerControl.PipeServer;
				if ((pipeServer != null) && pipeServer.IsRunning)
					return false;
				}
			return true;
			}
		private void Control_PipeClosed(object? sender, EventArgs e)
			{
			Debug.WriteLine("Control_PipeClosed");
			//if (m_CloseFormCalled) {
			//	foreach (MyTabPage? page in pipeServerTabs.TabPages) {
			//		PipeServer? pipeServer = page == null ? null : page.PipeServerControl.PipeServer;
			//		if ((pipeServer != null) && pipeServer.IsRunning)
			//			return;
			//		}
			//	this.Close();
			//	}
			}

		private void NewPage_Resize(object? sender, EventArgs e)
			{
			if (sender != null) {
				MyTabPage newPage = (MyTabPage)sender;
				newPage.PipeServerControl.Size = newPage.Size;
				}
			}

		private void PipeServerControl_ConfigChanged(object? sender, EventArgs e)
			{
			UpdateConfig();
			}

		private void LoadConfig()
			{
			if (m_PipeConfigs.Count == 0) {
				m_PipeConfigs = new List<OnePipeConfig>() { new OnePipeConfig() { PipeName = "exinda", AutoStart = false } };
				}

			foreach (OnePipeConfig config in m_PipeConfigs)
				AddTab(config);

			}
		private void UpdateConfig()
			{
			//if (!String.IsNullOrEmpty(pipeNameTextBox.Text))
			//	m_Config.PipeName = pipeNameTextBox.Text;
			//m_Config.AutoStart = autoStartCheckBox.Checked;
			m_Config.SetPipeConfigsDefinition(m_PipeConfigs);
			m_Config.WriteKeysToConfig();
			}
		//private void CloseAllPipes()
		//	{
		//	foreach (PipeServerControl control in this.pipeServerTabs.TabPages) {
		//		}
		//	}
		bool CloseAllPipeServers()
			{
			bool closedOne = false;
			foreach(MyTabPage? page in pipeServerTabs.TabPages) {
				if (StopServer(page, true)) 
					closedOne = true;
				}
			return closedOne;
			}
		private void Form1_FormClosing1(object? sender, FormClosingEventArgs e)
			{
			if (m_CloseFormCalled) {
				UpdateConfig();
				}
			else { // The first time close is called, see if any server is running. If any are they are asked to close and we try to wait for that to happen before closing the form
				m_CloseFormCalled = true;

				if (CloseAllPipeServers() && (e.CloseReason == CloseReason.UserClosing)) {
					e.Cancel = true;
					m_CloseTimerTicks = 0;
					new System.Windows.Forms.Timer() { Enabled = true, Interval = 100 }.Tick += CloseTimer_Tick;
					}
				else
					UpdateConfig();
				}
			}

		private void CloseTimer_Tick(object? sender, EventArgs e)
			{
			if (NoServersRunning() || (m_CloseTimerTicks++ == 40)) // allow a couple of seconds, after that just close
				this.Close();
			}

		private void Form1_Load(object? sender, EventArgs e)
			{
			if (pipeServerTabs.TabPages.Count > 0)
				pipeServerTabs.SelectedIndex = 0;
			}


		private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
			{
			//m_PipeServer1?.Close();
			////m_PipeServer2?.Close();
			//m_PipeClient?.Close();
			// Close all tabs
			}
		bool PipeNameExists(string pipeName)
			{
			foreach (MyTabPage? page in pipeServerTabs.TabPages) {
				if ((page != null) && (page.PipeConfig.PipeName == pipeName))
					return true;
				}
			return false;
			}

		private void NewTabButton_Click(object sender, EventArgs e)
			{
			int index = 1;
			while (PipeNameExists("new pipe " + index))
				index++;

			OnePipeConfig config = new OnePipeConfig(){ PipeName = "new pipe " + index, AutoStart=false };
			m_PipeConfigs.Add(config);
			AddTab(config);
			}
		private bool StopServer(MyTabPage? page, bool waitingToCloseForm) 
			{
			if (page == null)
				return false;

			return page == null ? false : page.PipeServerControl.StopServer(waitingToCloseForm);
			}
		private void DeleteButton_Click(object sender, EventArgs e)
			{
			if (pipeServerTabs.TabPages.Count < 2)
				return;

			MyTabPage? page = (MyTabPage)pipeServerTabs.SelectedTab;
			if (page != null) {
				page.PipeServerControl.StopServer(false);
				if (m_PipeConfigs.Contains(page.PipeConfig))
					m_PipeConfigs.Remove(page.PipeConfig);

				pipeServerTabs.TabPages.Remove(page);
				}
			}
		}
	}
