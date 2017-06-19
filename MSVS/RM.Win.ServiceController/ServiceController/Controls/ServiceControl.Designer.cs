namespace ServiceController.Controls
{
	sealed partial class ServiceControl
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
			this.components = new System.ComponentModel.Container();
			this.labelFullName = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.timerUpdate = new System.Windows.Forms.Timer(this.components);
			this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
			this.buttonRestart = new System.Windows.Forms.Button();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelFullName
			// 
			this.labelFullName.AutoSize = true;
			this.labelFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelFullName.Location = new System.Drawing.Point(4, 4);
			this.labelFullName.Name = "labelFullName";
			this.labelFullName.Size = new System.Drawing.Size(0, 16);
			this.labelFullName.TabIndex = 0;
			// 
			// labelStatus
			// 
			this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelStatus.AutoSize = true;
			this.labelStatus.Location = new System.Drawing.Point(4, 32);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(0, 13);
			this.labelStatus.TabIndex = 1;
			// 
			// timerUpdate
			// 
			this.timerUpdate.Interval = 1000;
			this.timerUpdate.Tick += new System.EventHandler(this.OnTimerUpdateTick);
			// 
			// checkBoxEnabled
			// 
			this.checkBoxEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxEnabled.AutoSize = true;
			this.checkBoxEnabled.Checked = true;
			this.checkBoxEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxEnabled.Location = new System.Drawing.Point(335, 19);
			this.checkBoxEnabled.Name = "checkBoxEnabled";
			this.checkBoxEnabled.Size = new System.Drawing.Size(15, 14);
			this.checkBoxEnabled.TabIndex = 6;
			this.checkBoxEnabled.UseVisualStyleBackColor = true;
			this.checkBoxEnabled.CheckedChanged += new System.EventHandler(this.OnTimerUpdateTick);
			// 
			// buttonRestart
			// 
			this.buttonRestart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRestart.Image = global::ServiceController.Properties.Resources.Restart;
			this.buttonRestart.Location = new System.Drawing.Point(448, 5);
			this.buttonRestart.Name = "buttonRestart";
			this.buttonRestart.Size = new System.Drawing.Size(40, 40);
			this.buttonRestart.TabIndex = 5;
			this.buttonRestart.UseVisualStyleBackColor = true;
			this.buttonRestart.Click += new System.EventHandler(this.OnButtonRestartClick);
			// 
			// buttonStart
			// 
			this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStart.Image = global::ServiceController.Properties.Resources.Start;
			this.buttonStart.Location = new System.Drawing.Point(356, 5);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(40, 40);
			this.buttonStart.TabIndex = 4;
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.OnButtonStartClick);
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStop.Image = global::ServiceController.Properties.Resources.Stop;
			this.buttonStop.Location = new System.Drawing.Point(402, 5);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(40, 40);
			this.buttonStop.TabIndex = 2;
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.OnButtonStopClick);
			// 
			// ServiceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.checkBoxEnabled);
			this.Controls.Add(this.buttonRestart);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.buttonStop);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.labelFullName);
			this.DoubleBuffered = true;
			this.Name = "ServiceControl";
			this.Size = new System.Drawing.Size(500, 50);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelFullName;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Timer timerUpdate;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonRestart;
		private System.Windows.Forms.CheckBox checkBoxEnabled;

	}
}
