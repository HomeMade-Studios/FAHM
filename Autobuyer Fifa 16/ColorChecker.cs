using MoreLinq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Autobuyer_Fifa_16 {
	class ColorChecker {

		public struct ColorPoint {

			public Point position { get; }
			public Color color { get; }

			public ColorPoint(Point position, Color color) {
				this.position = position;
				this.color = color;
			}
		}

		public struct ColorRatio {

			public Ratio ratio { get; }
			public Color color { get; }

			public ColorRatio(Ratio ratio, Color color) {
				this.ratio = ratio;
				this.color = color;
			}
		}

		public static Color GetPixel(Point position) {
			using (var bitmap = new Bitmap(1, 1)) {
				using (var graphics = Graphics.FromImage(bitmap)) {
					graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
				}
				return bitmap.GetPixel(0, 0);
			}
		}

		static Dictionary<string, ColorRatio> colorPoints169 { get; } = new Dictionary<string, ColorRatio>() {
			{ "PlayerNotFound",     new ColorRatio( new Ratio(0.473958333333333, 0.652777777777778), Color.FromArgb(255, 56, 130, 149) ) },	//TASTO BLU DELL'OK SULL'ERRORE
			{ "PlayerFound",        new ColorRatio( new Ratio(0.005208333333333, 0.611111111111111), Color.FromArgb(255, 212, 222, 217)) },	//PARTE BIANCA DI FIANCO A "RIASSUNTO"
			{ "BuyNow",             new ColorRatio( new Ratio(0.593750000000000, 0.527777777777778), Color.FromArgb(255, 211, 174, 55) ) },	//PARTE ARANCIONE DEL COMPRA ORA
            { "ConfirmBuyNow",		new ColorRatio( new Ratio(0.432291666666667, 0.652777777777778), Color.FromArgb(255, 56, 130, 149) ) },	//TASTO BLU CONFERMA COMPRA ORA
            { "QuickList",          new ColorRatio( new Ratio(0.352083333333333, 0.407407407407407), Color.FromArgb(255, 56, 130, 149) ) },	//TASTO VENDI SUBITO
            { "Search",				new ColorRatio( new Ratio(0.341145833333333, 0.962962962962963), Color.FromArgb(255, 211, 174, 55) ) }	//TASTO CERCA
		};

		static Dictionary<string, ColorRatio> colorPoints1610 { get; } = new Dictionary<string, ColorRatio>() {
			{ "PlayerNotFound",     new ColorRatio( new Ratio(0.479166666666667, 0.638888888888889), Color.FromArgb(255, 56, 130, 149) ) },
			{ "PlayerFound",        new ColorRatio( new Ratio(0.006944444444444, 0.600000000000000), Color.FromArgb(255, 212, 222, 217)) },
			{ "BuyNow",             new ColorRatio( new Ratio(0.600694444444444, 0.525555555555556), Color.FromArgb(255, 211, 174, 55) ) },
			{ "ConfirmBuyNow",		new ColorRatio( new Ratio(0.434027777777778, 0.638888888888889), Color.FromArgb(255, 56, 130, 149) ) },
			{ "QuickList",          new ColorRatio( new Ratio(0.353472222222222, 0.411111111111111), Color.FromArgb(255, 56, 130, 149) ) },
			{ "Search",             new ColorRatio( new Ratio(0.340277777777778, 0.922222222222222), Color.FromArgb(255, 211, 174, 55) ) }	//TASTO CERCA
		};

		static Dictionary<string, ColorRatio> colorPoints43 { get; } = new Dictionary<string, ColorRatio>() {
			{ "PlayerNotFound",     new ColorRatio( new Ratio(0.478571428571429, 0.614285714285714), Color.FromArgb(255, 56, 130, 149) ) },
			{ "PlayerFound",        new ColorRatio( new Ratio(0.007142857142857, 0.585714285714286), Color.FromArgb(255, 212, 222, 217)) },
			{ "BuyNow",             new ColorRatio( new Ratio(0.600000000000000, 0.523809523809524), Color.FromArgb(255, 211, 174, 55) ) },
            { "ConfirmBuyNow",      new ColorRatio( new Ratio(0.432142857142857, 0.614285714285714), Color.FromArgb(255, 56, 130, 149) ) },
			{ "QuickList",          new ColorRatio( new Ratio(0.353571428571429, 0.433333333333333), Color.FromArgb(255, 56, 130, 149) ) },
			{ "Search",             new ColorRatio( new Ratio(0.342857142857143, 0.852380952380952), Color.FromArgb(255, 211, 174, 55) ) }	//TASTO CERCA
		};

		public static Dictionary<string, ColorPoint> colorPoints { get; } = new Dictionary<string, ColorPoint>();

		public static int SetColorPoints(Point screen) {
			double[] formats = new double[3] { (4d / 3d), (16d / 10d), (16d / 9d) };
			double ratio = (double)screen.X / (double)screen.Y;
			double nearest = formats.MinBy(x => Math.Abs((double)x - ratio));

			if (nearest == formats[0]) {		// 4 : 3
				colorPoints.Add("PlayerNotFound", new ColorPoint(Ratio.Multiply(screen, colorPoints43["PlayerNotFound"].ratio), colorPoints43["PlayerNotFound"].color));
				colorPoints.Add("PlayerFound", new ColorPoint(Ratio.Multiply(screen, colorPoints43["PlayerFound"].ratio), colorPoints43["PlayerFound"].color));
				colorPoints.Add("BuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints43["BuyNow"].ratio), colorPoints43["BuyNow"].color));
				colorPoints.Add("ConfirmBuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints43["ConfirmBuyNow"].ratio), colorPoints43["ConfirmBuyNow"].color));
				colorPoints.Add("QuickList", new ColorPoint(Ratio.Multiply(screen, colorPoints43["QuickList"].ratio), colorPoints43["QuickList"].color));
				colorPoints.Add("Search", new ColorPoint(Ratio.Multiply(screen, colorPoints43["Search"].ratio), colorPoints43["Search"].color));
				return 43;
			}
			if (nearest == formats[1]) {		// 16 : 10
				colorPoints.Add("PlayerNotFound", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["PlayerNotFound"].ratio), colorPoints1610["PlayerNotFound"].color));
				colorPoints.Add("PlayerFound", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["PlayerFound"].ratio), colorPoints1610["PlayerFound"].color));
				colorPoints.Add("BuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["BuyNow"].ratio), colorPoints1610["BuyNow"].color));
				colorPoints.Add("ConfirmBuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["ConfirmBuyNow"].ratio), colorPoints1610["ConfirmBuyNow"].color));
				colorPoints.Add("QuickList", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["QuickList"].ratio), colorPoints1610["QuickList"].color));
				colorPoints.Add("Search", new ColorPoint(Ratio.Multiply(screen, colorPoints1610["Search"].ratio), colorPoints1610["Search"].color));
				return 1610;
			}
			if (nearest == formats[2]) {      // 16 : 9
				colorPoints.Add("PlayerNotFound", new ColorPoint(Ratio.Multiply(screen, colorPoints169["PlayerNotFound"].ratio), colorPoints169["PlayerNotFound"].color));
				colorPoints.Add("PlayerFound", new ColorPoint(Ratio.Multiply(screen, colorPoints169["PlayerFound"].ratio), colorPoints169["PlayerFound"].color));
				colorPoints.Add("BuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints169["BuyNow"].ratio), colorPoints169["BuyNow"].color));
				colorPoints.Add("ConfirmBuyNow", new ColorPoint(Ratio.Multiply(screen, colorPoints169["ConfirmBuyNow"].ratio), colorPoints169["ConfirmBuyNow"].color));
				colorPoints.Add("QuickList", new ColorPoint(Ratio.Multiply(screen, colorPoints169["QuickList"].ratio), colorPoints169["QuickList"].color));
				colorPoints.Add("Search", new ColorPoint(Ratio.Multiply(screen, colorPoints169["Search"].ratio), colorPoints169["Search"].color));
				return 169;
			}
			return 0;
		}
	}
}
