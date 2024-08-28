[![MIT License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Slim Script

Slim script is an interpreted scripting language for embedding it into your
projects. The interpreter is written in C# so it can (at least to a certain level)
interact with .Net ecosystem.

## Notes

It's a bit slow because of 
the usage of reflection under the hood but still, its decent enough (i think?)

## Further Reading

See [the docs](docs/docs.md)

## TODOs

- [ ] Current `@module` directive is stupid. Gonna make them act like both header-guards and namespaces.
- [ ] `SlimScript.Word` loses every single quotation mark (") in given value. We just want to lose the ones denoting start and end.
- [ ] `delete` keyword is broken. Shame on me.
- [ ] A lot of functions in standard library are broken. Again, shame on me.
- [ ] Exception handling is still on plan. (probably will stay a plan forever)
- [ ] nameof function (probably won't)
- [ ] Adding arrays to arrays without having to create temporary variables. Should be easy.
- [ ] Initializing multidimensional arrays (Adding arrays to arrays in init-time). 
- [ ] `range` function (currently `array.createRange` is a workaround)
- [ ] Standard library documentation
- [ ] Standard library improvements
- [ ] proper `@module`ing

Yep. That's all, and that's a lot.
