#
# Chimera compiler - Project make file: 
# Copyright (C) 2013 Ariel Ortiz, ITESM CEM
#  
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#  
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#  
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.
#

all: chimera.exe chimeralib.dll

chimera.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs               
	mcs -out:chimera.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs Node.cs SpecificNodes.cs SemanticAnalyzer.cs Type.cs SemanticError.cs SymbolTable.cs FunctionTable.cs CLIGenerator.cs

chimeralib.dll: chimeralib.cs
	mcs /t:library chimeralib.cs

clean:
	rm chimera.exe
