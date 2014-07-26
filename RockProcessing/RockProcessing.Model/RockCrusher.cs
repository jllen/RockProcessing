using System;

namespace RockProcessing.Model
{
	public class RockCrusher : RockProcessor
	{
		public static double ProcessTimePerKilo = 50;

		public override void ProcessRock(RockJob rockJob) {
			double variableChange = RandomDouble(20.0, 30.0);
			var preProcessWeight = rockJob.CurrentWeight;
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
			rockJob.ProcessTime += preProcessWeight * ProcessTimePerKilo;
			Console.WriteLine("RockCrusher process time {0}", rockJob.ProcessTime);
		}
	}
}