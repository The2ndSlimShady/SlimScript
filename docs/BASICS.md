# Slim Script Basics

## Sample Program

The program below uses recursion to mimic a for loop and find factorials of numbers.

```
-- This is a comment

-- A function named factorial with one parameter named number
func factorial num begin
    if = num 2 then
        return 2
    end
        
    return * num do factorial - num 1		--You'll learn about the operators later
end
    
func loop times begin
    if = times 1 then
        return
    end

    write "Factorial of " times " is: " do factorial times
        
    do loop - times 1
end
    
do loop 5   -- Loop 5 times

-- Output:
--      Factorial of 5 is: 120
--      Factorial of 4 is: 24
--      Factorial of 3 is: 6
--      Factorial of 2 is: 2
```

---------------------------------------------------
<br/>

## Creating Variables

```
define <variable_name> as <value>
```

By this way you can create variables. Note that SlimScript is not a strong typed language so you can assign any value to any variable.

---------------------------------------------------
<br/>

## Using Variables

```
define someNum as 15
    
write "Value of Some Num is " someNum

-- Output
--      Value of Some Num is 15
```

---------------------------------------------------
<br/>

## Changing The Value of A Variable

```
set <variable_name> to <new_value>
```

As mentioned before SlimScript is not strong typed so variable types are dynamic. Check the program below

```
define var as 2

write "Var before change is " var
    
set var to "Is not 2"
    
write "Var after change " var

-- Output
--      Var before change is 2
--      Var after change Is not 2
```

---------------------------------------------------
<br/>

## Deleting A Variable From Memory

Let's say you made a program that uses a lot of recursion. And let's say that you use lots of temporary variables inside the function. Wouldn't it be good if you could delete those temporary variables manually so the memory footprint becomes much lesser?

Here's a basic example

```
define someVal as "Some Value"

write someVal

delete someVal

write someVal

-- Output
--		Some Value
--		Variable named 'someVal' does not exists. line 4
--
--		Exit Code: 14 <NullReferenceError>
```

---------------------------------------------------
<br/>

## Operators

- (+) 		Plus
- (-) 		Minus
- (*)  		Multiply
- (/) 		Divide
- (=) 		Equals
- (^)       Power
- (!=) 		Not Equals
- (<) 		Lesser Than
- (>) 		Greater Than
- (<=) 	    Lesser Equals
- (>=)		Greater Equals
- (not)		Not
- (both)	And
- (any)		Or

Operators in SlimScript are a bit different than other languages. (It's a bit like Elisp arithmetic operators)

Check the sample code below

```
define num as 15
define otherNum as + num 1

write otherNum

-- Output
--		16
```

As you can see, operators are just like the functions. They take everything at their right as their operands.

But note that the operators just accepts two operands. Like if you write

`define num as - 5 1 1`

the value of variable `num` will be `4`.

### <br> Using Return Values as Operands

Let's assume that you gotta use an operators return value as operand in your code. How are you gonna do it? It's simple but a bit tricky.

```
define five as 5
define four as 4
define three as 3

write - 5 - 4 3
```

Seems a bit intimidating isn't it? Lets split the expression into parts. As mentioned before operators accepts just two operands, so the operands of the first '-' will be `5` and `- 4 3`

So our operation will be `- 5 1`.

### <br> Note:

We mentioned that '`operators take everything at their right as their operands`' so if you're gonna use return values as operands, they always should be the right operand. For example;

If you write an expression like this: `define someNum as + + 5 1 2`

Interpreter will detect the first plus (left) and try to initialize the operands from remaining expression. So it'll look at right and see the other plus, and execute it. The second plus will take all the remaining tokens as operands but only use the first two of them so it'll use 5 and 1 but 2 will be invisible for the first plus.

We can write the final expression that interpreter sees like this: 

`define someNum as + 6`.

### <br> Another Note About Numeric Operators:

Actually there is no operator precedence in SlimScript. Everything will be executed the way you write it. You want the addition to be executed before multiplication, go on, you have the right to do whatever you want.

```
-- Expression (2 * 2 + 1)'s result will be '5' in other --  -- languages.


-- But in SlimScript (* 2 + 2 1) will be '6'

write * 2 + 2 1

-- Output
--      6
```

---------------------------------------------------
<br/>

## Arrays

Arrays in SlimScript are non-generic and can hold any value type including functions and arrays. To create array you can use `define` standart method but with a little difference and restriction.

Let's create an array holding a number, a bool, a string and a function.

```
func someFunc begin
    write "Hello"
end

define arr as [ 12, false, "Test String", someFunc ]      
-- There should be at least one  space between braces and -- arguments
```

The space between arguments are optional whereas the space between arguments and braces are a must.

Now lets loop through our array and write the elements to console.

```
func someFunc begin
    write "Hello"
end

define arr as [ 12, false, "Test String", someFunc ]      
-- There should be at least one  space between braces and -- arguments

--You'll learn foreach loop at loops section
foraech element in arr begin
    if = element someFunc then
        do element
    else
        write element
    end
end

-- Output
--      12
--      False
--      Test String
--      Hello
```

### <br> Important Note

All the variables in SlimScript except arrays (arrays are sometimes value types too. It's a bit complicated) are value types. So if you write something like this:

```
define arr as [ 1, 2, 3, 4, 5 ]

foreach var in arr begin
    delete var
end

foreach var in arr begin
    write var
end
```

The output will still be

```
1
2
3
4
5
```

Because when `delete var` executes, it will delete the local copy of the item in array. So array will not be affected nor the elements.

### <br> With More Detail

```
define arr as [ 1, 2, 3 ]
define otherArr as arr

set index 0 of arr to 5   -- You'll see this later. It basically sets the first element of array to 5

write otherArr

-- Output
--      [ 5, 2, 3 ]
```

This is a situation where arrays act like reference type. As long as `arr` exists in memory, `otherArr` will point to it. But if you do something like this:

```
define arr as [ 1, 2, 3 ]
define otherArr as arr

delete arr

write otherArr

-- Output
--      [ 5, 2, 3 ]
```

`otherArr` is now has it's own value.

### <br> Getting Values From Array

An array that you can't get the desired element would not be that much useful. Keep in mind that indexes start from `0` not `1`!

```
define arr as [ 1, 2, 3, 4 ]

define i as 3   -- We want to get the 4th element

write index i of arr

-- Output
--      4
```

### <br> Adding Values To Array

Arrays in SlimScript are dynamic sized. So you can add as many items as you want without hesitating (but your memory might not be very happy about it).

```
define arr as [ 1, 2, 3, 4 ]

append 125 to arr

write arr

-- Output
--      [ 1, 2, 3, 4, 125 ]
```

### <br> Removing Values From Array

Not much to explaing. Just take a look at the example below.

```
define arr as [ 0, 1, 2, 3, 4 ]

write "Array before deletion: " arr

delete index 0 of arr   -- remove index 0 of array

write "Array after deletion: " arr

-- Output
--      Array before deletion: [ 0, 1, 2, 3, 4 ]
--      Array after deletion: [ 1, 2, 3, 4 ]
```

### <br> Mixed Sample

Now lets create a method that takes an array as parameter and reverses it.

```
func reverse arr begin
    write "Reversing " arr

    define length as 0
    define newArr as [ ]

    foreach item in arr begin
        set length to + length 1
    end

    write "Length: " length

    foreach item in arr begin
    
        -- Don't worry im gonna explain whats going on
        append index - length 1 of arr to newArr

        set length to - length 1
    end

    write "Done " newArr
end
    
define array as [ 1, 2, 3, 4 ]

do reverse array

-- Output
--      Reversing [ 1, 2, 3, 4 ]
--      Length: 4
--      Done [ 1, 2, 3, 4 ]
```

It may seem a bit intimidating but hey, I'm here to help you understand.

I assume that you understand whats going on in first foreach loop so lets jump to the second one.

```
foreach item in arr begin
    
    -- Don't worry im gonna explain whats going on
    append index - length 1 of arr to newArr

    set length to - length 1
end
```

Lets split the line `append index - length 1 of arr to newArr` to parts so its easier to understand.

Keep this, `append <item> to <array>`;

And this, `index <number> of <array>` in mind.

First we see the append function. It takes 2 parameters `<item>` and `<array>`. 

Our `<item>` parameter is  `index - length 1 of arr`,

and `<array>` parameter is `newArr`

`<array>` is simple so lets look at `<item>`.

It's clear that `<item>` is index `(length - 1)` of `arr`

---------------------------------------------------
<br/>

## If-Else Blocks

If-else blocks in SlimScript are just like the Lua's. One sample program should be enough for you to understand.

```
define age as 18

if >= age 18 then
    write "You're too old"
elif >= age 15 then
    write "You're a teen"
else
    write "Baby"
end

-- Output
--      You're too old
```

---------------------------------------------------
<br/>

## Functions

Now this is where the fun part begins. Everybody loves functions right?

First of all, the structure of a function should be like this:

```
func <function_name> <params> begin
    -- Do some operations
    -- here
end
```

It's simple enough isn't it? But calling functions is a bit different. Let's create a function to use as an example first.

```
func greet greeting name begin
    write greeting " " name "!"
end
```

Generally in other languages you would probably write something like this;

`greet "Hi" "Quandale"`

and expect an output like this;

`Hi Quandale!`

but in SlimScript you'll get this instead:

```
Cannot find grammar rule of expression 'greet'.
Line: <the_line_number>
Expression: greet "Hi" "Quandale"

Exit Code: 8 <GrammarError>
```

So how are we gonna call the methods? Let me show you;

`do greet "Hi" "Quandale"`

and with that you should get your desired output:

`Hi Quandale!`

### <br> Using Return Values As Parameters

It's the same as operators. The expression that is gonna return a value should be written as the last expression.

### <br> Recursion

SlimScript has recurson support but it's not recommended to  use it for long loops (around 700-800 recursion will cause a StackOverFlow). It's because interpreter will create a new unique CallStack for every call and CallStacks will only be cleaned after they finished their execution.

Prefer loops over recursion when possible because loops wont craete a new CallStack every time.

---------------------------------------------------
<br/>

## Loops

### <br> While

Well there's not much to explain. Basically the structure of a while loop is:

```
while <condition> begin
    -- Do something here
end
```

For the sake of giving an example, let's recreate the factorial finder function with a while loop.

```
func factorial num begin
    define fact as 1

    while != num 1 begin
        set fact to * fact num

        set num to - num 1
    end

    return fact
end

write do factorial 5

-- Output 
--      120
```

### <br> Foreach

`Foreach` loop is the same as other languages foreach loops just like `for` and `while` loops. The structure is simple:

```
forach <local_element_name> in <array> begin
    -- Code
end
```

Let's assume that you have an array of functions that's gonna be executed with an order. The code of that operation will be:

```
func hello begin write "Hello" end
func work begin write "Doing some work..." end     -- One-liner functions
func bye begin write "Work Done. Bye!" end

define funcs as [ hello, work, bye ]

foreach operation in funcs begin
    do operation
end

-- Output
--      Hello
--      Doing some work...
--      Work Done. Bye!
```

### <br> For

`For` loops are a bit weirder than other languages. Their structure looks like:

```
for <var_name> as <start_value> || <condition> || <action> begin
    -- Code
end
```

Seems a bit weird right? But don't worry I'm gonna explain it to you.

First of all, the parameters/statements for `for` loops are splitted via `||` symbol.

The first statement determines the variable that we'll be using inside loop,

the second statement is the condition

and the third statement is the action will be executed at the end of every loop.

Let's look at this example:

```
for i as 0 || < i 5 || 1 begin
    write i
end
```

Let's roleplay as the interpreter.

We're reading the statements for the loop. There are three of them. `[i as 0] [< i 5] [1]`.

At the first execution we create a variable named `i` assigned to `0`.

Then we check the condition `< i 5`, its true. So we execute `write i`

After that we add `1` to `i` and proceed with the next loop.

So at the end of the loop our output will be

```
0
1
2
3
4
```

---------------------------------------------------
<br/>

## Splitting Code To Files

Well, imagine you're embedding SlimScript to your project and not being able to share code across files. Imagine a file that contains all the scripts that your project is going to use. That would be a real mess.

Fortunately you can split your code to files and use them wherever you want!

Let's create a `module` that returns the given 24 hours based time converted to AM/PM.

```
-- time.ss

func toAmPm time begin
    define mult as / time 12

    if <= mult 1 then
        return time
    else
        return - time 12
    end
end
```

```
-- main.ss

@include time   -- Now this is the neat part

define amBased as do toAmPm 15

write amBased

-- Output
--      3
```

It's just like the many other languages' include/import systems.

To be more technical the main file after the pre-processor operations looks like this:

```
-- pre-processed main.ss

func toAmPm time begin
    define mult as / time 12

    if <= mult 1 then
        return time
    else
        return - time 12
    end
end

define amBased as do toAmPm 15

write amBased
```

The pre-processor just copies and pastes the modules code into the main file.

### <br> Prevent Including A Module Multiple Times

Let's think about a scenario where two module includes each other.

```
-- main.ss

@include module

write "Include"
```

```
-- module.ss

@include main

write "Infinite"
```

Pre-processor will try creating something like this:

```
@include module

write "Include"
write "Infinite"
write "Include"
write "Infinite"
write "Include"
write "Infinite"
write "Include"
write "Infinite"
write "Include"
write "Infinite"
.
.
.
.

```

And program will never be interpreted. So what can we do to prevent this kind of a situation?

In some languages there is something called a `header guard`. It look like this:

```
-- main.ss
@module main

@include module

write "Include"
```

```
-- module.ss
@module module

@include main

write "Infinite"
```

With `@module <name>` we tell the pre-processor that the module that it's currently processing is named `<name>`. So when another module tries to include the same module, pre-processor will say "Hold on, i've already processed that module" and skip it.

So the processed file will be:

```
write "Infinite"

write "Include"
```

The lines that start with `@` in your code are what we call a `pre-processor directive`. If you want to learn more about them, the next section is perfect for you. You can skip it if don't wan to.

### <br> Advenced PreProcessor Directives

If you're reading this, that means you're interested in pre-processor directives. Firstly for the beginners, what is a pre-processor directive?

Before you start executing your code, a process named `pre-processor` will run through your code and remove comments, include modules etc. A `pre-processor directive` is a directive that tells something to pre-processor and only to pre-processor.

Let's take a look at previous sections example code.

```
-- main.ss
@module main

@include module

write "Include"
```

We can see that `@module` and `@include` are clearly pre-processor directives. If you run your script with flag `-D` (SlimScript.exe main.ss -D) you'll see that interpreter creates a file named `post_lexer.sso` in the same directory as `main.ss`. Now open that file and you'll see something like this:

```
[<Standart>: write][<String>: "Infinite"]
[<Standart>: write][<String>: "Include"]
```

If you builded the exe in debug mode interpreter will also create a file named `post_process.sso`. It'll look like this:

```



 write "Infinite"


 write "Include"

```

(Those white-spaces are intentional)

You see that the comments and pre-processor directives are gone. Because "A `pre-processor directive` is a directive that tells something to pre-processor and `only` to pre-processor.". So comments and directives does not exist at runtime.

#### <b> LIST OF PRE-PROCESSOR DIRECTIVES </b>

-   if
-   elif
-   else
-   endif
-   define
-   undef
-   include
-   module

#### <br> <b> @include and @module </b>

You know what these two do. So im gonna skip them.

#### <br> <b> @define </b>

Usage: `@define <the_thing_you_wanna_define>`

It basically defines a name so you can use it with other directives. The thing it defiens is just a name, it's not complicated like C-like language macros.

#### <br> <b> @undef </b>

Usage: `@undef <defined_name>`

In case you don't want to define something.

#### <br> <b> @if @elif @else and @endif </b>

Usage: 
```
@if <defined_name>
@if not <defined_name>

@elif <defined_name>
@elif not <defined_name>

@else

@endif
```

Let's explain this with an example.

```
@define V2

@if not V2
    write "Version is 1"
@else
    write "Version is 2"
@endif

write "End Program"

-- Tabs are optional
```

Let's role-play as pre-processor.

We're reading the code line-by-line. First we saw `@define V2`. So we defined `V2` an stored it in memory.

Then we saw `@if not V2`. We checked whether `V2` is defined or not. Since we defined it above, `not V2` evaluates to `false`. So the directive `@if not V2` is not true.

We skipped the line `write "Version is 1"` since it's for the case where `V2` is not defined.

`@else` directive automatically evaluates to true.

`write "Version is 2"` will be present at runtime because it's in `@else` block.

`@endif` closes the `@if-@else` block.

`write "End Program"` is automatically included cause there is no reason to exclude it.

`-- Tabs are optional` is skipped cause its a comment line.

So if you run the script with flag `-D` the output file `post_lexer.sso` will look like this:

```
[<Standart>: write][<String>: "Version is 2"]
[<Standart>: write][<String>: "End Program"]
```

With that way you can procedurally include-exclude code blocks from executing.

### <br> NOTE

Keep in mind that pre-processor can't see normal if-else blocks. So if you write

```
define someBool as false

if someBool then
    @define somebool
end

@if somebool
write "Hello"
@endif
```

Output will always be `Hello` because pre-processor sees `@define somebool` and defines it no matter what value someBool has.

### <br> Rewriting @module with @if-@define

`@module` is just a shorthand for

```
@if not <module_name>
@define <module_name>
    -- Module Code
@endif
```

Yet `@module` is my personal recommendation because it's easier to use (and its performance is slightly faster than `@if-@define`).