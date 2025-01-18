# MessageService

## Overview
MessageService is a simple message passing system for decoupling components in a Unity application. It provides a mechanism for subscribing to messages of a specific type and publishing messages to all interested subscribers.

## Installation
To install MessageService in your Unity project, add the package from the git URL: `https://github.com/PaulNonatomic/MessageService.git` using the Unity package manager.

## Features
- **Subscribe to Messages**: Listen for specific message types.
- **Unsubscribe from Messages**: Stop listening for specific message types.
- **Publish Messages**: Send messages to all subscribers of that message type.
- **Automatically unsubscribe**: After receiving a message once with SubscribeOnce feature

## Usage
### Creating Messages
Messages can be any type, struct or class, depending on your needs.  
**Structs** are often preferred because they are value types, can be more efficient in some scenarios, and have well-defined copy semantics.  
**Classes** might be a better choice if your message needs reference semantics, inheritance, or more complex structures.

```csharp
public struct MyMessage
{
    public string Content;
}
```

### Subscribing to a Message
To subscribe to a message type, use the `Subscribe<T>` method where `T` is your message type:

```csharp
_messageService.Subscribe<MyMessage>(HandleMyMessage);

private void HandleMyMessage(MyMessage message)
{
    // Handle the message
}
```

### Unsubscribing from a Message
To unsubscribe, use the Unsubscribe<T> method:

```csharp
_messageService.Unsubscribe<MyMessage>(HandleMyMessage);
```

### Publishing a Message
To publish a message, use the Publish<T> method:

```csharp
_messageService.Publish(new MyMessage { Content = "Hello, world!" });
```
### Subscribe Once
Messages can be subscribed to be received only once using SubscribeOnce. After the message is received for the first time, the handler is automatically unsubscribed.

```csharp
_messageService.SubscribeOnce<MyMessage>(HandleMyMessage);
```
## Contributing
Contributions to MessageService are welcome! Please refer to CONTRIBUTING.md for guidelines on contributing to the project.

## License
MessageService is licensed under the MIT license. See LICENSE for more details.
