# Slim Script

Slim script is an interpreted scripting language for embedding it into your
projects. The interpreter is written in C# so it can (at least to a certain level)
interact with .Net echosystem.

---------------------------------------------------
<br/>

## Jump To
- [Docs](docs/BASICS.md)
- [TODO](docs/TODO.md)

## Notes

Slim Script has many flaws in its design and it may lack some features
but it is a good place to start teaching someone programming because of 
its easy design.

After some time using SlimScript, you might wonder why error outputs line numbers does not match with source codes 
line numbers? Well thats because Interpreter uses the file post_lexer_humanized.ss as source code. If you're having trouble
finding it, it should be placed at the same directory as input file.

---------------------------------------------------
<br/>

## Contributors and Special Thanks 

- [@The2ndSlimShady](https://www.github.com/The2ndSlimShady) coding and design.

---------------------------------------------------
<br/>

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

Or try the interactive mode!

```bash
  ./bin/<Debug/Release>/net6.0/SlimScript -i
```

----------------------------------------------------

<br/>

[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)
