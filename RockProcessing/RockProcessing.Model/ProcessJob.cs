using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockProcessing.Model {
	public class ProcessJob {
		private readonly RockType _rockType;
		private readonly int _weight;
		private Guid _jobId;

		public ProcessJob(RockType rockType, int weight)
		{
			_rockType = rockType;
			_weight = weight;
			_jobId = Guid.NewGuid();
		}

		public RockType RockType
		{
			get { return _rockType; }
		}

		public int Weight
		{
			get { return _weight; }
		}

		public Guid JobId
		{
			get { return _jobId; }
		}
	}
}
