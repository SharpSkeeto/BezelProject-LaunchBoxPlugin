namespace SharpSkeeto.BezelManager.Plugin.Forms
{
	partial class FormBezelManager
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
			this.lblLaunchBoxInstallation = new System.Windows.Forms.Label();
			this.txtRetroInstallationFolder = new System.Windows.Forms.TextBox();
			this.gbStatusArea = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblProgressTitle = new System.Windows.Forms.Label();
			this.lblProgessStatus = new System.Windows.Forms.Label();
			this.pbProgressStatus = new System.Windows.Forms.ProgressBar();
			this.btnInstallBezel = new System.Windows.Forms.Button();
			this.cbSystemList = new System.Windows.Forms.ComboBox();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.cleanUptoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblSystems = new System.Windows.Forms.Label();
			this.lblSelectCore = new System.Windows.Forms.Label();
			this.cbCoreList = new System.Windows.Forms.ComboBox();
			this.gbStatusArea.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblLaunchBoxInstallation
			// 
			this.lblLaunchBoxInstallation.AutoSize = true;
			this.lblLaunchBoxInstallation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLaunchBoxInstallation.Location = new System.Drawing.Point(7, 60);
			this.lblLaunchBoxInstallation.Name = "lblLaunchBoxInstallation";
			this.lblLaunchBoxInstallation.Size = new System.Drawing.Size(142, 13);
			this.lblLaunchBoxInstallation.TabIndex = 3;
			this.lblLaunchBoxInstallation.Text = "Retroarch Installation Folder:";
			// 
			// txtRetroInstallationFolder
			// 
			this.txtRetroInstallationFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtRetroInstallationFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRetroInstallationFolder.HideSelection = false;
			this.txtRetroInstallationFolder.Location = new System.Drawing.Point(10, 76);
			this.txtRetroInstallationFolder.Name = "txtRetroInstallationFolder";
			this.txtRetroInstallationFolder.ReadOnly = true;
			this.txtRetroInstallationFolder.Size = new System.Drawing.Size(400, 13);
			this.txtRetroInstallationFolder.TabIndex = 6;
			this.txtRetroInstallationFolder.TabStop = false;
			this.txtRetroInstallationFolder.Text = "Please configure Retroarch in LaunchBox before continuing.";
			this.txtRetroInstallationFolder.WordWrap = false;
			// 
			// gbStatusArea
			// 
			this.gbStatusArea.Controls.Add(this.btnCancel);
			this.gbStatusArea.Controls.Add(this.lblProgressTitle);
			this.gbStatusArea.Controls.Add(this.lblProgessStatus);
			this.gbStatusArea.Controls.Add(this.pbProgressStatus);
			this.gbStatusArea.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gbStatusArea.Location = new System.Drawing.Point(0, 285);
			this.gbStatusArea.Name = "gbStatusArea";
			this.gbStatusArea.Size = new System.Drawing.Size(421, 78);
			this.gbStatusArea.TabIndex = 7;
			this.gbStatusArea.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(340, 14);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblProgressTitle
			// 
			this.lblProgressTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProgressTitle.AutoSize = true;
			this.lblProgressTitle.Location = new System.Drawing.Point(7, 14);
			this.lblProgressTitle.Name = "lblProgressTitle";
			this.lblProgressTitle.Size = new System.Drawing.Size(0, 13);
			this.lblProgressTitle.TabIndex = 2;
			// 
			// lblProgessStatus
			// 
			this.lblProgessStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProgessStatus.AutoSize = true;
			this.lblProgessStatus.Location = new System.Drawing.Point(7, 30);
			this.lblProgessStatus.Name = "lblProgessStatus";
			this.lblProgessStatus.Size = new System.Drawing.Size(0, 13);
			this.lblProgessStatus.TabIndex = 1;
			// 
			// pbProgressStatus
			// 
			this.pbProgressStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgressStatus.Location = new System.Drawing.Point(7, 47);
			this.pbProgressStatus.Name = "pbProgressStatus";
			this.pbProgressStatus.Size = new System.Drawing.Size(408, 23);
			this.pbProgressStatus.TabIndex = 0;
			// 
			// btnInstallBezel
			// 
			this.btnInstallBezel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnInstallBezel.Location = new System.Drawing.Point(151, 209);
			this.btnInstallBezel.Name = "btnInstallBezel";
			this.btnInstallBezel.Size = new System.Drawing.Size(123, 52);
			this.btnInstallBezel.TabIndex = 8;
			this.btnInstallBezel.Text = "Install";
			this.btnInstallBezel.UseVisualStyleBackColor = true;
			this.btnInstallBezel.Click += new System.EventHandler(this.btnInstallBezel_Click);
			// 
			// cbSystemList
			// 
			this.cbSystemList.DropDownHeight = 150;
			this.cbSystemList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSystemList.DropDownWidth = 225;
			this.cbSystemList.FormattingEnabled = true;
			this.cbSystemList.IntegralHeight = false;
			this.cbSystemList.Location = new System.Drawing.Point(151, 121);
			this.cbSystemList.Name = "cbSystemList";
			this.cbSystemList.Size = new System.Drawing.Size(225, 21);
			this.cbSystemList.Sorted = true;
			this.cbSystemList.TabIndex = 10;
			this.cbSystemList.SelectedIndexChanged += new System.EventHandler(this.cbSystemList_SelectedIndexChanged);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(421, 24);
			this.mainMenu.TabIndex = 11;
			this.mainMenu.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleanUptoolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
			this.toolStripMenuItem1.Text = "File";
			// 
			// cleanUptoolStripMenuItem
			// 
			this.cleanUptoolStripMenuItem.Name = "cleanUptoolStripMenuItem";
			this.cleanUptoolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.cleanUptoolStripMenuItem.Text = "Cleanup Files";
			this.cleanUptoolStripMenuItem.Click += new System.EventHandler(this.cleanUptoolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// lblSystems
			// 
			this.lblSystems.AutoSize = true;
			this.lblSystems.Location = new System.Drawing.Point(55, 124);
			this.lblSystems.Name = "lblSystems";
			this.lblSystems.Size = new System.Drawing.Size(77, 13);
			this.lblSystems.TabIndex = 12;
			this.lblSystems.Text = "Select System:";
			// 
			// lblSelectCore
			// 
			this.lblSelectCore.AutoSize = true;
			this.lblSelectCore.Location = new System.Drawing.Point(55, 155);
			this.lblSelectCore.Name = "lblSelectCore";
			this.lblSelectCore.Size = new System.Drawing.Size(65, 13);
			this.lblSelectCore.TabIndex = 13;
			this.lblSelectCore.Text = "Select Core:";
			// 
			// cbCoreList
			// 
			this.cbCoreList.DropDownHeight = 150;
			this.cbCoreList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCoreList.DropDownWidth = 225;
			this.cbCoreList.FormattingEnabled = true;
			this.cbCoreList.IntegralHeight = false;
			this.cbCoreList.Location = new System.Drawing.Point(151, 152);
			this.cbCoreList.Name = "cbCoreList";
			this.cbCoreList.Size = new System.Drawing.Size(225, 21);
			this.cbCoreList.Sorted = true;
			this.cbCoreList.TabIndex = 14;
			this.cbCoreList.SelectedIndexChanged += new System.EventHandler(this.cbCoreList_SelectedIndexChanged);
			// 
			// FormBezelManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(421, 363);
			this.Controls.Add(this.cbCoreList);
			this.Controls.Add(this.lblSelectCore);
			this.Controls.Add(this.lblSystems);
			this.Controls.Add(this.cbSystemList);
			this.Controls.Add(this.btnInstallBezel);
			this.Controls.Add(this.gbStatusArea);
			this.Controls.Add(this.txtRetroInstallationFolder);
			this.Controls.Add(this.lblLaunchBoxInstallation);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormBezelManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Bezel Manager";
			this.gbStatusArea.ResumeLayout(false);
			this.gbStatusArea.PerformLayout();
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblLaunchBoxInstallation;
		private System.Windows.Forms.TextBox txtRetroInstallationFolder;
		private System.Windows.Forms.GroupBox gbStatusArea;
		private System.Windows.Forms.Label lblProgressTitle;
		private System.Windows.Forms.Label lblProgessStatus;
		private System.Windows.Forms.ProgressBar pbProgressStatus;
		private System.Windows.Forms.Button btnInstallBezel;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox cbSystemList;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Label lblSystems;
		private System.Windows.Forms.Label lblSelectCore;
		private System.Windows.Forms.ComboBox cbCoreList;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem cleanUptoolStripMenuItem;
	}
}