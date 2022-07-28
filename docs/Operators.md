<- [Variables](Variables.md) | [Docs Main Page](BASICS.md) | [Arrays](Arrays.md) ->

# Operators

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

## <br> Using Return Values as Operands

Let's assume that you gotta use an operators return value as operand in your code. How are you gonna do it? It's simple but a bit tricky.

```
define five as 5
define four as 4
define three as 3

write - 5 - 4 3
```

Seems a bit intimidating isn't it? Lets split the expression into parts. As mentioned before operators accepts just two operands, so the operands of the first '-' will be `5` and `- 4 3`

So our operation will be `- 5 1`.

## <br> Note:

We mentioned that '`operators take everything at their right as their operands`' so if you're gonna use return values as operands, they always should be the right operand. For example;

If you write an expression like this: `define someNum as + + 5 1 2`

Interpreter will detect the first plus (left) and try to initialize the operands from remaining expression. So it'll look at right and see the other plus, and execute it. The second plus will take all the remaining tokens as operands but only use the first two of them so it'll use 5 and 1 but 2 will be invisible for the first plus.

We can write the final expression that interpreter sees like this: 

`define someNum as + 6`.

## <br> Another Note About Numeric Operators:

Actually there is no operator precedence in SlimScript. Everything will be executed the way you write it. You want the addition to be executed before multiplication, go on, you have the right to do whatever you want.

```
-- Expression (2 * 2 + 1)'s result will be '5' in other --  -- languages.


-- But in SlimScript (* 2 + 2 1) will be '6'

write * 2 + 2 1

-- Output
--      6
```