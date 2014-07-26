using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RockProcessing.Model;

namespace RockProcessing.Test {
	[TestFixture]
	class RockJobProcessorTest : IRockJobMonitor
	{
		[Test]
		public void RockJobMarkedCompleteAfterProcessing()
		{
			var processor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Granit, 3, this);
			Assert.IsFalse(rockJob.Complete, "Inital completed state of RockJob incorrect");
			processor.Process(rockJob);
			Assert.IsTrue(rockJob.Complete);
		}

		[Test]
		public void RockJobPostProcessWeightCorrectAfterProcessingWhenSmoothingProcessApplied() {
			var rockJobProcessor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Granit, 3, this);

			double expectedMinWeight = rockJob.CurrentWeight - SmoothProcessMaxWeightLoss(rockJob.CurrentWeight);
			double expectedMaxWeight = rockJob.CurrentWeight - SmoothProcessMinWeightLoss(rockJob.CurrentWeight);
			Console.WriteLine("Expected Minimum Weight {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);
			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void RockJobPostProcessWeightCorrectAfterProcessingWhenCrushingProcessApplied() {
			var rockJobProcessor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Pegmatite, 3, this);

			double expectedMinWeight = rockJob.CurrentWeight - CrushProcessMaxWeightLoss(rockJob.CurrentWeight);
			double expectedMaxWeight = rockJob.CurrentWeight - CrushProcessMinWeightLoss(rockJob.CurrentWeight);
			Console.WriteLine("Expected Minimum Weight {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);
			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void RockJobPostProcessWeightCorrectAfterProcessingWhenBothProcessingTypesApplied() {
			var rockJobProcessor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Gneiss, 7, this);

			double smoothProcessMaxWeightLoss = SmoothProcessMaxWeightLoss(rockJob.CurrentWeight);
			double smoothProcessMinWeightLoss = SmoothProcessMinWeightLoss(rockJob.CurrentWeight);
			double crushProcessMaxWeightLoss = CrushProcessMaxWeightLoss(rockJob.CurrentWeight);
			double crushProcessMinWeightLoss = CrushProcessMinWeightLoss(rockJob.CurrentWeight);
			double expectedMinWeight = rockJob.OriginWeight - (smoothProcessMaxWeightLoss + crushProcessMaxWeightLoss);
			double expectedMaxWeight = rockJob.OriginWeight - (smoothProcessMinWeightLoss + crushProcessMinWeightLoss);
			Console.WriteLine("Expected Minimum Weight {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		#region Private members
	
		private const double MinSmoothingPercentDegrade = 5;
		private const double MaxSmoothingPercentDegrade = 7;
		private const double MinCrushingPercentDegrade = 20;
		private const double MaxCrushingPercentDegrade = 30;

		private static double CrushProcessMinWeightLoss(double weight)
		{
			return Math.Round((weight / 100) * MinCrushingPercentDegrade, 3);
		}

		private static double CrushProcessMaxWeightLoss(double weight)
		{
			return Math.Round((weight / 100) * MaxCrushingPercentDegrade, 3);
		}

		private static double SmoothProcessMinWeightLoss(double weight)
		{
			return Math.Round((weight / 100) * MinSmoothingPercentDegrade, 3);
		}

		private static double SmoothProcessMaxWeightLoss(double weight)
		{
			return Math.Round((weight / 100) * MaxSmoothingPercentDegrade, 3);
		}

		#endregion

		#region IRockJobMonitor members

		void IRockJobMonitor.NotifiyJobcomplete(Guid jobId)
		{
			//TODO
		}

		#endregion
	}
}
