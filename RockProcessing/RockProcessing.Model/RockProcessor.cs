using System;

namespace RockProcessing.Model
{
	public abstract class RockProcessor : IRockProcessor
	{
		public abstract void ProcessRock(RockJob rockJob);

		protected double RandomDouble(double minimum, double maximum) {
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}
	}
}