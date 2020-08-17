
HOW TO USE:

1. Check everything in to VSS.

2. Create a repository if you don't have one already.

3. Run the program.

4. Fill in the fields:

SS Database - the ss.ini file for the VSS database.
              It will try to pull the last one used from the registry.
              
SS User - VSS User name.  Defaults to the current windows user name.

SS Password - VSS Password.

SS Root - Root VSS path to extract.  Default is $/ for the whole thing,
          but you can do a single project or tree if you want.
          
Extract path - A temporary directory to extract the files to.  It will be created if
               necessary.
               
SCM Create - Command line to create the new SCM.  {0} represents the
             extract path.
           
SCM Open - Command line to open or initialize the new SCM.  {0} represents the
           extract path.
           
Add new file - Command to add a new file to the new SCM.                                    

Update existing file - Command to update new version in the new SCM.

Commit all changes - Command to commit current changes.

SCM Close - Command to close the SCM.

NOTE: If a command is not necessary, leave it blank.

5. Hit the DUMP button.

6. Wait.  It may take awhile.


COMMANDS FOR GIT:
Create: none - leave blank

Open: git init

Add: git add "{0}"

Update: git add -u "{0}"

Commit: git commit -a

Close: none - leave blank

WHAT IT DOES:

1. Get a list of all versions for all non-deleted items in VSS.

2. Sort that list by the version date, file name in VSS, and version number.

3. Iterate through the list.  Extract each file version, and add or update
each individual file version to the new SCM store.

4. When the version date changes, commit all the updates.

NOTE: When you check in multiple files in VSS, it sets the version date for 
each individual item at the time of checkin, instead of setting them all
to the same date.  If there are a lot of items, or the checkin takes a long
time, the timestamps on the files will not be the same, and there could be
a gap as long as 10 seconds between items, even if they are part of the same
checkin.

What this program does is wait until the gap between consecutive items is more
than 10 seconds, then it does a full commit.

DIRECTORY STRUCTURE:

The directory structure in the new SCM will be the one present in VSS.  The
VSS working directories are ignored.

LINKS:

If you have any files shared between projects in the VSS store, then each
project will get its own copy of the file, and the link will be broken.

LABELS:

Not preserved.
