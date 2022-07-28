@module array

-- Passed
func array.addRange arr range begin
    foreach item in range begin
        append item to arr
    end
end

-- Passed
func array.copy arr begin
    define newCopied_arr as [ ]

    foreach item in arr begin
        append item to newCopied_arr
    end

    return newCopied_arr
end

-- Passed
func array.top arr begin
    define maxVal_inArr as index 0 of arr

    foreach item in arr begin
        if != "Number" typeof item then
            define errMsg_top as + "Cannot evaluate numeric comprasion on type " typeof item
            define errExpl_top as  + "array.top " tostring arr

            set errExpl_top to + errExpl_top " -> where item is "
            set errExpl_top to + errExpl_top item

            error errMsg_top errExpl_top
        end

        if > item maxVal_inArr then
            set maxVal_inArr to item
        end
    end

    return maxVal_inArr
end

-- Passed
func array.bottom arr begin
    define minVal_inArr as index 0 of arr

    foreach item in arr begin
        if != "Number" typeof item then
            define errMsg_top as + "Cannot evaluate numeric comprasion on type " typeof item
            define errExpl_top as  + "array.top " tostring arr

            set errExpl_top to + errExpl_top " -> where item is "
            set errExpl_top to + errExpl_top item

            error errMsg_top errExpl_top
        end

        if < item minVal_inArr then
            set minVal_inArr to item
        end
    end

    return minVal_inArr
end

-- Passed
func array.reverse arr begin
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
    return do array.removeRange arr 0 1
end

-- Passed
func array.head arr begin
    if = 0 do array.length arr then
        return [ ]
    end

    return index 0 of arr
end

-- Passed
func array.indexOf arr val begin
    for i as 0 || < i do array.length arr || 1 begin
        if = val index i of arr then
            return i
        end
    end

    return -1
end

-- Passed
func array.lastIndexOf arr val begin
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