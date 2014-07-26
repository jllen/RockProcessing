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
			
			Guid jobId;
			rockFactory.ProcessRock(RockType.Granit, 3, out jobId);
			
			WaitForJobCompletion();
		}

		private void WaitForJobCompletion(int timeout = 1000)
		{
			if(!_jobCompleteWaitEvent.WaitOne(timeout))
			{
				throw new Exception("Did not return to test");
			}
		}
		
		public void NotifiyJobcomplete(Guid jobId)
		{
			_jobCompleteWaitEvent.Set();
		}

		[SetUp]
		public void Init()
		{
			_jobCompleteWaitEvent.Reset();
		}

		#region Private fields

		private readonly ManualResetEvent _jobCompleteWaitEvent = new ManualResetEvent(false);
		
		#endregion
	}
}
