using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAScience
	{
	public class PipeClient
		{
		public event EventHandler<PipeMessageEventArgs>? OutputEvent;

		private NamedPipeClientStream? m_PipeClient = null;
		private StreamWriter? m_PipeWriter = null;

		private string m_PipeName;

		public PipeClient(string pipeName) {
			m_PipeName = pipeName;
			}
		public void Write(string s) {
			if (m_PipeWriter != null) {
				m_PipeWriter.WriteLine(s);
				m_PipeWriter.Flush();
				}
			}
		public void Close() {
			if (m_PipeWriter != null) {
				m_PipeWriter.Close();
				m_PipeWriter = null;
				}

			if (m_PipeClient != null) {
				m_PipeClient.Close();
				m_PipeClient = null;
				}
			}
		public bool Start(int timeout = 5000) {
			m_PipeClient = new NamedPipeClientStream(".", m_PipeName, PipeDirection.InOut);

			try {
				m_PipeClient.Connect(timeout);
				}
			catch (Exception ex) {
				Output("## Unable to open a connection to the PDF viewer application - please check that Acrobat Reader and the .NET Framework 4.7.2 Runtime are installed (" + ex.Message + ")");
				m_PipeClient = null;
				return false;
				}

			if (m_PipeClient.IsConnected) {
				//m_PipeReader = new StreamReader(m_DisplayPdfPipeClient);
				m_PipeWriter = new StreamWriter(m_PipeClient);
				Output("Comms pipe client process is running on pipe " + m_PipeName);
				return true;
				}
			else {
				Output("## Unable to open a communication pipe with the PDF viewer");
				return false;
				}
			}

		private void Output(string s) {
			OutputEvent?.Invoke(this, new PipeMessageEventArgs() { Message = s });
			}
		}
	}
