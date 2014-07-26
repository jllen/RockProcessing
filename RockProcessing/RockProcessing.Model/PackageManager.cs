using System;

namespace RockProcessing.Model
{
	public class PackageManager : IRockJobMonitor
	{
		public void NotifiyJobcomplete(Guid jobId)
		{
			//TODO - Hook up to some source of RockJobs via provider interface to allow completed jobs to be retrieved for package processing
		}

		public Guid GetContainerIdForJob(Guid jobId)
		{
			//TODO
			return Guid.Empty;
		}
	}
}