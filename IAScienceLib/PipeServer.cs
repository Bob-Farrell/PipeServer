using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IAScience
	{
	public class PipeServer
		{
		public event EventHandler<PipeMessageEventArgs>? OutputEvent;
		public event EventHandler<PipeMessageEventArgs>? MessageReceived;
		public event EventHandler? WaitingEvent;
		public event EventHandler? ConnectedEvent;
		public event EventHandler? DisconnectedEvent;
		public event EventHandler? ClosedEvent;

		public bool IsWaiting { get; private set; } = false;
		public bool IsConnected { get; private set; } = false;

      public bool IsRunning { get; private set; } = false;
		public string PipeName { get; set; } = "namedPipe";

		private bool m_debug = false;

		public PipeServer(string pipename, bool debug = false) {
         PipeName = pipename;
			m_debug = debug;
			}

		public void Close() {
         if (m_CancelToken != null)
            m_CancelToken.Cancel();
         }
      private void Output(string s) {
			OutputEvent?.Invoke(this,new PipeMessageEventArgs() { Message = s });
			}
		private void DebugOutput(string s) {
			if (m_debug)
				OutputEvent?.Invoke(this,new PipeMessageEventArgs() { Message = s });
			}

      CancellationTokenSource m_CancelToken = new CancellationTokenSource();
      
		// This is a bit hacky, it was not originally designed for asynchronous operation or being able to stop and restart the pipe server, and was adapted in a hurry
		// Anyway that's my excuse, it's just for testing and it works, so chill
		public void Start() {
			if (IsRunning)
				return;

			string pipeName = PipeName;

         DebugOutput("Starting communication pipe server " + pipeName);
			Task.Factory.StartNew(async () => {

				bool wasException = false;
				IsRunning = true;
				IsWaiting= false;
				IsConnected= false;

            m_CancelToken = new CancellationTokenSource();

            while (!m_CancelToken.IsCancellationRequested) {
					NamedPipeServerStream? server = null;
					StreamReader? reader = null;

               if (wasException) {
						Thread.Sleep(1000);
						wasException = false;
						}

					try {
						//var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
						//var account = (NTAccount)sid.Translate(typeof(NTAccount));
						//var rule = new PipeAccessRule(
						//	account.ToString(),
						//	PipeAccessRights.FullControl,
						//	AccessControlType.Allow);
						//PipeSecurity pipeSecurity = new PipeSecurity();
						//pipeSecurity.AddAccessRule(rule);

						//PipeSecurity pipeSecurity = new PipeSecurity();
						//pipeSecurity.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
						//pipeSecurity.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
						//pipeSecurity.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));
						////ps.AddAccessRule(pa);

						//PipeSecurity pipeSecurity = new PipeSecurity();
						//pipeSecurity.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
						//	PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
						//pipeSecurity.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null),
						//  PipeAccessRights.FullControl, AccessControlType.Allow));
						//pipeSecurity.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null),
						//  PipeAccessRights.FullControl, AccessControlType.Allow));

						server = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, 
							PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
						//server.SetAccessControl(pipeSecurity);

						reader = new StreamReader(server);

						DebugOutput($"Communication pipe server waiting for connection on {pipeName}");

						IsWaiting = true;
						WaitingEvent?.Invoke(this, EventArgs.Empty);

						await server.WaitForConnectionAsync(m_CancelToken.Token);
						//server.WaitForConnection();

						IsConnected = true;
						ConnectedEvent?.Invoke(this, EventArgs.Empty);
						
						DebugOutput($"Communication pipe server listening on pipe {pipeName}");
						while (server.IsConnected) {
							var line = await reader.ReadLineAsync(m_CancelToken.Token);
							DebugOutput("Comms pipe server message received: " + line);

							MessageReceived?.Invoke(this, new PipeMessageEventArgs() { Message = line ?? "" });
							}

						IsConnected = false;
						DisconnectedEvent?.Invoke(this, EventArgs.Empty);
						DebugOutput($"Comms pipe {pipeName} disconnected");
						}
					catch (OperationCanceledException) {
                  DebugOutput("PipeServer cancelled");
                  wasException = true;
                  }
               catch (Exception ex) {
						Output("Exception in PipeServer.RunPipeServer: " + ex.Message);
						wasException = true;
						m_CancelToken.Cancel();
						}
					finally {
						if (server != null) {
							try { server.Close(); } catch { } finally { server = null; }
							}
						if (reader != null) {
							try { reader.Close(); } catch { } finally { reader = null; }
							}
						}
               }

            IsWaiting = false;
            IsConnected = false;
				IsRunning = false;
            ClosedEvent?.Invoke(this, EventArgs.Empty);
         });
			}
		}
	public class PipeMessageEventArgs : EventArgs
		{
		public string Message { get; set; } = "";
		}
	}
