using System;

namespace Nonatomic.MessageService
{	
	/// <summary>
	/// Defines the interface for a message service that allows subscribing to,
	/// unsubscribing from, and publishing messages.
	/// </summary>
	public interface IMessageService
	{
		/// <summary>
		/// Subscribes to messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to subscribe to. Can be a class or struct.</typeparam>
		/// <param name="handler">The handler that will be called when a message is published.</param>
		void Subscribe<T>(Action<T> handler);
	
		/// <summary>
		/// Unsubscribes from messages of a specific type.
		/// </summary>
		/// <typeparam name="T">The type of message to unsubscribe from. Can be a class or struct.</typeparam>
		/// <param name="handler">The handler to unsubscribe.</param>
		void Unsubscribe<T>(Action<T> handler);
	
		/// <summary>
		/// Publishes a message to all subscribers of that message type.
		/// </summary>
		/// <typeparam name="T">The type of message to publish. Can be a class or struct.</typeparam>
		/// <param name="message">The message to publish.</param>
		void Publish<T>(T message);
	}
}