namespace ServiceController.Forms
{
	partial class FormSettings
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
			System.Windows.Forms.Label labelServices;
			System.Windows.Forms.Label labelUpdateRate;
			System.Windows.Forms.GroupBox groupBoxFormPosSize;
			System.Windows.Forms.Label labelSizeText;
			System.Windows.Forms.Label labelPosText;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
			this.buttonCurrentSize = new System.Windows.Forms.Button();
			this.buttonChooseSize = new System.Windows.Forms.Button();
			this.labelPos = new System.Windows.Forms.Label();
			this.labelSize = new System.Windows.Forms.Label();
			this.textBoxServices = new System.Windows.Forms.TextBox();
			this.comboBoxUpdateRate = new System.Windows.Forms.ComboBox();
			this.panelButtons = new System.Windows.Forms.Panel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			labelServices = new System.Windows.Forms.Label();
			labelUpdateRate = new System.Windows.Forms.Label();
			groupBoxFormPosSize = new System.Windows.Forms.GroupBox();
			labelSizeText = new System.Windows.Forms.Label();
			labelPosText = new System.Windows.Forms.Label();
			groupBoxFormPosSize.SuspendLayout();
			this.panelButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelServices
			// 
			labelServices.AutoSize = true;
			labelServices.Location = new System.Drawing.Point(13, 14);
			labelServices.Name = "labelServices";
			labelServices.Size = new System.Drawing.Size(144, 13);
			labelServices.TabIndex = 0;
			labelServices.Text = "Services (one name per line):";
			// 
			// labelUpdateRate
			// 
			labelUpdateRate.AutoSize = true;
			labelUpdateRate.Location = new System.Drawing.Point(13, 163);
			labelUpdateRate.Name = "labelUpdateRate";
			labelUpdateRate.Size = new System.Drawing.Size(82, 13);
			labelUpdateRate.TabIndex = 2;
			labelUpdateRate.Text = "Update interval:";
			// 
			// groupBoxFormPosSize
			// 
			groupBoxFormPosSize.Controls.Add(this.buttonCurrentSize);
			groupBoxFormPosSize.Controls.Add(this.buttonChooseSize);
			groupBoxFormPosSize.Controls.Add(labelSizeText);
			groupBoxFormPosSize.Controls.Add(labelPosText);
			groupBoxFormPosSize.Controls.Add(this.labelPos);
			groupBoxFormPosSize.Controls.Add(this.labelSize);
			groupBoxFormPosSize.Location = new System.Drawing.Point(16, 197);
			groupBoxFormPosSize.Name = "groupBoxFormPosSize";
			groupBoxFormPosSize.Size = new System.Drawing.Size(334, 76);
			groupBoxFormPosSize.TabIndex = 8;
			groupBoxFormPosSize.TabStop = false;
			groupBoxFormPosSize.Text = "Form position && size";
			// 
			// buttonCurrentSize
			// 
			this.buttonCurrentSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCurrentSize.Location = new System.Drawing.Point(159, 12);
			this.buttonCurrentSize.Name = "buttonCurrentSize";
			this.buttonCurrentSize.Size = new System.Drawing.Size(169, 28);
			this.buttonCurrentSize.TabIndex = 12;
			this.buttonCurrentSize.Text = "Get current position && size";
			this.buttonCurrentSize.UseVisualStyleBackColor = true;
			this.buttonCurrentSize.Click += new System.EventHandler(this.OnButtonCurrentSizeClick);
			// 
			// buttonChooseSize
			// 
			this.buttonChooseSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonChooseSize.Location = new System.Drawing.Point(159, 42);
			this.buttonChooseSize.Name = "buttonChooseSize";
			this.buttonChooseSize.Size = new System.Drawing.Size(169, 28);
			this.buttonChooseSize.TabIndex = 11;
			this.buttonChooseSize.Text = "Choose position && size…";
			this.buttonChooseSize.UseVisualStyleBackColor = true;
			this.buttonChooseSize.Click += new System.EventHandler(this.OnButtonChooseSizeClick);
			// 
			// labelSizeText
			// 
			labelSizeText.AutoSize = true;
			labelSizeText.Location = new System.Drawing.Point(7, 46);
			labelSizeText.Name = "labelSizeText";
			labelSizeText.Size = new System.Drawing.Size(30, 13);
			labelSizeText.TabIndex = 10;
			labelSizeText.Text = "Size:";
			// 
			// labelPosText
			// 
			labelPosText.AutoSize = true;
			labelPosText.Location = new System.Drawing.Point(7, 22);
			labelPosText.Name = "labelPosText";
			labelPosText.Size = new System.Drawing.Size(47, 13);
			labelPosText.TabIndex = 9;
			labelPosText.Text = "Position:";
			// 
			// labelPos
			// 
			this.labelPos.AutoSize = true;
			this.labelPos.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelPos.Location = new System.Drawing.Point(71, 22);
			this.labelPos.Name = "labelPos";
			this.labelPos.Size = new System.Drawing.Size(70, 15);
			this.labelPos.TabIndex = 4;
			this.labelPos.Text = "120 ; 200";
			// 
			// labelSize
			// 
			this.labelSize.AutoSize = true;
			this.labelSize.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelSize.Location = new System.Drawing.Point(71, 46);
			this.labelSize.Name = "labelSize";
			this.labelSize.Size = new System.Drawing.Size(70, 15);
			this.labelSize.TabIndex = 3;
			this.labelSize.Text = "500 x 300";
			// 
			// textBoxServices
			// 
			this.textBoxServices.AcceptsReturn = true;
			this.textBoxServices.Location = new System.Drawing.Point(16, 30);
			this.textBoxServices.Multiline = true;
			this.textBoxServices.Name = "textBoxServices";
			this.textBoxServices.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxServices.Size = new System.Drawing.Size(334, 114);
			this.textBoxServices.TabIndex = 1;
			this.textBoxServices.WordWrap = false;
			// 
			// comboBoxUpdateRate
			// 
			this.comboBoxUpdateRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxUpdateRate.FormattingEnabled = true;
			this.comboBoxUpdateRate.Location = new System.Drawing.Point(221, 160);
			this.comboBoxUpdateRate.Name = "comboBoxUpdateRate";
			this.comboBoxUpdateRate.Size = new System.Drawing.Size(129, 21);
			this.comboBoxUpdateRate.TabIndex = 3;
			// 
			// panelButtons
			// 
			this.panelButtons.Controls.Add(this.buttonCancel);
			this.panelButtons.Controls.Add(this.buttonApply);
			this.panelButtons.Controls.Add(this.buttonSave);
			this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelButtons.Location = new System.Drawing.Point(0, 279);
			this.panelButtons.Name = "panelButtons";
			this.panelButtons.Size = new System.Drawing.Size(367, 40);
			this.panelButtons.TabIndex = 9;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(275, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonApply.Location = new System.Drawing.Point(140, 8);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(90, 23);
			this.buttonApply.TabIndex = 1;
			this.buttonApply.Text = "Apply only";
			this.buttonApply.UseVisualStyleBackColor = true;
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.buttonSave.Location = new System.Drawing.Point(12, 8);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(90, 23);
			this.buttonSave.TabIndex = 0;
			this.buttonSave.Text = "Save && Apply";
			this.buttonSave.UseVisualStyleBackColor = true;
			// 
			// FormSettings
			// 
			this.AcceptButton = this.buttonApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(367, 319);
			this.Controls.Add(this.panelButtons);
			this.Controls.Add(groupBoxFormPosSize);
			this.Controls.Add(this.comboBoxUpdateRate);
			this.Controls.Add(labelUpdateRate);
			this.Controls.Add(this.textBoxServices);
			this.Controls.Add(labelServices);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSettings";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			groupBoxFormPosSize.ResumeLayout(false);
			groupBoxFormPosSize.PerformLayout();
			this.panelButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxServices;
		private System.Windows.Forms.ComboBox comboBoxUpdateRate;
		private System.Windows.Forms.Label labelPos;
		private System.Windows.Forms.Label labelSize;
		private System.Windows.Forms.Panel panelButtons;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonCurrentSize;
		private System.Windows.Forms.Button buttonChooseSize;
	}
}