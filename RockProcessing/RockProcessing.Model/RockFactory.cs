using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RockProcessing.Model {
	public class RockFactory : IRockJobMonitor
	{
		private readonly ProcessingLine _processingLine;
		private BlockingCollection<RockJob> _processJobs = new BlockingCollection<RockJob>();
		private BlockingCollection<RockJob> _completedJobs = new BlockingCollection<RockJob>();
		private List<IRockJobMonitor> _monitors = new List<IRockJobMonitor>();

		public RockFactory() {
			_processingLine = new ProcessingLine(_processJobs, this, true);
		}

		public void ProcessRock(RockType rockType, int weight, out Guid jobId) {
			var rockJob = new RockJob(rockType, weight, this);
			jobId = rockJob.JobId;
			_processJobs.Add(rockJob);
		}

		public RockJob GetProcessJob(Guid jobId) {
			return null;
		}

		public void NotifiyJobcomplete(Guid jobId)
		{
			foreach(var monitor in _monitors)
			{
				monitor.NotifiyJobcomplete(jobId);
			}
		}

		public void RegisterMonitor(IRockJobMonitor rockProcessMonitor)
		{
			_monitors.Add(rockProcessMonitor);
		}
	}
}
