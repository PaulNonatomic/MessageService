# MessageService

## Overview
MessageService is a Unity package designed to facilitate communication between different parts of a Unity project using a message-based system. It allows for easy subscription to, and publication of, messages, making it ideal for decoupling components in your game or application.

## Installation
To install MessageService in your Unity project, add the package from the git URL: `https://github.com/PaulNonatomic/MessageService.git` using the Unity package manager.

## Features
- **Subscribe to Messages**: Listen for specific message types.
- **Unsubscribe from Messages**: Stop listening for specific message types.
- **Publish Messages**: Send messages to all interested subscribers.

## Usage
### Subscribing to a Message
To subscribe to a message type, use the `Subscribe<T>` method where `T` is your message type:

```csharp
messageService.Subscribe<MyMessage>(OnMyMessageReceived);

void OnMyMessageReceived(MyMessage message)
{
    // Handle the message
}
```

### Unsubscribing from a Message
To unsubscribe, use the Unsubscribe<T> method:

```csharp
messageService.Unsubscribe<MyMessage>(OnMyMessageReceived);
```

### Publishing a Message
To publish a message, use the Publish<T> method:

```csharp
MyMessage message = new MyMessage();
messageService.Publish(message);
```

## Contributing
Contributions to MessageService are welcome! Please refer to CONTRIBUTING.md for guidelines on contributing to the project.

## License
MessageService is licensed under the MIT license. See LICENSE for more details.
