if test "$OS" = "Windows_NT"
then
    build/output/Publish.exe
else 
    mono build/output/Publish.exe
fi

