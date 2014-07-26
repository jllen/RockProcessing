using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockProcessing.Model {
	public class RockJob {
		private readonly RockType _rockType;
		private readonly double _originWeight;
		private readonly IRockJobMonitor _monitor;
		private readonly Guid _jobId;
		private bool _complete;
		private double _currentWeight;
		private double _processTime;

		public RockJob(RockType rockType, double weight, IRockJobMonitor monitor)
		{
			_rockType = rockType;
			_originWeight = weight;
			_currentWeight = _originWeight;
			_monitor = monitor;
			_jobId = Guid.NewGuid();
		}

		public RockType RockType
		{
			get { return _rockType; }
		}

		public double OriginWeight
		{
			get { return _originWeight; }
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

		public double CurrentWeight
		{
			get { return _currentWeight; } 
			set { _currentWeight = value; }
		}

		public double ProcessTime
		{
			get { return _processTime; }
			set { _processTime = value; }
		}
	}
}
