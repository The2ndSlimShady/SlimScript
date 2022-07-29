# Slim Script Basics

## Jump To

- [Variables](Variables.md)
- [Operators](Operators.md)
- [Arrays](Arrays.md)
- [If-Else](IfElse.md)
- [Functions](Functions.md)
- [Loops](Loops.md)
  - [While](Loops.md#br-while)
  - [Foreach](Loops.md#br-foreach)
  - [For](Loops.md#br-for)
- [Using Multiple Files](Include.md)
- [Pre-Processor Directives](PreProcessor.md)
  - [@include and @module](PreProcessor.md#br-b-include-and-module-b)
  - [@define](PreProcessor.md#br-b-define-b)
  - [@undef](PreProcessor.md#br-b-undef-b)
  - [@if-elif-else-endif](PreProcessor.md#br-b-if-elif-else-and-endif-b)
- [Debugging]

<br/>

---------------------------------------------------
<br/>

## Sample Program

The program below uses recursion to mimic a for loop and find factorials of numbers.

```
-- This is a comment

-- A function named factorial with one parameter named num
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