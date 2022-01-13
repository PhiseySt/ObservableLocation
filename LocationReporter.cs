using System;

namespace ObservableLocation
{
	public class LocationReporter : IObserver<Location>
	{
		private IDisposable _unsubscriber;
		public string Name { get; }

		public LocationReporter(string name)
		{
			Name = name;
		}

		public virtual void Subscribe(IObservable<Location> provider)
		{
			if (provider != null)
				_unsubscriber = provider.Subscribe(this);
		}

		public virtual void OnCompleted()
		{
			Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", Name);
			Unsubscribe();
		}

		public virtual void OnError(Exception e)
		{
			Console.WriteLine("{0}: The location cannot be determined.", Name);
		}

		public virtual void OnNext(Location value)
		{
			Console.WriteLine("{2}: The current location is {0}, {1}", value.Latitude, value.Longitude, Name);
		}

		public virtual void Unsubscribe()
		{
			_unsubscriber.Dispose();
		}
	}
}
