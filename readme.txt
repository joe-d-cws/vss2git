
HOW TO USE:

Check everything in to VSS.

Run Analyze on the VSS repository.

Clean up the VSS repository.  The program defines projects as the first
directories in the tree structure that have files in them.  So if there's
a file in the root folder of the VSS repository, everything will be put in
one giant project.

Install GIT from https://www.git-scm.com.

Run
  git config --global user.email "you@example.com"
  git config --global user.name "Your Name"

Run the program.

Fill in the fields:

SS Database - the ss.ini file for the VSS database.
              It will try to pull the last one used from the registry.
              
SS User - VSS User name.  Defaults to the current windows user name.

SS Password - VSS Password.

SS Root - Root VSS path to extract.  Default is $/ for the whole thing,
          but you can do a single project or tree if you want.
          
Extract path - A temporary directory to extract the files to.  It will be
               created if necessary.  Default is %TEMP%\VSS2Git

Log file - Saves all output.  Default is ExtractPath\VSS2Git.log

Report file - Report of all the file versions from VSS. Of dubious utility,
              but what the hell.  Default is ExtractPath\VSS2Git.html

Remote base URL - 

Remote base path - 
               
Hit the DUMP button.

I --STRONGLY-- recommend using test mode first.

Wait.  It may take awhile.

The program makes NO CHANGES to the VSS repository.  If the results are not
to your liking, fix whatever the problem was and try it again.

WHAT IT DOES:

1. Get a list of all versions for all non-deleted items in VSS.

2. Sort that list by the VSS version date, VSS file name, and VSS version
number.

3. Iterate through the list.  Extract each file version, and add or update
each individual file version to the new GIT repo.

4. When the version date or the project name changes, commit all the updates.

The program sets the project base directory as the first directory that has
actual files in it.

NOTE: When you check in multiple files in VSS, it sets the version date for 
each individual item at the time of checkin, instead of setting them all
to the same date.  If there are a lot of items, or the checkin takes a long
time, the timestamps on the files will not be the same, and there could be
a gap as long as 10 seconds between items, even if they are part of the same
checkin.

What this program does is wait until the gap between consecutive items is more
than 10 seconds, then it does a full commit.

DIRECTORY STRUCTURE:

The directory structure in GIT will be the one present in VSS.  Sort of.  The
VSS working directories are ignored.

LINKS:

If you have any files shared between projects in the VSS store, then each
project will get its own copy of the file, and the link will be broken.

LABELS:

Not preserved.
