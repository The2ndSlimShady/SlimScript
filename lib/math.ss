@module math

@include system

define math_t as {System::Math}

define math.E as 2.71828182845905
define math.PI as 3.14159265358979

func math.absolute num begin
    if != "Number" typeof num then
        define errMsg as + "Cannot get absolute value of type " typeof num
        define errData as + "math.absolute " typeof num
        error errMsg errData
    end

    return variable->ClrToVar math_t->Abs num
end

func math.mod dividend divisor begin
    set divisor to variable->ClrToVar convert->ToInt32 divisor
    set dividend to variable->ClrToVar convert->ToInt32 dividend

    define valTuple as math_t->DivRem dividend divisor

    return variable->ClrToVar valTuple:Item1
end

func math.max num1 num2 begin
    if < num1 num2 then
        return num2
    else
        return num1
    end
end

func math.min num1 num2 begin
    if < num1 num2 then
        return num1
    else
        return num2
    end
end

func math.root base root begin
    if < base 0 then
        return base
    end

    return ^ base / 1 root
end

func math.sqrt base begin
    return do math.root base 2
end

func math.fact num begin
    define resultofFact as 1

    for i as num || > i 1 || -1 begin
        set resultofFact to * resultofFact i
    end

    return resultofFact
end

func math.floor num begin
    return variable->ClrToVar math_t->Floor num
end

func math.ceiling num begin
    return variable->ClrToVar math_t->Ceiling num
end

func math.sin angle begin
    return variable->ClrToVar math_t->Sin angle
end

func math.tan angle begin
    return variable->ClrToVar math_t->Tan angle
end

func math.cos angle begin
    return variable->ClrToVar math_t->Cos angle
end

func math.round num begin
    return variable->ClrToVar math_t->Round num
end

func math.truncate num begin
    return variable->ClrToVar math_t->Truncate num
end