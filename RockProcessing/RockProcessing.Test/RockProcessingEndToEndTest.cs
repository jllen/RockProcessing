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
    public class RockProcessingEndToEndTest : IRockProcessMonitor{
		[Test]
		public void RockFactoryReceivesAndProcessesSingleRockItemReturningProcessInformationOnCompletion(){
			var rockFactory = new RockFactory();
			Guid jobId;
			rockFactory.ProcessRock(RockType.Granit, 3, out jobId);
			WaitForJobCompletion();
			var job = rockFactory.GetProcessJob(jobId);
			Assert.IsNotNull(job, "Process job returned found to be invalid");
		}

		private void WaitForJobCompletion(int timeout = 1000)
		{
			if(!_jobCompleteWaitEvent.WaitOne(timeout))
			{
				throw new Exception("Did not return to test");
			}
		}

		private readonly ManualResetEvent _jobCompleteWaitEvent = new ManualResetEvent(false);
		
		public void NotifiyJobcomplete(Guid jobId)
		{
			_jobCompleteWaitEvent.Set();
		}
    }
}
