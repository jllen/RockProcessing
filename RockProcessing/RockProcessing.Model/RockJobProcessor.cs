namespace RockProcessing.Model
{
	public class RockJobProcessor
	{
		public RockJobProcessor()
		{
		}

		public void Process(RockJob job)
		{
			job.Complete = true;
		}
	}
}