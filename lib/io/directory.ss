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

-- Creates and returns the DirectoryInfo of given path. If it already exists an error is thrown.
func directory.create path begin
    if do directory.exists path then
        error "Cannot create directory at given path. It already exists." path
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

-- Moves a directory and all of it's components to given path
func directory.move source target begin
    if not do directory.exists source then
        error "Cannot move given directory. It does not exists." source
    elif do directory.exists target then
        error "Cannot move given directory to target. A directory with the same name already exists." target
    else
        directory_t->Move source target  
    end
end

-- Sets the executing directory of program
func directory.setCurrentDirectory path begin
    if not do directory.exists path then
        error "Cannot change current dir to given dir. It doesn't exists" path
    else
        directory_t->SetCurrentDirectory path
    end
end

-- Returns the executing directory of program
func directory.getCurrentDirectory begin
    return directory_t->GetCurrentDirectory
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