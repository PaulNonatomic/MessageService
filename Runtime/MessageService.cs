using System;
using System.Collections.Generic;
using Nonatomic.ServiceLocator;

namespace Messaging
{
	/// <summary>
	/// Defines the interface for a message service that allows subscribing to,
	/// unsubscribing from, and publishing messages.
	/// </summary>
	public interface IMessageService : IService
	{
		/// <summary>
		/// Subscribes to messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to subscribe to.</typeparam>
		/// <param name="handler">The handler that will be called when a message is published.</param>
		void Subscribe<T>(Action<T> handler) where T : struct;
		
		/// <summary>
		/// Unsubscribes from messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
		/// <param name="handler">The handler to unsubscribe.</param>
		void Unsubscribe<T>(Action<T> handler) where T : struct;
		
		/// <summary>
		/// Publishes a message to all subscribers of that message type.
		/// </summary>
		/// <typeparam name="T">The type of message to publish.</typeparam>
		/// <param name="message">The message to publish.</param>
		void Publish<T>(T message) where T : struct;
	}
	
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
			var messageType = typeof(T);
			if (!_subscribers.ContainsKey(messageType))
			{
				_subscribers[messageType] = new List<Delegate>();
			}

			_subscribers[messageType].Add(handler);
		}

		/// <summary>
		/// Unsubscribes from messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
		/// <param name="handler">The handler to unsubscribe.</param>
		public void Unsubscribe<T>(Action<T> handler) where T : struct
		{
			var messageType = typeof(T);
			if (!_subscribers.ContainsKey(messageType)) return;
			
			_subscribers[messageType].Remove(handler);
		}

		/// <summary>
		/// Publishes a message to all subscribers of that message type.
		/// </summary>
		/// <typeparam name="T">The type of message to publish.</typeparam>
		/// <param name="message">The message to publish.</param>
		public void Publish<T>(T message) where T : struct
		{
			var messageType = typeof(T);
			if (!_subscribers.ContainsKey(messageType)) return;
			
			foreach (var subscriber in _subscribers[messageType])
			{
				(subscriber as Action<T>)?.Invoke(message);
			}
		}
	}
}