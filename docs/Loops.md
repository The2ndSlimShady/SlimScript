# Loops

## <br> While

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

## <br> Foreach

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

## <br> For

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