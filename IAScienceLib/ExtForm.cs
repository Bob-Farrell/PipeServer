using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

/* A Form class with some standard form functions
 *		- Save/restore form location & size by calling SetProfileRect(ref profileMyRectangleMember) 
 *			with the data member that stores the form rect,e.g. a data member of an AppConfig-derived class
 *			You can call this function after InitializeComponent() in the form constructor
 * 
 *		e.g. put this in the constructor after InitializeComponent()
 *		
 *			public Form1() {
 *				InitializeComponent();
 *				SetProfileValue(ref _AppConfig.MainFormState);	// _AppConfig must have been loaded, obviously
 *				...
 *					
 *		and don't forget! the config file needs to be updated when the form is closed
 *		
 *			private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
 *				_AppConfig.WriteKeysToConfig();
 *				...
 * 
 *		If you need to delay calling SetProfileValue(...), say because the config file can't be loaded,
 *		you can call	SetProfileValue(ref _AppConfig.MainFormState,true);
 *		from the Form1_Load or Form1_Shown handlers
 * 
 *		- Ctrl-Alt-H can move the form to a home position, on by default
 *		
 *		AllowUpdates is used to prevent the size & position being changed in the case where you create a Form and 
 *		call Show() or ShowDialog() on it more than once, i.e. where the reference is kept after the form is closed so you don't need to create the form again.
 *		In this case you need to reset AllowUpdates when you're showing the Form for not-the-first time, and Set it again in a form Load handler
 *		
 *		SomeForm m_SomeForm;
 *		void ShowSomeForm()
 *			if (m_SomeForm == null) {
 *				m_SomeForm = new SomeForm();
 *				m_SomeForm.Load += new EventHandler(SomeForm_Load);
 *				}
 *			else { // block size & position updates, otherwise m_FormState will be updated with the default size & position when Show() or ShowDialog() is called
 *				m_SomeForm.AllowUpdates = false;	
 *				}
 *		
 *			m_SomeForm.ShowDialog();
 *			}
 *		
 *		
 *		void SomeForm_Load(object sender, EventArgs e) {
 *			m_SomeForm.AllowUpdates = true;
 *			}
 *		
 */
namespace IAScience {
#pragma warning disable IDE0018 // Inline variable declaration
	public partial class ExtForm : Form {
#if !PocketPC
		ExtFormState? m_FormState = null;
		public bool AutoUpdateFormState { get; set; } = true;
		public bool MoveToHomePos = true;	// move to home position on ctrl-alt-H
		public bool SetFormLocation = true;
		private bool DoUpdateFormState = true;
		public bool m_Changed = false;
		public bool CheckPositionIsOnScreen { get; set; } = true;

#endif

#if (PocketPC || WindowsCE)
		private bool _FirstActivation = true;
		protected bool m_Active = false;
		protected int m_FormIndex;

		protected static int m_NextFormIndex = 0;
		protected static ArrayList m_AllForms = new ArrayList();
#endif

		public bool SetFormSize = true;
		public static bool GlobalNoResize = false;

		protected bool m_Initialised = false;

		public ExtForm() {
			InitializeComponent();
			AutoUpdateFormState = true;

#if !PocketPC
			this.Resize += new EventHandler(ExtForm_Resize);
#endif

#if (PocketPC || WindowsCE)
			m_FormIndex = m_NextFormIndex++;
			m_AllForms.Add(this);
			this.Activated += new EventHandler(ExtForm_Activated);
			this.Deactivate += new EventHandler(ExtForm_Deactivate);
			this.Closing += new CancelEventHandler(ExtForm_Closing);
#else
			this.Move += new EventHandler(ExtForm_Move);
			this.Shown += new EventHandler(ExtForm_Shown);
			//this.KeyDown += new KeyEventHandler(ExtForm_KeyDown);
#endif
			if (GlobalNoResize)	// suppresses the form size being set to what it was last time it closed
				this.SetFormSize = false;
			}

// Code to implement functionality of Form.ActiveForm of .NET framework
#if (PocketPC || WindowsCE)
		void ExtForm_Activated(object sender, EventArgs e) {
			m_Active = true;
			if (_FirstActivation) {
				m_Initialised = true;
				_FirstActivation = false;
#if WindowsCE
				CheckFormSize();
#endif
				}
			}
#if WindowsCE
		Panel FindMainPanel() {
			int count = this.Controls.Count;
			for (int n = 0; n < count; n++)
				if (this.Controls[n].Name.Equals("mainPanel", StringComparison.InvariantCultureIgnoreCase))
					return (Panel)this.Controls[n];
			return null;
			}

		Timer _Timer;

		void CheckFormSize() {

			Size size = Screen.PrimaryScreen.WorkingArea.Size;
			//Size s2 = this.Size;
			//MessageBox.Show(String.Format("({0:F0},{1:F1}) ({0:F0},{1:F1})", size.Width, size.Height, s2.Width, s2.Height));

			if (this.Width > size.Width)
				this.Width = size.Width;
			if (this.Height > size.Height-4)
				this.Height = size.Height-4;

			Panel mainPanel = FindMainPanel();
			if (mainPanel != null) {
				// Create a timer with a ten second interval.
				_Timer = new Timer();	// System.Timers.Timer(10000);
				_Timer.Tick += new EventHandler(_Timer_Tick);
				_Timer.Interval = 500;
				_Timer.Enabled = true;
				}
			}

		void _Timer_Tick(object sender, EventArgs e) {
			_Timer.Enabled = false;
			_Timer.Dispose();
			_Timer = null;
			Panel mainPanel = FindMainPanel();
			Point p = mainPanel.AutoScrollPosition;
			//MessageBox.Show(String.Format("({0:F0},{1:F1})", p.X, p.Y));
			mainPanel.AutoScrollPosition = new Point(0, 0);
			}

		void mainPanel_Resize(object sender, EventArgs e) {
			Panel mainPanel = FindMainPanel();
			Point p = mainPanel.AutoScrollPosition;
			MessageBox.Show(String.Format("({0:F0},{1:F1})", p.X, p.Y));
			mainPanel.AutoScrollPosition = new Point(0, 0);
			}
#endif
		void ExtForm_Deactivate(object sender, EventArgs e) {
			m_Active = false;
			}
		void ExtForm_Closing(object sender, CancelEventArgs e) {
#if WindowsCE
			UpdateFormState();
#endif
			RemoveFromList();
			}
		public static ExtForm FindActiveForm() {
			for (int n = 0; n < m_AllForms.Count; n++) {
				if (((ExtForm)m_AllForms[n]).m_Active)
					return (ExtForm)m_AllForms[n];
				}
			return null;
			}
		protected void RemoveFromList() {
			for (int n = 0; n < m_AllForms.Count; n++) {
				if (((ExtForm)m_AllForms[n]).m_FormIndex == this.m_FormIndex) {
					m_AllForms.RemoveAt(n);
					return;
					}
				}
			}
#endif

#if PocketPC
			private void ExtForm_Load(object sender, EventArgs e) {
				}
#endif

#if PocketPC
			public void SetProfileValue(ref ExtFormState profileState) {
				}
#else
		protected void UpdateConfigIfChanged(AppConfig config) {
			if (m_Changed)
				config.WriteKeysToConfig();
			}
		void ExtForm_Shown(object? sender, EventArgs e) {
			m_Initialised = true;
			}
		void ExtForm_Move(object? sender, EventArgs e) {
			UpdateFormState();
			}
		void ExtForm_Resize(object? sender, EventArgs e) {
			UpdateFormState();
			}

		// These are mainly reminders
#pragma warning disable IDE0051 // Remove unused private members
		bool ControlKeyDown {
			get { return (Control.ModifierKeys & Keys.Control) != 0; }
			}
		bool ShiftKeyDown {
			get { return (Control.ModifierKeys & Keys.Shift) != 0; }
			}
		bool AltKeyDown {
			get { return (Control.ModifierKeys & Keys.Alt) != 0; }
			}
#pragma warning restore IDE0051 // Remove unused private members

		protected void UpdateFormState2() {
			if (DoUpdateFormState && (m_FormState != null)) {
				// System.Diagnostics.Trace.WriteLine("UpdateFormState");
				ExtFormState state = new ExtFormState(m_FormState);
				if (this.WindowState == FormWindowState.Normal)
					m_FormState.Set(this.Bounds, 0);
				else if (this.WindowState == FormWindowState.Maximized)
					m_FormState.Maximized = true;

				m_Changed = !state.EqualTo(m_FormState);
				}
			}
		protected void UpdateFormState(bool forceUpdate = false) {
			if (m_Initialised && (AutoUpdateFormState || forceUpdate))
				UpdateFormState2();
			}
		protected bool PointInsideRect(int x, int y, Point topLeft, Size size) {
			return (x > topLeft.X) && (y > topLeft.Y) && (x < topLeft.X + size.Width) && (y < topLeft.Y + size.Height);
			}

		public void SetProfileValue(ref ExtFormState profileState) {
			SetProfileValue(ref profileState, false);
			}
		public void SetProfileValue(ref ExtFormState profileState,bool setSizeAndPosition) {
			m_FormState = profileState;
			if (setSizeAndPosition)
				SetSizeAndPosition();
			}
		public void SetSizeAndPosition() {
			if (m_FormState == null)
				return;

			bool updateVal = DoUpdateFormState;
			DoUpdateFormState = false;

			if (m_FormState.Area > 0) {	// it's been initialised
				if (SetFormLocation && SetFormSize) {
					this.Bounds = new Rectangle(m_FormState.Left, m_FormState.Top, m_FormState.Width, m_FormState.Height);
					}
				else if (SetFormLocation) { 
					this.Left = m_FormState.Left;
					this.Top = m_FormState.Top;
					}
				else if (SetFormSize) {
					this.Width = m_FormState.Width;
					this.Height = m_FormState.Height;
					}
				if (CheckPositionIsOnScreen && (this.WindowState == FormWindowState.Normal) && !Utility.PointVisible(this.Left, this.Top)) {
					int x, y;
					Utility.GetNearestScreenTopLeft(this.Bounds, out x, out y);
					this.Left = x;
					this.Top = y;
					}
				}

			DoUpdateFormState = updateVal;
			this.WindowState = m_FormState.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
			}
		private void ExtForm_Load(object sender, EventArgs e) {
			SetSizeAndPosition();
			}
#endif

#if (!PocketPC && !WindowsCE)
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (MoveToHomePos && keyData == (Keys.H | Keys.Control | Keys.Alt)) {
				this.Bounds = new Rectangle(50, 50, 800, 600);
				return true;
				}
			return false;
			}
		protected void HandleMouseWheel(TextBox textBox, MouseEventArgs e) {
			if (PointInsideRect(e.X, e.Y, textBox.Location, textBox.Size)) {
				int lines = e.Delta * SystemInformation.MouseWheelScrollLines / 120;	// e.delta is lines * 120
				//int deltaLines = lineHeight * e.Delta * SystemInformation.MouseWheelScrollLines / 120;
				int msg;
				if (lines > 0)
					msg = Win32.SB_LINEUP;
				else {
					lines = -lines;
					msg = Win32.SB_LINEDOWN;
					}

				for (int n = 0; n < lines; n++)
					Win32.SendMessage(textBox.Handle, (int)Win32.WM_VSCROLL, (IntPtr)msg, IntPtr.Zero);
				}
			}
#endif

		}
#pragma warning restore IDE0018 // Inline variable declaration
	}