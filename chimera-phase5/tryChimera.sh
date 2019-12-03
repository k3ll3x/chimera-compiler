#!/bin/bash
make clean; make
./chimera.exe ../chimeraPrograms/$1.chimera $1.il > tmp.ast
#./chimera.exe ../chimeraPrograms/$1.chimera $1.il; ilasm $1.il; mono $1.exe
