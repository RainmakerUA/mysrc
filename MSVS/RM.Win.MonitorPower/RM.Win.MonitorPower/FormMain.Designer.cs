namespace RM.Win.MonitorPower
{
	partial class FormMain
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.comboBoxMonitors = new System.Windows.Forms.ComboBox();
			this.labelMonitors = new System.Windows.Forms.Label();
			this.buttonOn = new System.Windows.Forms.Button();
			this.buttonOff = new System.Windows.Forms.Button();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// comboBoxMonitors
			// 
			this.comboBoxMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMonitors.FormattingEnabled = true;
			this.comboBoxMonitors.Location = new System.Drawing.Point(13, 35);
			this.comboBoxMonitors.Name = "comboBoxMonitors";
			this.comboBoxMonitors.Size = new System.Drawing.Size(359, 21);
			this.comboBoxMonitors.TabIndex = 0;
			// 
			// labelMonitors
			// 
			this.labelMonitors.AutoSize = true;
			this.labelMonitors.Location = new System.Drawing.Point(13, 13);
			this.labelMonitors.Name = "labelMonitors";
			this.labelMonitors.Size = new System.Drawing.Size(47, 13);
			this.labelMonitors.TabIndex = 1;
			this.labelMonitors.Text = "Monitors";
			// 
			// buttonOn
			// 
			this.buttonOn.Location = new System.Drawing.Point(13, 63);
			this.buttonOn.Name = "buttonOn";
			this.buttonOn.Size = new System.Drawing.Size(152, 23);
			this.buttonOn.TabIndex = 2;
			this.buttonOn.Text = "Turn ON";
			this.buttonOn.UseVisualStyleBackColor = true;
			this.buttonOn.Click += new System.EventHandler(this.OnButtonOnClick);
			// 
			// buttonOff
			// 
			this.buttonOff.Location = new System.Drawing.Point(220, 62);
			this.buttonOff.Name = "buttonOff";
			this.buttonOff.Size = new System.Drawing.Size(152, 23);
			this.buttonOff.TabIndex = 3;
			this.buttonOff.Text = "Turn OFF";
			this.buttonOff.UseVisualStyleBackColor = true;
			this.buttonOff.Click += new System.EventHandler(this.OnButtonOffClick);
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.Location = new System.Drawing.Point(292, 8);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(80, 23);
			this.buttonRefresh.TabIndex = 4;
			this.buttonRefresh.Text = "Refresh";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new System.EventHandler(this.OnButtonRefreshClick);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 111);
			this.Controls.Add(this.buttonRefresh);
			this.Controls.Add(this.buttonOff);
			this.Controls.Add(this.buttonOn);
			this.Controls.Add(this.labelMonitors);
			this.Controls.Add(this.comboBoxMonitors);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Monitor Power test";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxMonitors;
		private System.Windows.Forms.Label labelMonitors;
		private System.Windows.Forms.Button buttonOn;
		private System.Windows.Forms.Button buttonOff;
		private System.Windows.Forms.Button buttonRefresh;
	}
}

