using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nonatomic.MessageService
{
	/// <summary>
	/// A service that allows for decoupled communication via message passing.
	/// Can handle both struct (value type) and class (reference type) messages.
	/// </summary>>
	public class MessageService : IMessageService
	{
		public event Action<Type> SubscriptionAdded;
		public event Action<Type> SubscriptionRemoved;
		public event Action<Type, object> MessagePublished;

		/// <summary>
		/// Stores a multicast delegate per message type
		/// </summary>
		private readonly ConcurrentDictionary<Type, Delegate> _subscribers = new();

		public IEnumerable<Type> GetActiveMessageTypes()
		{
			return _subscribers.Keys.ToArray();
		}

		/// <summary>
		/// Subscribe a handler for a specific message type.
		/// </summary>
		public void Subscribe<T>(Action<T> handler)
		{
			var type = typeof(T);

			_subscribers.AddOrUpdate(
				type,
				handler,
				(key, existing) => Delegate.Combine(existing, handler)
			);

			SubscriptionAdded?.Invoke(type);
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
			if (!_subscribers.TryGetValue(type, out var existing)) return;

			// Remove the handler from the multicast delegate.
			var newDelegate = Delegate.Remove(existing, handler);
			if (newDelegate == null)
			{
				_subscribers.TryRemove(type, out _);
			}
			else
			{
				_subscribers[type] = newDelegate;
			}

			SubscriptionRemoved?.Invoke(type);
		}

		/// <summary>
		/// Unsubscribes from all messages.
		/// </summary>
		public void UnsubscribeAll()
		{
			_subscribers.Clear();
		}

		/// <summary>
		/// Publish a message to all subscribers of type T,
		/// optionally specifying the publisher for debugging/tracking.
		/// </summary>
		public void Publish<T>(T message, object publisher = null)
		{
			var type = typeof(T);
			if (!_subscribers.TryGetValue(type, out var subscribers)) return;

			// Cast the delegate to Action<T>
			if (subscribers is Action<T> action)
			{
				try
				{
					action.Invoke(message);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
					throw;
				}
			}

			MessagePublished?.Invoke(type, publisher);
		}
	}
}