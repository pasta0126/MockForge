# MockForge

A lightweight .NET 8 library to generate fake data with zero configuration.

## Features

- Answers: Magic 8 Ball, Yes/No, True/False.
- Identity: fake persons (names, gender, birthday, company, department, city).
- Deterministic colors from text (HEX, RGB, RGBA).
- Dates and times: past/future, random, and by age range.
- Numbers: random integers and decimals.
- Cards: poker, Spanish deck, UNO, tarot.
- Optional seed for reproducible results.
- Works with or without DI.

## Install

- NuGet: `MockForge` (or add a project reference).

## Quick start

```csharp
using System;
using MockForge;
using MockForge.Providers;

var forge = MockForgeFactory.Create();

var answer = forge.Get<AnswerProvider>().Magic8Ball();
Console.WriteLine($"Magic 8 Ball: {answer}");
```
