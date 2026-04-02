# Agent Context: Dject (Dependency Injection Container)

## Project Overview
**Dject** (formerly known as **AInjection**) is a lightweight, fluent Dependency Injection (DI) container for C# .NET. The project is currently undergoing a significant architectural rework to improve its design, performance, and feature set.

## Core Mandates & Rules
- **Naming Transition:** The project is migrating from the name "AInjection" to "Dject". All new code must use the `Dject` namespace. Legacy references to `AInjection` should be updated to `Dject` whenever encountered in the scope of work.
- **Modern C#:** Leverage modern C# features (records, primary constructors, expression-bodied members) where appropriate.
- **Test-Driven:** Maintain and expand the xUnit test suite in `test/AInjection.XUnitTests`. Every new feature or bug fix must be accompanied by tests.
- **Performance:** Keep the core resolution path efficient. Future goals include moving away from pure reflection towards expression-tree-based factories.

## Project Layout
- `src/AInjection.Library/`: Core logic for the DI container.
  - `ServiceFactory.cs`: The central container class (implements `IServiceProvider`).
  - `ServiceFactoryBuilder.cs`: The fluent registration entry point.
  - `InstanceLifetime.cs`: Base class for lifetime management (Singleton, Transient, etc.).
- `test/AInjection.XUnitTests/`: Unit tests for registration and resolution.
- `test/AInjection.Benchmarks/`: Performance testing harness.
- `plans/`: Architectural notes and critiques (Note: Some may refer to deprecated versions).

## Current Architectural State
The project is mid-rework. You may encounter:
- **Missing Types:** References to `Component`, `ComponentRegistrationBuilder`, or `InstanceProvider` in the code may refer to old designs being phased out.
- **Incomplete Logic:** `ServiceFactory` and `InstanceLifetime` currently contain placeholder logic or invalid code paths that need refinement.
- **Namespace Mismatch:** You will see a mix of `AInjection` and `Dject`. Prioritize `Dject`.

## Technical Stack
- **Target SDK:** .NET 10 (Preview) with fallback to .NET 8 (LTS).
- **Test Framework:** xUnit.
- **Benchmark Tool:** BenchmarkDotNet.
