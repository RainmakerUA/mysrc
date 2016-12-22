namespace RM.UzTicket.App
{
	partial class UzForm
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
			this.completableTextBoxFromStation = new RM.UzTicket.App.Controls.CompletableTextBox();
			this.completableTextBoxDestination = new RM.UzTicket.App.Controls.CompletableTextBox();
			this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
			this.labelDate = new System.Windows.Forms.Label();
			this.buttonTrain = new System.Windows.Forms.Button();
			this.textBoxTrain = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// completableTextBoxFromStation
			// 
			this.completableTextBoxFromStation.Label = "Select the initial station:";
			this.completableTextBoxFromStation.Location = new System.Drawing.Point(12, 12);
			this.completableTextBoxFromStation.MinimumSize = new System.Drawing.Size(200, 50);
			this.completableTextBoxFromStation.Name = "completableTextBoxFromStation";
			this.completableTextBoxFromStation.Size = new System.Drawing.Size(260, 50);
			this.completableTextBoxFromStation.TabIndex = 3;
			this.completableTextBoxFromStation.Value = "";
			this.completableTextBoxFromStation.Complete += new System.EventHandler<RM.UzTicket.App.Controls.CompleteAsyncEventArgs>(this.completableTextBoxFromStation_Complete);
			// 
			// completableTextBoxDestination
			// 
			this.completableTextBoxDestination.Label = "Select destinatiom station:";
			this.completableTextBoxDestination.Location = new System.Drawing.Point(12, 68);
			this.completableTextBoxDestination.MinimumSize = new System.Drawing.Size(200, 50);
			this.completableTextBoxDestination.Name = "completableTextBoxDestination";
			this.completableTextBoxDestination.Size = new System.Drawing.Size(260, 50);
			this.completableTextBoxDestination.TabIndex = 4;
			this.completableTextBoxDestination.Value = "";
			this.completableTextBoxDestination.Complete += new System.EventHandler<RM.UzTicket.App.Controls.CompleteAsyncEventArgs>(this.completableTextBoxFromStation_Complete);
			// 
			// dateTimePickerDate
			// 
			this.dateTimePickerDate.Location = new System.Drawing.Point(278, 33);
			this.dateTimePickerDate.Name = "dateTimePickerDate";
			this.dateTimePickerDate.Size = new System.Drawing.Size(200, 20);
			this.dateTimePickerDate.TabIndex = 5;
			// 
			// labelDate
			// 
			this.labelDate.AutoSize = true;
			this.labelDate.Location = new System.Drawing.Point(279, 13);
			this.labelDate.Name = "labelDate";
			this.labelDate.Size = new System.Drawing.Size(112, 13);
			this.labelDate.TabIndex = 6;
			this.labelDate.Text = "Select departure date:";
			// 
			// buttonTrain
			// 
			this.buttonTrain.Location = new System.Drawing.Point(278, 68);
			this.buttonTrain.Name = "buttonTrain";
			this.buttonTrain.Size = new System.Drawing.Size(200, 49);
			this.buttonTrain.TabIndex = 7;
			this.buttonTrain.Text = "Train";
			this.buttonTrain.UseVisualStyleBackColor = true;
			this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
			// 
			// textBoxTrain
			// 
			this.textBoxTrain.Location = new System.Drawing.Point(12, 124);
			this.textBoxTrain.Multiline = true;
			this.textBoxTrain.Name = "textBoxTrain";
			this.textBoxTrain.ReadOnly = true;
			this.textBoxTrain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxTrain.Size = new System.Drawing.Size(470, 275);
			this.textBoxTrain.TabIndex = 8;
			this.textBoxTrain.WordWrap = false;
			// 
			// UzForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 411);
			this.Controls.Add(this.textBoxTrain);
			this.Controls.Add(this.buttonTrain);
			this.Controls.Add(this.labelDate);
			this.Controls.Add(this.dateTimePickerDate);
			this.Controls.Add(this.completableTextBoxDestination);
			this.Controls.Add(this.completableTextBoxFromStation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "UzForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "UZ Ticket";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private Controls.CompletableTextBox completableTextBoxFromStation;
		private Controls.CompletableTextBox completableTextBoxDestination;
		private System.Windows.Forms.DateTimePicker dateTimePickerDate;
		private System.Windows.Forms.Label labelDate;
		private System.Windows.Forms.Button buttonTrain;
		private System.Windows.Forms.TextBox textBoxTrain;
	}
}

