#Building the publish service 
To build, copy the contents of this folder to an empty directory on a linux VM. 
If you are already on linux, no need to do anything. 

Then simply run `./dockerBuildPublish.sh` to build the docker image for publish from the latest master.

If you want to test the build before something is in master, 
you'll need to change dockerBuildPublish to clone a branch instead of master. The command for this is
`git clone --depth 1 -b my-branch https://github.com/BristechSRM/Publish.git $DIR/source`