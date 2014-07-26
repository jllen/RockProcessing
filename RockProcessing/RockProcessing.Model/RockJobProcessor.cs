using System;

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

	public class RockSmoother : IRockProcessor
	{
		public void ProcessRock(RockJob rockJob)
		{
			double variableChange = RandomDouble(5.0, 7.0);
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
		}

		public double RandomDouble(double minimum, double maximum) {
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}
	}

	public class RockCrusher : IRockProcessor
	{
		public void ProcessRock(RockJob rockJob) {
			double variableChange = RandomDouble(20.0, 30.0);
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
		}

		public double RandomDouble(double minimum, double maximum) {
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}
	}
}