using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace FifaAutobuyerInstaller {
	public partial class Form1 : Form {

		string installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "HomeMade Studios", "AutobuyerFifa16");
		string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TempDownload");
		string zipName = "AutobuyerFifa16.zip";

		string downloadUrl = "http://54.171.191.32/AutobuyerFifa16/AutobuyerFifa16.zip";

		bool auto = false;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			appDataPath += "\\AutobuyerFifa16\\installDirectory.txt";
			if (System.IO.File.Exists(appDataPath)) {
				installFolder = System.IO.File.ReadAllText(appDataPath);
			}
			installationFolderBrowser.SelectedPath = installFolder;
			folderPathTextBox.Text = installationFolderBrowser.SelectedPath;
			if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AutobuyerFifa16\\updateRequired.txt")) {
				auto = true;
				System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AutobuyerFifa16\\updateRequired.txt");
				this.Enabled = false;
				if (folderPathTextBox.Text != string.Empty) {

					installFolder = folderPathTextBox.Text;

					Properties.Settings.Default.installDirectory = installFolder;
					Properties.Settings.Default.Save();

					Directory.CreateDirectory(installFolder);

					try {

						Path.GetFullPath(installFolder);
						Download();

					}
					catch {

						MessageBox.Show("Error in install directory or download.");

					}
				}
			}
		}

		private void Start() {

			Process process = new Process();

			process.StartInfo.FileName = Path.Combine(installFolder, "AutobuyerFifa16.exe");

			process.Start();


		}

		private void Download() {

			Directory.CreateDirectory(tempFolder);

			using (WebClient wc = new WebClient()) {
				wc.DownloadProgressChanged += DownloadProgress;
				wc.DownloadFileCompleted += DownloadCompleted;
				wc.DownloadFileAsync(new Uri(downloadUrl), Path.Combine(tempFolder, zipName));
			}
		}

		private void Extract() {
			ZipFile.ExtractToDirectory(Path.Combine(tempFolder, zipName), installFolder);
			EndOperation();
		}

		private void EndOperation() {

			//System.IO.File.Delete(tempFolder);
			CopyInstallerToAppData();

			Finished.Text = "Completed!";
			this.Enabled = true;
			if (shortcut.Checked)
				CreateShortcut();
			if (autostartProgram.Checked || auto) {
				Start();
				Application.Exit();
			}
		}

		private void DownloadCompleted(object sender, AsyncCompletedEventArgs e) {
			Extract();
		}

		private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e) {
			progressBar1.Value = e.ProgressPercentage;
		}

		private void installButton_Click(object sender, EventArgs e) {
			this.Enabled = false;
			if (folderPathTextBox.Text != string.Empty) {

				installFolder = folderPathTextBox.Text;

				Properties.Settings.Default.installDirectory = installFolder;
				Properties.Settings.Default.Save();

				Directory.CreateDirectory(installFolder);

				try {

					Path.GetFullPath(installFolder);
					Download();

				}
				catch {

					MessageBox.Show("Error in install directory or download.");

				}
			}

		}

		private void openFolderBrowserButton_Click(object sender, EventArgs e) {
			DialogResult result = installationFolderBrowser.ShowDialog();
			folderPathTextBox.Text = installationFolderBrowser.SelectedPath;
		}

		private void CreateShortcut() {

			string desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			string appDir = Path.Combine(installFolder, "AutobuyerFifa16.exe");
			string icoDir = Path.Combine(installFolder, "AutobuyerFifa16.ico");

			string shortcutLocation = System.IO.Path.Combine(desktopDir, "Autobuyer Fifa 16" + ".lnk");
			WshShell shell = new WshShell();
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

			shortcut.Description = "Autobuyer Fifa 16"; // The description of the shortcut
			shortcut.IconLocation = icoDir;             // The icon of the shortcut
			shortcut.TargetPath = appDir;               // The path of the file that will launch when the shortcut is run
			shortcut.Save();                           // Save the shortcut

		}

		private void CopyInstallerToAppData() {
			string exePath = Application.ExecutablePath;
			string copyPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			copyPath += "\\AutobuyerFifa16\\";
			Directory.CreateDirectory(copyPath);
			try {
				System.IO.File.WriteAllText(copyPath + "installDirectory.txt", installFolder);
			}
			catch (Exception ex) { throw ex; }
			copyPath += "AutobuyerFifa16Installer.exe";
			System.IO.File.Copy(exePath, copyPath, true);
			progressBar1.PerformStep();
			return;
		}
	}
}

