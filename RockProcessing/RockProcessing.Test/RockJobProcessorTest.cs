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

		//TODO - check this. probably passes due to range but not likely accurate due to secondary weight loss dependent upon variable loss of first loss
		[Test]
		public void RockJobPostProcessWeightCorrectAfterProcessingWhenBothProcessingTypesApplied() {
			var rockJobProcessor = new RockJobProcessor();
			const RockType rockType = RockType.Gneiss;
			var rockJob = new RockJob(rockType, 6.8, this);

			double smoothProcessMaxWeightLoss = SmoothProcessMaxWeightLoss(rockJob.CurrentWeight);
			double smoothProcessMinWeightLoss = SmoothProcessMinWeightLoss(rockJob.CurrentWeight);
			double crushProcessMaxWeightLoss = CrushProcessMaxWeightLoss(rockJob.CurrentWeight);
			double crushProcessMinWeightLoss = CrushProcessMinWeightLoss(rockJob.CurrentWeight);
			double expectedMinWeight = rockJob.OriginWeight - (smoothProcessMaxWeightLoss + crushProcessMaxWeightLoss);
			double expectedMaxWeight = rockJob.OriginWeight - (smoothProcessMinWeightLoss + crushProcessMinWeightLoss);
			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, rockJob.OriginWeight);
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight, "Post process weight not within the expected bounds");
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		// TODO - Bit of duplication going on here between this and the E2E tests. Revisit, probably change the E2E tests to be higher level concerns

		[Test]
		public void RockJobProcessTimeOfCompletedJobIsCorrectWhenRockRequiringSmoothingSentForProcessing() {
			var rockJobProcessor = new RockJobProcessor();
			const double weight = 5.6;
			const RockType rockType = RockType.Granit;
			const int expectedProcessTime = (int) (weight * SmoothingProcessingTimePerKilo);
			var rockJob = new RockJob(rockType, weight, this);

			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, weight);
			Console.WriteLine("Expected process time {0}ms", expectedProcessTime);

			rockJobProcessor.Process(rockJob);

			Assert.AreEqual(expectedProcessTime, rockJob.ProcessTime, "Process time not as expected");
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void RockJobProcessTimeOfCompletedJobIsCorrectWhenRockRequiringCrushingSentForProcessing() {
			var rockJobProcessor = new RockJobProcessor();
			const double weight = 3;
			const RockType rockType = RockType.Pegmatite;
			const int expectedProcessTime = (int) (weight * CrushingProcessingTimePerKilo);
			var rockJob = new RockJob(rockType, weight, this);

			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, weight);
			Console.WriteLine("Expected process time {0}ms", expectedProcessTime);

			rockJobProcessor.Process(rockJob);

			Assert.AreEqual(expectedProcessTime, rockJob.ProcessTime, "Process time not as expected");
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void RockJobProcessTimeOfCompletedJobIsCorrectWhenRockRequiringBothProcessingTypesSentForProcessing() {
			//TODO - rationalise this its not pretty

			var rockJobProcessor = new RockJobProcessor();
			const double weight = 3;
			const RockType rockType = RockType.Gneiss;
		
			double crushProcessMinWeightLoss = CrushProcessMinWeightLoss(weight);
			double crushProcessMaxWeightLoss = CrushProcessMaxWeightLoss(weight);
			double expectedCrushProcessTimeLow = ((weight - crushProcessMaxWeightLoss) * CrushingProcessingTimePerKilo);
			double expectedCrushProcessTimeHigh = ((weight - crushProcessMinWeightLoss) * CrushingProcessingTimePerKilo);

			double expectedProcessTimeLow = (expectedCrushProcessTimeLow) + ((weight - crushProcessMaxWeightLoss) * SmoothingProcessingTimePerKilo);
			double expectedProcessTimeHigh = (expectedCrushProcessTimeHigh) + ((weight - crushProcessMinWeightLoss) * SmoothingProcessingTimePerKilo);
			
			var rockJob = new RockJob(rockType, weight, this);

			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, weight);
			Console.WriteLine("Expected process time between {0}ms and {1}ms", expectedProcessTimeLow, expectedProcessTimeHigh);

			rockJobProcessor.Process(rockJob);

			Assert.GreaterOrEqual(rockJob.ProcessTime, expectedProcessTimeLow, "Process time not as expected");
			Assert.LessOrEqual(rockJob.ProcessTime, expectedProcessTimeHigh, "Process time not as expected");
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
