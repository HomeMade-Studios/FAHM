using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using Ini;

namespace Autobuyer_Fifa_16 {
	public static class Config {

		public static IniFile iniFile;

		public static bool work { get; set; }						= false;
		public static bool unlimitedAttempts { get; set; }			= false;
		public static bool usePlayersList { get; set; }				= true;
		public static bool sellPlayers { get; set; }				= true;
		public static bool pauseEnabled { get; set; }				= true;
		public static bool transferTargetEachSearch { get; set; }	= true;
		public static int attempts { get; set; }					= 0;
		public static int currentPlayerIndex { get; set; }			= 0;
		public static int pauseSecond { get; set; }					= 10;
		public static int remainingPauseSecond { get; set; }		= 10;
		public static int pauseAttempts { get; set; }				= 25;
		public static int pauseCounter { get; set; }				= 0;
		public static int delayBeforeNextSearch { get; set; }		= 1;
		public static string loginEmail { get; set; }				= "";
		public static string language { get; set; }					= "en";

		public static void SaveConfig() {
			iniFile.WriteValue("Option", "usePlayersList",			usePlayersList.ToString());
			iniFile.WriteValue("Option", "sellPlayers",				sellPlayers.ToString());
			iniFile.WriteValue("Option", "pauseEnabled",			pauseEnabled.ToString());
			iniFile.WriteValue("Option", "transferTargetEachSearch",transferTargetEachSearch.ToString());
			iniFile.WriteValue("Option", "pauseSecond",				pauseSecond.ToString());
			iniFile.WriteValue("Option", "pauseAttempts",			pauseAttempts.ToString());
			iniFile.WriteValue("Option", "delayBeforeNextSearch",	delayBeforeNextSearch.ToString());
			iniFile.WriteValue("Option", "language",				language.ToString());
		}

		public static void LoadConfig() {
			Console.WriteLine(iniFile.ReadValue("Option", "usePlayersList") + "v");
			usePlayersList =			(iniFile.ReadValue(	"Option", "usePlayersList") == "True") ? true : false;
			sellPlayers =				(iniFile.ReadValue( "Option", "sellPlayers") == "True" ) ? true : false;
			pauseEnabled =				(iniFile.ReadValue( "Option", "pauseEnabled") == "True" ) ? true : false;
			transferTargetEachSearch =	(iniFile.ReadValue( "Option", "transferTargetEachSearch") == "True" ) ? true : false;
			pauseSecond =				int.Parse	(iniFile.ReadValue(	"Option", "pauseSecond" ));
			pauseAttempts =				int.Parse	(iniFile.ReadValue(	"Option", "pauseAttempts" ));
			delayBeforeNextSearch =		int.Parse	(iniFile.ReadValue(	"Option", "delayBeforeNextSearch" ));
			language =								(iniFile.ReadValue(	"Option", "language" ));
		}

		

	}

	public static class Stats {
		public static string currentAction { get; set; } = "Waiting";
		public static string currentPlayer { get; set; } = "Null";
        public static int playersFound { get; set; } = 0;
		public static int playersBought { get; set; } = 0;
		public static int playersLost { get; set; } = 0;
		public static Stopwatch searchTime { get; set; } = new Stopwatch();

	}
}
