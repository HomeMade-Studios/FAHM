using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AutobuyerFifa16;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace AutobuyerFifa16 {
	public static class SearchOperations {

		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

		static void SimulateClickAtPosition(Point coords, int waitTime) {
			Cursor.Position = new Point(coords.X, coords.Y);
			Thread.Sleep(waitTime);
			Form1.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)coords.X, (uint)coords.Y, 0, 0);
		}

		static void SimulateDoubleClickAtPosition(Point coords, int waitTime) {
			Cursor.Position = new Point(coords.X, coords.Y);
			Thread.Sleep(waitTime);
			Form1.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)coords.X, (uint)coords.Y, 0, 0);
			Thread.Sleep(20);
			Form1.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)coords.X, (uint)coords.Y, 0, 0);
		}

		public static void SetSearchParameters(int playerIndex) {
			string name = Players.players[playerIndex].name;
			string buyNow = Players.players[playerIndex].buyNow.ToString();
            bool isSpecial = Players.players[playerIndex].isSpecial;

			SimulateClickAtPosition(Coordinates.Coords["ResetPlayerName"], 1500);
			SimulateDoubleClickAtPosition(Coordinates.Coords["PlayerName"], 2000);
			Keyboard.Write(name);
			Keyboard.PressEnter();
			SimulateDoubleClickAtPosition(Coordinates.Coords["BuyNowMax"], 1000);
			Keyboard.Write(buyNow);

            SimulateClickAtPosition(Coordinates.Coords["CardQuality"], 1000);
            if (isSpecial)
            {
                SimulateClickAtPosition(Coordinates.Coords["SpecialQuality"], 1000);
            }
            else
            {
                SimulateClickAtPosition(Coordinates.Coords["AnyQuality"], 1000);
            }
        }

		public static void Search() {
			SimulateClickAtPosition(Coordinates.Coords["Search"], 50);
		}

		public static bool WaitSearchResult() {
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (Config.work && stopwatch.ElapsedMilliseconds < 5000) {
				if (ColorChecker.GetPixel(ColorChecker.colorPoints["PlayerNotFound"].position) == ColorChecker.colorPoints["PlayerNotFound"].color) {
					return false;
				}

				if (ColorChecker.GetPixel(ColorChecker.colorPoints["PlayerFound"].position) == ColorChecker.colorPoints["PlayerFound"].color) {
					return true;
				}
				Thread.Sleep(5);
			}
			return false;
		}

		public static void Buy() {
			Stopwatch stopwatch = new Stopwatch();

			SimulateClickAtPosition(Coordinates.Coords["Card"], 50);
			stopwatch.Start();
			while (Config.work && ColorChecker.GetPixel(ColorChecker.colorPoints["BuyNow"].position) != ColorChecker.colorPoints["BuyNow"].color && stopwatch.ElapsedMilliseconds < 1500) {
				Thread.Sleep(5);
			}
			SimulateClickAtPosition(Coordinates.Coords["BuyNow"], 50);
			stopwatch.Restart();
			while (Config.work && ColorChecker.GetPixel(ColorChecker.colorPoints["ConfirmBuyNow"].position) != ColorChecker.colorPoints["ConfirmBuyNow"].color && stopwatch.ElapsedMilliseconds < 1500) {
				Thread.Sleep(5);
			}
			SimulateClickAtPosition(Coordinates.Coords["ConfirmBuy"], 50);
		}

		public static bool WaitBuyResult() {
			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();

			while (Config.work && stopwatch.ElapsedMilliseconds < 1000) {
				if (ColorChecker.GetPixel(ColorChecker.colorPoints["PlayerNotFound"].position) == ColorChecker.colorPoints["PlayerNotFound"].color) {
					return false;
				}

				if (ColorChecker.GetPixel(ColorChecker.colorPoints["QuickList"].position) == ColorChecker.colorPoints["QuickList"].color) {
					return true;
				}
				Thread.Sleep(5);
			}
			return false;
		}

		public static void Sell(int playerIndex) {
			string sellPrice = Players.players[playerIndex].sellPrice.ToString();

			SimulateClickAtPosition(Coordinates.Coords["QuickList"], 50);
			SimulateDoubleClickAtPosition(Coordinates.Coords["SellBuyNowPrice"], 1000);
			Keyboard.Write("0");
			SimulateDoubleClickAtPosition(Coordinates.Coords["SellStartPrice"], 1000);
			Keyboard.Write(sellPrice);
			SimulateClickAtPosition(Coordinates.Coords["ConfirmSell"], 1000);
		}

		public static void SendToTransferList() {
			SimulateClickAtPosition(Coordinates.Coords["SendToTransfer"], 1000);
		}

		public static void ToTransferTargets() {

			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();

			SimulateClickAtPosition(Coordinates.Coords["TransferTargets"], 500);
			while (Config.work && ColorChecker.GetPixel(ColorChecker.colorPoints["PlayerFound"].position) != ColorChecker.colorPoints["PlayerFound"].color && stopwatch.ElapsedMilliseconds < 2000) {
				Thread.Sleep(5);
			}
		}

		public static void BackToMarket() {

			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();

			SimulateClickAtPosition(Coordinates.Coords["TransferMarket"], 100);

			while (Config.work && ColorChecker.GetPixel(ColorChecker.colorPoints["Search"].position) != ColorChecker.colorPoints["Search"].color && stopwatch.ElapsedMilliseconds < 3000) {
				Thread.Sleep(5);
			}
		}

		public static void CloseError() {
			SimulateClickAtPosition(Coordinates.Coords["CloseNotFoundError"], 100);
		}

	}
}
