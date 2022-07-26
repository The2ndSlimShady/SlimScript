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

#### Using Variables

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
- (!=) 		Not Equals
- (<) 		Lesser Than
- (>) 		Greater Than
- (<=) 	   Lesser Equals
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

### Using Return Values as Operands

Let's assume that you gotta use an operators return value as operand in your code. How are you gonna do it? It's simple but a bit tricky.

```
define five as 5
define four as 4
define three as 3

write - 5 - 4 3
```

Seems a bit frustating isn't it? Lets split the expression into parts. As mentioned before operators accepts just two operands, so the operands of the first '-' will be `5` and `- 4 3`

So our operation will be `- 5 1`.

### Note:

We mentioned that '`operators take everything at their right as their operands`' so if you're gonna use return values as operands, they always should be the right operand. For example;

If you write an expression like this: `define someNum as + + 5 1 2`

Interpreter will detect the first plus (left) and try to initialize the operands from remaining expression. So it'll look at right and see the other plus, and execute it. The second plus will take all the remaining tokens as operands but only use the first two of them so it'll use 5 and 1 but 2 will be invisible for the first plus.

We can write the final expression that interpreter sees like this: 

`define someNum as + 6`.

### Another Note About Numeric Operators:

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

### Important Note

All the variables in SlimScript are value types. So if you write something like this:

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

### Getting Values From Array

An array that you can't get the desired element would not be that much useful. 

```
define arr as [ 1, 2, 3, 4 ]

define i as 3   -- We want to get the 3rd element

write index i of arr

-- Output
--      4
```

### Adding Values To Array

Arrays in SlimScript are dynamic sized. So you can add as many items as you want without hesitating (but your memory might not be very happy about it).

```
define arr as [ 1, 2, 3, 4 ]

append 125 to arr

write arr

-- Output
--      [ 1, 2, 3, 4, 125 ]
```

### Mixed Sample

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

It may seem a bit frustating but hey, I'm here to help you understand.

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
elif >= 15 then
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

### Using Return Values As Parameters

It's the same as operators. The expression that is gonna return a value should be written as the last expression.

### Recursion

SlimScript has recurson support but it's not recommended to  use it for long loops (around 700-800 recursion will cause a StackOverFlow). It's because interpreter will create a new unique CallStack for every call and CallStacks will only be cleaned after they finished their execution.

Prefer loops over recursion when possible because loops wont craete a new CallStack every time.

---------------------------------------------------
<br/>

## Loops

### While

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

### Foreach

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