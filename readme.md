# Dject

**Dject** is a learning project implementing an IoC (Inversion of Control)
library to understand the workings of dependency injection.

It makes heavy use of `System.Reflection` to analyze type metadata to
automatically determine the best way to instantiate services.

## Status

Dject is under active development and not production-ready.

Core service registration and constructor injection works. Service
lifetimes, open generic resolution, wrapper types (Lazy, collections, etc.),
property/function injection, and instancing semantics are not yet implemented.

## License

This project is licensed under the OSL-3.0 terms found in the `license.md` file.