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

        public Node Program() {
            var decList = new DeclarationList();
            var stmtList = new StatementList();

            while (firstOfDeclaration.Contains(CurrentToken)) {
                decList.Add(Declaration());
            }

            var result = new Program() {
                AnchorToken = Expect(TokenCategory.PROG)
            };

            while (firstOfStatement.Contains(CurrentToken)) {
                stmtList.Add(Statement());
            }

            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            Expect(TokenCategory.EOF);

            result.Add(decList);
            result.Add(stmtList);

            return result;
                
        }

        public Node Declaration() {
            switch(CurrentToken)
            {
                case TokenCategory.VAR:
                    return Var();
                
                case TokenCategory.CONST:
                    return Const();

                case TokenCategory.PROC:
                    return Proc();

                default:
                    throw new SyntaxError(firstOfDeclaration, 
                                      tokenStream.Current);
            }
        }

        public Node ConstDeclaration() {
            var id = new Identifier() {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            };
             var result = new ConstDeclaration(){
                AnchorToken = Expect(TokenCategory.ASSIGN)
            };
            result.Add(id);
            result.Add(Literal());
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node Literal() {
            if (simpleLiteral.Contains(CurrentToken)) {
                return SimpleLiteral();
            } else {
                return List();
            }
        }

        public Node SimpleLiteral() {
            switch(CurrentToken) {
                case TokenCategory.INT_LITERAL:
                    return new IntLiteral(){
                        AnchorToken = Expect(TokenCategory.INT_LITERAL)
                    };
                
                case TokenCategory.STR_LITERAL:
                    return new StrLiteral(){
                        AnchorToken = Expect(TokenCategory.STR_LITERAL)
                    };

                case TokenCategory.TRUE:
                    return new True() {
                        AnchorToken = Expect(TokenCategory.TRUE)
                    };

                case TokenCategory.FALSE:
                    return new False() {
                        AnchorToken = Expect(TokenCategory.FALSE)
                    };

                default:
                    throw new SyntaxError(simpleLiteral, 
                                      tokenStream.Current);
            }
        }

        public Node ChimeraType() {
            if(type.Contains(CurrentToken)){
                return new ChimeraType() {
                    AnchorToken = SimpleType()
                };
            }else{
                return ListType();
            }
        }

        public Token SimpleType(){
            switch (CurrentToken) {

            case TokenCategory.INT:
                return Expect(TokenCategory.INT);

            case TokenCategory.BOOL:
                return Expect(TokenCategory.BOOL);

            case TokenCategory.STR:
                return Expect(TokenCategory.STR);

            default:
                throw new SyntaxError(type, 
                                      tokenStream.Current);
            }
        }

        public Node ListType(){
            var result = new ListType() {
                AnchorToken = Expect(TokenCategory.LIST)
            };
            Expect(TokenCategory.OF);
            result.Add(new ChimeraType() {
                AnchorToken = SimpleType()
            });
            return result;   
        }

        public Node List(){
            Expect(TokenCategory.CURLYBRACKET_OPEN);
            var result =  new List();

            if(simpleLiteral.Contains(CurrentToken)){
                result.Add(SimpleLiteral());
                while (CurrentToken != TokenCategory.CURLYBRACKET_CLOSE) {
                    Expect(TokenCategory.COMA);
                    result.Add(SimpleLiteral());
                }
            }
            Expect(TokenCategory.CURLYBRACKET_CLOSE);
            return result;
        }

        public Node Proc(){
            var result = new ProcDeclaration() {
                AnchorToken = Expect(TokenCategory.PROC)
            };
            result.Add(new Identifier(){
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var param = new ProcParam();
            while(CurrentToken == TokenCategory.IDENTIFIER) {
                param.Add(ParamDeclaration());
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            result.Add(param);
            if (CurrentToken == TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.TWOPOINTS);
                result.Add(ChimeraType());
            }
            Expect(TokenCategory.SEMICOL);
            if (CurrentToken == TokenCategory.CONST) {
                result.Add(Const());
            }
            if (CurrentToken == TokenCategory.VAR) {
                result.Add(Var());
            }
            var procStatement = new ProcStatement(){
                AnchorToken = Expect(TokenCategory.BEGIN)
            };
            while (firstOfStatement.Contains(CurrentToken)) {
                procStatement.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            result.Add(procStatement);
            return result;
        }

        public Node ParamDeclaration() {
            var result = new ParamDeclaration();
            result.Add(new Identifier(){
                AnchorToken =  Expect(TokenCategory.IDENTIFIER)
            });
            while (CurrentToken != TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.COMA);
                result.Add(new Identifier(){
                    AnchorToken =  Expect(TokenCategory.IDENTIFIER)
                });
            }
            Expect(TokenCategory.TWOPOINTS);
            result.Add(ChimeraType());
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node Statement() {
            switch (CurrentToken) {
            case TokenCategory.IDENTIFIER:
                return assignOrCallStatement();

            case TokenCategory.IF:
                return If();

            case TokenCategory.LOOP:
                return Loop();
            
            case TokenCategory.FOR:
                return For();
            
            case TokenCategory.RETURN:
                return Return();

            case TokenCategory.EXIT:
                return Exit();

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }

        public Node callStatement(Token identifier) {
            var result = new CallStatement() { 
                AnchorToken = identifier
            };
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if(firstOfSimpleExpression.Contains(CurrentToken)){
                result.Add(Expression());
                while(CurrentToken == TokenCategory.COMA){
                    Expect(TokenCategory.COMA);
                    result.Add(Expression());
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node assignStatement(Token identifier){
            var id = new Identifier() {
                AnchorToken = identifier
            };
            var result = new Assignment();
            if(CurrentToken == TokenCategory.SQUAREDBRACKET_OPEN){
                Expect(TokenCategory.SQUAREDBRACKET_OPEN);
                id.Add(Expression());
                Expect(TokenCategory.SQUAREDBRACKET_CLOSE);
            }
            result.Add(id);
            result.AnchorToken = Expect(TokenCategory.ASSIGN);
            result.Add(Expression());
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node assignOrCallStatement(){
            Token id  = Expect(TokenCategory.IDENTIFIER);

            if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                return callStatement(id);
            }else{
                return assignStatement(id);
            }
        }

        public Node Var(){
            var result = new VarDeclaration {
                AnchorToken = Expect(TokenCategory.VAR)
            };
            result.Add(new Identifier() {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });
            while (CurrentToken != TokenCategory.TWOPOINTS) {
                Expect(TokenCategory.COMA);
                result.Add(new Identifier() {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
                });
            }
            Expect(TokenCategory.TWOPOINTS);
            result.Add(ChimeraType());
            Expect(TokenCategory.SEMICOL);
            return result;
        }
        
        public Node Const(){
            var result = new ConstDeclaration {
               AnchorToken = Expect(TokenCategory.CONST)
            };
            do {
                result.Add(ConstDeclaration());
            } while (CurrentToken == TokenCategory.IDENTIFIER);
            return result;
        }

        public Node Exit(){
            var result = new ConstDeclaration {
               AnchorToken = Expect(TokenCategory.EXIT)
            };
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node For() {
            var result = new For() {
                AnchorToken = Expect(TokenCategory.FOR)
            };
            result.Add( new Identifier() {
                AnchorToken =  Expect(TokenCategory.IDENTIFIER)
            });
            Expect(TokenCategory.IN);
            result.Add(Expression());
            var dostatement = new Do() {
                AnchorToken = Expect(TokenCategory.DO)
            };
            while (firstOfStatement.Contains(CurrentToken)) {
                dostatement.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            result.Add(dostatement);
            return result;
        }

        public Node Loop() {
            var result = new Loop() {
                AnchorToken = Expect(TokenCategory.LOOP)
            };
            while (firstOfStatement.Contains(CurrentToken)) {
                result.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            return result;

        }

        public Node Return() {
            var result = new Return() {
                AnchorToken = Expect(TokenCategory.RETURN)
            };
            if(firstOfSimpleExpression.Contains(CurrentToken)){
               result.Add(Expression());
            }
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node If() {
            var result = new If() {
                AnchorToken = Expect(TokenCategory.IF)
            };
            result.Add(Expression());
            var thenstatement = new Then() {
                AnchorToken = Expect(TokenCategory.THEN)
            };
            while (firstOfStatement.Contains(CurrentToken)) {
                thenstatement.Add(Statement());
            }
            result.Add(thenstatement);
            while (CurrentToken == TokenCategory.ELSEIF) {
                var elif = new ElseIf() {
                    AnchorToken = Expect(TokenCategory.ELSEIF)
                };
                elif.Add(Expression());
                var thenst = new Then() {
                    AnchorToken = Expect(TokenCategory.THEN)
                };
                while (firstOfStatement.Contains(CurrentToken)) {
                    thenst.Add(Statement());
                }
                elif.Add(thenst);
                result.Add(elif);
            }
            if (CurrentToken == TokenCategory.ELSE) {
                var elsestatement = new Else(){
                    AnchorToken = Expect(TokenCategory.ELSE)
                };
                while (firstOfStatement.Contains(CurrentToken)) {
                    elsestatement.Add(Statement());
                }
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.SEMICOL);
            return result;
        }

        public Node LogicOperator(){
            switch(CurrentToken){
                case TokenCategory.AND:
                return new And() {
                    AnchorToken = Expect(TokenCategory.AND)
                };

                case TokenCategory.OR:
                return new Or() {
                    AnchorToken = Expect(TokenCategory.OR)
                };

                case TokenCategory.XOR:
                return new Xor() {
                    AnchorToken = Expect(TokenCategory.XOR)
                };

                default:
                throw new SyntaxError(logicOperatorTokens, tokenStream.Current);
            }
        }

        public Node RelationalOperator(){
            switch(CurrentToken){

                case TokenCategory.EQUAL:
                return new Equal(){
                    AnchorToken = Expect(TokenCategory.EQUAL)
                };

                case TokenCategory.BOOLINEQ://also for IntIneq
                return new BoolIneq(){
                    AnchorToken = Expect(TokenCategory.BOOLINEQ)
                };

                case TokenCategory.LESS:
                return new Less(){
                    AnchorToken = Expect(TokenCategory.LESS)
                };
                
                case TokenCategory.MORE:
                return new More(){
                    AnchorToken = Expect(TokenCategory.MORE)
                };

                case TokenCategory.LESSEQ:
                return new LessEq(){
                    AnchorToken = Expect(TokenCategory.LESSEQ)
                };

                case TokenCategory.MOREEQ:
                return new MoreEq(){
                    AnchorToken = Expect(TokenCategory.MOREEQ)
                };

                default:
                throw new SyntaxError(relationalOperatorTokens, tokenStream.Current);
            }
        }

        public Node MulOperator(){
            switch(CurrentToken){
                case TokenCategory.MUL:
                return new Mul(){
                    AnchorToken = Expect(TokenCategory.MUL)
                };

                case TokenCategory.DIV:
                return new Div(){
                    AnchorToken = Expect(TokenCategory.DIV)
                };

                case TokenCategory.REM:
                return new Rem(){
                    AnchorToken = Expect(TokenCategory.REM)
                };

                default:
                throw new SyntaxError(mulOperatorTokens, tokenStream.Current);
            }
        }

        public Node Call(Token identifier){
            var result = new CallStatement() { 
                AnchorToken = identifier
            };
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if(firstOfSimpleExpression.Contains(CurrentToken)){
                result.Add(Expression());
                while(CurrentToken == TokenCategory.COMA){
                    Expect(TokenCategory.COMA);
                    result.Add(Expression());
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            return result;
        }

        public Node SimpleExpression(){
            Node result;
            if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                Expect(TokenCategory.PARENTHESIS_OPEN);
                result = Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
            }else if(CurrentToken == TokenCategory.IDENTIFIER){
                var id = Expect(TokenCategory.IDENTIFIER);
                if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
                    result = Call(id);
                }
                else {
                    result = new Identifier() {
                        AnchorToken = id
                    };
                }
            }else{
                result = Literal();
            }
            if(CurrentToken == TokenCategory.SQUAREDBRACKET_OPEN){
                Expect(TokenCategory.SQUAREDBRACKET_OPEN);
                result.Add(Expression());
                Expect(TokenCategory.SQUAREDBRACKET_CLOSE);
            }
            return result;
        }

        public Node UnaryExpression(){
            if(CurrentToken == TokenCategory.NOT){
                var result = new Not() {
                    AnchorToken = Expect(TokenCategory.NOT)
                };
                result.Add(UnaryExpression());
                return result;
            }else if(CurrentToken == TokenCategory.NEG){
                var result = new Neg() {
                    AnchorToken = Expect(TokenCategory.NEG)
                };
                result.Add(UnaryExpression());
                return result;
            }else{
                return SimpleExpression();
            }
        }

        public Node MulExpression(){
            var expr1 = UnaryExpression();
            while(mulOperatorTokens.Contains(CurrentToken)){
                var expr2 = MulOperator();
                expr2.Add(expr1);
                expr2.Add(UnaryExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        public Node SumOperator(){
            switch(CurrentToken){
                case TokenCategory.PLUS:
                return new Plus() {
                    AnchorToken = Expect(TokenCategory.PLUS)
                };

                case TokenCategory.NEG:
                return new Neg() {
                    AnchorToken = Expect(TokenCategory.NEG)
                };

                default:
                throw new SyntaxError(sumOperatorTokens, tokenStream.Current);
            }
        }

        public Node SumExpression(){
            var expr1 = MulExpression();
            while(sumOperatorTokens.Contains(CurrentToken)){
                var expr2 = SumOperator();
                expr2.Add(expr1);
                expr2.Add(MulExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        public Node RelationalExpression(){
            var expr1  =  SumExpression();
            while(relationalOperatorTokens.Contains(CurrentToken)){
                var expr2 = RelationalOperator();
                expr2.Add(expr1);
                expr2.Add(SumExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        public Node LogicExpression(){
            var expr1  = RelationalExpression();
            while(logicOperatorTokens.Contains(CurrentToken)){
                var expr2 = LogicOperator();
                expr2.Add(expr1);
                expr2.Add(RelationalExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        public Node Expression(){
            return LogicExpression();
        }
    }
}
