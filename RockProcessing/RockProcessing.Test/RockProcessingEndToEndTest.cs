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
