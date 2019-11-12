/*
  Chimera compiler - Semantic analyzer.
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

    class SemanticAnalyzer {
        public SymbolTable globalSymbolTable;
        public FunctionTable globalFunctionTable;
        public SymbolTable namespaceTable;
        public IDictionary<string, SymbolTable> localSymbolTables = new SortedDictionary<string, SymbolTable>();

        //-----------------------------------------------------------
        static readonly IDictionary<TokenCategory, Type> typeMapper =
            new Dictionary<TokenCategory, Type>() {
                { TokenCategory.BOOL, Type.BOOL },
                { TokenCategory.INT, Type.INT },               
                { TokenCategory.STR, Type.STR }
            };

        //-----------------------------------------------------------
        /*public SymbolTable Table {
            get;
            private set;
        }*/

        //-----------------------------------------------------------
        public SemanticAnalyzer() {
            //Table = new SymbolTable();
            globalSymbolTable = new SymbolTable();
            globalFunctionTable = new FunctionTable();
            namespaceTable = new SymbolTable();

            //global functions, chimera API with number of params
            globalFunctionTable["WrInt"] = 1;
            globalFunctionTable["WrStr"] = 1;
            globalFunctionTable["WrBool"] = 1;
            globalFunctionTable["WrLn"] = 0;
            globalFunctionTable["RdInt"] = 0;
            globalFunctionTable["RdStr"] = 0;
            globalFunctionTable["AtStr"] = 2;
            globalFunctionTable["LenStr"] = 1;
            globalFunctionTable["CmpStr"] = 2;
            globalFunctionTable["CatStr"] = 2;
            globalFunctionTable["LenLstInt"] = 1;
            globalFunctionTable["LenLstStr"] = 1;
            globalFunctionTable["LenLstBool"] = 1;
            globalFunctionTable["NewLstInt"] = 1;
            globalFunctionTable["NewLstStr"] = 1;
            globalFunctionTable["NewLstBool"] = 1;
            globalFunctionTable["IntToStr"] = 1;
            globalFunctionTable["StrToInt"] = 1;
        }

        //types
        //-----------------------------------------------------------
        public Type Visit(True node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(False node) {
            return Type.BOOL;
        }

        //operators
        //-----------------------------------------------------------
        void VisitBinaryOperator(string op, Node node, Type type) {
            /*if(node.TokenCategory == BOOLINEQ //two cases for BoolIneq and IntIneq
            && (
                (Visit((dynamic) node[0]) != type || Visit((dynamic) node[1]) != type)
                || (Visit((dynamic) node[0]) != Type.INT || Visit((dynamic) node[1]) != Type.INT))){
                    throw new SemanticError(
                        String.Format(
                            "Operator {0} requires two operands of type {1} or {2}",
                            op, 
                            type,
                            Type.INT),
                        node.AnchorToken);
            }*/
            if (Visit((dynamic) node[0]) != type || 
                Visit((dynamic) node[1]) != type) {
                throw new SemanticError(
                    String.Format(
                        "Operator {0} requires two operands of type {1}",
                        op, 
                        type),
                    node.AnchorToken);
            }
        }

        public Type Visit(And node){
            VisitBinaryOperator("and",node, Type.BOOL);
            return Type.BOOL;
        }

        public Type Visit(Or node){
            VisitBinaryOperator("or",node, Type.BOOL);
            return Type.BOOL;
        }

        public Type Visit(Xor node){
            VisitBinaryOperator("xor",node, Type.BOOL);
            return Type.BOOL;
        }

        public Type Visit(Equal node){
            VisitBinaryOperator("=",node, Type.INT);
            return Type.BOOL;
        }

        public Type Visit(BoolIneq node){//also for int ineq
            if(Visit((dynamic) node[0]) == Type.INT){
                VisitBinaryOperator("<>",node, Type.INT);
            }else{
                VisitBinaryOperator("<>",node, Type.BOOL);
            }
            return Type.BOOL;
        }

        public Type Visit(Less node){
            VisitBinaryOperator("<",node,Type.INT);
            return Type.BOOL;
        }

        public Type Visit(More node){
            VisitBinaryOperator(">",node,Type.INT);
            return Type.BOOL;
        }

        public Type Visit(LessEq node){
            VisitBinaryOperator("<=",node,Type.INT);
            return Type.BOOL;
        }

        public Type Visit(MoreEq node){
            VisitBinaryOperator(">=",node,Type.INT);
            return Type.BOOL;
        }

        public Type Visit(Plus node){
            VisitBinaryOperator("+",node,Type.INT);
            return Type.INT;
        }

        public Type Visit(Neg node){//also for substraction?
            if(node[1]){//two operands -> substraction
                VisitBinaryOperator("-",node,Type.INT);
            }else{//one operand -> negation
                if(Visit((dynamic) node[0]) != Type.INT){
                    throw new SemanticError(
                        String.Format(
                            "Operator - requires one operand of type {1}",
                            Type.INT),
                        node.AnchorToken);
                }
            }
            return Type.INT;
        }

        public Type Visit(Mul node){
            VisitBinaryOperator("*",node,Type.INT);
            return Type.INT;
        }

        public Type Visit(Div node){
            VisitBinaryOperator("div",node,Type.INT);
            return Type.INT;
        }

        public Type Visit(Rem node){
            VisitBinaryOperator("rem",node,Type.INT);
            return Type.INT;
        }

        public Type Visit(Not node){
            if(Visit((dynamic) node[0]) != Type.BOOL){
                throw new SemanticError(
                    String.Format(
                        "Operator not requires one operand of type {1}",
                        Type.BOOL),
                    node.AnchorToken);
            }
            return Type.BOOL;
        }
        /*-----------------------------------------------------------
        public Type Visit(Program node) {
            VisitChildren(node);//containing something
            Visit((dynamic) node[0]);
            Visit((dynamic) node[1]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(DeclarationList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Declaration node) {

            var variableName = node[0].AnchorToken.Lexeme;

            if (Table.Contains(variableName)) {
                throw new SemanticError(
                    "Duplicated variable: " + variableName,
                    node[0].AnchorToken);

            } else {
                Table[variableName] = 
                    typeMapper[node.AnchorToken.Category];              
            }

            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(StatementList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Assignment node) {

            var variableName = node.AnchorToken.Lexeme;

            if (Table.Contains(variableName)) {

                var expectedType = Table[variableName];

                if (expectedType != Visit((dynamic) node[0])) {
                    throw new SemanticError(
                        "Expecting type " + expectedType 
                        + " in assignment statement",
                        node.AnchorToken);
                }

            } else {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node.AnchorToken);
            }

            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Print node) {
            node.ExpressionType = Visit((dynamic) node[0]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(If node) {
            if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Identifier node) {

            var variableName = node.AnchorToken.Lexeme;

            if (Table.Contains(variableName)) {
                return Table[variableName];
            }

            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
        }

        //-----------------------------------------------------------
        public Type Visit(IntLiteral node) {

            var intStr = node.AnchorToken.Lexeme;

            try {
                Convert.ToInt32(intStr);

            } catch (OverflowException) {
                throw new SemanticError(
                    "Integer literal too large: " + intStr, 
                    node.AnchorToken);
            }

            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(True node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(False node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Neg node) {          
            if (Visit((dynamic) node[0]) != Type.INT) {
                throw new SemanticError(
                    "Operator - requires an operand of type " + Type.INT,
                    node.AnchorToken);
            }
            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(And node) {
            VisitBinaryOperator('&', node, Type.BOOL);
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Less node) {
            VisitBinaryOperator('<', node, Type.INT);
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Plus node) {
            VisitBinaryOperator('+', node, Type.INT);
            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(Mul node) {
            VisitBinaryOperator('*', node, Type.INT);
            return Type.INT;
        }

        //-----------------------------------------------------------
        void VisitChildren(Node node) {
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }

        //-----------------------------------------------------------
        void VisitBinaryOperator(char op, Node node, Type type) {
            if (Visit((dynamic) node[0]) != type || 
                Visit((dynamic) node[1]) != type) {
                throw new SemanticError(
                    String.Format(
                        "Operator {0} requires two operands of type {1}",
                        op, 
                        type),
                    node.AnchorToken);
            }
        }*/
    }
}
