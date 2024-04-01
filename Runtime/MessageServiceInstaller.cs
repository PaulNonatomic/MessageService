#if SERVICE_LOCATOR

using UnityEngine;

namespace Nonatomic.MessageService
{
	public class MessageServiceInstaller : MonoBehaviour
	{
		[SerializeField] private ServiceLocator.ServiceLocator _serviceLocator;

		private void Awake()
		{
			_serviceLocator.Register<IMessageService>(new MessageService());
			Debug.Log("Regitered MessageService with ServiceLocator.");
		}
	}
}

#endif
