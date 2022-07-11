func execute_func function param begin
    do function param
end

func func_to_execute param begin
    write param
end

-- Passing function as parameter and then executing it
do execute_func func_to_execute "Yusuf"
