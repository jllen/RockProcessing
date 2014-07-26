using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockProcessing.Model {
	public class RockJob {
		private readonly RockType _rockType;
		private readonly int _weight;
		private readonly IRockJobMonitor _monitor;
		private readonly Guid _jobId;
		private bool _complete;

		public RockJob(RockType rockType, int weight, IRockJobMonitor monitor)
		{
			_rockType = rockType;
			_weight = weight;
			_monitor = monitor;
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

		public bool Complete
		{
			get { return _complete; }
			set
			{
				_complete = value;
				if(_complete)
				{
					_monitor.NotifiyJobcomplete(_jobId);
				}
			}
		}
	}
}
