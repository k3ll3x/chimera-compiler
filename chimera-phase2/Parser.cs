/*
  Chimera compiler - This class performs the syntactic analysis,
  (a.k.a. parsing).
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

namespace Chimera {

    class Parser {      

        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                TokenCategory.CONST,
                TokenCategory.VAR,
                TokenCategory.PROC
            };

        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.IF, 
                TokenCategory.LOOP,
            };

        static readonly ISet<TokenCategory> firstOfOperator =
            new HashSet<TokenCategory>() {
                TokenCategory.AND,
                TokenCategory.LESS,
                TokenCategory.PLUS,
                TokenCategory.MUL,
                TokenCategory.OR,
                TokenCategory.DIV,
                TokenCategory.XOR,
                TokenCategory.LESSEQ,
                TokenCategory.MORE,
                TokenCategory.MOREEQ,
                TokenCategory.REM,
                TokenCategory.NOT,
                TokenCategory.NEG,

            };

        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.INT_LITERAL,
                TokenCategory.STR_LITERAL,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.PARENTHESIS_OPEN,
                TokenCategory.NEG
            };
        static readonly ISet<TokenCategory> nextOfIdentifier = 
            new HashSet<TokenCategory>() {
                TokenCategory.PARENTHESIS_OPEN,
                TokenCategory.ASSIGN,
            };
        static readonly ISet<TokenCategory> type = 
            new HashSet<TokenCategory>() {
                TokenCategory.BOOL,
                TokenCategory.STR,
                TokenCategory.INT
            };
                
        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);                
            }
        }

        public void Program() {            
            
            while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
            }
            Expect(TokenCategory.PROG);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            Expect(TokenCategory.EOF);
        }

        public void Declaration() {
            switch(CurrentToken)
            {
                case TokenCategory.VAR:
                    Var();
                    break;
                case TokenCategory.CONST:
                    Const();
                    break;
                case TokenCategory.PROC:
                    Proc();
                    break;
                default:
                    throw new SyntaxError(firstOfDeclaration, 
                                      tokenStream.Current);
            }
        }

        public void Var(){
            Expect(TokenCategory.VAR);
            VarDeclaration();
            while (CurrentToken == TokenCategory.IDENTIFIER) {
                VarDeclaration();
            } 
        }
        public void VarDeclaration() {
            Expect(TokenCategory.IDENTIFIER);
            while (CurrentToken != TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
            Expect(TokenCategory.TWOPOINTS);
            Type();
            Expect(TokenCategory.SEMICOL);
        }
        public void Const(){
            Expect(TokenCategory.CONST);
            ConstDeclaration();
            while (CurrentToken == TokenCategory.IDENTIFIER) {
                ConstDeclaration();
            }
        }
        public void ConstDeclaration() {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.ASSIGN);
            Literal();
            Expect(TokenCategory.SEMICOL);
        }

        public void Proc(){
            Expect(TokenCategory.PROC);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            while(CurrentToken == TokenCategory.IDENTIFIER) {
                ParamDeclaration();
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            if (CurrentToken == TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.TWOPOINTS);
                Type();
            }
            Expect(TokenCategory.SEMICOL);
            if (CurrentToken == TokenCategory.CONST) {
                Const();
            }
            if (CurrentToken == TokenCategory.VAR) {
                Var();
            }
            Expect(TokenCategory.BEGIN);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);

        }

        public void ParamDeclaration() {
            Expect(TokenCategory.IDENTIFIER);
            while (CurrentToken != TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
            Expect(TokenCategory.TWOPOINTS);
            Type();
            Expect(TokenCategory.SEMICOL);

        }
        public void Literal() {

        }

        public void Statement() {

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Identifier();
                break;

            case TokenCategory.LOOP:
                Loop();
                break;
            
            case TokenCategory.FOR:
                For();
                break;
            
            case TokenCategory.RETURN:
                Return();
                break;

            case TokenCategory.IF:
                If();
                break;

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }

        public void Type() {
            switch (CurrentToken) {

            case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

            case TokenCategory.BOOL:
                Expect(TokenCategory.BOOL);
                break;

            case TokenCategory.STR:
                Expect(TokenCategory.STR);
                break;

            default:
                throw new SyntaxError(type, 
                                      tokenStream.Current);
            }
        }

        public void Identifier() {
            Expect(TokenCategory.IDENTIFIER);
            switch(CurrentToken) {
                case TokenCategory.ASSIGN:
                    Expect(TokenCategory.ASSIGN);
                    Expression();
                    break;
                case TokenCategory.PARENTHESIS_OPEN:
                    Call();
                    break;
                default:
                    throw new SyntaxError(nextOfIdentifier, 
                                      tokenStream.Current);
            }
        }

        public void Call(){
            Expect(TokenCategory.PARENTHESIS_OPEN);
            bool first = true;
            while(CurrentToken != TokenCategory.PARENTHESIS_CLOSE) {
                if (!first) {
                    Expect(TokenCategory.COMA);
                } else {
                    first = false;
                }
                Expression();
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOL);
        }

        public void For() {

        }

        public void Loop() {

        }

        public void Return() {
            Expect(TokenCategory.RETURN);
            Expression();
        }


        public void Print() {
            Expect(TokenCategory.PRINT);
            Expression();
        }

        public void If() {
            Expect(TokenCategory.IF);
            Expression();
            Expect(TokenCategory.THEN);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
        }

        public void Expression() {
            SimpleExpression();
            while (firstOfOperator.Contains(CurrentToken)) {
                Operator();
                SimpleExpression();
            }
            Expect(TokenCategory.SEMICOL);
        }

        public void SimpleExpression() {

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Identifier();
                break;

            case TokenCategory.INT_LITERAL:
                Expect(TokenCategory.INT_LITERAL);
                break;

            case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

            case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

            case TokenCategory.PARENTHESIS_OPEN:
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                break;

            case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                SimpleExpression();
                break;

            case TokenCategory.STR_LITERAL:
                Expect(TokenCategory.STR_LITERAL);
                break;

            default:
                throw new SyntaxError(firstOfSimpleExpression, 
                                      tokenStream.Current);
            }
        }

        public void Operator() {

            switch (CurrentToken) {

            case TokenCategory.AND:
                Expect(TokenCategory.AND);
                break;

            case TokenCategory.LESS:
                Expect(TokenCategory.LESS);
                break;

            case  TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;

            case TokenCategory.MUL:
                Expect(TokenCategory.MUL);
                break;

            default:
                throw new SyntaxError(firstOfOperator, 
                                      tokenStream.Current);
            }
        }
    }
}
