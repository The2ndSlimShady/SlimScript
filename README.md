# Slim Script

Slim script is an interpreted scripting language for embedding into your
projects. The interpreter is written in C# so it can (at least to a certain level)
interact with .Net ecosystem.
## Notes

Slim Script has many flaws in its design and it may lack some features
but it is a good place to start teaching someone programming because of 
its easy design.

## Contributors and Special Thanks 

- [@The2ndSlimShady](https://www.github.com/The2ndSlimShady) coding and design.
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)


## How To Work With SlimScript

Clone the project

```bash
  git clone https://github.com/The2ndSlimShady/SlimScript.git
```

Go to project dir

```bash
  cd SlimScript
```

Build the interpreter

```bash
  dotnet build
```

Run your script!

```bash
  ./bin/<Debug/Release>/net6.0/SlimScript path/to/script/file
```

### TODO

- [x] Bool Type
- [x] Number Type
- [x] String Type
- [x] Operators
  - [x] Logical Operations
    - [x] Both
    - [x] Not
    - [x] Any
    - [x] Equals        (=)
    - [x] Not Equals    (!=)
    - [x] Lesser Than   (<)
    - [x] Greater Than  (>)
    - [x] Lesser Equal  (<=)
    - [x] Greater Equal (>=)
  - [x] Numeric Operators
    - [x] Plus
    - [x] Minus
    - [x] Multiply
    - [x] Divide
- [ ] Standart Library Functions
  - [x] Define
  - [x] Set
  - [x] Write
  - [ ] Do
  - [x] Delete
- [ ] Blocks
  - [ ] Custom Functions
  - [ ] If-Else Blocks
- [ ] Other Features
  - [ ] Callback To C#
  - [ ] Using .Net Libraries
  - [x] Interactive Mode
