using System;

namespace RockProcessing.Model
{
	public class Container<T>
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

		public void Add(T item)
		{
		}
	}
}