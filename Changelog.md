# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


<br><br>*****************************************

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

<br><br>*****************************************

## [1.0.1-alpha] 02-11-2022
### Unreleased

### Added
- Importing files at runtime (good for text based games etc.)
- `tobool` command. Because, why not?

### Changed
- Add `import` macro to `sytactic/swag.ss`
- Add `tobool` macto to `syntactic/swag.ss` 

### Removed


<br><br>*****************************************

## [1.0.0-alpha] 30-10-2022
### Unreleased

### Added

### Changed
- Add changelog (Gotta explain changes more detailed)
- Add support to undef macros
- Now you can include `syntactic/<lib-name>` to convert SlimScript to an esoteric language (try `swag` macros, my personal favourite) 

### Removed