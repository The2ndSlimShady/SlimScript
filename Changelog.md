
# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.6] 25-08-2024

### Unreleased
- Exception handling is still on plan.
- nameof function
- Adding arrays to arrays
- range function
- Standard library documentation
- Standard library improvements
- proper `@module`ing

### Added
- Nothing at all.

### Changed
- Turns out `.ss` was `Scheme`'s file format. Now we use `.sscript`.
- The error messages are a bit (a tiny bit) better now.

### Fixed
- `array` module used `set` on the parameters, but it was from a time where SlimScript was dynamic. So I changed them and introduced a local variable.
- Path delimiters removed. I replaced every hand-coded path to `Path.Combine`. So now paths are working on Linux as well.

### Detected Bugs
- Nested `for` and `foreach` loops are causing problems. When you nest `for` with `foreach`, its okay but `for` with `for`... Oh boy...

### Removed

## [1.0.4] 22-01-2024

### Unreleased
- Exception handling is still on plan.
- nameof function
- Adding arrays to arrays
- range function
- Standard library documentation
- Standard library improvements

### Added
- Introductory documentation
- Escape character for `"`.
- Null check for `SourceChunk.SetVar` function.
- Support for `SlimScript.Word` in `append` and `set index` functions.
- Support for using existing variables in `for` loops.

### Changed
- `IVariable.Type`'s setter was `init`. Its now `set`.
- Empty array constructor was calling standard array constructor with empty token list and null. Why did I do that, honestly I don't know. Now it directly initializes an empty array.

### Fixed
- Fixes in `tobool` function.
- It was still possible to get an initialization fail, I promise its now impossible. I swear.
- In `SlimScript.Bool` constructor, `IVariable.Type` was not set in most places. It caused type mismatches when using `SourceChunk.SetVal`. Its now fixed.
- Fixed type mismatches in `thisSet` function.
- `IVariable.Type` was not set when using `Variable.Copy`. It caused type mismatches. It works now.

### Removed

## [1.0.3-alpha] 06-11-2022

### Unreleased
- Exception handling is a must. When I do it, its gonna be the release `1.1.0-alpha`.
- Also I gotta improve the standard libraries.

### Added
- `io/directory.ss` is completed, for now.

### Changed
- Just realized that i was developing on the master branch so switched to dev now.

### Fixed
- Indexers with clr variables didn't seem to be working. Now it seems.
- Array initalization was still a problem. Now I think it's impossible to get an initialization fail.

### Removed
- The "Release" tag I accidentally craeted.

## [1.0.2-alpha] 06-11-2022

### Unreleased
- Exception handling, maybe?
- Perhaps some code refactoring, I admit that I wrote dirty (very dirty) code.

### Added
- Escape Characters!!!!
- `io/directory.ss` is updated. It's nearly completed!

### Changed
- Moved macro replacing from Pre-Process to Pre-Pre-Process. 
- Ability to acces to command-line arguments without main function. If there is no arguments then os.args null, os.argc is 0. (os.args, os.argc)

### Fixed
- Array initialization was a bit problematic, can't explain shortly. Fixed it. Yeah.
- Even if string is not finished with a " symbol, pre-processor will accept it as a string if it comes to an EOL. Fixed it now.

### Removed

## [1.0.1-alpha] 02-11-2022

### Unreleased

### Added
- Importing files at runtime (good for text based games etc.)
- `tobool` command. Because, why not?

### Changed
- Add `import` macro to `sytactic/swag.ss`
- Add `tobool` macto to `syntactic/swag.ss` 

### Removed

## [1.0.0-alpha] 30-10-2022

### Unreleased

### Added

### Changed
- Add changelog (Gotta explain changes more detailed)
- Add support to undef macros
- Now you can include `syntactic/<lib-name>` to convert SlimScript to an esoteric language (try `swag` macros, my personal favourite) 

### Removed
