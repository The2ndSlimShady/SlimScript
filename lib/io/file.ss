@module io.file

@include system

define file_t as {System::IO::File}
define file_mode_t as {System::IO::FileMode}
define file_access_t as {System::IO::FileAccess}
define file_stream_t as {System::IO::FileStream}
define stream_writer_t as {System::IO::StreamWriter}
define stream_reader_t as {System::IO::StreamReader}

define fileAccess.read as "Read"
define fileAccess.write as "Write"
define fileAccess.readWrite as "ReadWrite"

-- Opens a file if exists, creates new otherwise. If appendTo == true, open file in append mode.
-- Does not overwrites the file
func file.openOrCreate path accessMode appendTo begin
    if both appendTo != accessMode fileAccess.write then
        error "Cannot open file in append mode. Append mode only works with fileAccess.write."
    end

    if appendTo then
        define fileMode as enum_t->Parse file_mode_t "Append"
        return file_stream_t->new path fileMode
    end

    define fileMode as enum_t->Parse file_mode_t "Open"
    define fileAccess as enum_t->Parse file_access_t accessMode

    return file_stream_t->new path fileMode fileAccess
end

-- Creates a file or overwrites a file if already exists.
func file.create path accessMode begin
    define fileAccess as enum_t->Parse file_access_t accessMode
    define fileMode as enum_t->Parse file_mode_t "Create"

    return file_stream_t->new path fileMode fileAccess
end

-- Writes all changes to file and closes the stream
func file.close file begin
    file->Flush
    file->Close
end

-- Creates a reader to read the file from the beginning
func file.createReader file begin
    define seek_origin_t as {System::IO::SeekOrigin}
    define origin as enum_t->Parse seek_origin_t "Begin"
    file->Seek 0 origin
    return stream_reader_t->new file
end

-- Creates a writer to write to given file
func file.createWriter file begin
    return stream_writer_t->new file
end

-- Reads a line from given reader
func file.readLine reader begin
    define result as variable->ClrToVar reader->ReadLine

    if = result null then
        return "EOF"
    else
        return result
    end
end

-- Writes a line to given writer
func file.writeLine writer str noFlush begin
    writer->WriteLine str

    if not noFlush then 
        writer->Flush
    end
end

-- Checks if file on given path exists
func file.exists path begin
    return variable->ClrToVar file_t->Exists path
end

-- Reads all lines from file on given path and returns the lines as an array
func file.readAllLines path begin
    return variable->ClrToVar file_t->ReadAllLines path
end

-- Writes given array as lines to file at given path
func file.writeAllLines path lines begin
    file_t->WriteAllLines path variable->VarToClr lines
end