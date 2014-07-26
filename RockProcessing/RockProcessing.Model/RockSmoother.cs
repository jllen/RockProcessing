using System;

namespace RockProcessing.Model
{
	public class RockSmoother : RockProcessor
	{
		public static double ProcessTimePerKilo = 500;
		public static double MinProcessDegredation = 5.0;
		public static double MaxProcessDegredation = 7.0;
		 
		public override void ProcessRock(RockJob rockJob)
		{
			double variableChange = RandomDouble(MinProcessDegredation, MaxProcessDegredation);
			var preProcessWeight = rockJob.CurrentWeight;
			rockJob.CurrentWeight = Math.Round(rockJob.CurrentWeight - ((rockJob.CurrentWeight / 100) * variableChange), 2);
			rockJob.ProcessTime += preProcessWeight * ProcessTimePerKilo;
			Console.WriteLine("RockSmoother process time {0}", rockJob.ProcessTime);
		}
	}
}