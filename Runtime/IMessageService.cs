using System;
using System.Collections.Generic;

namespace Nonatomic.MessageService
{	
	/// <summary>
	/// Defines the interface for a message service that allows subscribing to,
	/// unsubscribing from, and publishing messages.
	/// </summary>
	public interface IMessageService
	{
		/// <summary>
		/// Retrieves the message types that currently have active subscribers.
		/// </summary>
		/// <returns>A collection of message types that are actively subscribed to.</returns>
		IEnumerable<Type> GetActiveMessageTypes();

		/// <summary>
		/// Subscribes to messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to subscribe to. Can be a class or struct.</typeparam>
		/// <param name="handler">The handler that will be called when a message is published.</param>
		void Subscribe<T>(Action<T> handler);
		
		/// <summary>
		/// Subscribes the specified handler to the specified message type (T).
		/// After the message is received for the first time, the handler is automatically unsubscribed.
		/// </summary>
		/// <param name="handler">The handler that will deal with the message of type T.</param>
		/// <typeparam name="T">The type of the message to subscribe to.</typeparam>
		void SubscribeOnce<T>(Action<T> handler);
	
		/// <summary>
		/// Unsubscribes from messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to unsubscribe from. Can be a class or struct.</typeparam>
		/// <param name="handler">The handler to unsubscribe.</param>
		void Unsubscribe<T>(Action<T> handler);

        void UnsubscribeAll();
	
		/// <summary>
		/// Publishes a message to all subscribers of that message type.
		/// Optionally provide a publisher object for debugging/tracking.
		/// </summary>
		/// <typeparam name="T">The type of message to publish. Can be a class or struct.</typeparam>
		/// <param name="message">The message to publish.</param>
		/// <param name="publisher">Optional. The source/publisher of this message.</param>
		void Publish<T>(T message, object publisher = null);
	}
}