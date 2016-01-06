using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MoreLinq;

namespace Autobuyer_Fifa_16 {

	public static class Coordinates {

		static Dictionary<string, Ratio> coordsRatio169 { get; } = new Dictionary<string, Ratio>() {	//16:9
			{ "Search",             new Ratio( 0.285505124450952, 0.963541666666667 ) },
			{ "Card",               new Ratio( 0.080527086383601, 0.820312500000000 ) },
			{ "BuyNow",             new Ratio( 0.677159590043924, 0.527343750000000 ) },
			{ "ConfirmBuy",         new Ratio( 0.453879941434846, 0.651041666666667 ) },
			{ "SendToTransfer",     new Ratio( 0.274524158125915, 0.468750000000000 ) },
			{ "Back",               new Ratio( 0.237920937042460, 0.611979166666667 ) },
			{ "CloseNotFoundError", new Ratio( 0.499267935578331, 0.651041666666667 ) },
            { "TransferMarket",		new Ratio( 0.354166666666667, 0.189814814814815 ) },
			{ "TransferList",       new Ratio( 0.636896046852123, 0.190104166666667 ) },
			{ "TransferTargets",    new Ratio( 0.497803806734993, 0.188802083333333 ) },
			{ "QuickList",			new Ratio( 0.273437500000000, 0.402777777777778 ) },
			{ "SellBuyNowPrice",    new Ratio( 0.695312500000000, 0.453703703703704 ) },
			{ "SellStartPrice",     new Ratio( 0.513020833333333, 0.458333333333333 ) },
			{ "ConfirmSell",		new Ratio( 0.447916666666667, 0.824074074074074 ) },
			{ "ResetPlayerName",    new Ratio( 0.300000000000000, 0.401851851851852 ) },
			{ "PlayerName",			new Ratio( 0.164062500000000, 0.402777777777778 ) },
			{ "BuyNowMax",			new Ratio( 0.252604166666667, 0.828703703703704 ) },
            { "CardQuality",        new Ratio( 0.48437500000000, 0.569444444444444 ) }, //
            { "SpecialQuality",     new Ratio( 0.48437500000000, 0.777777777777778 ) }, //
            { "AnyQuality",         new Ratio( 0.48437500000000, 0.602777777777778 ) }, //
        };

		static Dictionary<string, Ratio> coordsRatio1610 { get; } = new Dictionary<string, Ratio>() {	//16:10
			{ "Search",             new Ratio( 0.284722222222222, 0.922222222222222 ) },
			{ "Card",               new Ratio( 0.079861111111111, 0.783333333333333 ) },
			{ "BuyNow",             new Ratio( 0.684027777777778, 0.524444444444444 ) },
			{ "ConfirmBuy",         new Ratio( 0.451388888888889, 0.638888888888889 ) },
			{ "SendToTransfer",     new Ratio( 0.253472222222222, 0.472222222222222 ) },
			{ "Back",               new Ratio( 0.239583333333333, 0.600000000000000 ) },
			{ "CloseNotFoundError", new Ratio( 0.500000000000000, 0.633333333333333 ) },
			{ "TransferMarket",		new Ratio( 0.357142857142857, 0.222222222222222 ) },
			{ "TransferList",       new Ratio( 0.642361111111111, 0.222222222222222 ) },
			{ "TransferTargets",    new Ratio( 0.500000000000000, 0.222222222222222 ) },
            { "QuickList",          new Ratio( 0.277777777777778, 0.411111111111111 ) },
			{ "SellBuyNowPrice",    new Ratio( 0.698611111111111, 0.461111111111111 ) },
			{ "SellStartPrice",     new Ratio( 0.513888888888889, 0.461111111111111 ) },
			{ "ConfirmSell",        new Ratio( 0.447916666666667, 0.788888888888889 ) },
			{ "ResetPlayerName",    new Ratio( 0.300694444444444, 0.411111111111111 ) },
			{ "PlayerName",         new Ratio( 0.160416666666667, 0.413333333333333 ) },
			{ "BuyNowMax",          new Ratio( 0.254166666666667, 0.797777777777778 ) },
            { "CardQuality",        new Ratio( 0.485119047619048, 0.557142857142857 ) }, //
            { "SpecialQuality",     new Ratio( 0.485119047619048, 0.747619047619048 ) }, //
            { "AnyQuality",         new Ratio( 0.485119047619048, 0.597142857142857 ) }, //

		};

		static Dictionary<string, Ratio> coordsRatio43 { get; } = new Dictionary<string, Ratio>() {		//4:3
			{ "Search",             new Ratio( 0.285714285714286, 0.847619047619048 ) },
			{ "Card",               new Ratio( 0.078571428571428, 0.731428571428571 ) },
			{ "BuyNow",             new Ratio( 0.678571428571429, 0.519047619047619 ) },
			{ "ConfirmBuy",         new Ratio( 0.457142857142857, 0.614285714285714 ) },
			{ "SendToTransfer",     new Ratio( 0.257142857142857, 0.476190476190476 ) },
			{ "Back",               new Ratio( 0.235714285714286, 0.580952380952381 ) },
			{ "CloseNotFoundError", new Ratio( 0.500000000000000, 0.614285714285714 ) },
			{ "TransferMarket",		new Ratio( 0.357142857142857, 0.266666666666667 ) },
			{ "TransferList",       new Ratio( 0.642857142857143, 0.266666666666667 ) },
			{ "TransferTargets",    new Ratio( 0.496428571428571, 0.266666666666667 ) },
            { "QuickList",          new Ratio( 0.292857142857143, 0.428571428571429 ) },
			{ "SellBuyNowPrice",    new Ratio( 0.698571428571429, 0.466666666666667 ) },
			{ "SellStartPrice",     new Ratio( 0.515000000000000, 0.466666666666667 ) },
			{ "ConfirmSell",        new Ratio( 0.450000000000000, 0.742857142857143 ) },
			{ "ResetPlayerName",    new Ratio( 0.300714285714286, 0.426666666666667 ) },
			{ "PlayerName",         new Ratio( 0.166428571428571, 0.428571428571429 ) },
			{ "BuyNowMax",          new Ratio( 0.253571428571429, 0.744761904761905 ) },
            { "CardQuality",        new Ratio( 0.484375000000000, 0.546875000000000 ) },
            { "SpecialQuality",     new Ratio( 0.484375000000000, 0.693359375000000 ) },
            { "AnyQuality",         new Ratio( 0.484375000000000, 0.573242187500000 ) },
		};

		public static Dictionary<string, Point> Coords { get; } = new Dictionary<string, Point>();

		public static int SetCoord(Point screen) {
			double[] formats = new double[3] { (4d/3d), (16d/10d), (16d/9d) };
			double ratio = (double)screen.X / (double)screen.Y;
			double nearest = formats.MinBy(x => Math.Abs((double)x - ratio));

			if (nearest == formats[0]) {       // 4 : 3
				Coords.Add("Search", Ratio.Multiply(screen, coordsRatio43["Search"]));
				Coords.Add("Card", Ratio.Multiply(screen, coordsRatio43["Card"]));
				Coords.Add("BuyNow", Ratio.Multiply(screen, coordsRatio43["BuyNow"]));
				Coords.Add("ConfirmBuy", Ratio.Multiply(screen, coordsRatio43["ConfirmBuy"]));
				Coords.Add("SendToTransfer", Ratio.Multiply(screen, coordsRatio43["SendToTransfer"]));
				Coords.Add("Back", Ratio.Multiply(screen, coordsRatio43["Back"]));
				Coords.Add("CloseNotFoundError", Ratio.Multiply(screen, coordsRatio43["CloseNotFoundError"]));
				Coords.Add("TransferMarket", Ratio.Multiply(screen, coordsRatio43["TransferMarket"]));
				Coords.Add("TransferList", Ratio.Multiply(screen, coordsRatio43["TransferList"]));
				Coords.Add("TransferTargets", Ratio.Multiply(screen, coordsRatio43["TransferTargets"]));
				Coords.Add("QuickList", Ratio.Multiply(screen, coordsRatio43["QuickList"]));
				Coords.Add("SellBuyNowPrice", Ratio.Multiply(screen, coordsRatio43["SellBuyNowPrice"]));
				Coords.Add("SellStartPrice", Ratio.Multiply(screen, coordsRatio43["SellStartPrice"]));
				Coords.Add("ConfirmSell", Ratio.Multiply(screen, coordsRatio43["ConfirmSell"]));
				Coords.Add("ResetPlayerName", Ratio.Multiply(screen, coordsRatio43["ResetPlayerName"]));
				Coords.Add("PlayerName", Ratio.Multiply(screen, coordsRatio43["PlayerName"]));
				Coords.Add("BuyNowMax", Ratio.Multiply(screen, coordsRatio43["BuyNowMax"]));
                Coords.Add("CardQuality", Ratio.Multiply(screen, coordsRatio43["CardQuality"]));
                Coords.Add("SpecialQuality", Ratio.Multiply(screen, coordsRatio43["SpecialQuality"]));
                Coords.Add("AnyQuality", Ratio.Multiply(screen, coordsRatio43["AnyQuality"]));
                return 43;
			}
			if (nearest == formats[1]) {		// 16 : 10
				Coords.Add("Search", Ratio.Multiply(screen, coordsRatio1610["Search"]));
				Coords.Add("Card", Ratio.Multiply(screen, coordsRatio1610["Card"]));
				Coords.Add("BuyNow", Ratio.Multiply(screen, coordsRatio1610["BuyNow"]));
				Coords.Add("ConfirmBuy", Ratio.Multiply(screen, coordsRatio1610["ConfirmBuy"]));
				Coords.Add("SendToTransfer", Ratio.Multiply(screen, coordsRatio1610["SendToTransfer"]));
				Coords.Add("Back", Ratio.Multiply(screen, coordsRatio1610["Back"]));
				Coords.Add("CloseNotFoundError", Ratio.Multiply(screen, coordsRatio1610["CloseNotFoundError"]));
				Coords.Add("TransferMarket", Ratio.Multiply(screen, coordsRatio1610["TransferMarket"]));
				Coords.Add("TransferList", Ratio.Multiply(screen, coordsRatio1610["TransferList"]));
				Coords.Add("TransferTargets", Ratio.Multiply(screen, coordsRatio1610["TransferTargets"]));
				Coords.Add("QuickList", Ratio.Multiply(screen, coordsRatio1610["QuickList"]));
				Coords.Add("SellBuyNowPrice", Ratio.Multiply(screen, coordsRatio1610["SellBuyNowPrice"]));
				Coords.Add("SellStartPrice", Ratio.Multiply(screen, coordsRatio1610["SellStartPrice"]));
				Coords.Add("ConfirmSell", Ratio.Multiply(screen, coordsRatio1610["ConfirmSell"]));
				Coords.Add("ResetPlayerName", Ratio.Multiply(screen, coordsRatio1610["ResetPlayerName"]));
				Coords.Add("PlayerName", Ratio.Multiply(screen, coordsRatio1610["PlayerName"]));
				Coords.Add("BuyNowMax", Ratio.Multiply(screen, coordsRatio1610["BuyNowMax"]));
                Coords.Add("CardQuality", Ratio.Multiply(screen, coordsRatio1610["CardQuality"]));
                Coords.Add("SpecialQuality", Ratio.Multiply(screen, coordsRatio1610["SpecialQuality"]));
                Coords.Add("AnyQuality", Ratio.Multiply(screen, coordsRatio1610["AnyQuality"]));
                return 1610;
			}
			if (nearest == formats[2]){               // 16 : 9
				Coords.Add("Search", Ratio.Multiply(screen, coordsRatio169["Search"]));
				Coords.Add("Card", Ratio.Multiply(screen, coordsRatio169["Card"]));
				Coords.Add("BuyNow", Ratio.Multiply(screen, coordsRatio169["BuyNow"]));
				Coords.Add("ConfirmBuy", Ratio.Multiply(screen, coordsRatio169["ConfirmBuy"]));
				Coords.Add("SendToTransfer", Ratio.Multiply(screen, coordsRatio169["SendToTransfer"]));
				Coords.Add("Back", Ratio.Multiply(screen, coordsRatio169["Back"]));
				Coords.Add("CloseNotFoundError", Ratio.Multiply(screen, coordsRatio169["CloseNotFoundError"]));
				Coords.Add("TransferMarket", Ratio.Multiply(screen, coordsRatio169["TransferMarket"]));
				Coords.Add("TransferList", Ratio.Multiply(screen, coordsRatio169["TransferList"]));
				Coords.Add("TransferTargets", Ratio.Multiply(screen, coordsRatio169["TransferTargets"]));
				Coords.Add("QuickList", Ratio.Multiply(screen, coordsRatio169["QuickList"]));
				Coords.Add("SellBuyNowPrice", Ratio.Multiply(screen, coordsRatio169["SellBuyNowPrice"]));
				Coords.Add("SellStartPrice", Ratio.Multiply(screen, coordsRatio169["SellStartPrice"]));
				Coords.Add("ConfirmSell", Ratio.Multiply(screen, coordsRatio169["ConfirmSell"]));
				Coords.Add("ResetPlayerName", Ratio.Multiply(screen, coordsRatio169["ResetPlayerName"]));
				Coords.Add("PlayerName", Ratio.Multiply(screen, coordsRatio169["PlayerName"]));
				Coords.Add("BuyNowMax", Ratio.Multiply(screen, coordsRatio169["BuyNowMax"]));
                Coords.Add("CardQuality", Ratio.Multiply(screen, coordsRatio169["CardQuality"]));
                Coords.Add("SpecialQuality", Ratio.Multiply(screen, coordsRatio169["SpecialQuality"]));
                Coords.Add("AnyQuality", Ratio.Multiply(screen, coordsRatio169["AnyQuality"]));
                return 169;
			}
			return 0;
		}
	}

	public class Ratio {

		public double X { get; }
		public double Y { get; }

		public Ratio(double x, double y) {
			X = x;
			Y = y;
		}

		public static Point Multiply(Point point, Ratio ratio) {
			Point result = new Point();
			result.X = (int)(point.X * ratio.X);
			result.Y = (int)(point.Y * ratio.Y);
			return result;
		}
	}
}
