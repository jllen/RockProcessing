using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RockProcessing.Model {
	public class RockFactory : IRockJobMonitor
	{
		private readonly ProcessingLine _processingLine;
		private Dictionary<Guid, RockJob> _jobCatalogue = new Dictionary<Guid, RockJob>();
		private BlockingCollection<RockJob> _processJobs = new BlockingCollection<RockJob>();
		private List<IRockJobMonitor> _monitors = new List<IRockJobMonitor>();

		public RockFactory() {
			//todo introduce multiple lines - probably create list of lines fed off _processJobs
			_processingLine = new ProcessingLine(_processJobs, this, true);
		}

		public Guid ProcessRock(RockType rockType, double weight) {
			var rockJob = new RockJob(rockType, weight, this);
			_jobCatalogue.Add(rockJob.JobId, rockJob);
			_processJobs.Add(rockJob);
			return rockJob.JobId;
		}

		public RockJob GetProcessJob(Guid jobId) {
			return _jobCatalogue[jobId];
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
