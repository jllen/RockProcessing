namespace RockProcessing.Model
{
	public class RockJobProcessor
	{
		private IRockProcessor _crusher;
		private IRockProcessor _smoother;

		public RockJobProcessor()
		{
			_crusher = new RockCrusher();
			_smoother = new RockSmoother();
		}

		public void Process(RockJob job)
		{
			if(job.RockType.Equals(RockType.Granit))
			{
				_smoother.ProcessRock(job);
				job.Complete = true;
			}
			else if(job.RockType.Equals(RockType.Pegmatite))
			{
				_crusher.ProcessRock(job);
				job.Complete = true;
			}
			else if(job.RockType.Equals(RockType.Gneiss)) {
				_crusher.ProcessRock(job);
				_smoother.ProcessRock(job);
				job.Complete = true;
			}
		}
	}
}