using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RockProcessing.Model;

namespace RockProcessing.Test{
	[TestFixture]
    public class RockProcessingEndToEndTest : TestBase, IRockJobMonitor{
		[Test]
		public void RockFactoryReceivesAndProcessesSingleRockItemIssuingNotificationOnCompletion(){
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			const double weight = 3;

			Console.WriteLine("Sending {0} item of weight {1} for processing", RockType.Granit, weight);
			Guid jobId = rockFactory.ProcessRock(RockType.Granit, weight);

			Console.WriteLine("Waiting for notifications.");
			WaitForNotfication();
			Console.WriteLine("Notifications received.");

			Console.WriteLine("Test complete");
		}

		[Test]
		public void ProcessInformationAvailableAfterNotificationCompletionReceived() {
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			const double weight = 5.6;

			Console.WriteLine("Sending {0} item of weight {1} for processing", RockType.Granit, weight);
			Guid jobId = rockFactory.ProcessRock(RockType.Granit, weight);

			Console.WriteLine("Waiting for notifications.");
			WaitForNotfication();
			Console.WriteLine("Notifications received.");
			
			var rockjob = rockFactory.GetProcessJob(jobId);
			
			Assert.IsNotNull(rockjob, "Failed to retrieve Job object from factory");
			Assert.AreEqual(jobId, rockjob.JobId);
			Console.WriteLine("Test complete");
		}

		[Test]
		public void FinishedWeightOfCompletedJobIsCorrectWhenRockRequiringSmoothingSentForProcessing() {
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			const double weight = 5.6;
			double expectedMinWeight = weight - SmoothProcessMaxWeightLoss(weight);
			double expectedMaxWeight = weight - SmoothProcessMinWeightLoss(weight);

			Console.WriteLine("Sending {0} item of weight {1} for processing", RockType.Granit, weight);
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			Guid jobId = rockFactory.ProcessRock(RockType.Granit, weight);

			Console.WriteLine("Waiting for notifications.");
			WaitForNotfication();
			Console.WriteLine("Notifications received.");

			var rockJob = rockFactory.GetProcessJob(jobId);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void FinishedWeightOfCompletedJobIsCorrectWhenRockRequiringCrushingSentForProcessing() {
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			const double weight = 2.8;
			const RockType rockType = RockType.Pegmatite;
			double expectedMinWeight = weight - CrushProcessMaxWeightLoss(weight);
			double expectedMaxWeight = weight - CrushProcessMinWeightLoss(weight);

			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, weight);
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			Guid jobId = rockFactory.ProcessRock(rockType, weight);

			Console.WriteLine("Waiting for notifications.");
			WaitForNotfication();
			Console.WriteLine("Notifications received.");

			var rockJob = rockFactory.GetProcessJob(jobId);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight);
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		[Test]
		public void FinishedWeightOfCompletedJobIsCorrectWhenRockRequiringSmoothingAndCrushingSentForProcessing() {
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			const double weight = 6.8;
			const RockType rockType = RockType.Gneiss;
			double smoothProcessMaxWeightLoss = SmoothProcessMaxWeightLoss(weight);
			double smoothProcessMinWeightLoss = SmoothProcessMinWeightLoss(weight);
			double crushProcessMaxWeightLoss = CrushProcessMaxWeightLoss(weight);
			double crushProcessMinWeightLoss = CrushProcessMinWeightLoss(weight);
			double expectedMinWeight = weight - (smoothProcessMaxWeightLoss + crushProcessMaxWeightLoss);
			double expectedMaxWeight = weight - (smoothProcessMinWeightLoss + crushProcessMinWeightLoss);

			Console.WriteLine("Sending {0} item of weight {1} for processing", rockType, weight);
			Console.WriteLine("Expected Minimum Weight after processing {0}", expectedMinWeight);
			Console.WriteLine("Expected Maximum Weight after processing {0}", expectedMaxWeight);

			Guid jobId = rockFactory.ProcessRock(rockType, weight);

			Console.WriteLine("Waiting for notifications.");
			WaitForNotfication();
			Console.WriteLine("Notifications received.");

			var rockJob = rockFactory.GetProcessJob(jobId);

			Assert.GreaterOrEqual(rockJob.CurrentWeight, expectedMinWeight, "Post process weight not within the expected bounds");
			Assert.LessOrEqual(rockJob.CurrentWeight, expectedMaxWeight, "Post process weight not within the expected bounds");
			Console.WriteLine("Actual Post Process Weight {0}", rockJob.CurrentWeight);
			Console.WriteLine("Test Complete");
		}

		private void WaitForNotfication(int timeout = 1000)
		{
			if(!_jobCompleteWaitEvent.WaitOne(timeout))
			{
				throw new Exception("Did not return to test");
			}
		}

		#region IRockJobMonitor members

		void IRockJobMonitor.NotifiyJobcomplete(Guid jobId)
		{
			_jobCompleteWaitEvent.Set();
		}

		#endregion

		#region NUnit stuff

		[SetUp]
		public void Init()
		{
			_jobCompleteWaitEvent.Reset();
		}

		#endregion

		#region Private fields

		private readonly ManualResetEvent _jobCompleteWaitEvent = new ManualResetEvent(false);
		
		#endregion
	}
}
