<- [Docs Main Page](BASICS.md) | [Operators](Operators.md) ->

# Variables

## <br> Creating Variables

```
define <variable_name> as <value>
```

By this way you can create variables. Note that SlimScript is not a strong typed language so you can assign any value to any variable.

---------------------------------------------------
<br/>

## <br> Using Variables

```
define someNum as 15
    
write "Value of Some Num is " someNum

-- Output
--      Value of Some Num is 15
```

---------------------------------------------------
<br/>

## <br> Changing The Value of A Variable

```
set <variable_name> to <new_value>
```

As mentioned before SlimScript is not strong typed so variable types are dynamic. Check the code below

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

## <br> Deleting A Variable From Memory

Let's say you made a program that uses a lot of recursion. And let's say that you use lots of temporary variables inside the function. Wouldn't it be good if you could delete those temporary variables manually so the memory footprint becomes much lesser?

Here's a basic example

```
define someVal as "Some Value"

write someVal

delete someVal

write someVal

-- Output
--		Some Value
--		Variable named 'someVal' does not exists.
--      File: <file_name>
--      Line: <line_num>
--      Expression: write someVal
--
--		Exit Code: 14 <NullReferenceError>
```