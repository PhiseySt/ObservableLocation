using System;
using System.Collections.Generic;

namespace ObservableLocation
{
	public class LocationTracker : IObservable<Location>
	{
		private readonly List<IObserver<Location>> _observers;
		public LocationTracker()
		{
			_observers = new List<IObserver<Location>>();
		}

	      public IDisposable Subscribe(IObserver<Location> observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);
			return new Unsubscriber(_observers, observer);
		}

		private class Unsubscriber : IDisposable
		{
			private List<IObserver<Location>> _observers;
			private IObserver<Location> _observer;

			public Unsubscriber(List<IObserver<Location>> observers, IObserver<Location> observer)
			{
				_observers = observers;
				_observer = observer;
			}

			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}

		public void TrackLocation(Location? loc)
		{
			foreach (var observer in _observers)
			{
				if (!loc.HasValue)
					observer.OnError(new LocationUnknownException());
				else
					observer.OnNext(loc.Value);
			}
		}

		public void EndTransmission()
		{
			foreach (var observer in _observers.ToArray())
				if (_observers.Contains(observer))
					observer.OnCompleted();

			_observers.Clear();
		}
	}

	public class LocationUnknownException : Exception
	{
		internal LocationUnknownException()
		{ }
	}
}
