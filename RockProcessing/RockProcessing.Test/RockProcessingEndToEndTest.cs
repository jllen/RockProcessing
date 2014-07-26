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
    public class RockProcessingEndToEndTest : IRockJobMonitor{
		[Test]
		public void RockFactoryReceivesAndProcessesSingleRockItemIssuingNotificationOnCompletion(){
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);
			
			Guid jobId = rockFactory.ProcessRock(RockType.Granit, 3);
			
			WaitForJobCompletion();
			Console.WriteLine("Test complete");
		}

		[Test]
		public void ProcessInformationAvailableAfterNotificationCompletionReceived() {
			var rockFactory = new RockFactory();
			rockFactory.RegisterMonitor(this);

			Guid jobId = rockFactory.ProcessRock(RockType.Granit, 3);

			WaitForJobCompletion();
			Console.WriteLine("Notifications received.");
			var rockjob = rockFactory.GetProcessJob(jobId);
			Assert.IsNotNull(rockjob, "Failed to retrieve Job object from factory");
			Console.WriteLine("Test complete");
		}

		private void WaitForJobCompletion(int timeout = 1000)
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
