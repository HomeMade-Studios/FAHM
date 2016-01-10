using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutobuyerFifa16 {
	public static class Players {

		public static List<Player> players { get; } = new List<Player>();

		public static int CalculateEarn(int row, TableLayoutPanel playerTable) {
			int earn;
			int buyPrice = (int)(playerTable.GetControlFromPosition(3, row) as NumericUpDown).Value;
			int sellPrice = (int)(playerTable.GetControlFromPosition(4, row) as NumericUpDown).Value;

			earn = sellPrice - buyPrice - (sellPrice / 100 * 5);

			return earn;
		}

		public static bool SetPlayerList(TableLayoutPanel playerTable) {
			players.Clear();
			for (int i = 1; i < 11; i++) {
				if ((playerTable.GetControlFromPosition(0, i) as CheckBox).Checked) {
					string name = (playerTable.GetControlFromPosition(1, i) as TextBox).Text;
                    bool isSpecial = (playerTable.GetControlFromPosition(2, i) as CheckBox).Checked;
                    int buyPrice = (int)(playerTable.GetControlFromPosition(3, i) as NumericUpDown).Value;
					int sellPrice = (int)(playerTable.GetControlFromPosition(4, i) as NumericUpDown).Value;


                    if (!Regex.IsMatch(name, "[|'\\!\"£$%&/()=?*+@#°§\\-_.:,;<>^0-9]", RegexOptions.IgnoreCase) && name != "") {
						Console.WriteLine("OK " + i + " name = " + name + " buyPrice = " + buyPrice + " sellPrice = " + sellPrice);
						players.Add(new Player(name, isSpecial, buyPrice, sellPrice));
					}
					else {
						MessageBox.Show(string.Format("Name of {0}° player is empty or contain incorrect characters.", i), "Reading error.");
						return false;
					}
				}
			}
			if(players.Count == 0) {
				MessageBox.Show("No player are selected in Players list", "Empty Players list!");
				return false;
			}
			return true;
		}

		public static void SavePlayersList(TableLayoutPanel playerTable) {
            string filePath = Path.Combine(	Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fifa16Autobuyer", "playersList.txt");

			using (StreamWriter sw = new StreamWriter(filePath, false)) {

				for (int i = 1; i < 11; i++) {
					string playerData;
					string name = (playerTable.GetControlFromPosition(1, i) as TextBox).Text;
                    string isSpecial = (playerTable.GetControlFromPosition(2, i) as CheckBox).Checked.ToString();
                    string buyPrice = (playerTable.GetControlFromPosition(3, i) as NumericUpDown).Value.ToString();
					string sellPrice = (playerTable.GetControlFromPosition(4, i) as NumericUpDown).Value.ToString();


					if (name == "")
						name = "null";

					playerData = string.Format("{0}\t{1}\t{2}\t{3}", name, isSpecial, buyPrice, sellPrice);
					sw.WriteLine(playerData);
				}
			}
			MessageBox.Show("Players list saved.");
		}

		public static void LoadPlayersList(TableLayoutPanel playerTable) { 

			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fifa16Autobuyer", "playersList.txt");

			if (File.Exists(filePath)) {
				using (StreamReader sr = new StreamReader(filePath)) {

					for (int i = 1; i < 11; i++) {
						string[] playerData = sr.ReadLine().Split(new char[1] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

						if (playerData[0] == "null")
							playerData[0] = "";

						(playerTable.GetControlFromPosition(1, i) as TextBox).Text = playerData[0];
                        (playerTable.GetControlFromPosition(2, i) as CheckBox).Checked = Convert.ToBoolean(playerData[1]);
                        (playerTable.GetControlFromPosition(3, i) as NumericUpDown).Value = Convert.ToDecimal(playerData[2]);
						(playerTable.GetControlFromPosition(4, i) as NumericUpDown).Value = Convert.ToDecimal(playerData[3]);					
					}
				}
			}
			else {
				MessageBox.Show("No save file found.");
			}
		}

		public struct Player {

			public string name { get; }
            public bool isSpecial { get; }
            public int buyNow { get; }
			public int sellPrice { get; }

			public Player(string name, bool isSpecial, int buyNow, int sellPrice) {
				this.name = name;
                this.isSpecial = isSpecial;
                this.buyNow = buyNow;
				this.sellPrice = sellPrice;
            }
		}
	}
}
