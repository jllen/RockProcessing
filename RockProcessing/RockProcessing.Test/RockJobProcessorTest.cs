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
		public void RockJobPostProcessWeightCorrectAfterProcessing() {
			var processor = new RockJobProcessor();
			var rockJob = new RockJob(RockType.Granit, 3, this);
			processor.Process(rockJob);
			double expectedMinWeight = rockJob.PreProcessWeight - ((rockJob.PreProcessWeight / 100) * 7);
			double expectedMaxWeight = rockJob.PreProcessWeight - ((rockJob.PreProcessWeight / 100) * 5);
			Assert.GreaterOrEqual(rockJob.PostProcessWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.PostProcessWeight, expectedMaxWeight);
		}

		public void NotifiyJobcomplete(Guid jobId)
		{
			//TODO
		}
	}
}
