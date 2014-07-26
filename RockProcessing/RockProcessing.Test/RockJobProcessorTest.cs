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
		private const double MinSmoothingDegrade = 5;
		private const double MaxSmoothingDegrade = 7;
		private const double MinCrushingDegrade = 20;
		private const double MaxCrushingDegrade = 30;

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
	
			double expectedMinWeight = Math.Round(rockJob.OriginWeight - ((rockJob.OriginWeight / 100) * MaxSmoothingDegrade), 3);
			double expectedMaxWeight = Math.Round(rockJob.OriginWeight - ((rockJob.OriginWeight / 100) * MinSmoothingDegrade), 3);
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

			double expectedMinWeight = Math.Round(rockJob.OriginWeight - ((rockJob.OriginWeight / 100) * MaxCrushingDegrade), 3);
			double expectedMaxWeight = Math.Round(rockJob.OriginWeight - ((rockJob.OriginWeight / 100) * MinCrushingDegrade), 3);
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

			double smoothProcessMaxWeightLoss = Math.Round((rockJob.OriginWeight / 100) * MaxSmoothingDegrade, 3);
			double smoothProcessMinWeightLoss = Math.Round((rockJob.OriginWeight / 100) * MinSmoothingDegrade, 3);
			double crushProcessMaxWeightLoss = Math.Round((rockJob.OriginWeight / 100) * MaxCrushingDegrade, 3);
			double crushProcessMinWeightLoss = Math.Round((rockJob.OriginWeight / 100) * MinCrushingDegrade, 3);
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


		public void NotifiyJobcomplete(Guid jobId)
		{
			//TODO
		}
	}
}
