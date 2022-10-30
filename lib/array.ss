@module array

@include system

define array_t as {System::Collections::ArrayList}
define enumerable as {System::Linq::Enumerable,System::Linq}

func array.addRange arr range begin
    set arr to array_t->new arr
    
    arr->AddRange range

    return variable->ClrToVar arr
end

func array.copy arr begin
    set arr to array_t->new arr

    set arr to arr->Clone

    return variable->ClrToVar arr
end

func array.reverse arr begin
    set arr to array_t->new arr

    arr->Reverse

    return variable->ClrToVar arr
end

func array.indexOf arr val begin
    set arr to array_t->new arr

    return variable->ClrToVar arr->IndexOf val
end

func array.lastIndexOf arr val begin
    set arr to array_t->new arr

    return variable->ClrToVar arr->LastIndexOf val
end

func array.length arr begin
    define tmp_Arr as array_t->new arr
    define len as variable->ClrToVar tmp_Arr:Count

    return len
end

func array.takeRange arr startIndex count begin
    set arr to array_t->new arr

    return variable->ClrToVar arr->GetRange startIndex count
end

func array.removeRange arr startIndex count begin
    set arr to array_t->new arr

    return variable->ClrToVar arr->RemoveRange startIndex count
end

func array.createRange startNum count begin
    return variable->ClrToVar enumerable->Range startNum count
end

func array.repeat val times begin
    return variable->ClrToVar enumerable->Repeat val times
end