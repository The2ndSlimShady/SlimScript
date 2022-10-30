# Slim Script

Slim script is an interpreted scripting language for embedding it into your
projects. The interpreter is written in C# so it can (at least to a certain level)
interact with .Net echosystem.

---------------------------------------------------
<br/>

## Notes

SlimScript is a little embeddable Turing-complete scripting language for .NET (mainly C#). It's a bit slow because of 
its dynamic design and the usage of reflection under the hood but still, its decent enough.

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

You can also get project installer from `Release` section.

----------------------------------------------------

<br/>

[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/GPL-3.0)
