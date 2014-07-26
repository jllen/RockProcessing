using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RockProcessing.Model
{
	public class PackageManager : IRockJobMonitor
	{
		private readonly IRockJobProvider _rockJobProvider;
		private List<Container> _containers = new List<Container>();

		public PackageManager(IRockJobProvider rockJobProvider)
		{
			_rockJobProvider = rockJobProvider;
		}

		public void NotifiyJobcomplete(Guid jobId)
		{
			var job = _rockJobProvider.GetRockJobById(jobId);
			if(job != null)
			{
			}
		}

		public Guid GetContainerIdForJob(Guid jobId)
		{
			//TODO
			return Guid.Empty;
		}
	}
}