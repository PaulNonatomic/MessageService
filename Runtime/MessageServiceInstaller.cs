#if SERVICE_LOCATOR

using UnityEngine;

namespace Nonatomic.MessageService
{
	public class MessageServiceInstaller : MonoBehaviour
	{
		[SerializeField] private ServiceLocator.ServiceLocator _serviceLocator;

		private IMessageService _messageService;

		private void Awake()
		{
			_messageService = new MessageService();
			_serviceLocator.Register<IMessageService>(new MessageService());
		}
		
		private void OnDestroy()
		{
			_messageService.UnsubscribeAll();
			_serviceLocator.Unregister<IMessageService>();
			_messageService = null;
		}
	}
}

#endif
