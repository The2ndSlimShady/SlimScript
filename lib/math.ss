@module math

define math.E as 2.71828182845905
define math.PI as 3.14159265358979

-- Passed
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
    define divNum as != "Number" typeof dividend
    define divsNum as != "Number" typeof divisor

    if any divNum divsNum then
        define errMsg as + "Cannot get division remainder of types " typeof dividend
        set errMsg to + errMsg + " and " typeof divisor

        define errData as + "math.floor " typeof dividend
        set errData to + errData + " " typeof divisor  
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

-- Passed
func math.max num1 num2 begin
    if < num1 num2 then
        return num2
    else
        return num1
    end
end

-- Passed
func math.min num1 num2 begin
    if < num1 num2 then
        return num1
    else
        return num2
    end
end

-- Passed
func math.root base root begin
    if < base 0 then
        return base
    end

    return ^ base / 1 root
end

-- Passed
func math.sqrt base begin
    return do math.root base 2
end

-- Passed
func math.fact num begin
    define resultofFact as 1

    for i as num || > i 1 || -1 begin
        set resultofFact to * resultofFact i
    end

    return resultofFact
end