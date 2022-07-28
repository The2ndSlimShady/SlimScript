<- [Using Multiple Files](Include.md) | [Docs Main Page](BASICS.md)

# Advenced PreProcessor Directives

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

##  LIST OF PRE-PROCESSOR DIRECTIVES

-   if
-   elif
-   else
-   endif
-   define
-   undef
-   include
-   module

### <br> <b> @include and @module </b>

You know what these two do. So im gonna skip them.

### <br> <b> @define </b>

Usage: `@define <the_thing_you_wanna_define>`

It basically defines a name so you can use it with other directives. The thing it defiens is just a name, it's not complicated like C-like language macros.

### <br> <b> @undef </b>

Usage: `@undef <defined_name>`

In case you don't want to define something.

### <br> <b> @if @elif @else and @endif </b>

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

## <br> NOTE

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

## <br> Rewriting @module with @if-@define

`@module` is just a shorthand for

```
@if not <module_name>
@define <module_name>
    -- Module Code
@endif
```

Yet `@module` is my personal recommendation because it's easier to use (and its performance is slightly better than `@if-@define`).