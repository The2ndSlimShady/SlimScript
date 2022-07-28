<- [Operators](Operators.md) | [Docs Main Page](BASICS.md) | [If-Else](IfElse.md) ->

# Arrays

Arrays in SlimScript are non-generic and can hold any value type including functions and arrays. To create array you can use `define` standart method but with a little difference and restriction.

Let's create an array holding a number, a bool, a string and a function.

```
func someFunc begin
    write "Hello"
end

define arr as [ 12, false, "Test String", someFunc ]      
-- There should be at least one  space between braces and arguments
```

The space between arguments and commas are optional whereas the space between arguments and braces are a must.

Now lets loop through our array and write the elements to console.

```
func someFunc begin
    write "Hello"
end

define arr as [ 12, false, "Test String", someFunc ]      
-- There should be at least one  space between braces and arguments

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

## <br> Important Note

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

## <br> With More Detail

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

## <br> Getting Values From Array

An array that you can't get the desired element would not be that much useful. Keep in mind that indexes start from `0` not `1`!

```
define arr as [ 1, 2, 3, 4 ]

define i as 3   -- We want to get the 4th element

write index i of arr

-- Output
--      4
```

## <br> Adding Values To Array

Arrays in SlimScript are dynamic sized. So you can add as many items as you want without hesitating (but your memory might not be very happy about it).

```
define arr as [ 1, 2, 3, 4 ]

append 125 to arr

write arr

-- Output
--      [ 1, 2, 3, 4, 125 ]
```

## <br> Removing Values From Array

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

## <br> Mixed Sample

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