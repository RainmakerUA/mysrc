namespace ServiceController.Forms
{
	sealed partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.panelAll = new System.Windows.Forms.Panel();
			this.buttonRestart = new System.Windows.Forms.Button();
			this.buttonMin = new System.Windows.Forms.Button();
			this.buttonSettings = new System.Windows.Forms.Button();
			this.labelCopy = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.panelList = new System.Windows.Forms.Panel();
			this.panelAll.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// panelAll
			// 
			this.panelAll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelAll.Controls.Add(this.buttonRestart);
			this.panelAll.Controls.Add(this.buttonMin);
			this.panelAll.Controls.Add(this.buttonSettings);
			this.panelAll.Controls.Add(this.labelCopy);
			this.panelAll.Controls.Add(this.pictureBoxLogo);
			this.panelAll.Controls.Add(this.buttonStart);
			this.panelAll.Controls.Add(this.buttonStop);
			this.panelAll.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelAll.Location = new System.Drawing.Point(0, 214);
			this.panelAll.Margin = new System.Windows.Forms.Padding(5);
			this.panelAll.Name = "panelAll";
			this.panelAll.Size = new System.Drawing.Size(534, 52);
			this.panelAll.TabIndex = 0;
			// 
			// buttonRestart
			// 
			this.buttonRestart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRestart.Image = global::ServiceController.Properties.Resources.Restart;
			this.buttonRestart.Location = new System.Drawing.Point(472, 5);
			this.buttonRestart.Name = "buttonRestart";
			this.buttonRestart.Size = new System.Drawing.Size(40, 40);
			this.buttonRestart.TabIndex = 10;
			this.buttonRestart.UseVisualStyleBackColor = true;
			this.buttonRestart.Click += new System.EventHandler(this.OnButtonRestartClick);
			// 
			// buttonMin
			// 
			this.buttonMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonMin.Image = global::ServiceController.Properties.Resources.Minimize;
			this.buttonMin.Location = new System.Drawing.Point(310, 5);
			this.buttonMin.Name = "buttonMin";
			this.buttonMin.Size = new System.Drawing.Size(40, 40);
			this.buttonMin.TabIndex = 9;
			this.buttonMin.UseVisualStyleBackColor = true;
			this.buttonMin.Click += new System.EventHandler(this.OnButtonMinClick);
			// 
			// buttonSettings
			// 
			this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSettings.Image = global::ServiceController.Properties.Resources.Settings;
			this.buttonSettings.Location = new System.Drawing.Point(264, 5);
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.Size = new System.Drawing.Size(40, 40);
			this.buttonSettings.TabIndex = 8;
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.OnButtonSettingsClick);
			// 
			// labelCopy
			// 
			this.labelCopy.AutoSize = true;
			this.labelCopy.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelCopy.Location = new System.Drawing.Point(54, 13);
			this.labelCopy.Name = "labelCopy";
			this.labelCopy.Size = new System.Drawing.Size(180, 25);
			this.labelCopy.TabIndex = 7;
			this.labelCopy.Text = "© RM, 2010-2017";
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(48, 48);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxLogo.TabIndex = 6;
			this.pictureBoxLogo.TabStop = false;
			// 
			// buttonStart
			// 
			this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStart.Image = global::ServiceController.Properties.Resources.Start;
			this.buttonStart.Location = new System.Drawing.Point(381, 5);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(40, 40);
			this.buttonStart.TabIndex = 5;
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.OnButtonStartClick);
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStop.Image = global::ServiceController.Properties.Resources.Stop;
			this.buttonStop.Location = new System.Drawing.Point(427, 5);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(40, 40);
			this.buttonStop.TabIndex = 4;
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.OnButtonStopClick);
			// 
			// panelList
			// 
			this.panelList.AutoScroll = true;
			this.panelList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelList.Location = new System.Drawing.Point(0, 0);
			this.panelList.Margin = new System.Windows.Forms.Padding(5);
			this.panelList.Name = "panelList";
			this.panelList.Padding = new System.Windows.Forms.Padding(5);
			this.panelList.Size = new System.Drawing.Size(534, 214);
			this.panelList.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 266);
			this.Controls.Add(this.panelList);
			this.Controls.Add(this.panelAll);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(550, 300);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "[RM] Service Controller™";
			this.panelAll.ResumeLayout(false);
			this.panelAll.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelAll;
		private System.Windows.Forms.Panel panelList;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelCopy;
		private System.Windows.Forms.Button buttonSettings;
		private System.Windows.Forms.Button buttonMin;
		private System.Windows.Forms.Button buttonRestart;
	}
}

