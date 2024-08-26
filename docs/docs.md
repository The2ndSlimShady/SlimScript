
# SlimScript Documentation

(This documentation is heavily inspired by [documentation of V lang](https://github.com/vlang/v/blob/master/doc/docs.md))

## Introduction

SlimScript is a static typed scripting language. Since I did not have enough mental power and knowledge when developing this language, the language is not compiled to byte code or machine code. It's interpreted. Line by line. So it's slow. Very slow. It was designed merely for fun. I was bored and wanted to feel cool so I made this.

It's theoretically Turing-complete. It looks a bit like Lua. It does not have such cool features, the only cool thing about it is that you have access to C# from inside the script. I didn't test it but potentially it has the ability to use any .NET language from the script.

This little cool feature of it could be used to add a basic app of yours the ability to be modded. A basic language for basic uses.

Also SlimScript is designed to be a language that is close to speaking. Meaning when you write code, the code also makes sense linguistically. We'll see this later in this documentation.

## Installing SlimScript

Just go to the [Releases](https://github.com/The2ndSlimShady/SlimScript/releases) page and grab the installer of the last version,

or;

You can build the project by yourself, copy the `appinfo.json` and `lib/` to `%AppData%/SlimScript/`, add the executable to your path and be on your way.

> NOTE: \
>   I made this project 2-3 years ago, when I was just getting comfortable with programming. And my machine was Windows at that time. So I've never used
>   SlimScript on a Linux machine. And so there is no support for Linux (sadly)<I'm sorry>.

> ANOTHER NOTE: \
>   I previously had a repo, which I had created when I started this project, but due to some complications, I had to delete it and recreate the repo, resulting in this
>   blank commit history. A part of the changelog is still available tho. 

> !!! ANNOUNCEMENT !!!
>   As of 25.08.2024, I'm happy to announce that I've tested SlimScript on a Linux machine (with Debian 12). There was some problems at first but
>   After making a couple little changes (mostly because of path delimiters), it runs smoothly now.

## Table of Contents

- [SlimScript Documentation](#slimscript-documentation)
	- [Introduction](#introduction)
	- [Installing SlimScript](#installing-slimscript)
	- [Table of Contents](#table-of-contents)
	- [Hello World](#hello-world)
	- [Passing Parameters](#passing-parameters)
	- [Comments](#comments)
	- [Functions](#functions)
		- [Calling Functions](#calling-functions)
		- [Returning Values](#returning-values)
		- [Hoistings](#hoistings)
		- [Operators](#operators)
	- [Symbol Visibility](#symbol-visibility)
	- [Variables](#variables)
		- [Assignment](#assignment)
		- [Variable Shadowing](#variable-shadowing)
	- [Data Types](#data-types)
		- [Words (Strings)](#words-strings)
			- [String Operators](#string-operators)
		- [Numbers](#numbers)
		- [Arrays](#arrays)
			- [Array Initialization](#array-initialization)
	- [Modules](#modules)
		- [Header Guards](#header-guards)
			- [Another Crooked Way for Header Guards](#another-crooked-way-for-header-guards)
		- [Importing Modules From Another Folder](#importing-modules-from-another-folder)
		- [Selective Includes](#selective-includes)
		- [Module Aliasing](#module-aliasing)
		- [Runtime Importing](#runtime-importing)
	- [Statements and Loops](#statements-and-loops)
		- [If](#if)
		- [For Loop](#for-loop)
		- [Foreach Loop](#foreach-loop)
		- [While Loop](#while-loop)
	- [Treating Functions as Variables](#treating-functions-as-variables)
		- [Adding Functions to Arrays](#adding-functions-to-arrays)
		- [Passing Functions to Functions](#passing-functions-to-functions)
		- [Returning Functions From Functions](#returning-functions-from-functions)
	- [CLR Type and Using .NET](#clr-type-and-using-net)
		- [Defining Variables From CLR Types](#defining-variables-from-clr-types)
		- [Calling Functions](#calling-functions-1)
			- [Constructing Instances](#constructing-instances)
		- [Accessing to Fields and Properties](#accessing-to-fields-and-properties)
			- [Changing the Values of Fields and Properties](#changing-the-values-of-fields-and-properties)
			- [Indexing](#indexing)
	- [Memory Management](#memory-management)
	- [Throwing Errors](#throwing-errors)
	- [Builtin Functions](#builtin-functions)
		- [write](#write)
	- [Builtin Function-ish Keywords](#builtin-function-ish-keywords)
		- [define](#define)
		- [set](#set)
		- [index](#index)
		- [append](#append)
	- [Appendices](#appendices)
		- [Keywords](#keywords)
		- [Operators](#operators-1)
	- [Using CLI](#using-cli)
		- [The Flag `-C`](#the-flag--c)
	
## Hello World

```mizar
func main args argc begin
	write "Hello World"
end
```

Save this to a file with extension `.sscript`. Then do a `SlimScript <your_file>.sscript`

> Assuming you have SlimScript in your environment variables.
> If you do not then write the path to SlimScript manually.

Voila! That was your first SlimScript script. You can compresspile (compress + compile) your script using `SlimScript`

> You can see the modes of SlimScript by passing the flag `-h` or `--help` as an argument. `SlimScript -h` or `SlimScript --help`

The main function is optional. You can write the program above with just `write "Hello World"`. We'll discuss this with more detail later.

## Passing Parameters

You probably recognized the parameters `args` and `argc`. These are exactly what you think they are.  `Arguments` and `argument count` respectively. This documentation will show you how to use arrays and variables later but for now you should know that `args` is an array. It's null by default if nothing is passed and `argc` is zero.

You can pass parameters by using `%p` in command line.

`SlimScript main.sscript %p Some Args Here And Here`

Now you can use this in your main function, or you can use `os.args` and `os.argc` in `system` module.

## Comments

```lua
-- This is a single line comment
-- Unfortunately there are no multiline comments in SlimScript
```

## Functions

```mizar
func writeSmth smth begin
	write smth
end

func writeSmth' smArr begin
	append 5 to smArr
	write smArr
end
```
There is no need to specify types. Technically you can pass parameters with wrong types but please don't do that. I didn't implement type checking in functions. I mean I could've done it but I did not. I can't change it now.

Functions cannot be overloaded but you can include non-lethal symbols in your function name. For example the symbol `!` and `'` are not special symbols in SlimScript. So when you want to overload the function `add` you can simply name it `add'` or `add!` or even `!add'`!

### Calling Functions

When you call a function, that actually means that you are saying "Do this function" to the computer. So in SlimScript we call functions by `do <function_name>`.

```mizar
do writeSmth "Smth"		-- Smth
```

It's as simple as that!

### Returning Values

Just as usual, we use `return` keyword. You can discard the return values, or define a variable with them.

```mizar
func add x y begin
	return + x y
end

define var as do add 5 4		-- var = 9
do add 9 6						-- returns 15, but its discarded
```

### Hoistings

Functions canNOT be used before their declaration. Because I was so lazy to implement it. Functions are treated like variables is SlimScript. Meaning they can only be used after their declaration. But you can use undefined functions inside a function. Because SlimScript is completely interpreted meaning unless you call the function, program does not know what is inside of it so it won't care if you use something that don't exist.

### Operators

Operators are also functions. That's why we use the stupid syntax `<operator> <operand> <operand>`. The list of operators and standard functions will be shown later. I just wanted to clear up your thoughts such as "Why the hell we use operators this way?"

## Symbol Visibility

There is no such thing as `access modifiers` in SlimScript. Everything is public as long as you know their name!

## Variables

```mizar
define name as "Duke"
define age as 45
define arr as [5,4, "Hello", "Another", "Array"]
define nullVar as null

write name
write age
write arr
write null
```

Variables are defined with the keyword `define` (How surprising). Unlike some languages, you can't declare a variable and initialize it later. You have to define and initialize it together. Meaning every variable has to have a initial value.

Preferred naming style is `camelCase`. It just looks right.

### Assignment

Assignment is done with the keyword `set`.

```mizar
set name to "AnotherName"
set arr to 5
set age to ["An", "Array"]
set nullVar to "NonNull"
```

Since SlimScript is static, you absolutely **can't** change the type of the variable. It'll cause a `DisordantTokenError`.

But you know, you can set a null variable to a non-null value. SlimScript will then decide the type of the variable. After that you can't change its type. It's the whole purpose of the type `null`.

```
define variable as null

set variable to 5
write "variable is : " variable
set variable to "No"
```

> Output:
> 
> variable is : 5
> Cannot set variable 'variable' to 'No'. Type 'Number' does not match 'Word'.
> File: main_p.sso
> Line: 4
> Expression: set variable to "No"
> Exit Code: 10 \<DisordantTokenError\>

As you can see, unless the type is `null`, you can't set the value of a variable to a value with another type.

### Variable Shadowing

```mizar
define someVar as "Some Variable"

func someFunc begin
	define someVar as ["Completely", "Different", "Value"]
	return someVar
end

write "Out someVar : " someVar
write "In someVar : " do someFunc
```

The output of the code above is:

```
Out someVar : Some Variable
In someVar : [ Completely, Different, Value ]
```

No need of explanation I suppose.

## Data Types

```
number (double): Double for all

bool: classic true or false

word (string): "No chars. Only words"

array: ["It", 5, variable, "can hold everything", evenFunctions]

clr: Too soon to explain this

null:

function: Yes! Function is a type! 
```
That's all. SlimScript has only seven data types. Well `null` is actually not a type. So you can even say six types.

### Words (Strings)

```mizar
define word as "Word"
write index 0 of word			-- 'W'

define newLn as "`n"			-- like PowerShell
```

Sadly no `\u2605`'s in words. I  was too lazy to implement it.

Words are mutable. You can do:

```mizar
define str as "Helo"
set index 2 of str to "a"		-- str : "Halo"
```

Only double quotes can denote words. So its safe to use single quotes in a word. You can also use ``` `q ``` (escape character for double quotes) in words.

Don't expect to get `195` when you do a `tonumber "0xc3"`. It's not implemented. Same for octal and binary conversions.

#### String Operators

```mizar
define str as "Some"
define str2 as " Str"
define str3 as + str str2	-- "Some Str"

define str4 as "St"
set str4 to + str4 "r"		-- "Str"
```

No. You can't concatenate a word and a number. But you can do a:

```mizar
define num as 5
define str as + "Num : " tostring num		-- "Num : 5"
```

The standard string library of SlimScript includes some more functions for string. But its not our goal to learn the libraries here. So that's all for words now.

### Numbers

```mizar
define num as 185
```

No doubles, no floats, no i32, i64, byte or anything. One number type to rule them all!

### Arrays

An array in SlimScript is dynamic. So it can hold any type of value EXCEPT another array. I have plans to add that feature too but for now, it can't hold another array.

By saying it can't hold another array, I mean in the definition. you cant give `[5, [3, 2]]` as initial value to an array but you can give `[5, otherArr]`.

```mizar
define arr as [5, 4, "Value"]

write arr				-- [ 5, 4, Value ]
write index 0 of arr	-- 5

set index 0 of arr to 0
write arr				-- [ 0, 4, Value ]
```

We use square brackets when defining arrays. An element in an array can be accessed with `index <idx> of <arr>` operation.

You can append elements with `append` operator.

```mizar
define nums as [1, 2]
append 3 to nums
write nums 				-- [ 1, 2, 3 ]
```

However, since arrays can't hold another arrays, appending arrays is not possible and will result a grammar error.

```mizar
define nums as [1, 2]
append [5,2] to nums
```
> Error: Arrays cannot be appended to anything.
> 
> Exit Code: 8 < GrammarError>

But just as before you can do `append otherArr to nums`.

#### Array Initialization

You can also use identifiers while defining arrays.

```mizar
define num as 5
define arr as [num, 4]
write arr 	-- [ 5, 4 ]
```

Be aware that when you use `num` in `arr`, the value of `num` is copied to `arr`. So when you do:

```mizar
set index 0 of arr to 0
write arr
write num
```

The output will be:

```
[ 0, 4 ]
5
```

There is an array library in SlimScript standard library. I plan to create separate docs for libraries so for now, this information about arrays is enough.

## Modules

Modules are script files containing scripts that meant to be reused. Every module is a file, but every file is not a module. But what does that exactly mean? First let's see how we can create modules.

Let's assume we need a module named `operations`. We first create a file named `operations.sscript`.

```mizar
-- operations.sscript

func libFunc begin
	write "My First Module!"
end
```

You just have to include it in another script file. Then voila! That's a module for you.

### Header Guards

Now technically you can import this to another script using `@include operations`. But what happens if we then include another module that also includes `operations`? That'd cause a circular include and our script will not work. To solve this problem we should use something like `header guards` in C. 

We simply put `@module <module_name>` at the beginning of our script.

```mizar
-- operations.sscript

@module operations

func libFunc begin
	write "My First Module!"
end
```

Be aware that we use the file name when adding our `header guard`. That's not a must, but I recommend using the file names for consistency.

Now in another file, we simply do

```mizar
-- anotherFile.sscript

@include operations

do libFunc				-- My First Module!
```

Congratulations! You've just made your first module! You can also include `*.csso` files in your script. `*.csso` files will be explained later in this documentation. For now just know that you can include them.

#### Another Crooked Way for Header Guards

You can also use `define/undef`, `if/elif/else/endif` for guarding your module.

```mizar
@if not operations
@define operations

func libFunc begin
	write "My First Module!"
end

@endif
```

This might come in handy in an extreme situation. Although I've never seen a situation like that. It's best to use `module`. There are some technical reasons for it too. 

When you use `module`, the pre-processor skips the file right away if you already have included it. But when you use the **"crooked"** way, pre-processor keeps going until the end of the file. Pre-processor does not know if its a header guard or just an if statement. So in large libraries, using `module` is a little faster.

### Importing Modules From Another Folder

Let's assume that we have a folder structure that looks like this:

* base
	* lib
		* thirdParty
			* tpLib.sscript
		* lib1.sscript
	* main.sscript
	* file.sscript

In `main.sscript` you can include those modules

```mizar
@include lib.thirdParty.tpLib
@include lib.lib1
@include file

-- Do something in main file
```

like this.

You don't have to use `'.'` when including modules. You can use `'\'` and `'/'` too. But that's all. Use whichever suits you the best.

### Selective Includes

Sadly, there is no such thing in SlimScript. But standard libraries are splitted to multiple files when they're too long. For example the `io` library is 220~ lines long. It includes both directory and file operations. But you might not want both sometimes. So because of that, standard library structure is like this:

* lib
	* io
		* file.sscript
		* directory.sscript
	* io.sscript

This way you can use

```
@include io
@include io.file
@include io.directory
```

one of these according to the situation. I recommend doing the same in your projects.

### Module Aliasing

No such thing. Sorry.

### Runtime Importing

Actually importing modules in SlimScript can be done in two ways. One way is our classic way which is done at pre-processing time. But that way is way too limited, choosing what to include can only be done with pre-processor directives.

But the second way is way more powerful. It's called `runtime importing`. It's done with the function `import`. Instead of adding the modules codes into the source of our main file, it runs through the imported module, copies its variables to main stack, then destroys the temporary source code chunk created for it.

It might come in handy in a lot of situations. I don't have a specific example in my mind right now but I have an example. 

Let's say we have a folder structure that looks like this:

- base
	- main.sscript
	- testLib.sscript
	- otherLib.sscript

```mizar
-- testLib.sscript

@module testLib

func testFunc begin
	write "Test Lib"
end
```

```mizar
-- otherLib.sscript

@module otherLib

func testFunc begin
	write "Other Lib"
end
```

```mizar
-- main.sscript

define module as input "Enter Library Mode : "

import module "Lib.sscript"

do testFunc
```

The import function takes everything to its right, gets their string representation then imports the corresponding module if it exists. Notice that when runtime importing, file extension is written manually.

Let's run the code with input `test`:

> Output:
> Enter Library Mode : test
> Test Lib

Now let's try it with input `other`:

> Output:
> Enter Library Mode : other
> Other Lib

As you can see its quite the cool feature. I'm sure you can find a way to use it.

## Statements and Loops

### If

```
define limit as 20
define age as 15

if < age limit then
	write "You are too young"
elif = age limit then
	write "You are at the limit"
else
	write "You good"
end
```

If statements in SlimScript look like Lua's. Everything between `if` and `then` is used to evaluate a boolean value.

`if` cannot be used as an expression. Sadly. It can be shrunk to one line tho.

```mizar
define age as -1

if < age 0 then write "How?" end
```

That's all our poor `if` can do.

### For Loop

`For` loops in SlimScript follow the style of C. The logic style I mean.

```mizar
for i as 1 || < i 5 || 1 begin
	write i
end
```

> Output:
>1
>2
>3
>4

Now let's break it down. The structure of a `for` loop looks like this:

```mizar
for <loop_variable> || <condition> || <increment_value> begin
	... Action ...
end
```

Loop variable is a number. You can create a temporary variable for the loop using `<name> as <start_value>`. Or you can use an existing variable with just `<name>`.

Condition is independent from loop variable. As long as it returns a boolean value, everything is fine. You can call a function or use an operator. You can just loop to infinity by leaving a `true`!

Increment value is the value that will be added to loop variable at the end of each loop.

### Foreach Loop

```mizar
define arr as [1, 4, "Hello!", "Again"]

foreach item in arr begin 
	write item
end
```

The `foraech` loop is used for going through elements of an array. The structure of a `foreach` loop looks like this:

```mizar
foreach <element_name> in <array> begin
	... Action ...
end
```

The element is not read-only. Nothing in SlimScript is read-only. Just know it.

I'm planning to add another standard function `range`. So that we can do `define arr as range <etc.>` or `foreach i in range <etc.> begin`. For now there is no such thing.

### While Loop

```mizar
define condition as true

while condition begin
	define age as tonumber input "Enter Age : "

	if < age 20 then
		set condition to false
	end
end
```

While loop might be the most basic loop in existence. It literally does a thing while the condition is true. It's structure is:

```mizar
while <condition> begin
	... Action ...
end
```

## Treating Functions as Variables

It was mentioned before that functions were treated like variables in SlimScript. Meaning you can add functions to arrays, pass functions to functions and return functions!

Let's go step by step.

### Adding Functions to Arrays

A function can be added to an array. This allows you to create something like an action pool. Then you can iterate over the functions and call them.

```mizar
func add x y begin
	return + x y
end

func sub x y begin
	return - x y
end

func mul x y begin
	return * x y
end

define funcPool as [add, sub, mul]

define val1 as 5
define res as 4

foreach fn in funcPool begin
	set res to do fn val1 res
	write "Res : " res
end
```

> Output:
> Res : 9
>Res : -4
>Res : -20

### Passing Functions to Functions

This is a perfect scenario for a scripting language. Let's assume that we need to encrypt something. But we also want to be able to change the encryption standard. What do we do?

We'll first create a function named `encrypt`. This function will take another function as an argument, which will be the encryption standard.

```mizar
func encrypt data standard begin
	... Perhaps Some Other Operations ...
	
	do standard data

	... Some Other Operations ...
end

-- in somewhere else
do encrypt anotherData AES

-- in another place
do encrypt yetAnotherData RSA
```

### Returning Functions From Functions

I mean, this is a cool feature isn't it? I believe you can find a place to use this feature too.

```mizar
func returnFunc begin
	func someFunc begin
		write "How Cool is That!!"
	end
	
	return someFunc
end

define returnedFunc as do returnFunc

do returnedFunc
```

> Output:
> How Cool is That!!

## CLR Type and Using .NET

Perhaps the most significant feature in SlimScript (as if there are important fetures in it) is the ability to use .NET framework. It's quite simple actually. But also complicated.

.NET is evolving but SlimScript is not. So SlimScript might not be able to use (actually, I'm writing this documentation a year after the creation of SlimScript so it **is not** able to use) some features. Anyway. Let's get going.

### Defining Variables From CLR Types

To address to a CLR type, we use a curly braced syntax :`{Namespaces::To::Class}`. The rest is same.

```mizar
define settings as {SlimScript::GlobalSettings}

write typeof settings
write settings
```

> Output:
> CLR : SlimScript.GlobalSettings
> SlimScript.GlobalSettings

### Calling Functions

To address to functions from an instance or a type, we use the symbol `->`. Unfortunately we can't use a syntax like `{Namespace::Class}->Function`. We first have to assign the type to a variable.

```mizar
define math as {System::Math}
define pi as 3.14159265358979

write math->Sin / pi 2
```

> Output: 1

#### Constructing Instances

Constructors are treated like functions. First we assign the type to a variable. Then we call the constructor and assign it to another variable.

```mizar
define sb_t as {System::Text::StringBuilder}
define sb as sb_t->new
```

You can pass parameters to constructors. If there are multiple constructors, SlimScript will choose the most suitable one and use it.

### Accessing to Fields and Properties

To address to fields, properties, etc. from an instance or type, we use the symbol `:`.  Again we can't use a syntax like `{Namespace::Class}:Field`. We have to assign the type to a variable.

```mizar
define settings as {SlimScript::GlobalSettings}

write settings:AppInfo
```

> Output:
>Name: SlimScript
>Description: Small Embeddable
>Scripting Language for C#
>Version: 1.0.3-alpha
>Contributors:
>       The2ndSlimShady  - github.com/The2ndSlimShady/

#### Changing the Values of Fields and Properties

Setting fields and properties is quite simple in SlimScript. We just use the classic accessing syntax, then write its new value. `instance:Field <new_value>`.

```mizar
define sb_t as {System::Text::StringBuilder}
define sb as sb_t->new

write "Len : " sb:Length
sb:Length 5
write "Len : " sb:Length
```

#### Indexing

SlimScript uses imaginary fields called `thisGet` and `thisSet` while dealing with indexers. This example will be enough for you to understand:

```mizar
define variable as {SlimScript::Variable}
define array_t as {System::Collections::ArrayList}
define arr1 as array_t->new [5, 6, 8]

write arr1:thisGet 1

arr1:thisSet 2 0
write arr1:thisGet 2
```

> Output:
> 6
> 0

As you can see their structure is quite simple. `instance:thisGet <index>` and `instance:thisSet <index> <value>`.

<br/>

That's all that SlimScript can do with CLR type. It's quite limited but you know, this was just a boredom boredom go away project so I think its enough.

## Memory Management

SlimScript does not manage the memory for you at all. Since there is no such thing as a `pointer` there is no way that you can cause a memory leak. 

Still, you might want to delete some variables, define them again. It's useful, say, when you want to change the type of a variable. As you know, normally that's not possible. But if you delete a variable and then define it again, it works. Let's see the example below.

```mizar
define number as 5
write typeof number

delete number
define number as "Haha no longer a number"
write typeof number
```

> Output:
> Number
> Word

As you can see we use the function `delete` (how surprising) to delete variables. You can also delete functions.

```mizar
func function begin
	write "Exists"
end

do function
delete function
do function
```

> Output:
> Exists
> Variable named 'function' does not exists.
> File: main_p.sso
> Line: 4
> Expression: do function
> Exit Code: 14 \<NullReferenceError\>

## Throwing Errors

Well, sometimes things don't go the way they should. When that happens we just throw an error and ruin the program flow. But while doing this we also provide some information about the error too.

To do this, we use the function `error` in SlimScript.

```mizar
func factorial n begin
	if < n 0 then
		error "An error occured in function 'factorial'" "'n' can't be negative"
	end
	... Factorial Code ...
end

do factorial -5
```

> Output:
> An error occured in function 'factorial'
> Data: 'n' can't be negative
> File: main_p.sso
> Line: 3
> Expression: error "An error occured in function 'factorial'" "'n' can't be negative"

The second parameter error data is optional.

## Builtin Functions

| Function         | Description |
|--------------|-----------|
| write| Writes the given data to standard output|
| clrname | Returns the CLR name of given variable/type etc. (For example `clrname "a string"` is `SlimScript.Word`)|
| delete| Deletes the given variable from stack |
| do | Executes the given function |
|typeof|Returns a string containing the type name of given variable|
| error | Throws an error, halting the program, with given data and message |
| tobool | Converts given data to bool, if it can be converted|
| tonumber | Converts the given data to number, if it can be converted |
| tostring | Gets the string representation of given data |
|input|Reads the standart input and returns a line from it|
|import|Imports modules at runtime|

### write

It explains itself. One little thing to add is that you can pass multiple parameters. The function will concatenate them the best way it can and print them.

The others are simple so no need of sub-heading for them. But there are some function-ish keywords that should be mentioned. Let's break them into sub-headings and dive.

## Builtin Function-ish Keywords

|Function-ish|Description|
|-----|-----|
|define|Surprisingly, its a function-ish. It defines variables.|
|set|Yes. Another known face. It changes the values of variables.|
|index|Returns an index. Tho it has some complicated features as well.|
|append|Appends something to an array or a word.|

### define

`define <name> as <value>`

It simply defines variables.

### set

`set <variable> to <new_value>`
`set index <idx> of <variable> to <new_value>`

Guess what. It sets variables.

### index

`index <idx> of <array_or_word>`

Index function-ish is a special one. Because it can be combined with other functions and function-ishes. It's usually combined with `set` and `append`.

With `set index <idx> of <var> to <value>` you can alter elements at indexes of strings and arrays.

With `append <value> to index <idx> of <var>` you can insert elements to indexes of arrays and strings.

### append

`append <value> to <var>`
`append <value> to index <idx> of <var>`

You can append or insert elements to arrays and strings with this function-ish.

## Appendices

### Keywords

SlimScript has 23 keywords:

```
as
begin
end
to
then
in
of
not
both
any
define
func
set
return
if
elif
else
while
foreach
index
append
for
break
```

Some of these keywords are half functionish things. For example `index` and `append`. They were mentioned before in this documentation.

### Operators

There are only 11 operators in SlimScript:

```
+	sum
-	difference
*	product
^	power
/	quotient
=	equals
!= 	not equals
<	lesser than
>   greater than
>=  greater or equal
<=  lesser or equal
```

Also since operators are treated like functions, there is no precedence among them.

## Using CLI

```
>>> SlimScript -h

Name: SlimScript
Description: Small Embeddable Scripting Language for .NET
Version: 1.0.5
Contributors:
        The2ndSlimShady - github.com/The2ndSlimShady/

Usage:
        SlimScript <file>
        SlimScript <file> <flags>
        SlimScript <file> <flags> %p <arguments>
        SlimScript -i
        SlimScript -h --help

Available Flags:
        -D              Run With Debug Mode. Generates lexer output (for curious ones). (SlimScript <file> -D)
        -C              Generates Compressed Standalone Script File That Has No
                        Dependencies On Any Other File.                                 (SlimScript <file> -C)
        -i              Run Interactive Mode                                            (SlimScript -i)
        -h --help       Show This Output.                                               (SlimScript -h --help)

        Passing Arguments:
                        SlimScript <file> <flags> %p <arguments>       Arguments are passed to main function as an array.
```

SlimScript has three special flags in its CLI. The flag `-D` generates lexer output, the flag `-i` runs the interactive mode, `-h` or `--help` writes the output above to the standard output and `-C` generates compressed standalone files. All the flags except `-C` seems to be understandable. So let us explain it.

### The Flag `-C`

SlimScript does not generate preprocessed files, meaning there are no such file containing your code with included modules. But sometimes we might want to have a single file containing everything it needs to be executed (a standalone file). But that standalone file of ours might be too big because of included modules. To solve both of those problems SlimScript has a `compress standalone mode`.

For example this little code below:

```
-- main.sscript

@include system
@include io
@include array
@include math
@include string

func main args argc begin
	write args
end
```

What a little harmless piece of code isn't it? **Wrong**. In debug mode (not the debug mode with `-D` flag. the debug mode of SlimScript's codebase) it generates a file with **324** lines, approximately 9KB in size. But we need to make it standalone and small. What do we do? Let's run our file with mode `-C`.

```
>>> SlimScript main.sscript -C

Successfully created and compressed standalone script file main.csso

Exit Code: 0 <Normal>
```

Now we have a little file named `main.csso`, which can be executed with `SlimScript main.csso`. It's only 2KB's in size and it runs faster than `main.sscript`. It's because when executing `*.csso` files, preprocessor skip everything, since everything has been preprocessed before. Be aware that runtime importing is still made in runtime, therefore you can't create standalone files when importing at runtime.
