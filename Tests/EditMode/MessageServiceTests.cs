using System;
using Nonatomic.MessageService;
using NUnit.Framework;

namespace Tests.EditMode
{
	public class MessageServiceTests
	{
		private class TestMessage
		{
			public string Content { get; set; }
		}
		
		private IMessageService _messageService;
		private TestMessage _lastReceivedMessage;

		[SetUp]
		public void Setup()
		{
			_lastReceivedMessage = null;
			_messageService = new MessageService();
		}

		[Test]
		public void Test_SingleSubscriberReceivesMessage()
		{
			_messageService.Subscribe<TestMessage>(HandleTestMessage);

			var expectedMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Publish(expectedMessage);

			Assert.That(_lastReceivedMessage, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Test_MultipleSubscribersReceiveMessage()
		{
			_messageService.Subscribe<TestMessage>(HandleTestMessage);
			var additionalMessageReceived = false;
			_messageService.Subscribe<TestMessage>(_ => additionalMessageReceived = true);

			var expectedMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Publish(expectedMessage);

			Assert.That(_lastReceivedMessage, Is.EqualTo(expectedMessage));
			Assert.That(additionalMessageReceived, Is.True);
		}

		[Test]
		public void Test_UnsubscribedSubscriberDoesNotReceiveMessage()
		{
			Action<TestMessage> handler = HandleTestMessage;
			_messageService.Subscribe(handler);

			var expectedMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Unsubscribe(handler);
			_messageService.Publish(expectedMessage);

			Assert.That(_lastReceivedMessage, Is.Null);
		}
		
		[Test]
		public void Test_PublishWithoutAnySubscribers()
		{
			var testMessage = new TestMessage { Content = "Hello, world!" };
			Assert.DoesNotThrow(() => _messageService.Publish(testMessage));
		}
		
		[Test]
		public void Test_MultipleSubscribersAndRemoves()
		{
			var secondReceivedMessage = false;
			Action<TestMessage> firstHandler = HandleTestMessage;
			Action<TestMessage> secondHandler = (m) => secondReceivedMessage = true;

			_messageService.Subscribe(firstHandler);
			_messageService.Subscribe(secondHandler);

			var testMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Publish(testMessage);

			Assert.That(_lastReceivedMessage, Is.EqualTo(testMessage));
			Assert.That(secondReceivedMessage, Is.True);

			_messageService.Unsubscribe(firstHandler);

			testMessage = new TestMessage { Content = "Goodbye, world!" };
			secondReceivedMessage = false;
			_messageService.Publish(testMessage);

			Assert.That(_lastReceivedMessage, Is.Not.EqualTo(testMessage)); // firstHandler should not be called
			Assert.That(secondReceivedMessage, Is.True); // secondHandler should have been called
		}
		
		[Test]
		public void Test_SameSubscriberMultipleTimes()
		{
			Action<TestMessage> handler = HandleTestMessage;
			_messageService.Subscribe(handler);
			_messageService.Subscribe(handler);

			var testMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Publish(testMessage);

			Assert.That(_lastReceivedMessage, Is.EqualTo(testMessage));
		}
		
		[Test]
		public void Test_RemoveNonExistingSubscriber()
		{
			Action<TestMessage> handler = HandleTestMessage;
			Assert.DoesNotThrow(() => _messageService.Unsubscribe(handler));
		}
		
		[Test]
		public void Test_SubscribeUnsubscribeAndSubscribeAgain()
		{
			Action<TestMessage> handler = HandleTestMessage;

			_messageService.Subscribe(handler);

			var testMessage = new TestMessage { Content = "Hello, world!" };
			_messageService.Publish(testMessage);
			Assert.That(_lastReceivedMessage, Is.EqualTo(testMessage));

			_messageService.Unsubscribe(handler);

			_lastReceivedMessage = null;
			_messageService.Publish(testMessage);
			Assert.That(_lastReceivedMessage, Is.Null);

			_messageService.Subscribe(handler);

			_messageService.Publish(testMessage);
			Assert.That(_lastReceivedMessage, Is.EqualTo(testMessage));
		}

		[Test]
		public void Test_PublishDoesNotCauseCollectionModificationException()
		{
			var selfRemovingHandlerCalled = false;
			Action<TestMessage> selfRemovingHandler = null;
			
			selfRemovingHandler = (message) =>
			{
				selfRemovingHandlerCalled = true;
				_messageService.Unsubscribe<TestMessage>(selfRemovingHandler);
			};

			_messageService.Subscribe<TestMessage>(selfRemovingHandler);
			var testMessage = new TestMessage { Content = "Hello, test!" };

			Assert.DoesNotThrow(() => _messageService.Publish(testMessage));
			Assert.That(selfRemovingHandlerCalled, Is.True);
		}

		private void HandleTestMessage(TestMessage message)
		{
			_lastReceivedMessage = message;
		}
	}
}
