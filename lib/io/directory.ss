@module io.directory

@include system

define directory_t as {System::IO::Directory}
define directory_info_t as {System::IO::DirectoryInfo}

-- Returns the directory info of path if exists, creates new otherwise.
func directory.openOrCreate path begin
    if do directory.exists path then
        return directory_info_t->new path
    else
        return directory_t->CreateDirectory path
    end
end

-- Deletes the empty directory at given path. If its not empty, an error is thrown.
func directory.deleteEmpty path begin
    if do directory.exists path then
        if not do directory.empty path then
            error "Cannot delete non-empty directory with directory.deleteEmpty" path
        else
            directory_t->Delete path
        end
    else
        error "Directory on given path does not exists" path
    end

    if do directory.exists path then
        error "Failed to delete directory" path
    end
end

-- Deletes the folder at given path recursively
func directory.delete path begin
    if do directory.exists path then
        directory_t->Delete path true
    else
        error "Directory on given path does not exists"
    end

    if do directory.exists path then
        error "Failed to delete directory" path
    end
end

-- Returns an array of names of children of the directory at given path
func directory.getDirectories path begin
    if do directory.empty path then
        return []
    else
        return variable->ClrToVar directory_t->GetDirectories path 
    end
end

-- Returns an array of names of files in the directory at given path
func directory.getFiles path begin
    if do directory.empty path then
        return []
    else
        return variable->ClrToVar directory_t->GetFiles path
    end
end

-- Returns true if directory exists
func directory.exists path begin
    return variable->ClrToVar directory_t->Exists path
end

-- Returns true if given directory is empty
func directory.empty path begin
    define dirs as directory_t->EnumerateDirectories path
    define files as directory_t->EnumerateFiles path

    foreach d in dirs begin 
        return false
    end
    foreach f in files begin 
        return false
    end

    return true
end