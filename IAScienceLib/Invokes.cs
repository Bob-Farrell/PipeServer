using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IAScience {
	public class Invokes {
		private static TextBox? m_OutputTextBox = null;
		public static TextBox? OutputTextBox { get { return m_OutputTextBox; } set { m_OutputTextBox = value; } }
		private static bool m_TraceOutput = false;
		public static bool TraceOutput { get { return m_TraceOutput; } set { m_TraceOutput = value; } }

		//delegate void AppendTextboxCallback(TextBox tb,String message, bool newline);
		//public static void AppendTextbox(TextBox tb, String message, bool newline) {
		//	if (tb.InvokeRequired) {
		//		AppendTextboxCallback d = new AppendTextboxCallback(AppendTextbox);
		//		tb.Invoke(d, new object[] { tb, message, newline });
		//		}
		//	else {
		//		tb.AppendText(message + (newline ? Environment.NewLine : ""));
		//		}
		//	}
		delegate void AppendTextboxCallback(TextBox tb, String message, bool newline, bool scrollToEnd);
		public static void AppendTextbox(TextBox tb, String message, bool newline, bool scrollToEnd = false) {
			if (tb.InvokeRequired) {
				AppendTextboxCallback d = new AppendTextboxCallback(AppendTextbox);
				tb.Invoke(d, new object[] { tb, message, newline, scrollToEnd });
				}
			else {
				tb.AppendText(message + (newline ? Environment.NewLine : ""));
				if (scrollToEnd) {
					tb.SelectionStart = tb.TextLength;
					tb.ScrollToCaret();
					}
				}
			}

		public static void Output(string message) {
			Output(message, true);
			}

		delegate void OutputCallback(String message, bool newline);
		public static void Output(String message, bool newline) {
			if (m_OutputTextBox == null)
				throw (new Exception("Invokes.Output called before setting OutputTextBox"));

			if (m_OutputTextBox.InvokeRequired) {
				OutputCallback d = new OutputCallback(Output);
				m_OutputTextBox.Invoke(d, new object[] { message, newline });
				}
			else {
				m_OutputTextBox.AppendText(message + (newline ? Environment.NewLine : ""));
				if (m_TraceOutput) System.Diagnostics.Trace.Write(message + (newline ? Environment.NewLine : ""));
				}
			}

		delegate void EnableControlCallback(Control c, bool enabled);
		public static void EnableControl(Control c, bool enabled) {
			if (c.InvokeRequired) {
				EnableControlCallback d = new EnableControlCallback(EnableControl);
				c.Invoke(d, new object[] { c, enabled });
				}
			else
				c.Enabled = enabled;
			}
		delegate void CheckBoxCheckCallback(CheckBox c, bool isChecked);
		public static void CheckBoxCheck(CheckBox c, bool isChecked) {
			if (c.InvokeRequired) {
				CheckBoxCheckCallback d = new CheckBoxCheckCallback(CheckBoxCheck);
				c.Invoke(d, new object[] { c, isChecked });
				}
			else
				c.Checked = isChecked;
			}

		delegate void RefreshControlCallback(Control c);
		public static void RefreshControl(Control c) {
			if (c.InvokeRequired) {
				RefreshControlCallback d = new RefreshControlCallback(RefreshControl);
				c.Invoke(d, new object[] { c });
				}
			else
				c.Refresh();
			}

		delegate void EnableControlsCallback(Control[] controls, bool enabled);
		public static void EnableControls(Control[] controls, bool enabled)
			{
			if (controls[0].InvokeRequired) {
				EnableControlsCallback d = new EnableControlsCallback(EnableControls);
				controls[0].Invoke(d, new object[] { controls, enabled });
				}
			else {
				foreach(Control c in controls)
					c.Enabled = enabled;
				}
			}

		delegate void SetControlVisibleCallback(Control c, bool visible);
		public static void SetControlVisible(Control c, bool visible) {
			if (c.InvokeRequired) {
				SetControlVisibleCallback d = new SetControlVisibleCallback(SetControlVisible);
				c.Invoke(d, new object[] { c, visible });
				}
			else
				c.Visible = visible;
			}
		delegate void SetFormVisibleCallback(Form form, bool visible);
		public static void SetFormVisible(Form form, bool visible) {
			if (form.InvokeRequired) {
				SetFormVisibleCallback d = new SetFormVisibleCallback(SetFormVisible);
				form.Invoke(d, new object[] { form, visible });
				}
			else
				form.Visible = visible;
			}

		delegate void SetControlColorCallback(Control c, Color color);
		public static void SetControlBackColor(Control c, Color backColor) {
			if (c.InvokeRequired) {
				SetControlColorCallback d = new SetControlColorCallback(SetControlBackColor);
				c.Invoke(d, new object[] { c, backColor });
				}
			else
				c.BackColor = backColor;
			}
		public static void SetControlForeColor(Control c, Color foreColor) {
			if (c.InvokeRequired) {
				SetControlColorCallback d = new SetControlColorCallback(SetControlBackColor);
				c.Invoke(d, new object[] { c, foreColor });
				}
			else
				c.ForeColor = foreColor;
			}

		delegate void SetControlTextCallback(Control c, string s);
		public static void SetControlText(Control c, string s) {
			if (c.InvokeRequired) {
				SetControlTextCallback d = new SetControlTextCallback(SetControlText);
				c.Invoke(d, new object[] { c, s });
				}
			else
				c.Text = s;
			}

		delegate void BringToFrontCallback(Control c);
		public static void BringToFront(Control c)
			{
			if (c.InvokeRequired) {
				BringToFrontCallback d = new BringToFrontCallback(BringToFront);
				c.Invoke(d, new object[] { c });
				}
			else
				c.BringToFront();
			}

		delegate string GetControlTextCallback(Control c);
		public static string GetControlText(Control c) {
			if (c.InvokeRequired) {
				GetControlTextCallback d = new GetControlTextCallback(GetControlText);
				return (string) c.Invoke(d, new object[] { c });
				}
			else
				return c.Text;
			}
		}
	}
