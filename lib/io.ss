@module io

@include system

define file_t as {System::IO::File}
define file_stream as {System::IO::FileStream}
define stream_writer as {System::IO::StreamWriter}

define fileMode.createNew as 0
define fileMode.openIfExists as 1

-- Returns {System::IO::FileStream} for passing to other file functions
func file.create path fileMode begin
    if variable->ClrToVar file_t->Exists path then
        if = fileMode fileMode.createNew then
            return file_t->Create path
        elif = fileMode fileMode.openIfExists then
            return file_t->Open path
        else
            error + "Cannot determine what to do with file " path
        end
    else
        return file_t->Create path
    end
end

func file.open path begin
    if variable->ClrToVar file_t->Exists path then
        return file_t->Open path FileMode::Open
    else
        error "Cannot open file on path" path
    end
end

func file.writeLine file str begin
    define writer as stream_writer->new file
    writer->WriteLine str
    writer->Flush
end