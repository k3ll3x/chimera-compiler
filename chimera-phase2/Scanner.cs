/*
  Chimera compiler - This class performs the lexical analysis, 
  (a.k.a. scanning).
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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Chimera {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"                          
                (?<Assign>      :=      )
              | (?<BoolIneq>   [<][>]   )
              | (?<LongComment> [/][*](.|\n)*?[*][/]    )
              | (?<Comment>     [/][/](.)*?$    )
              | (?<String>     [""]([^""\n]|""{2})*[""]        )
              | (?<Identifier> [a-zA-Z]+([_]|[0-9])*    )
              | (?<IntLiteral> \d+       )
              | (?<Less>       [<]       )
              | (?<LessEq>     [<][=]    )
              | (?<More>       [>]       )
              | (?<MoreEq>     [>][=]    )
              | (?<Mul>        [*]       )
              | (?<Equal>        [=]       )
              | (?<Neg>        [-]       )
              | (?<Newline>    \n        )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<CurLeft>    [{]       )
              | (?<CurRight>   [}]       )
              | (?<SquLeft>     \[       )
              | (?<SquRight>    \]       )
              | (?<Plus>       [+]       )
              | (?<Coma>       [,]       )
              | (?<SemiCol>    [;]       )
              | (?<TwoPoints>  [:]       )
              | (?<WhiteSpace> \s        )     # Must go anywhere after Newline.
              | (?<Other>      .         )     # Must be last: match any other character.
            ", 
            RegexOptions.IgnorePatternWhitespace 
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"const", TokenCategory.CONST},
                {"var", TokenCategory.VAR},
                {"program", TokenCategory.PROG},
                {"integer", TokenCategory.INT},
                {"string", TokenCategory.STR},
                {"boolean", TokenCategory.BOOL},
                {"and", TokenCategory.AND},
                {"procedure", TokenCategory.PROC},
                {"or", TokenCategory.OR},
                {"xor", TokenCategory.XOR},
                {"true", TokenCategory.TRUE},
                {"false", TokenCategory.FALSE},
                {"end", TokenCategory.END},
                {"exit", TokenCategory.EXIT},
                {"if", TokenCategory.IF},
                {"elseif", TokenCategory.ELSEIF},
                {"else", TokenCategory.ELSE},
                {"loop", TokenCategory.LOOP},
                {"for", TokenCategory.FOR},
                {"in", TokenCategory.IN},
                {"do", TokenCategory.DO},
                {"return", TokenCategory.RETURN},
                {"print", TokenCategory.PRINT},
                {"then", TokenCategory.THEN},
                {"div", TokenCategory.DIV},
                {"rem", TokenCategory.REM},
                {"not", TokenCategory.NOT},
                {"list",TokenCategory.LIST},
                {"of", TokenCategory.OF},
                {"begin",TokenCategory.BEGIN}
            };

        static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"Assign", TokenCategory.ASSIGN},
                {"BoolIneq", TokenCategory.BOOLINEQ},
                {"Less", TokenCategory.LESS},
                {"LessEq", TokenCategory.LESSEQ},
                {"More", TokenCategory.MORE},
                {"MoreEq", TokenCategory.MOREEQ},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Mul", TokenCategory.MUL},
                {"Equal", TokenCategory.EQUAL},
                {"Neg", TokenCategory.NEG},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"CurLeft", TokenCategory.CURLYBRACKET_OPEN},
                {"CurRight", TokenCategory.CURLYBRACKET_CLOSE},
                {"SquRight", TokenCategory.SQUAREDBRACKET_CLOSE},
                {"SquLeft", TokenCategory.SQUAREDBRACKET_OPEN},
                {"Plus", TokenCategory.PLUS},
                {"Coma", TokenCategory.COMA},
                {"SemiCol", TokenCategory.SEMICOL},
                {"TwoPoints", TokenCategory.TWOPOINTS},
            };

        public Scanner(string input) {
            this.input = input;
        }

        public IEnumerable<Token> Start() {

            var row = 1;
            var columnStart = 0;

            Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);

            foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Success) {

                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;

                } else if (m.Groups["WhiteSpace"].Success 
                    || m.Groups["Comment"].Success) {

                    // Skip white space and comments.

                } else if (m.Groups["LongComment"].Success) {
                    try{
                        row+= m.Value.Split('\n').Length - 1; 
                    }catch{
                        
                    }
                } else if (m.Groups["Identifier"].Success) {

                    if (keywords.ContainsKey(m.Value)) {

                        // Matched string is a Chimera keyword.
                        yield return newTok(m, keywords[m.Value]);                                               

                    } else { 

                        // Otherwise it's just a plain identifier.
                        yield return newTok(m, TokenCategory.IDENTIFIER);
                    }

                } else if(m.Groups["String"].Success) {
                    yield return newTok(m,TokenCategory.STR_LITERAL);

                }else if (m.Groups["Other"].Success) {

                    // Found an illegal character.
                    yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                    // Match must be one of the non keywords.
                    foreach (var name in nonKeywords.Keys) {
                        if (m.Groups[name].Success) {
                            yield return newTok(m, nonKeywords[name]);
                            break;
                        }
                    }
                }
            }

            yield return new Token(null, 
                                   TokenCategory.EOF, 
                                   row, 
                                   input.Length - columnStart + 1);
        }
    }
}
