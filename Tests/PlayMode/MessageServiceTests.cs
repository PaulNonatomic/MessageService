using System;
using Nonatomic.MessageService;
using NUnit.Framework;

namespace Tests.PlayMode
{
	public class MessageServiceTests
	{
		private IMessageService _messageService;

		[SetUp]
		public void Setup()
		{
			_messageService = new MessageService();
		}

		[Test]
		public void Test_Subscribe_With_Valid_Handler()
		{
			
		}
		
		[Test]
		public void Test_Unsubscribe_With_Valid_Handler()
		{
			
		}

		[Test]
		public void Test_Publish_With_Subscribers()
		{
			
		}

		[Test]
		public void Test_Publish_With_No_Subscribers()
		{
			
		}

		[Test]
		public void Test_Subscribe_Unsubscribe_Multiple_Types()
		{
			
		}
	}

}