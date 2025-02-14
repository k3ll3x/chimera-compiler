/*
  Chimera compiler - Token class for the scanner.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/* 
 * Siegfried Paul Keller Schippner A01375356
 * José Javier Rodríguez Mota A01372812
 * Ana Paula Mejía Quiroz A01371880
 */

using System;

namespace Chimera4 {

    class Token {

        readonly string lexeme;

        readonly TokenCategory category;

        readonly int row;

        readonly int column;

        public string Lexeme { 
            get { return lexeme; }
        }

        public TokenCategory Category {
            get { return category; }          
        }

        public int Row {
            get { return row; }
        }

        public int Column {
            get { return column; }
        }

        public Token(string lexeme, 
                     TokenCategory category, 
                     int row, 
                     int column) {
            this.lexeme = lexeme;
            this.category = category;
            this.row = row;
            this.column = column;
        }

        public override string ToString() {
            return string.Format("{{{0}, \"{1}\", @({2}, {3})}}",
                                 category, lexeme, row, column);
        }
    }
}

