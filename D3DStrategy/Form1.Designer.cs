namespace D3DStrategy
{
	partial class Form1
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
			if(disposing && (components != null))
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
			this.gameTickTimer = new System.Windows.Forms.Timer(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.animationTimer = new System.Windows.Forms.Timer(this.components);
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.btnRifle = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.lblCoord = new System.Windows.Forms.Label();
			this.chkNudge = new System.Windows.Forms.CheckBox();
			this.lblCredits = new System.Windows.Forms.Label();
			this.btnGiveCredits = new System.Windows.Forms.Button();
			this.btnRefinery = new System.Windows.Forms.Button();
			this.btnCancelTraining = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// gameTickTimer
			// 
			this.gameTickTimer.Interval = 10;
			this.gameTickTimer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(43, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(419, 342);
			this.panel1.TabIndex = 0;
			this.panel1.TabStop = true;
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
			// 
			// animationTimer
			// 
			this.animationTimer.Interval = 5;
			this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
			// 
			// button2
			// 
			this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.button2.Location = new System.Drawing.Point(468, 185);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(25, 87);
			this.button2.TabIndex = 2;
			this.button2.Text = ">";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button3.Location = new System.Drawing.Point(208, 388);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(87, 22);
			this.button3.TabIndex = 3;
			this.button3.Text = "v";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.button4.Location = new System.Drawing.Point(12, 168);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(25, 87);
			this.button4.TabIndex = 4;
			this.button4.Text = "<";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.button5.Location = new System.Drawing.Point(208, 12);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(87, 22);
			this.button5.TabIndex = 5;
			this.button5.Text = "^";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// btnRifle
			// 
			this.btnRifle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRifle.Enabled = false;
			this.btnRifle.Location = new System.Drawing.Point(468, 98);
			this.btnRifle.Name = "btnRifle";
			this.btnRifle.Size = new System.Drawing.Size(75, 23);
			this.btnRifle.TabIndex = 6;
			this.btnRifle.Text = "Rifle infantry";
			this.btnRifle.UseVisualStyleBackColor = true;
			this.btnRifle.Click += new System.EventHandler(this.btnRifle_Click);
			// 
			// button6
			// 
			this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button6.Location = new System.Drawing.Point(468, 69);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 0;
			this.button6.Text = "Barracks";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// lblCoord
			// 
			this.lblCoord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCoord.AutoSize = true;
			this.lblCoord.Location = new System.Drawing.Point(465, 388);
			this.lblCoord.Name = "lblCoord";
			this.lblCoord.Size = new System.Drawing.Size(35, 13);
			this.lblCoord.TabIndex = 7;
			this.lblCoord.Text = "label1";
			// 
			// chkNudge
			// 
			this.chkNudge.AutoSize = true;
			this.chkNudge.Location = new System.Drawing.Point(12, 12);
			this.chkNudge.Name = "chkNudge";
			this.chkNudge.Size = new System.Drawing.Size(104, 17);
			this.chkNudge.TabIndex = 8;
			this.chkNudge.Text = "Benyt kant-scroll";
			this.chkNudge.UseVisualStyleBackColor = true;
			// 
			// lblCredits
			// 
			this.lblCredits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCredits.AutoSize = true;
			this.lblCredits.Location = new System.Drawing.Point(465, 13);
			this.lblCredits.Name = "lblCredits";
			this.lblCredits.Size = new System.Drawing.Size(54, 13);
			this.lblCredits.TabIndex = 9;
			this.lblCredits.Text = "Credits: -1";
			// 
			// btnGiveCredits
			// 
			this.btnGiveCredits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGiveCredits.Location = new System.Drawing.Point(468, 40);
			this.btnGiveCredits.Name = "btnGiveCredits";
			this.btnGiveCredits.Size = new System.Drawing.Size(75, 23);
			this.btnGiveCredits.TabIndex = 10;
			this.btnGiveCredits.Text = "Giv $";
			this.btnGiveCredits.UseVisualStyleBackColor = true;
			this.btnGiveCredits.Click += new System.EventHandler(this.btnGiveCredits_Click);
			// 
			// btnRefinery
			// 
			this.btnRefinery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefinery.Location = new System.Drawing.Point(468, 127);
			this.btnRefinery.Name = "btnRefinery";
			this.btnRefinery.Size = new System.Drawing.Size(75, 23);
			this.btnRefinery.TabIndex = 11;
			this.btnRefinery.Text = "Refinery";
			this.btnRefinery.UseVisualStyleBackColor = true;
			this.btnRefinery.Click += new System.EventHandler(this.btnRefinery_Click);
			// 
			// btnCancelTraining
			// 
			this.btnCancelTraining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancelTraining.Location = new System.Drawing.Point(468, 156);
			this.btnCancelTraining.Name = "btnCancelTraining";
			this.btnCancelTraining.Size = new System.Drawing.Size(75, 23);
			this.btnCancelTraining.TabIndex = 12;
			this.btnCancelTraining.Text = "Stop træn.";
			this.btnCancelTraining.UseVisualStyleBackColor = true;
			this.btnCancelTraining.Click += new System.EventHandler(this.btnCancelTraining_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 422);
			this.Controls.Add(this.btnCancelTraining);
			this.Controls.Add(this.btnRefinery);
			this.Controls.Add(this.btnGiveCredits);
			this.Controls.Add(this.lblCredits);
			this.Controls.Add(this.chkNudge);
			this.Controls.Add(this.lblCoord);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.btnRifle);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.panel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer gameTickTimer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Timer animationTimer;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button btnRifle;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label lblCoord;
		private System.Windows.Forms.CheckBox chkNudge;
		private System.Windows.Forms.Label lblCredits;
		private System.Windows.Forms.Button btnGiveCredits;
		private System.Windows.Forms.Button btnRefinery;
		private System.Windows.Forms.Button btnCancelTraining;
	}
}

