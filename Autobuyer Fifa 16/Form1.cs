using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using Autobuyer_Fifa_16;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using Ini;
using System.Deployment.Application;

namespace Autobuyer_Fifa_16 {
	public partial class Form1 : Form {

		[DllImport("user32.dll")]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("gdi32.dll")]
		static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
		public enum DeviceCap {
			VERTRES = 10,
			DESKTOPVERTRES = 117
		}

		const int STARTSTOP_HOTKEY_ID = 1;

		const int IDLE = 0;
		const int SETTING = 1;
		const int SEARCHING = 2;
		const int PAUSE = 3;
		int currentState = IDLE;

		bool Online = false;
		bool closing = false;

		Size STARTSIZE;

		public static string IniPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fifa16Autobuyer", "Autobuyer.ini");

		public Form1() {
			Config.iniFile = new IniFile(IniPath);

			Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fifa16Autobuyer"));

			if (File.Exists(IniPath) && File.OpenRead(IniPath).Length != 0) {
				Config.LoadConfig();
			}

			Thread.CurrentThread.CurrentCulture = new CultureInfo(Config.language);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Config.language);
			
			InitializeComponent();
			RegisterHotKey(Handle, STARTSTOP_HOTKEY_ID, 2, (int)Keys.S);
		}

		private void Form1_Load(object sender, EventArgs e) {       //LOAD

			if (ApplicationDeployment.IsNetworkDeployed) {
				bundleVersionLabel.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
			}

			int control = NetCommunication.CheckBetaEnd();
			if (control == 0) { MessageBox.Show("Hi Tester, remember that this is a Beta. Please contact facebook page for any problem.", "Open Beta", MessageBoxButtons.OK, MessageBoxIcon.Information); Online = true; loginStatusLabel.Text = "Online ( BETA TEST )"; }

			if (getScalingFactor() != 1) {
				MessageBox.Show("Desktop zoom is not set to 100%. This can cause some problems. Please set the Dpi scale to 100%.", "Dpi scale not recommended", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			int screenWidth = SystemInformation.PrimaryMonitorSize.Width;
			int screenHeight = SystemInformation.PrimaryMonitorSize.Height;

			screenResolutionLabel.Text = string.Format("{0} x {1}", screenWidth, screenHeight);

			int coordRatio = Coordinates.SetCoord(new Point(screenWidth, screenHeight));
			int colorRatio = ColorChecker.SetColorPoints(new Point(screenWidth, screenHeight));

			if (coordRatio == 0 || colorRatio == 0)
				MessageBox.Show("Contact facebook page.", "Coords error!");
			else {
				if (coordRatio == 43)
					screenRatioLabel.Text = "4 : 3";
				else if (coordRatio == 1610)
					screenRatioLabel.Text = "16 : 10";
				else if (coordRatio == 169)
					screenRatioLabel.Text = "16 : 9";
			}

			Config.work = false;
			STARTSIZE = this.Size;

			ReadConfig();

			loginEmailTextBox.Text = Properties.Settings.Default.Email;
			loginPasswordTextBox.Text = Properties.Settings.Default.Password;

			searcherBW.RunWorkerAsync();
			statsBW.RunWorkerAsync();

		}

		protected override void WndProc(ref Message m) {
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == STARTSTOP_HOTKEY_ID) {
				Config.work = !Config.work;
			}
			base.WndProc(ref m);
		}

		private bool SetConfig() {
			if (Online) {
				this.Invoke(new Action(() => { this.Location = new Point(0, 0); }));
				this.Invoke(new Action(() => { this.Size = new Size(215, 215); }));
				this.Invoke(new Action(() => { mainTabControl.SelectedTab = statsTabPage; }));
				Config.attempts = (int)attemptsNumericBox.Value;
				Config.sellPlayers = sellPlayersRadioButton.Checked;
				Config.usePlayersList = usePlayerListCheckBox.Checked;
				Config.transferTargetEachSearch = transferTargetEachSearch.Checked;
				Config.currentPlayerIndex = 0;
				Config.pauseCounter = 0;
				Config.unlimitedAttempts = ((int)attemptsNumericBox.Value == 0) ? true : false;
				Config.pauseEnabled = pauseCheckBox.Checked;
				Config.pauseAttempts = (int)pauseAttemptsNumericBox.Value;
				Config.pauseSecond = (int)pauseSecondsNumericBox.Value;
				Config.delayBeforeNextSearch = (int)delayBeforeNextSearchNumericBox.Value;
				Stats.searchTime.Reset();
				Stats.playersFound = 0;
				Stats.playersBought = 0;
				Stats.playersLost = 0;
				if (Config.usePlayersList) {
					if (!Players.SetPlayerList(playersTablePanel)) {
						return false;
					}
				}
				return true;
			}
			else {
				MessageBox.Show("You need to login in order to use the program.", "You're not logged in!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.Invoke(new Action(() => { mainTabControl.SelectedTab = loginTabPage; }));
				return false;
			}
		}

		//WORK
		private void SearcherBW_DoWork(object sender, DoWorkEventArgs e) {

			BackgroundWorker worker = sender as BackgroundWorker;

			while (true && !closing) {
				if ((worker.CancellationPending == true)) {
					e.Cancel = true;
					break;
				}
				else {
					switch (currentState) {

						case IDLE:                  //IDLE

							Stats.currentAction = "Waiting for orders";
							Stats.searchTime.Stop();
							if (Config.work) {
								if (SetConfig()) {
									Stats.searchTime.Start();
									currentState = SETTING;
								}
								else {
									AbortSearch();
								}
							}
							break;

						case SETTING:                  //SETTING
							Stats.currentAction = "Setting search parameters";

							if (Config.usePlayersList) {
								SearchOperations.SetSearchParameters(Config.currentPlayerIndex);
							}

							currentState = SEARCHING;

							break;

						case SEARCHING:             //SEARCHING
							Stats.currentAction = "Searching";

							if ((Config.attempts == 0 && !Config.unlimitedAttempts) || (!Config.work)) {
								Config.work = false;
								this.Invoke(new Action(() => { this.Size = STARTSIZE; }));
								currentState = IDLE;
							}
							else if (Config.pauseEnabled && Config.pauseCounter == Config.pauseAttempts)
								currentState = PAUSE;
							else
								Search();

							break;

						case PAUSE:                 //PAUSE
							SearchOperations.ToTransferTargets();
							Stats.searchTime.Stop();
							Config.pauseCounter = 0;
							Pause();
							Stats.searchTime.Start();

							Config.currentPlayerIndex++;
							if (Config.currentPlayerIndex == Players.players.Count)
								Config.currentPlayerIndex = 0;

							SearchOperations.BackToMarket();
							Thread.Sleep(1000);

							currentState = SETTING;
							break;
					}
				}
				Thread.Sleep(1);
			}
		}

		//STATS
		private void StatsBW_DoWork(object sender, DoWorkEventArgs e) {

			BackgroundWorker worker = sender as BackgroundWorker;

			while (true && !closing) {
				if ((worker.CancellationPending == true)) {
					e.Cancel = true;
					break;
				}
				else {
					switch (currentState) {

						case IDLE:                  //IDLE
							SetStatus(Stats.currentAction);
							break;

						case SETTING:                  //SETTING
							SetStatus(Stats.currentAction);
							if (Config.usePlayersList)
								Stats.currentPlayer = Players.players[Config.currentPlayerIndex].name;
							else
								Stats.currentPlayer = "Null";
							statsTabPage.Invoke(new Action(() => { currentPlayerLabel.Text = Stats.currentPlayer; }));
							break;

						case SEARCHING:             //SEARCHING
							SetStatus(Stats.currentAction);

							statsTabPage.Invoke(new Action(() => { remainingAttemptsStatsText.Text = Config.attempts.ToString(); }));
							statsTabPage.Invoke(new Action(() => { searchTimeLabel.Text = string.Format("{0:hh\\:mm\\:ss}", Stats.searchTime.Elapsed); }));
							statsTabPage.Invoke(new Action(() => { playersFoundLabel.Text = Stats.playersFound.ToString(); }));
							statsTabPage.Invoke(new Action(() => { playersBoughtLabel.Text = Stats.playersBought.ToString(); }));
							statsTabPage.Invoke(new Action(() => { playersLostLabel.Text = Stats.playersLost.ToString(); }));

							break;

						case PAUSE:                 //PAUSE
							SetStatus("Pause {0}s remaining", Config.remainingPauseSecond);

							break;

						default:
							break;

					}

					Thread.Sleep(1);
				}
			}
		}

		private void AbortSearch() {
			currentState = IDLE;
			Config.work = false;
			this.Invoke(new Action(() => { this.Size = STARTSIZE; }));
		}

		private void Search() {

			SearchOperations.Search();                              //CERCA
			Thread.Sleep(200);

			if (SearchOperations.WaitSearchResult() == true) {      //ATTENDI I RISULTATI DELLA RICERCA E DECIDI SE COMPRARE O NO

				Stats.playersFound++;

				SearchOperations.Buy();                             //COMPRA
				Thread.Sleep(400);
				if (SearchOperations.WaitBuyResult() == true) {     //CONTROLLA SE L'ACQUISTO E' RIUSCITO

					Stats.playersBought++;

					if (Config.sellPlayers)                             //CONTROLLA SE LA VENDITA DEL GIOCATORE E' ABILITATA
						SearchOperations.Sell(Config.currentPlayerIndex); //VENDI
					else
						SearchOperations.SendToTransferList();          //O MANDA ALLA LISTA
				}
				else {
					Stats.playersLost++;

					SearchOperations.CloseError();                  //CHIUDI L'ERRORE DI GIOCATORE FREGATO DA QUALCUN ALTRO
				}
				Thread.Sleep(800);

				if (Config.transferTargetEachSearch) {
					SearchOperations.ToTransferTargets();
				}

				SearchOperations.BackToMarket();
			}
			else {
				SearchOperations.CloseError();                      //CHIUDI L'ERRORE DI NESSUN GIOCATORE TROVATO
				if (Config.transferTargetEachSearch) {
					SearchOperations.ToTransferTargets();
					SearchOperations.BackToMarket();
				}
			}

			Thread.Sleep(Config.delayBeforeNextSearch * 1000);      //ATTENDI IL TEMPO PRESTABILITO PRIMA DELLA RICERCA SUCESSIVA

			Config.pauseCounter++;                                  //AUMENTA I CONTATORI
			if (!Config.unlimitedAttempts)
				Config.attempts--;
		}

		private void Pause() {
			double remainingPauseSecond = Config.pauseSecond;
			while (remainingPauseSecond > 0 && Config.work) {
				Thread.Sleep(100);
				remainingPauseSecond -= 0.1;
				Config.remainingPauseSecond = (int)Math.Round(remainingPauseSecond);
			}
		}

		private void SetStatus(string status, object param1 = null, object param2 = null) {
			if (!this.closing) {
				statsTabPage.Invoke(new Action(() => { if (!closing) currentActionLabel.Text = string.Format(status, param1, param2); }));
			}
		}

		private void homeMadeButton_Click(object sender, EventArgs e) {
			Process.Start("https://www.facebook.com/HomeMade-Studios-1504621553141486/timeline/");
			Process.Start("https://play.google.com/store/apps/dev?id=8610543020411511969");
		}

		private void button1_Click(object sender, EventArgs e) {
			//MessageBox.Show(ColorChecker.GetPixel(ColorChecker.colorPoints["Search"].position).ToString(), MessageBoxButtons.OK ,MessageBoxIcon.Error);
			//Cursor.Position = ColorChecker.colorPoints["Search"].position;
		}

		private void playerPriceChanged(object sender, EventArgs e) {

			int row = playersTablePanel.GetPositionFromControl((NumericUpDown)sender).Row;
			int newEarn = Players.CalculateEarn(row, playersTablePanel);
			Label earnLabel = playersTablePanel.GetControlFromPosition(4, row) as Label;

			if (newEarn < 0) {
				earnLabel.Text = "- " + Math.Abs(newEarn).ToString("N0");
				earnLabel.ForeColor = Color.Red;
			}
			else if (newEarn > 0) {
				earnLabel.Text = "+ " + Math.Abs(newEarn).ToString("N0");
				earnLabel.ForeColor = Color.Green;
			}
			else {
				earnLabel.Text = Math.Abs(newEarn).ToString("N0");
				earnLabel.ForeColor = SystemColors.ControlText;
			}
		}

		private void playerEnabled(object sender, EventArgs e) {

			CheckBox playerCheckBox = (CheckBox)sender;

			int row = playersTablePanel.GetPositionFromControl(playerCheckBox).Row;

			playersTablePanel.GetControlFromPosition(1, row).Enabled = playerCheckBox.Checked;
			playersTablePanel.GetControlFromPosition(2, row).Enabled = playerCheckBox.Checked;
			playersTablePanel.GetControlFromPosition(3, row).Enabled = playerCheckBox.Checked;
		}

		private void usePlayerListCheckBox_CheckedChanged(object sender, EventArgs e) {
			if (usePlayerListCheckBox.Checked) {
				sellPlayersRadioButton.Enabled = true;
			}
			else {
				sellPlayersRadioButton.Enabled = false;
				sendToTransferListRadioButton.PerformClick();
			}
		}

		private void loginButton_Click(object sender, EventArgs e) {
			string email = loginEmailTextBox.Text;
			string pass = loginPasswordTextBox.Text;

			if (!email.Contains("@")) {
				MessageBox.Show("Incorrect email form.", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (email == "") {
				MessageBox.Show("Write an email.", "Email Error", MessageBoxButtons.OK ,MessageBoxIcon.Error);
				return;
			}
			if (pass == "") {
				MessageBox.Show("Write a password.", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else {
				switch (NetCommunication.Login(email, pass)) {

					case 0:
						MessageBox.Show("Incorrect email or password. Please sign up if you have not an account on our website yet.", "No account found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;

					case 1:
						NetCommunication.ChangeAccountOnlineStatus(email, true);
						break;

					case -1:
						MessageBox.Show("This account is already online on another Pc. Please use a different account or contact facebook page.", "Account already online!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;

					case -2:
						MessageBox.Show("Error while reading data, please contact facebook page.", "Reading data error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;

					case -3:
						MessageBox.Show("Impossible to connect with server, check your internet connection or try again later!", "Network error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;

				}

				Config.loginEmail = email;
				Online = true;
				loginStatusLabel.Text = "Online";
				loginPanel.Enabled = false;
				logoutButton.Enabled = true;
				Properties.Settings.Default.Email = email;
				Properties.Settings.Default.Password = pass;
			}
		}

		private void signUpButton_Click(object sender, EventArgs e) {
			Process.Start("http://fifa16autobuyer.altervista.org/");
		}

		private void savePlayersButton_Click(object sender, EventArgs e) {
			Players.SavePlayersList(playersTablePanel);
		}

		private void loadPlayersButton_Click(object sender, EventArgs e) {
			Players.LoadPlayersList(playersTablePanel);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			closing = true;

			searcherBW.CancelAsync();
			statsBW.CancelAsync();

			Config.SaveConfig();
			Properties.Settings.Default.Save();

			if (Online) {
				MessageBox.Show("You must logout before quit application.", "You're Online!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				mainTabControl.SelectedTab = loginTabPage;
				e.Cancel = true;
			}

			NetCommunication.ChangeAccountOnlineStatus(Config.loginEmail, false);
		}

		private float getScalingFactor() {
			Graphics g = Graphics.FromHwnd(IntPtr.Zero);
			IntPtr desktop = g.GetHdc();
			int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
			int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

			float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

			return ScreenScalingFactor; // 1.25 = 125%
		}

		private void ReadConfig() {

			usePlayerListCheckBox.Checked = Config.usePlayersList;
			pauseCheckBox.Checked = Config.pauseEnabled;
			transferTargetEachSearch.Checked = Config.transferTargetEachSearch;
			pauseSecondsNumericBox.Value = Convert.ToDecimal(Config.pauseSecond);
			pauseAttemptsNumericBox.Value = Convert.ToDecimal(Config.pauseAttempts);
			delayBeforeNextSearchNumericBox.Value = Convert.ToDecimal(Config.delayBeforeNextSearch);

			sellPlayersRadioButton.Checked = Config.sellPlayers;
			sendToTransferListRadioButton.Checked = !Config.sellPlayers;

			if (Config.language == "en") {
				languageEngRadioButton.Checked = true;
				languageEngRadioButton.Enabled = false;
				languageItaRadioButton.Enabled = true;
			}
			else {
				languageItaRadioButton.Checked = true;
				languageEngRadioButton.Enabled = true;
				languageItaRadioButton.Enabled = false;
			}

		}

		private void Translate(object sender, EventArgs e) {
			if (languageEngRadioButton.Checked) {
				Config.language = "en";
			}
			else {
				Config.language = "it";
			}
			MessageBox.Show("Application will close. Start it again.", "A restart is needed.", MessageBoxButtons.OK, MessageBoxIcon.Information);
			Application.Restart();
		}

		private void ManualUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			if (ApplicationDeployment.IsNetworkDeployed) {
				ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;
				UpdateCheckInfo info = updateCheck.CheckForDetailedUpdate();

				if (info.UpdateAvailable) {
					updateCheck.Update();
					MessageBox.Show("The application has been upgraded, please restart application.");
					this.Close();
				}
				else {
					MessageBox.Show("This is the latest version.");
				}
			}
		}

		private void facebookLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://www.facebook.com/DanielUgoliniFifaAutoBuyer?fref=ts");
		}

		private void siteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://fifa16autobuyer.altervista.org/");
		}

		private void webAppLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://www.easports.com/it/fifa/ultimate-team/web-app");
        }

		private void logoutButton_Click(object sender, EventArgs e) {
			if (Online && Config.loginEmail != "") {
				int result = NetCommunication.ChangeAccountOnlineStatus(Config.loginEmail, false);
                if (result == -2) {
					MessageBox.Show("Unable to connect to server, check your connection", "Network Error", MessageBoxButtons.OK ,MessageBoxIcon.Error);
				}
				else {
					Online = false;
					loginStatusLabel.Text = "Offline";
					loginPanel.Enabled = true;
					logoutButton.Enabled = false;
				}
			}
		}
	}
}
