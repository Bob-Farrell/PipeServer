namespace PipeServerApp {
	partial class TabbedForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
			{
			if (disposing && (components != null)) {
				components.Dispose();
				}
			base.Dispose(disposing);
			}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
			{
			this.pipeServerTabs = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.newTabButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.pipeServerTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// pipeServerTabs
			// 
			this.pipeServerTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pipeServerTabs.Controls.Add(this.tabPage1);
			this.pipeServerTabs.Controls.Add(this.tabPage2);
			this.pipeServerTabs.Location = new System.Drawing.Point(12, 45);
			this.pipeServerTabs.Name = "pipeServerTabs";
			this.pipeServerTabs.SelectedIndex = 0;
			this.pipeServerTabs.Size = new System.Drawing.Size(645, 544);
			this.pipeServerTabs.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(637, 516);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(637, 516);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// newTabButton
			// 
			this.newTabButton.Location = new System.Drawing.Point(12, 6);
			this.newTabButton.Name = "newTabButton";
			this.newTabButton.Size = new System.Drawing.Size(84, 23);
			this.newTabButton.TabIndex = 0;
			this.newTabButton.Text = "New Pipe";
			this.newTabButton.UseVisualStyleBackColor = true;
			this.newTabButton.Click += new System.EventHandler(this.NewTabButton_Click);
			// 
			// deleteButton
			// 
			this.deleteButton.Location = new System.Drawing.Point(112, 6);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(94, 23);
			this.deleteButton.TabIndex = 1;
			this.deleteButton.Text = "Delete current";
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// TabbedForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(669, 601);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.newTabButton);
			this.Controls.Add(this.pipeServerTabs);
			this.Name = "TabbedForm";
			this.Text = "TabbedForm";
			this.pipeServerTabs.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private TabControl pipeServerTabs;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private Button newTabButton;
		private Button deleteButton;
		}
	}