namespace RM.BossKey
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
			System.Windows.Forms.ToolStripSeparator toolStripSeparatorExit;
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabPageControl = new System.Windows.Forms.TabPage();
			this.tabPageLog = new System.Windows.Forms.TabPage();
			this.labelIndi = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.textBoxLog = new System.Windows.Forms.TextBox();
			toolStripSeparatorExit = new System.Windows.Forms.ToolStripSeparator();
			this.contextMenuStrip.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabPageControl.SuspendLayout();
			this.tabPageLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripSeparatorExit
			// 
			toolStripSeparatorExit.Name = "toolStripSeparatorExit";
			toolStripSeparatorExit.Size = new System.Drawing.Size(133, 6);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Text = "Bozz Kei";
			this.notifyIcon.Visible = true;
			this.notifyIcon.DoubleClick += new System.EventHandler(this.showToolStripMenuItem_Click);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            toolStripSeparatorExit,
            this.exitToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(137, 54);
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.showToolStripMenuItem.Text = "&Show/Hide";
			this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabPageControl);
			this.tabControlMain.Controls.Add(this.tabPageLog);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(344, 231);
			this.tabControlMain.TabIndex = 1;
			// 
			// tabPageControl
			// 
			this.tabPageControl.Controls.Add(this.labelIndi);
			this.tabPageControl.Controls.Add(this.button3);
			this.tabPageControl.Controls.Add(this.button2);
			this.tabPageControl.Controls.Add(this.button1);
			this.tabPageControl.Location = new System.Drawing.Point(4, 22);
			this.tabPageControl.Name = "tabPageControl";
			this.tabPageControl.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageControl.Size = new System.Drawing.Size(336, 205);
			this.tabPageControl.TabIndex = 0;
			this.tabPageControl.Text = "Controls";
			this.tabPageControl.UseVisualStyleBackColor = true;
			// 
			// tabPageLog
			// 
			this.tabPageLog.Controls.Add(this.textBoxLog);
			this.tabPageLog.Location = new System.Drawing.Point(4, 22);
			this.tabPageLog.Name = "tabPageLog";
			this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageLog.Size = new System.Drawing.Size(336, 205);
			this.tabPageLog.TabIndex = 1;
			this.tabPageLog.Text = "Log";
			this.tabPageLog.UseVisualStyleBackColor = true;
			// 
			// labelIndi
			// 
			this.labelIndi.BackColor = System.Drawing.Color.Red;
			this.labelIndi.Location = new System.Drawing.Point(37, 24);
			this.labelIndi.Name = "labelIndi";
			this.labelIndi.Size = new System.Drawing.Size(79, 84);
			this.labelIndi.TabIndex = 8;
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button3.Location = new System.Drawing.Point(37, 131);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(259, 39);
			this.button3.TabIndex = 7;
			this.button3.Text = "E&xit";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(122, 69);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(173, 39);
			this.button2.TabIndex = 6;
			this.button2.Text = "&Unregister";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(122, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(174, 39);
			this.button1.TabIndex = 5;
			this.button1.Text = "&Register";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBoxLog
			// 
			this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxLog.Location = new System.Drawing.Point(3, 3);
			this.textBoxLog.Multiline = true;
			this.textBoxLog.Name = "textBoxLog";
			this.textBoxLog.ReadOnly = true;
			this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxLog.Size = new System.Drawing.Size(330, 199);
			this.textBoxLog.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 231);
			this.Controls.Add(this.tabControlMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "[RM] Bozz Kei";
			this.contextMenuStrip.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabPageControl.ResumeLayout(false);
			this.tabPageLog.ResumeLayout(false);
			this.tabPageLog.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabPageControl;
		private System.Windows.Forms.Label labelIndi;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabPage tabPageLog;
		private System.Windows.Forms.TextBox textBoxLog;
	}
}

