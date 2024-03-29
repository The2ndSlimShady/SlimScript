@module string

@include system

define string_t as {System::String}
define sbuilder_t as {System::Text::StringBuilder}

define string.empty as ""

func string.length str begin
    define builder as sbuilder_t->new str
    return variable->ClrToVar builder:Length
end

func string.indexOf str val begin
    define builder as sbuilder_t->new str
    define clrStr as builder->ToString

    delete builder

    return variable->ClrToVar clrStr->IndexOf val
end

func string.lastIndexOf str val begin
    define builder as sbuilder_t->new str
    define clrStr as builder->ToString

    delete builder

    return variable->ClrToVar clrStr->LastIndexOf val
end

func string.subString str startIndex length begin
    define builder as sbuilder_t->new str
    define clrStr as builder->ToString

    delete builder

    return variable->ClrToVar clrStr->Substring startIndex length
end

func string.endsWith str ch begin
    if = 0 do string.length str then
        return false
    end

    define lengthOf_Str as do string.length str

    return = ch index - lengthOf_Str 1 of str
end

func string.startsWith str ch begin
    if = 0 do string.length str then
        return false
    end

    return = ch index 0 of str
end

func string.isEmpty str begin
    define firstBool as variable->ClrToVar string_t->IsNullOrEmpty str
    define secondBool as variable->ClrToVar string_t->IsNullOrWhiteSpace str

    return both firstBool secondBool
end

func string.concat arr begin
    define nigga as string_t->Concat arr
end

func string.join seperator arr begin
    define nigga as string_t->Join seperator arr
end

func string.toLower str begin
    define builder as sbuilder_t->new str
    define clrStr as builder->ToString

    delete builder

    return variable->ClrToVar clrStr->ToLower
end

func string.toUpper str begin
    define builder as sbuilder_t->new str
    define clrStr as builder->ToString

    delete builder

    return variable->ClrToVar clrStr->ToUpper
end