﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RockProcessing.Model {
	public class RockFactory : IRockJobMonitor, IRockJobProvider
	{
		private readonly ProcessingLine _processingLine;
		private Dictionary<Guid, RockJob> _jobCatalogue = new Dictionary<Guid, RockJob>();//TODO push this out elsewhere
		private BlockingCollection<RockJob> _processJobs = new BlockingCollection<RockJob>();
		private List<IRockJobMonitor> _monitors = new List<IRockJobMonitor>();//TODO push this out elsewhere
		private PackageManager _packageManager;

		public RockFactory() {
			//TODO introduce multiple lines - probably create list of lines fed off _processJobs
			_processingLine = new ProcessingLine(_processJobs, this, true);
			_packageManager = new PackageManager(this);
			_monitors.Add(_packageManager);
		}

		public PackageManager PackageManager
		{
			get { return _packageManager; }
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

		#region IRockJobProvider members

		RockJob IRockJobProvider.GetRockJobById(Guid jobId)
		{
			return (_jobCatalogue.ContainsKey(jobId)) ? _jobCatalogue[jobId] : null;
		}

		#endregion
	}
}
