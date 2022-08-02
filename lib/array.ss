@module array

@include system

define array_t as {System::Collections::ArrayList}

-- Passed
func array.addRange arr range begin

    -- TODO

    set arr to array_t->new arr
end

-- Passed
func array.copy arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.copy"

        define errExpl as + "array.copy arr:" arr

        error errMsg errExpl
    end

    define newCopied_arr as [ ]

    foreach item in arr begin
        append item to newCopied_arr
    end

    return newCopied_arr
end

-- Passed
func array.top arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.top"

        define errExpl as + "array.top " + "arr:" tostring arr

        error errMsg errExpl
    end

    define maxVal_inArr as index 0 of arr

    foreach item in arr begin
        if != "Number" typeof item then
            define errMsg as + "Cannot evaluate numeric comprasion on type item:" typeof item

            define errExpl as  + "array.top arr:" tostring arr
            set errExpl to + errExpl " -> where item is "
            set errExpl to + errExpl tostring item

            error errMsg errExpl
        end

        if > item maxVal_inArr then
            set maxVal_inArr to item
        end
    end

    return maxVal_inArr
end

-- Passed
func array.bottom arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.bottom"

        define errExpl as + "array.bottom " + "arr:" tostring arr

        error errMsg errExpl
    end

    define minVal_inArr as index 0 of arr

    foreach item in arr begin
        if != "Number" typeof item then
            define errMsg as + "Cannot evaluate numeric comprasion on type " typeof item
            define errExpl as  + "array.bottom " tostring arr

            set errExpl to + errExpl " -> where item is "
            set errExpl to + errExpl item

            error errMsg errExpl
        end

        if < item minVal_inArr then
            set minVal_inArr to item
        end
    end

    return minVal_inArr
end

-- Passed
func array.reverse arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.reverse"

        define errExpl as + "array.reverse " + "arr:" tostring arr

        error errMsg errExpl
    end

    define tempArr_forReverse as [ ]

    define lengthOf_Arr as do array.length arr

    foreach item in arr begin
        append index - lengthOf_Arr 1 of arr to tempArr_forReverse

        set lengthOf_Arr to - lengthOf_Arr 1
    end

    set arr to tempArr_forReverse
    delete tempArr_forReverse

    return arr
end

-- Passed
func array.tail arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.tail"

        define errExpl as + "array.tail " + "arr:" tostring arr

        error errMsg errExpl
    end

    return do array.removeRange arr 0 1
end

-- Passed
func array.head arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.head"

        define errExpl as + "array.head " + "arr:" tostring arr

        error errMsg errExpl
    end

    if = 0 do array.length arr then
        return [ ]
    end

    return index 0 of arr
end

-- Passed
func array.indexOf arr val begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.indexOf"

        define errExpl as + "array.indexOf " + "arr:" tostring + arr + " val:" tostring val

        error errMsg errExpl
    end

    for i as 0 || < i do array.length arr || 1 begin
        if = val index i of arr then
            return i
        end
    end

    return -1
end

-- Passed
func array.lastIndexOf arr val begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.lastIndexOf"

        define errExpl as + "array.lastIndexOf " + "arr:" tostring + arr + " val:" tostring val

        error errMsg errExpl
    end

    define lengthOf_Arr as do array.length arr
    define tmpiFor_for as - lengthOf_Arr 1

    for i as tmpiFor_for || >= i 0 || -1 begin
        if = val index i of arr then
            return i
        end
    end

    return -1
end

-- Passed
func array.length arr begin
    if != "Array" typeof arr then
        define errMsg as + "Cannot use type arr:" typeof arr
        set errMsg to + errMsg " as parameter for array.length"

        define errExpl as + "array.length " + "arr:" tostring arr

        error errMsg errExpl
    end

    define lengthOf_Arr as 0

    foreach item in arr begin
        set lengthOf_Arr to + lengthOf_Arr 1
    end

    return lengthOf_Arr
end

-- Passed
func array.takeRange arr startIndex endIndex begin
    if = 0 do array.length arr then
        return [ ]
    end

    define tmpArr_forTakeRange as [ ]

    for i as startIndex || < i endIndex || 1 begin
        append index i of arr to tmpArr_forTakeRange
    end

    return tmpArr_forTakeRange
end

-- Passed
func array.removeRange arr startIndex endIndex begin
    if = 0 do array.length arr then
        return [ ]
    end

    define tmpArr_forRemoveRange as [ ]

    for i as 0 || < i do array.length arr || 1 begin
        define tmp_lessRemove as < i startIndex
        define tmp_grRemove as >= i endIndex

        define tmp_anyRemove as any tmp_lessRemove tmp_grRemove

        delete tmp_lessRemove
        delete tmp_grRemove

        if tmp_anyRemove then
            append index i of arr to tmpArr_forRemoveRange
        end
    end

    return tmpArr_forRemoveRange
end

-- Passed
func array.createRange startNum endNum begin
    define tmpArr_toRange as [ ]

    if < startNum endNum then
        for i as startNum || < i endNum || 1 begin
            append i to tmpArr_toRange
        end
    else
        for i as startNum || > i endNum || -1 begin
            append i to tmpArr_toRange
        end
    end

    return tmpArr_toRange
end

-- Passed
func array.repeat val times begin
    define tmpArr_toRepeat as [ ]

    for i as 0 || < i times || 1 begin
        append val to tmpArr_toRepeat
    end

    return tmpArr_toRepeat
end