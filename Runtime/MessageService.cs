using System;
using System.Collections.Generic;

namespace Nonatomic.MessageService
{
	/// <summary>
	/// A service for managing and dispatching messages.
	/// </summary>
	public class MessageService : IMessageService
	{
		private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

		/// <summary>
		/// Subscribes to messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to subscribe to.</typeparam>
		/// <param name="handler">The handler that will be called when a message is published.</param>
		public void Subscribe<T>(Action<T> handler) where T : struct
		{
			var type = typeof(T);
			if (!_subscribers.ContainsKey(type))
			{
				_subscribers[type] = new List<Delegate>();
			}

			_subscribers[type].Add(handler);
		}

		/// <summary>
		/// Unsubscribes from messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
		/// <param name="handler">The handler to unsubscribe.</param>
		public void Unsubscribe<T>(Action<T> handler) where T : struct
		{
			var type = typeof(T);
			if (!_subscribers.ContainsKey(type)) return;
			
			_subscribers[type].Remove(handler);
		}

		/// <summary>
		/// Publishes a message to all subscribers of that message type.
		/// </summary>
		/// <typeparam name="T">The type of message to publish.</typeparam>
		/// <param name="message">The message to publish.</param>
		public void Publish<T>(T message) where T : struct
		{
			var type = typeof(T);
			if (!_subscribers.ContainsKey(type)) return;
			
			foreach (var subscriber in _subscribers[type])
			{
				(subscriber as Action<T>)?.Invoke(message);
			}
		}
	}
}