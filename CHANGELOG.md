# Change Log

## [1.1.2] - Jun 12, 2025
- Hotfix to expose the UnsubscribeAll method in the IMessagerService interface.

## [1.1.1] - Jun 12, 2025
- Added uninstall method for MessageService installer

## [1.1.0] - Feb 06, 2025
- Added an optional publisher parameter to the Publish method for debugging purposes and potential future tooling.
- Found a bug where unsubscribing from a message within a subscription handler would modify the underlying collection and cause an exception  while iterating over it.
  - I've added a test to reproduce the issue and fixed the bug.
  - The solution is to use a multicast delegate to store subscribers which removes the need to iterate over the collection when publishing messages.
  - This is how C# events works under the hood and is a more robust solution.

## [1.0.0] - Apr 01, 2024
- Added unit tests
- Added documentation
- Added support for struct and class based messages
- Added a SubscribeOnce method

## [0.0.0-beta] - Feb 11, 2024
- First commit