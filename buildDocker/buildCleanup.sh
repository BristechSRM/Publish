#!/bin/sh -eu
DIR=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
echo $DIR

sudo rm -rf $DIR/source
sudo rm -rf $DIR/binaries
sudo rm -rf $DIR/context
