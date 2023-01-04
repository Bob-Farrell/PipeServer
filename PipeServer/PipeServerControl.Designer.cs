namespace PipeServerApp {
	partial class PipeServerControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
			{
			this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
			this.startServerButton = new System.Windows.Forms.Button();
			this.pipeNameTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.copyButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.closeClientButton = new System.Windows.Forms.Button();
			this.writeClientButton = new System.Windows.Forms.Button();
			this.openClientButton = new System.Windows.Forms.Button();
			this.outputTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// autoStartCheckBox
			// 
			this.autoStartCheckBox.AutoSize = true;
			this.autoStartCheckBox.Location = new System.Drawing.Point(347, 11);
			this.autoStartCheckBox.Name = "autoStartCheckBox";
			this.autoStartCheckBox.Size = new System.Drawing.Size(78, 19);
			this.autoStartCheckBox.TabIndex = 19;
			this.autoStartCheckBox.Text = "Auto start";
			this.autoStartCheckBox.UseVisualStyleBackColor = true;
			// 
			// startServerButton
			// 
			this.startServerButton.Location = new System.Drawing.Point(286, 8);
			this.startServerButton.Name = "startServerButton";
			this.startServerButton.Size = new System.Drawing.Size(55, 23);
			this.startServerButton.TabIndex = 18;
			this.startServerButton.Text = "Start";
			this.startServerButton.UseVisualStyleBackColor = true;
			// 
			// pipeNameTextBox
			// 
			this.pipeNameTextBox.Location = new System.Drawing.Point(94, 8);
			this.pipeNameTextBox.Name = "pipeNameTextBox";
			this.pipeNameTextBox.Size = new System.Drawing.Size(170, 23);
			this.pipeNameTextBox.TabIndex = 17;
			this.pipeNameTextBox.Text = "exindapipe";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 15);
			this.label1.TabIndex = 16;
			this.label1.Text = "Pipe name:";
			// 
			// copyButton
			// 
			this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.copyButton.Location = new System.Drawing.Point(727, 7);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(75, 23);
			this.copyButton.TabIndex = 15;
			this.copyButton.Text = "Copy text";
			this.copyButton.UseVisualStyleBackColor = true;
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.clearButton.Location = new System.Drawing.Point(808, 7);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(58, 23);
			this.clearButton.TabIndex = 14;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			// 
			// closeClientButton
			// 
			this.closeClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.closeClientButton.Location = new System.Drawing.Point(663, 7);
			this.closeClientButton.Name = "closeClientButton";
			this.closeClientButton.Size = new System.Drawing.Size(47, 23);
			this.closeClientButton.TabIndex = 13;
			this.closeClientButton.Text = "Close";
			this.closeClientButton.UseVisualStyleBackColor = true;
			this.closeClientButton.Visible = false;
			// 
			// writeClientButton
			// 
			this.writeClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.writeClientButton.Location = new System.Drawing.Point(582, 7);
			this.writeClientButton.Name = "writeClientButton";
			this.writeClientButton.Size = new System.Drawing.Size(75, 23);
			this.writeClientButton.TabIndex = 12;
			this.writeClientButton.Text = "Write client";
			this.writeClientButton.UseVisualStyleBackColor = true;
			this.writeClientButton.Visible = false;
			// 
			// openClientButton
			// 
			this.openClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.openClientButton.Location = new System.Drawing.Point(498, 7);
			this.openClientButton.Name = "openClientButton";
			this.openClientButton.Size = new System.Drawing.Size(78, 23);
			this.openClientButton.TabIndex = 11;
			this.openClientButton.Text = "Open client";
			this.openClientButton.UseVisualStyleBackColor = true;
			this.openClientButton.Visible = false;
			// 
			// outputTextBox
			// 
			this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputTextBox.Location = new System.Drawing.Point(11, 39);
			this.outputTextBox.Multiline = true;
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.ReadOnly = true;
			this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.outputTextBox.Size = new System.Drawing.Size(859, 415);
			this.outputTextBox.TabIndex = 10;
			// 
			// PipeServerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.autoStartCheckBox);
			this.Controls.Add(this.startServerButton);
			this.Controls.Add(this.pipeNameTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.copyButton);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.outputTextBox);
			this.Controls.Add(this.closeClientButton);
			this.Controls.Add(this.writeClientButton);
			this.Controls.Add(this.openClientButton);
			this.Name = "PipeServerControl";
			this.Size = new System.Drawing.Size(873, 457);
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private CheckBox autoStartCheckBox;
		private Button startServerButton;
		private TextBox pipeNameTextBox;
		private Label label1;
		private Button copyButton;
		private Button clearButton;
		private Button closeClientButton;
		private Button writeClientButton;
		private Button openClientButton;
		private TextBox outputTextBox;
		}
	}
