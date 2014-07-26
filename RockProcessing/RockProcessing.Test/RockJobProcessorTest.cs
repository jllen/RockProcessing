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
		private const double MinGranitDegrade = 5;
		private const double MaxGranitDegrade = 7;

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
	
			double expectedMinWeight = rockJob.PreProcessWeight - ((rockJob.PreProcessWeight / 100) * MaxGranitDegrade);
			double expectedMaxWeight = rockJob.PreProcessWeight - ((rockJob.PreProcessWeight / 100) * MinGranitDegrade);
			Console.WriteLine("Expected Minimum Weight {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight {0}", expectedMaxWeight);

			rockJobProcessor.Process(rockJob);
			Assert.GreaterOrEqual(rockJob.PostProcessWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.PostProcessWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.PostProcessWeight);
			Console.WriteLine("Test Complete");
		}


		public void NotifiyJobcomplete(Guid jobId)
		{
			//TODO
		}
	}
}
