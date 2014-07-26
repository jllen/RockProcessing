using System;
using System.Collections.Concurrent;
using System.Threading;

namespace RockProcessing.Model {
	public class ProcessingLine {
		private readonly BlockingCollection<RockJob> _jobs;
		private readonly Thread _processThead;
		private bool _doWork;
		private readonly RockJobProcessor _rockJobProcessor;

		public ProcessingLine(BlockingCollection<RockJob> jobs, IRockJobMonitor monitor, bool start) {
			_jobs = jobs;
			_processThead = new Thread(new ThreadStart(ThreadProc));
			_rockJobProcessor = new RockJobProcessor();
			if(start) {
				Start();
			}
		}

		public void Start() {
			_doWork = true;
			_processThead.Start();
		}

		private void ThreadProc() {
			while(_doWork) {
				RockJob rockJob;
				try {
					// call blocked if nothing to take
					rockJob = _jobs.Take();
				}
				catch(InvalidOperationException) {
					// Take called on a completed collection. 
					continue;
				}
				if(rockJob != null)
				{
					ProcessJob(rockJob);
				}
			}
		}

		private void ProcessJob(RockJob processJob)
		{
			_rockJobProcessor.Process(processJob);
		}
	}
}
