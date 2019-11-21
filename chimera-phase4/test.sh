#!/bin/bash
for i in ../chimeraPrograms/*
do
	./chimera.exe $i
	echo "Press Enter check next file..."
	read
done
