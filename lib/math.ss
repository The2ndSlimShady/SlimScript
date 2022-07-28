@module math

define math.E as 2.71828182845905
define math.PI as 3.14159265358979

func math.absolute num begin
    if != "Number" typeof num then
        define errMsg as + "Cannot get absolute value of type " typeof num
        define errData as + "math.absolute " typeof num
        error errMsg errData
    end

    if > 0 num then
        return * -1 num
    else
        return num
    end
end

func math.divRem dividend divisor begin
    if != "Number" typeof num then
        define errMsg as + "Cannot get floor value of type " typeof num
        define errData as + "math.floor " typeof num
        error errMsg errData
    end

    define isNegative as false

    if < dividend 0 then
        set dividend to - dividend
        set isNegative to true
    end

    while >= dividend divisor begin
        set dividend to - dividend divisor
    end

    if isNegative then
        set dividend to - dividend
    end

    return dividend
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