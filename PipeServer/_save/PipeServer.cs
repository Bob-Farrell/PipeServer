using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IAScience
	{
	public class PipeServer
		{
		public event EventHandler<PipeMessageEventArgs>? OutputEvent;
		public event EventHandler<PipeMessageEventArgs>? MessageReceived;

		private bool m_Running = true;
		private bool m_debug;
		private string m_PipeName;

		public PipeServer(string pipename, bool debug = false) {
			m_PipeName = pipename;
			m_debug = debug;
			}

		public void Close() {
			m_Running = false; 
			}
		private void Output(string s) {
			OutputEvent?.Invoke(this,new PipeMessageEventArgs() { Message = s });
			}
		private void DebugOutput(string s) {
			if (m_debug)
				OutputEvent?.Invoke(this,new PipeMessageEventArgs() { Message = s });
			}
		
		public void Start() {
			Output("Starting communication pipe server " + m_PipeName);
			Task.Factory.StartNew(async () => {

				bool wasException = false;

				while (m_Running) {
					NamedPipeServerStream? server = null;
					StreamReader? reader = null;
					StreamWriter? writer = null;
					if (wasException) {
						Thread.Sleep(1000);
						wasException = false;
						}

					try {
						server = new NamedPipeServerStream(m_PipeName, PipeDirection.InOut);
						reader = new StreamReader(server);
						//writer = new StreamWriter(server);

						Output("Communication pipe server waiting for connection");
						server.WaitForConnection();
						Output($"Communication pipe server listening on pipe {m_PipeName}");
						while (server.IsConnected) {
							var line = await reader.ReadLineAsync();
							//var line = reader.ReadLine();

							DebugOutput("Comms pipe server message received: " + line);

							MessageReceived?.Invoke(this, new PipeMessageEventArgs() { Message=(line == null ? "" : line) });
							}

						Output("Comms pipe disconnected");
						}
					catch (Exception ex) {
						Output("Exception in PipeServer.RunPipeServer: " + ex.Message);
						wasException = true;
						}
					finally {
						if (server != null) {
							try { server.Close(); } catch { }
							}
						if (reader != null) {
							try { reader.Close(); } catch { }
							}
						if (writer != null) {
							try { writer.Close(); } catch { }
							}
						}
					}
			});
			}
		}
	public class PipeMessageEventArgs : EventArgs
		{
		public string Message { get; set; } = "";
		}
	}
