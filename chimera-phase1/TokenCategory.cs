/*
  Chimera compiler - Token categories for the scanner.
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

namespace Chimera1 {

    enum TokenCategory {
        CONST,
        VAR,
        PROG,
        END,
        STR,
        STR_LITERAL,
        PROC,
        AND,
        OR,
        XOR,
        ASSIGN,
        BOOL,
        EOF,
        NOT,
        FALSE,
        IDENTIFIER,
        IF,
        ELSEIF,
        ELSE,
        LOOP,
        FOR,
        IN,
        DO,
        RETURN,
        BOOLINEQ,
        INT,
        INT_LITERAL,
        LESS,
        MORE,
        LESSEQ,
        MOREEQ,
        DIV,
        REM,
        MUL,
        NEG,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        CURLYBRACKET_OPEN,
        CURLYBRACKET_CLOSE,
        SQUAREDBRACKET_OPEN,
        SQUAREDBRACKET_CLOSE,
        PLUS,
        PRINT,
        COMA,
        TWOPOINTS,
        SEMICOL,
        THEN,
        TRUE,
        ILLEGAL_CHAR,
        LIST,
        OF,
        BEGIN
    }
}

