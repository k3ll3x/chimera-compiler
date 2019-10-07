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
                TokenCategory.EXIT,
                TokenCategory.RETURN,
                TokenCategory.FOR,
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

            static readonly ISet<TokenCategory> simpleLiteral =
            new HashSet<TokenCategory>() {
                TokenCategory.INT_LITERAL,
                TokenCategory.STR_LITERAL,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
            };

        static readonly ISet<TokenCategory> type = 
            new HashSet<TokenCategory>() {
                TokenCategory.BOOL,
                TokenCategory.STR,
                TokenCategory.INT
            };

        static readonly ISet<TokenCategory> logicOperatorTokens = 
            new HashSet<TokenCategory>() {
                TokenCategory.AND,
                TokenCategory.OR,
                TokenCategory.XOR
            };

        static readonly ISet<TokenCategory> relationalOperatorTokens =
            new HashSet<TokenCategory>() {
                TokenCategory.EQUAL,
                TokenCategory.BOOLINEQ,
                TokenCategory.LESS,
                TokenCategory.MORE,
                TokenCategory.MOREEQ,
                TokenCategory.LESSEQ,
            };

        static readonly ISet<TokenCategory> sumOperatorTokens =
            new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.NEG
            };

        static readonly ISet<TokenCategory> mulOperatorTokens =
            new HashSet<TokenCategory>() {
                TokenCategory.MUL,
                TokenCategory.DIV,
                TokenCategory.REM
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

        public void ConstDeclaration() {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.ASSIGN);
            Literal();
            Expect(TokenCategory.SEMICOL);
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

        public void Literal() {
            if (simpleLiteral.Contains(CurrentToken)) {
                SimpleLiteral();
            } else {
                List();
            }
        }

        public void SimpleLiteral() {
            switch(CurrentToken) {
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                
                case TokenCategory.STR_LITERAL:
                    Expect(TokenCategory.STR_LITERAL);
                    break;

                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;

                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;

                default:
                    throw new SyntaxError(simpleLiteral, 
                                      tokenStream.Current);
            }
        }

        public void Type() {
            if(type.Contains(CurrentToken)){
                SimpleType();
            }else{
                ListType();
            }
        }

        public void SimpleType(){
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

        public void ListType(){
            Expect(TokenCategory.LIST);
            Expect(TokenCategory.OF);
            SimpleType();   
        }

        public void List(){
            Expect(TokenCategory.CURLYBRACKET_OPEN);
            if(simpleLiteral.Contains(CurrentToken)){
                SimpleLiteral();
                while (CurrentToken != TokenCategory.CURLYBRACKET_CLOSE) {
                    Expect(TokenCategory.COMA);
                    SimpleLiteral();
                }
            }
            Expect(TokenCategory.CURLYBRACKET_CLOSE);
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

        public void Statement() {
            switch (CurrentToken) {
            case TokenCategory.IDENTIFIER:
                assignOrCallStatement();
                break;

            case TokenCategory.IF:
                If();
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

            case TokenCategory.EXIT:
                Exit();
                break;

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }

        public void callStatement(){
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if(firstOfSimpleExpression.Contains(CurrentToken)){
                Expression();
                while(CurrentToken == TokenCategory.COMA){
                    Expect(TokenCategory.COMA);
                    Expression();
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOL);
        }

        public void assignStatement(){
            if(CurrentToken == TokenCategory.SQUAREDBRACKET_OPEN){
                Expect(TokenCategory.SQUAREDBRACKET_OPEN);
                Expression();
                Expect(TokenCategory.SQUAREDBRACKET_CLOSE);
            }
            Expect(TokenCategory.ASSIGN);
            Expression();
            Expect(TokenCategory.SEMICOL);
        }

        public void assignOrCallStatement(){
            Expect(TokenCategory.IDENTIFIER);
            if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                callStatement();
            }else{
                assignStatement();
            }
        }

        public void Var(){
            Expect(TokenCategory.VAR);
            do {
                VarDeclaration();
            } while (CurrentToken == TokenCategory.IDENTIFIER);
        }
        
        public void Const(){
            Expect(TokenCategory.CONST);
            do {
                ConstDeclaration();
            } while (CurrentToken == TokenCategory.IDENTIFIER);
        }

        public void Exit(){
            Expect(TokenCategory.EXIT);
            Expect(TokenCategory.SEMICOL);
        }

        public void For() {
            Expect(TokenCategory.FOR);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.IN);
            Expression();
            Expect(TokenCategory.DO);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
        }

        public void Loop() {
            Expect(TokenCategory.LOOP);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);

        }

        public void Return() {
            Expect(TokenCategory.RETURN);
            if(firstOfSimpleExpression.Contains(CurrentToken)){
                Expression();
            }
            Expect(TokenCategory.SEMICOL);
        }

        public void If() {
            Expect(TokenCategory.IF);
            Expression();
            Expect(TokenCategory.THEN);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            while (CurrentToken == TokenCategory.ELSEIF) {
                Expect(TokenCategory.ELSEIF);
                Expression();
                Expect(TokenCategory.THEN);
                while (firstOfStatement.Contains(CurrentToken)) {
                    Statement();
                }
            }
            if (CurrentToken == TokenCategory.ELSE) {
                Expect(TokenCategory.ELSE);
                while (firstOfStatement.Contains(CurrentToken)) {
                    Statement();
                }
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
        }

        public void LogicOperator(){
            switch(CurrentToken){
                case TokenCategory.AND:
                Expect(TokenCategory.AND);
                break;

                case TokenCategory.OR:
                Expect(TokenCategory.OR);
                break;

                case TokenCategory.XOR:
                Expect(TokenCategory.XOR);
                break;

                default:
                throw new SyntaxError(logicOperatorTokens, tokenStream.Current);
            }
        }

        public void RelationalOperator(){
            switch(CurrentToken){
                case TokenCategory.EQUAL:
                Expect(TokenCategory.EQUAL);
                break;
                case TokenCategory.BOOLINEQ:
                Expect(TokenCategory.BOOLINEQ);
                break;
                case TokenCategory.LESS:
                Expect(TokenCategory.LESS);
                break;
                case TokenCategory.MORE:
                Expect(TokenCategory.MORE);
                break;
                case TokenCategory.LESSEQ:
                Expect(TokenCategory.LESSEQ);
                break;
                case TokenCategory.MOREEQ:
                Expect(TokenCategory.MOREEQ);
                break;
                default:
                throw new SyntaxError(relationalOperatorTokens, tokenStream.Current);
            }
        }

        public void MulOperator(){
            switch(CurrentToken){
                case TokenCategory.MUL:
                Expect(TokenCategory.MUL);
                break;
                case TokenCategory.DIV:
                Expect(TokenCategory.DIV);
                break;
                case TokenCategory.REM:
                Expect(TokenCategory.REM);
                break;
                default:
                throw new SyntaxError(mulOperatorTokens, tokenStream.Current);
            }
        }

        public void Call(){
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if(firstOfSimpleExpression.Contains(CurrentToken)){
                Expression();
                while(CurrentToken == TokenCategory.COMA){
                    Expect(TokenCategory.COMA);
                    Expression();
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
        }

        public void SimpleExpression(){
            if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
            }else if(CurrentToken == TokenCategory.IDENTIFIER){
                Expect(TokenCategory.IDENTIFIER);
                if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                    Call();
                }
            }else{
                Literal();
            }
            if(CurrentToken == TokenCategory.SQUAREDBRACKET_OPEN){
                Expect(TokenCategory.SQUAREDBRACKET_OPEN);
                Expression();
                Expect(TokenCategory.SQUAREDBRACKET_CLOSE);
            }
        }

        public void UnaryExpression(){
            if(CurrentToken == TokenCategory.NOT){
                Expect(TokenCategory.NOT);
                UnaryExpression();
            }else if(CurrentToken == TokenCategory.NEG){
                Expect(TokenCategory.NEG);
                UnaryExpression();
            }else{
                SimpleExpression();
            }
        }

        public void MulExpression(){
            UnaryExpression();
            while(mulOperatorTokens.Contains(CurrentToken)){
                MulOperator();
                UnaryExpression();
            }
        }

        public void SumOperator(){
            switch(CurrentToken){
                case TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;
                case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                break;
                default:
                throw new SyntaxError(sumOperatorTokens, tokenStream.Current);
            }
        }

        public void SumExpression(){
            MulExpression();
            while(sumOperatorTokens.Contains(CurrentToken)){
                SumOperator();
                MulExpression();
            }
        }

        public void RelationalExpression(){
            SumExpression();
            while(relationalOperatorTokens.Contains(CurrentToken)){
                RelationalOperator();
                SumExpression();
            }
        }

        public void LogicExpression(){
            RelationalExpression();
            while(logicOperatorTokens.Contains(CurrentToken)){
                LogicOperator();
                RelationalExpression();
            }
        }

        public void Expression(){
            LogicExpression();
        }
    }
}
