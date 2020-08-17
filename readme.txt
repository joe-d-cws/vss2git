
VSS2Git will extract files from a Visual Source Safe repository and check
them in to a new Git repository, and preserve the version history.

It makes NO CHANGES to the VSS repository.

It will also OPTIONALLY generate commands to create remote repositories, but 
it will not run them.  You have to do that.

HOW TO USE:

1. Check everything in to VSS.

2. Run Analyze on the VSS repository.

3. Clean up the VSS repository.  

The program defines projects as the first directories in the tree structure
that have files in them.  So if there's a file in the root folder of the 
VSS repository, everything will be put in one giant project.

So make sure that the directory in VSS that you want to be the base
directory of each project is the first one in the heirarchy that has
a file in it, and that there are no files in any of the higher-level
directories.  You may need to create dummy files.

4. Install GIT from https://www.git-scm.com.

Run
  git config --global user.email "you@example.com"
  git config --global user.name "Your Name"

5. Run this program.

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

Remote base URL - OPTIONAL - Base url of remote git repo.

For example, if you have all your projects saved to a remote repo
at https://www.example.com/code/repo/path-to-project, the base url is
https://www.example.com/code/repo

The program will NOT push to a remote repo for you.  It WILL create the
commands to push and you can run them yourself if you choose.

If you're not going to push to a remote repo, leave this blank.

Remote base path - OPTIONAL - The base path in the remote repo.

For example, if all your project is in /home/username/git/path-to-project,
the base path is /home/username/git.

If you're not going to push to a remote repo, leave this blank.
               
Hit the DUMP button.

I --STRONGLY-- recommend using test mode first.

Wait.  It may take awhile.

The program makes NO CHANGES to the VSS repository.  If the results are not
to your liking, fix whatever the problem was and try it again.

6. OPTIONAL - Set up public keys for access to remote repo

The steps for this vary on who does your hosting.

I self-host, so it took me awhile to get this working.

You will need to use ssh-keygen to create a public key on your LOCAL system.

Add the public key to the ~/.ssh/authorized_keys file on the REMOTE system.


------------------------------------------------------------------------------

WHAT IT DOES:

1. Get a list of all versions for all non-deleted items in VSS.

2. Sort that list by the VSS version date, VSS file name, and VSS version
number.

3. Iterate through the list.  Extract each file version, and add or update
each individual file version to the new GIT repo. A .gitignore will be
created if it does not already exist.  It's the default Visual Studio 
.gitignore that's included in this project as an embedded resource.

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

5. The program will create all the new repositories under the path specified
by the "Extract path" setting.

6. Don't like how it structured things?  Delete it, fix things, and run it
again.

DIRECTORY STRUCTURE:

The directory structure in GIT will be the one present in VSS.  The
VSS working directories are ignored.

LINKS:

If you have any files shared between projects in the VSS store, then each
project will get its own copy of the file, and the link will be broken.

LABELS:

Not preserved.
