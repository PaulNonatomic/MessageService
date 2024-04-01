using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Nonatomic.MessageService
{
	/// <summary>
	/// A service that allows for decoupled communication via message passing.
	/// Can handle both struct (value type) and class (reference type) messages.
	/// </summary>>
	public class MessageService : IMessageService
	{
		/// <summary>
		/// Stores all subscribers, keyed by message type.
		/// </summary>
		private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new ();

		/// <summary>
		/// Subscribe a handler for a specific message type.
		/// </summary>
		public void Subscribe<T>(Action<T> handler)
		{
			var type = typeof(T);
			
			if (!_subscribers.ContainsKey(type))
			{
				_subscribers[type] = new List<Delegate>();
			}

			_subscribers[type].Add(handler);
		}

		/// <summary>
		/// Unsubscribe a handler from a specific message type.
		/// </summary>
		public void Unsubscribe<T>(Action<T> handler)
		{
			var type = typeof(T);
			if (!_subscribers.ContainsKey(type)) return;
			
			_subscribers[type].Remove(handler);
		}
		
		/// <summary>
		/// Unsubscribes from all messages.
		/// </summary>
		public void UnsubscribeAll()
		{
			_subscribers.Clear();
		}

		/// <summary>
		/// Publish a message of a specific type to all its subscribers.
		/// </summary>
		public void Publish<T>(T message)
		{
			var type = typeof(T);
			if (!_subscribers.ContainsKey(type)) return;
			
			foreach (var subscriber in _subscribers[type])
			{
				try
				{
					(subscriber as Action<T>)?.Invoke(message);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
		}
	}
}