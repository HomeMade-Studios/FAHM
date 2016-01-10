namespace FifaAutobuyerInstaller {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.installationFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.folderPathTextBox = new System.Windows.Forms.TextBox();
			this.openFolderBrowserButton = new System.Windows.Forms.Button();
			this.installButton = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.shortcut = new System.Windows.Forms.CheckBox();
			this.autostartProgram = new System.Windows.Forms.CheckBox();
			this.Finished = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// installationFolderBrowser
			// 
			this.installationFolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// folderPathTextBox
			// 
			this.folderPathTextBox.Location = new System.Drawing.Point(12, 26);
			this.folderPathTextBox.Name = "folderPathTextBox";
			this.folderPathTextBox.Size = new System.Drawing.Size(260, 20);
			this.folderPathTextBox.TabIndex = 0;
			// 
			// openFolderBrowserButton
			// 
			this.openFolderBrowserButton.Location = new System.Drawing.Point(197, 52);
			this.openFolderBrowserButton.Name = "openFolderBrowserButton";
			this.openFolderBrowserButton.Size = new System.Drawing.Size(75, 23);
			this.openFolderBrowserButton.TabIndex = 1;
			this.openFolderBrowserButton.Text = "Browse";
			this.openFolderBrowserButton.UseVisualStyleBackColor = true;
			this.openFolderBrowserButton.Click += new System.EventHandler(this.openFolderBrowserButton_Click);
			// 
			// installButton
			// 
			this.installButton.Location = new System.Drawing.Point(197, 159);
			this.installButton.Name = "installButton";
			this.installButton.Size = new System.Drawing.Size(75, 23);
			this.installButton.TabIndex = 2;
			this.installButton.Text = "Install";
			this.installButton.UseVisualStyleBackColor = true;
			this.installButton.Click += new System.EventHandler(this.installButton_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 104);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(260, 23);
			this.progressBar1.Step = 17;
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 4;
			// 
			// shortcut
			// 
			this.shortcut.AutoSize = true;
			this.shortcut.Checked = true;
			this.shortcut.CheckState = System.Windows.Forms.CheckState.Checked;
			this.shortcut.Location = new System.Drawing.Point(12, 58);
			this.shortcut.Name = "shortcut";
			this.shortcut.Size = new System.Drawing.Size(154, 17);
			this.shortcut.TabIndex = 5;
			this.shortcut.Text = "Create shortcut on desktop";
			this.shortcut.UseVisualStyleBackColor = true;
			// 
			// autostartProgram
			// 
			this.autostartProgram.AutoSize = true;
			this.autostartProgram.Checked = true;
			this.autostartProgram.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autostartProgram.Location = new System.Drawing.Point(12, 81);
			this.autostartProgram.Name = "autostartProgram";
			this.autostartProgram.Size = new System.Drawing.Size(163, 17);
			this.autostartProgram.TabIndex = 8;
			this.autostartProgram.Text = "Start at the end of installation";
			this.autostartProgram.UseVisualStyleBackColor = true;
			// 
			// Finished
			// 
			this.Finished.AutoSize = true;
			this.Finished.Location = new System.Drawing.Point(12, 181);
			this.Finished.Name = "Finished";
			this.Finished.Size = new System.Drawing.Size(0, 13);
			this.Finished.TabIndex = 9;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 191);
			this.Controls.Add(this.Finished);
			this.Controls.Add(this.autostartProgram);
			this.Controls.Add(this.shortcut);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.installButton);
			this.Controls.Add(this.openFolderBrowserButton);
			this.Controls.Add(this.folderPathTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Fifa16 Autobuyer Installer";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog installationFolderBrowser;
		private System.Windows.Forms.TextBox folderPathTextBox;
		private System.Windows.Forms.Button openFolderBrowserButton;
		private System.Windows.Forms.Button installButton;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.CheckBox shortcut;
		private System.Windows.Forms.CheckBox autostartProgram;
		private System.Windows.Forms.Label Finished;
	}
}

