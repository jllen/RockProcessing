using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockProcessing.Model {
	public class RockFactory {
		public RockFactory()
		{
		}

		public void ProcessRock(RockType rockType, int weight, out Guid jobId)
		{
			var processJob = new ProcessJob(rockType, weight);
			jobId = processJob.JobId;
		}

		public ProcessJob GetProcessJob(Guid jobId)
		{
			return null;
		}
	}
}
