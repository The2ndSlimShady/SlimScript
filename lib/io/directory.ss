@module io.directory

@include system

define directory_t as {System::IO::Directory}
define directory_info_t as {System::IO::DirectoryInfo}

-- Returns the directory info of path if exists, creates new otherwise.
func directory.openOrCreate path begin
    
end