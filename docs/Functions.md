# Functions

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

## <br> Using Return Values As Parameters

It's the same as operators. The expression that is gonna return a value should be written as the last expression.

## <br> Recursion

SlimScript has recurson support but it's not recommended to  use it for long loops (around 700-800 recursion will cause a StackOverFlow). It's because interpreter will create a new unique CallStack for every call and CallStacks will only be cleaned after they finished their execution.

Prefer loops over recursion when possible because loops wont craete a new CallStack every time.