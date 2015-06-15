namespace RM.Shooter
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStripIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemShot = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemChangeFrames = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemChangeFramesNow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemAntiAfk = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemProfiles = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStripProfiles = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorExit = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonClose = new System.Windows.Forms.Button();
			this.labelAboutText = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.timerAntiAfk = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStripIconMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// notifyIconMain
			// 
			this.notifyIconMain.ContextMenuStrip = this.contextMenuStripIconMenu;
			this.notifyIconMain.Text = "[RM] Shooter (Preview Version)\r\nClick to make screenshot";
			this.notifyIconMain.Visible = true;
			this.notifyIconMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseClick);
			// 
			// contextMenuStripIconMenu
			// 
			this.contextMenuStripIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShot,
            this.toolStripMenuItemChangeFrames,
            this.toolStripMenuItemChangeFramesNow,
            this.toolStripMenuItemAntiAfk,
            this.toolStripMenuItemProfiles,
            this.toolStripMenuItemSettings,
            this.toolStripMenuItemAbout,
            this.toolStripSeparatorExit,
            this.toolStripMenuItemExit});
			this.contextMenuStripIconMenu.Name = "contextMenuStrip1";
			this.contextMenuStripIconMenu.Size = new System.Drawing.Size(185, 186);
			// 
			// toolStripMenuItemShot
			// 
			this.toolStripMenuItemShot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.toolStripMenuItemShot.Name = "toolStripMenuItemShot";
			this.toolStripMenuItemShot.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemShot.Text = "Make &Screenshot";
			this.toolStripMenuItemShot.Click += new System.EventHandler(this.toolStripMenuItemShot_Click);
			// 
			// toolStripMenuItemChangeFrames
			// 
			this.toolStripMenuItemChangeFrames.Checked = true;
			this.toolStripMenuItemChangeFrames.CheckOnClick = true;
			this.toolStripMenuItemChangeFrames.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripMenuItemChangeFrames.Name = "toolStripMenuItemChangeFrames";
			this.toolStripMenuItemChangeFrames.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemChangeFrames.Text = "Change &Frames";
			// 
			// toolStripMenuItemChangeFramesNow
			// 
			this.toolStripMenuItemChangeFramesNow.Name = "toolStripMenuItemChangeFramesNow";
			this.toolStripMenuItemChangeFramesNow.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemChangeFramesNow.Text = "Change Frames &Now";
			this.toolStripMenuItemChangeFramesNow.Click += new System.EventHandler(this.toolStripMenuItemChangeFramesNow_Click);
			// 
			// toolStripMenuItemAntiAfk
			// 
			this.toolStripMenuItemAntiAfk.CheckOnClick = true;
			this.toolStripMenuItemAntiAfk.Enabled = false;
			this.toolStripMenuItemAntiAfk.Name = "toolStripMenuItemAntiAfk";
			this.toolStripMenuItemAntiAfk.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemAntiAfk.Text = "Enable Anti A&FK";
			this.toolStripMenuItemAntiAfk.CheckedChanged += new System.EventHandler(this.toolStripMenuItemAntiAfk_CheckedChanged);
			// 
			// toolStripMenuItemProfiles
			// 
			this.toolStripMenuItemProfiles.DropDown = this.contextMenuStripProfiles;
			this.toolStripMenuItemProfiles.Name = "toolStripMenuItemProfiles";
			this.toolStripMenuItemProfiles.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemProfiles.Text = "Profile";
			this.toolStripMenuItemProfiles.Visible = false;
			// 
			// contextMenuStripProfiles
			// 
			this.contextMenuStripProfiles.Name = "contextMenuStrip2";
			this.contextMenuStripProfiles.OwnerItem = this.toolStripMenuItemProfiles;
			this.contextMenuStripProfiles.Size = new System.Drawing.Size(61, 4);
			// 
			// toolStripMenuItemSettings
			// 
			this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
			this.toolStripMenuItemSettings.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemSettings.Text = "Settings...";
			this.toolStripMenuItemSettings.Visible = false;
			// 
			// toolStripMenuItemAbout
			// 
			this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
			this.toolStripMenuItemAbout.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemAbout.Text = "About...";
			this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
			// 
			// toolStripSeparatorExit
			// 
			this.toolStripSeparatorExit.Name = "toolStripSeparatorExit";
			this.toolStripSeparatorExit.Size = new System.Drawing.Size(181, 6);
			// 
			// toolStripMenuItemExit
			// 
			this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
			this.toolStripMenuItemExit.Size = new System.Drawing.Size(184, 22);
			this.toolStripMenuItemExit.Text = "E&xit";
			this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Location = new System.Drawing.Point(52, 275);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(360, 34);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// labelAboutText
			// 
			this.labelAboutText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelAboutText.Location = new System.Drawing.Point(114, 15);
			this.labelAboutText.Name = "labelAboutText";
			this.labelAboutText.Size = new System.Drawing.Size(258, 197);
			this.labelAboutText.TabIndex = 3;
			this.labelAboutText.Text = "About Shooter...";
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(52, 232);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(360, 13);
			this.labelVersion.TabIndex = 5;
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Image = global::RM.Shooter.Properties.Resources.logo;
			this.pictureBoxLogo.Location = new System.Drawing.Point(102, 12);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(256, 200);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxLogo.TabIndex = 4;
			this.pictureBoxLogo.TabStop = false;
			// 
			// timerAntiAfk
			// 
			this.timerAntiAfk.Interval = 10000;
			this.timerAntiAfk.Tick += new System.EventHandler(this.timerAntiAfk_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(464, 321);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.labelAboutText);
			this.Controls.Add(this.buttonClose);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "[RM] Shooter (Preview+Frameless)";
			this.contextMenuStripIconMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIconMain;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripIconMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShot;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorExit;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemProfiles;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripProfiles;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
		private System.Windows.Forms.Label labelAboutText;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeFrames;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeFramesNow;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAntiAfk;
		private System.Windows.Forms.Timer timerAntiAfk;
	}
}

