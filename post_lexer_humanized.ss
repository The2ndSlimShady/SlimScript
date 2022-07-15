func hello begin write "Hello" end
func work begin write "Doing some work..." end
func bye begin write "Work Done. Bye!" end
define funcs as [ hello, work, bye ]
foreach operation in funcs begin
do operation
end
