namespace RM.UzTicket.App.Controls
{
	partial class CompletableTextBox
	{
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
			if (disposing && (components != null))
			{
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
			this.buttonComplete = new System.Windows.Forms.Button();
			this.labelLabel = new System.Windows.Forms.Label();
			this.textBoxText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonComplete
			// 
			this.buttonComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonComplete.Location = new System.Drawing.Point(169, 19);
			this.buttonComplete.Name = "buttonComplete";
			this.buttonComplete.Size = new System.Drawing.Size(23, 23);
			this.buttonComplete.TabIndex = 3;
			this.buttonComplete.Text = "…";
			this.buttonComplete.UseVisualStyleBackColor = true;
			this.buttonComplete.Click += new System.EventHandler(this.buttonComplete_Click);
			// 
			// labelLabel
			// 
			this.labelLabel.AutoSize = true;
			this.labelLabel.Location = new System.Drawing.Point(4, 4);
			this.labelLabel.Name = "labelLabel";
			this.labelLabel.Size = new System.Drawing.Size(0, 13);
			this.labelLabel.TabIndex = 1;
			// 
			// textBoxText
			// 
			this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxText.Location = new System.Drawing.Point(7, 21);
			this.textBoxText.Name = "textBoxText";
			this.textBoxText.Size = new System.Drawing.Size(156, 20);
			this.textBoxText.TabIndex = 2;
			// 
			// CompletableTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textBoxText);
			this.Controls.Add(this.labelLabel);
			this.Controls.Add(this.buttonComplete);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "CompletableTextBox";
			this.Size = new System.Drawing.Size(200, 50);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonComplete;
		private System.Windows.Forms.Label labelLabel;
		private System.Windows.Forms.TextBox textBoxText;
	}
}
