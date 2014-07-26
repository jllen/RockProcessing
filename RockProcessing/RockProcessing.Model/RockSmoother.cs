using System;

namespace RockProcessing.Model
{
	public class RockSmoother : RockProcessor
	{
		public static double ProcessTimePerKilo = 500;
		 
		public override void ProcessRock(RockJob rockJob)
		{
			double variableChange = RandomDouble(5.0, 7.0);
			var preProcessWeight = rockJob.CurrentWeight;
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
			rockJob.ProcessTime = preProcessWeight * ProcessTimePerKilo;
		}
	}
}