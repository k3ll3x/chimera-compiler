#!/bin/bash
mcs $1.cs
monodis $1.exe > $1.il
