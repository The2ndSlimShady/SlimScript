<- [Loops](Loops.md) | [Docs Main Page](BASICS.md) | [Pre-Processor Directives](PreProcessor.md) ->

# Splitting Code To Files

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

## <br> Prevent Including A Module Multiple Times

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