using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockProcessing.Test {
	public class TestBase {

		#region Protected members

		protected const double MinSmoothingPercentDegrade = 5;
		protected const double MaxSmoothingPercentDegrade = 7;
		protected const double MinCrushingPercentDegrade = 20;
		protected const double MaxCrushingPercentDegrade = 30;

		protected double CrushProcessMinWeightLoss(double weight) {
			return Math.Round((weight / 100) * MinCrushingPercentDegrade, 3);
		}

		protected double CrushProcessMaxWeightLoss(double weight) {
			return Math.Round((weight / 100) * MaxCrushingPercentDegrade, 3);
		}

		protected double SmoothProcessMinWeightLoss(double weight) {
			return Math.Round((weight / 100) * MinSmoothingPercentDegrade, 3);
		}

		protected double SmoothProcessMaxWeightLoss(double weight) {
			return Math.Round((weight / 100) * MaxSmoothingPercentDegrade, 3);
		}

		#endregion

	}
}
