using System;

namespace RockProcessing.Model
{
	public interface IRockJobMonitor
	{
		void NotifiyJobcomplete(Guid jobId);
	}
}