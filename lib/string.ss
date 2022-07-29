@module string

-- Passed
func string.length str begin
    define lengthOf_Str as 0

    foreach ch in str begin
        set lengthOf_Str to + lengthOf_Str 1
    end

    return lengthOf_Str
end

-- Passed
func string.indexOf str val begin
    for i as 0 || < i do string.length str || 1 begin
        if = val index i of str then
            return i
        end
    end

    return -1
end

-- Passed
func string.lastIndexOf str val begin
    define lengthOf_Str as do string.length str
    define tmpiFor_for as - lengthOf_Str 1

    for i as tmpiFor_for || >= i 0 || -1 begin
        if = val index i of str then
            return i
        end
    end

    return -1
end

-- Passed
func string.subString str startIndex endIndex begin
    define tmpArr_toSubStr as [ ]

    for i as 0 || < i do string.length str || 1 begin
        define greaterThanStr as >= i startIndex
        define lesserThanStr as < i endIndex

        if both greaterThanStr lesserThanStr then
            append index i of str to tmpArr_toSubStr
        end
    end

    return do string.concat tmpArr_toSubStr
end

-- Passed
func string.endsWith str ch begin
    if = 0 do string.length str then
        return false
    end

    define lengthOf_Str as do string.length str

    return = ch index - lengthOf_Str 1 of str
end

-- Passed
func string.startsWith str ch begin
    if = 0 do string.length str then
        return false
    end

    return = ch index 0 of str
end

-- Passed
func string.isEmpty str begin
    if = 0 do string.length str then
        return true
    end

    foreach ch in str begin
        if != ch " " then
            return false
        end
    end

    return true
end

-- Passed
func string.concat arr begin
    define tmpStrForConcat as ""

    foreach item in arr begin
        set tmpStrForConcat to + tmpStrForConcat tostring item
    end

    write tmpStrForConcat
    return tmpStrForConcat
end