using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IAScience {
	// A reference type rectangle class
	public class ExtFormState {
		int m_Left = 0;
		int m_Top = 0;
		int m_Width = 0;
		int m_Height = 0;
		int m_State = 0;	// -1 -> minimized, 0 -> Normal, 1 -> Maximized
		int m_Visible = 1;
		// bool m_Maximized = false;

		public string ToDisplayString()
			{
			string state = m_State == -1 ? "Minimized" : m_State == 1 ? "Maximized" : "Normal";
			string visible = m_Visible == 1 ? "true" : "false";
			return $"Location: ({m_Left}, {m_Top}), Size: ({m_Width}x{m_Height}), State: {state}, Visible: {visible}";
			//StringBuilder sb = new StringBuilder();
			//sb.Append($
			}
		public ExtFormState() {
			}
		public ExtFormState(ExtFormState s) {
			Set(s.m_Left, s.m_Top, s.m_Width, s.m_Height, s.m_State);
			}
		public ExtFormState(int left, int top, int width, int height) {
			Set(left, top, width, height);
			}
		public ExtFormState(int left, int top, int width, int height,int state) {
			Set(left, top, width, height, state);
			}
		public bool EqualTo(ExtFormState s) {
			return (m_Left == s.m_Left) && (m_Top == s.m_Top)
				&& (m_Width == s.m_Width) && (m_Height == s.m_Height)
				&& (m_State == s.m_State);
			}

		public void Set(Rectangle rect) {
			Left = rect.Left;
			Top = rect.Top;
			Width = rect.Width;
			Height = rect.Height;
			}
		public void Set(Rectangle rect, int state) {
			Set(rect);
			m_State = state;
			}
		public void Set(int left, int top, int width, int height) {
			Left = left;
			Top = top;
			Width = width;
			Height = height;
			}
		public void Set(int left, int top, int width, int height, int state) {
			Left = left;
			Top = top;
			Width = width;
			Height = height;
			m_State = state;
			}
		public bool Visible {
			get { return m_Visible != 0; }
			set { m_Visible = value ? 1 : 0; }
			}
		public int Left {
			get { return m_Left; }
			set { m_Left = value; }
			}
		public int Top {
			get { return m_Top; }
			set { m_Top = value; }
			}
		public int Width {
			get { return m_Width; }
			set { m_Width = value; }
			}
		public int Height {
			get { return m_Height; }
			set { m_Height = value; }
			}
		public int Area {
			get { return m_Width * m_Height; }
			}
		public bool Maximized {
			get { return m_State == 1; }
			set { m_State = value ? 1 : 0; }
			}
		public bool Minimized {
			get { return m_State == -1; }
			set { m_State = value ? -1 : 0; }
			}
		public override String ToString()
			{
			return m_Left.ToString() + ", " + m_Top.ToString() + ", " + m_Width.ToString() + ", " + m_Height.ToString()
				+ ", " + m_State.ToString() + ", " + m_Visible.ToString();
			}
		public static ExtFormState FromString(String s) {
			String[] elements = s.Split(new Char[] { ',' });
			ExtFormState state = new ExtFormState();
			for (int n = 0; n < elements.Length; n++)
				state.SetElement(n, elements[n]);
			return state;
			}
		private void SetElement(int elementIndex,int elementValue) {
			switch (elementIndex) {
				case 0: Left = elementValue; break;
				case 1: Top = elementValue; break;
				case 2: Width = elementValue; break;
				case 3: Height = elementValue; break;
				case 4: m_State = elementValue < 0 ? -1 : elementValue > 0 ? 1 : 0; break;	//  Maximized = elementValue == 1 ? true : false; break;
				case 5: m_Visible = elementValue; break;	//  Maximized = elementValue == 1 ? true : false; break;
				}
			}
		private void SetElement(int elementIndex,String elementValue) {
			try {
				SetElement(elementIndex,System.Convert.ToInt32(elementValue));
				}
			catch {
				}
			}
		}
	}
