using System;

namespace RockProcessing.Model
{
	//TODO - confirm naming
	public interface IRockJobProvider
	{
		RockJob GetRockJobById(Guid jobId);
	}
}