using System;

namespace RockProcessing.Model
{
	public class Container
	{
		private readonly Guid _containerId;
		private double _currentCapacity;

		public Container(Guid containerId)
		{
			_containerId = containerId;
		}

		public Guid ContainerId
		{
			get { return _containerId; }
		}

		public double CurrentCapacity
		{
			get { return _currentCapacity; }
		}

		//TODO query this notion - adding a job to a container sounds wrong
		public void Add(RockJob job)
		{
			//TODO
		}
	}
}