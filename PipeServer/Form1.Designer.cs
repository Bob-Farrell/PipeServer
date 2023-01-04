namespace PipeServerApp
{
	partial class Form1 {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
			{
         this.outputTextBox = new System.Windows.Forms.TextBox();
         this.openClientButton = new System.Windows.Forms.Button();
         this.writeClientButton = new System.Windows.Forms.Button();
         this.closeClientButton = new System.Windows.Forms.Button();
         this.clearButton = new System.Windows.Forms.Button();
         this.copyButton = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.pipeNameTextBox = new System.Windows.Forms.TextBox();
         this.startServerButton = new System.Windows.Forms.Button();
         this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // outputTextBox
         // 
         this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.outputTextBox.Location = new System.Drawing.Point(12, 47);
         this.outputTextBox.Multiline = true;
         this.outputTextBox.Name = "outputTextBox";
         this.outputTextBox.ReadOnly = true;
         this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.outputTextBox.Size = new System.Drawing.Size(1031, 762);
         this.outputTextBox.TabIndex = 0;
         // 
         // openClientButto
         // 
         this.openClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.openClientButton.Location = new System.Drawing.Point(675, 12);
         this.openClientButton.Name = "openClientButto";
         this.openClientButton.Size = new System.Drawing.Size(78, 23);
         this.openClientButton.TabIndex = 1;
         this.openClientButton.Text = "Open client";
         this.openClientButton.UseVisualStyleBackColor = true;
         this.openClientButton.Visible = false;
         this.openClientButton.Click += new System.EventHandler(this.OpenClientButton_Click);
         // 
         // writeClientButton
         // 
         this.writeClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.writeClientButton.Location = new System.Drawing.Point(759, 12);
         this.writeClientButton.Name = "writeClientButton";
         this.writeClientButton.Size = new System.Drawing.Size(75, 23);
         this.writeClientButton.TabIndex = 2;
         this.writeClientButton.Text = "Write client";
         this.writeClientButton.UseVisualStyleBackColor = true;
         this.writeClientButton.Visible = false;
         this.writeClientButton.Click += new System.EventHandler(this.WriteClientButton_Click);
         // 
         // closeClientButton
         // 
         this.closeClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.closeClientButton.Location = new System.Drawing.Point(840, 12);
         this.closeClientButton.Name = "closeClientButton";
         this.closeClientButton.Size = new System.Drawing.Size(47, 23);
         this.closeClientButton.TabIndex = 3;
         this.closeClientButton.Text = "Close";
         this.closeClientButton.UseVisualStyleBackColor = true;
         this.closeClientButton.Visible = false;
         this.closeClientButton.Click += new System.EventHandler(this.CloseClientButton_Click);
         // 
         // clearButton
         // 
         this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.clearButton.Location = new System.Drawing.Point(985, 12);
         this.clearButton.Name = "clearButton";
         this.clearButton.Size = new System.Drawing.Size(58, 23);
         this.clearButton.TabIndex = 4;
         this.clearButton.Text = "Clear";
         this.clearButton.UseVisualStyleBackColor = true;
         this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
         // 
         // copyButton
         // 
         this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.copyButton.Location = new System.Drawing.Point(904, 12);
         this.copyButton.Name = "copyButton";
         this.copyButton.Size = new System.Drawing.Size(75, 23);
         this.copyButton.TabIndex = 5;
         this.copyButton.Text = "Copy text";
         this.copyButton.UseVisualStyleBackColor = true;
         this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(23, 19);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(66, 15);
         this.label1.TabIndex = 6;
         this.label1.Text = "Pipe name:";
         // 
         // pipeNameTextBox
         // 
         this.pipeNameTextBox.Location = new System.Drawing.Point(95, 16);
         this.pipeNameTextBox.Name = "pipeNameTextBox";
         this.pipeNameTextBox.Size = new System.Drawing.Size(170, 23);
         this.pipeNameTextBox.TabIndex = 7;
         this.pipeNameTextBox.Text = "exindapipe";
         // 
         // startServerButton
         // 
         this.startServerButton.Location = new System.Drawing.Point(287, 16);
         this.startServerButton.Name = "startServerButton";
         this.startServerButton.Size = new System.Drawing.Size(55, 23);
         this.startServerButton.TabIndex = 8;
         this.startServerButton.Text = "Start";
         this.startServerButton.UseVisualStyleBackColor = true;
         // 
         // autoStartCheckBox
         // 
         this.autoStartCheckBox.AutoSize = true;
         this.autoStartCheckBox.Location = new System.Drawing.Point(348, 19);
         this.autoStartCheckBox.Name = "autoStartCheckBox";
         this.autoStartCheckBox.Size = new System.Drawing.Size(78, 19);
         this.autoStartCheckBox.TabIndex = 9;
         this.autoStartCheckBox.Text = "Auto start";
         this.autoStartCheckBox.UseVisualStyleBackColor = true;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1055, 821);
         this.Controls.Add(this.autoStartCheckBox);
         this.Controls.Add(this.startServerButton);
         this.Controls.Add(this.pipeNameTextBox);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.copyButton);
         this.Controls.Add(this.clearButton);
         this.Controls.Add(this.closeClientButton);
         this.Controls.Add(this.writeClientButton);
         this.Controls.Add(this.openClientButton);
         this.Controls.Add(this.outputTextBox);
         this.Name = "Form1";
         this.Text = "Named pipe server";
         this.ResumeLayout(false);
         this.PerformLayout();

			}

		#endregion

		private TextBox outputTextBox;
		private Button openClientButton;
		private Button writeClientButton;
		private Button closeClientButton;
		private Button clearButton;
		private Button copyButton;
		private Label label1;
		private TextBox pipeNameTextBox;
		private Button startServerButton;
		private CheckBox autoStartCheckBox;
		}
	}