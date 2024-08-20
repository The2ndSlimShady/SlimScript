@module math

@include system

define math_t as {System::Math}

define math.e as 2.71828182845905
define math.pi as 3.14159265358979

func math.absolute num begin
    return variable->ClrToVar math_t->Abs num
end

func math.mod dividend divisor begin
    set divisor to math_t->Floor divisor
    set dividend to math_t->Floor dividend

    define valTuple as math_t->DivRem dividend divisor

    return variable->ClrToVar valTuple:Item2
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

func math.sin rads begin
    return variable->ClrToVar math_t->Sin rads
end

func math.tan rads begin
    return variable->ClrToVar math_t->Tan rads
end

func math.cos rads begin
    return variable->ClrToVar math_t->Cos rads
end

func math.round num begin
    return variable->ClrToVar math_t->Round num
end

func math.truncate num begin
    return variable->ClrToVar math_t->Truncate num
end