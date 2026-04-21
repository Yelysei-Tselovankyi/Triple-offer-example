# Triple Offer Example

**Triple Offer Example** is a demonstration project showcasing a fully functional and easily extensible system for handling offers and events, built around a triple-offer scenario.

## Features

- **Production-like architecture**
  - Code is organized into separate systems and assemblies
  - Designed with scalability and maintainability in mind

- **Loose coupling**
  - Most components are decoupled
  - Communication in change-prone areas is handled via interfaces

- **Remote configuration**
  - Offer configs and triple-offer settings are loaded from Firebase
  - No code changes required to update offers

- **Dynamic content loading**
  - Uses Unity Addressables
  - Prefabs are loaded and instantiated at runtime

## Architecture Overview

The project follows a modular approach:
- Systems are isolated and interact through well-defined interfaces
- Dependencies are minimized to improve flexibility
- Easily extendable for new offer types or event logic