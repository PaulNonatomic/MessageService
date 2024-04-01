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
		/// Subscribes the specified handler to the specified message type (T).
		/// After the message is received for the first time, the handler is automatically unsubscribed.
		/// The subscription action involves creating a wrapper for the original handler.
		/// This wrapper calls the original handler and then unsubscribes itself from the message service.
		/// </summary>
		/// <param name="handler">The handler that will deal with the message of type T.</param>
		/// <typeparam name="T">The type of the message to subscribe to.</typeparam>
		public void SubscribeOnce<T>(Action<T> handler)
		{
			Action<T> wrappedHandler = null;
			wrappedHandler = (message) =>
			{
				handler(message);
				Unsubscribe(wrappedHandler);
			};

			Subscribe(wrappedHandler);
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