using System;

namespace RockProcessing.Model
{
	public interface IRockProcessMonitor
	{
		void NotifiyJobcomplete(Guid jobId);
	}
}