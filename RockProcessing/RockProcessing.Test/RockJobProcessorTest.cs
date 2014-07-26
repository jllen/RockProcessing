using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RockProcessing.Model;

namespace RockProcessing.Test {
	[TestFixture]
	class RockJobProcessorTest : TestBase, IRockJobMonitor
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
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

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
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);
			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void RockJobPostProcessWeightCorrectAfterProcessingWhenBothProcessingTypesApplied() {
			var rockJobProcessor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Gneiss, 6.8, this);

			double smoothProcessMaxWeightLoss = SmoothProcessMaxWeightLoss(rockJob.CurrentWeight);
			double smoothProcessMinWeightLoss = SmoothProcessMinWeightLoss(rockJob.CurrentWeight);
			double crushProcessMaxWeightLoss = CrushProcessMaxWeightLoss(rockJob.CurrentWeight);
			double crushProcessMinWeightLoss = CrushProcessMinWeightLoss(rockJob.CurrentWeight);
			double expectedMinWeight = rockJob.OriginWeight - (smoothProcessMaxWeightLoss + crushProcessMaxWeightLoss);
			double expectedMaxWeight = rockJob.OriginWeight - (smoothProcessMinWeightLoss + crushProcessMinWeightLoss);
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight, "Post process weight not within the expected bounds");
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		#region IRockJobMonitor members

		void IRockJobMonitor.NotifiyJobcomplete(Guid jobId)
		{
			//TODO
		}

		#endregion
	}
}
