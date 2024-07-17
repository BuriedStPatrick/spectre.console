# Spectre.Console (fork)

This is a fork of `Spectre.Console` that fixes a bunch of architectural problems i have with the existing code base, mainly in `Spectre.Console.Cli`.

## Motivation

I love `Spectre.Console.Cli`'s functionality, but its implementation leaves a lot to be desired. For instance, you can't:

- Use `records` or `structs` as `CommandSettings`.
- Set up the app with a DI container without fiddling around with custom resolvers.
- Fluently build your `CommandApp` (at least to the degree I prefer).
- Meaninfully extend the existing functionality in the configurator. Like wiring it up with other command-pattern libraries.

Since I'd rather not re-invent the wheel with all the great features implemented in `Spectre.Console`, this is instead me taking an existing wheel and re-imagining it in my own image.