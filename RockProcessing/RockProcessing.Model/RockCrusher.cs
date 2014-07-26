using System;

namespace RockProcessing.Model
{
	public class RockCrusher : RockProcessor
	{
		public override void ProcessRock(RockJob rockJob) {
			double variableChange = RandomDouble(20.0, 30.0);
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
		}
	}
}