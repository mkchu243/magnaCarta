<h1>magnaCarta</h1>
http://grapefruitgames.com/2013/05/05/unitygitpt3/<br>
ALWAYS MERGE MASTER INTO YOUR BRANCH FIRST<br>
//commit your new feature (on your branch)<br>
git commit -m "my newest feature<br>
git push<br>
<br>
//get latest version of master<br>
git checkout master<br>
git pull<br>
<br>
//merge master into my branch<br>
git checkout myname_dev<br>
git merge master<br>
<resolve conflict><br>
run and make sure the game still works<br>
git commit<br>
<br>
do a pull request via github <br>

##useful commands<br>
cd path [change directory]<br>
ls [list contents of current directory] <br>
git status [show your status] <br>
git diff [show the differences (arrow keys or jk to move, q to quit)] <br>
git mergetool [merge using the mergetool]<br>

###switch branch <br>
git checkout branchname <br>

###merge myBranch into master <br>
//get latest version of master branch<br>
git checkout master <br>
git pull <br>
//merge the branches <br>
git checkout myBranch <br>
git merge master <br>

###//COMMIT changes to local repository<br>
//interactive add <br>
git add -i<br>
//update and add untracked<br>
git commit -m "message<br>
####OR<br>
git commit -a -m "message"<br>

###Review<br>
order:<br>
git commit (save changes to local repo)<br>
git pull (get and auto merge from remote repo)<br>
merge and run again <br>
git push (save local changes to remote repo)<br>
