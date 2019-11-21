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
        public SymbolTable globalSymbolTable {
            get;
            private set;
        }
        public FunctionTable globalFunctionTable {
            get;
            private set;
        }
        public SymbolTable globalConstTable {
            get;
            private set;
        }

        public IDictionary<string, SymbolTable> localSymbolTables = new SortedDictionary<string, SymbolTable>();
        public IDictionary<string, SymbolTable> localConstTables = new SortedDictionary<string, SymbolTable>();
        public IDictionary<string, SymbolTable> functionParamTables = new SortedDictionary<string, SymbolTable>();
        //TODO: Esto sí deberían ser varias tablas, pero cómo sabemos cual es cual?
        public SymbolTable currentLocalSymbolTable {
            get;
            private set;
        }
        public SymbolTable currentLocalConstTable {
            get;
            private set;
        }
         public SymbolTable currentFunctionParamTable {
            get;
            private set;
        }
        public int inLoop;
        public string localscope;

        //-----------------------------------------------------------
        static readonly IDictionary<TokenCategory, Type> typeMapper =
            new Dictionary<TokenCategory, Type>() {
                { TokenCategory.BOOL, Type.BOOL },
                { TokenCategory.INT, Type.INT },               
                { TokenCategory.STR, Type.STR },
                {TokenCategory.INT_LITERAL, Type.INT},
                {TokenCategory.STR_LITERAL, Type.STR},
                {TokenCategory.TRUE, Type.BOOL},
                {TokenCategory.FALSE, Type.BOOL},
            };

        //-----------------------------------------------------------
        public SymbolTable Table {
            get;
            private set;
        }

        //Helper functions
        public void printTables(){
            string[] tables = {
                globalSymbolTable.ToString(),
                globalFunctionTable.ToString(),
                globalConstTable.ToString(),
                localSymbolTables.ToString(),
                localConstTables.ToString(),
                functionParamTables.ToString(),
                currentLocalSymbolTable.ToString(),
                currentLocalConstTable.ToString(),
                currentFunctionParamTable.ToString()                
            };
            string[] tableNames = {
                "globalSymbolTable",
                "globalFunctionTable",
                "globalConstTable",
                "localSymbolTables",
                "localConstTables",
                "functionParamTables",
                "currentLocalSymbolTable",
                "currentLocalConstTable",
                "currentFunctionParamTable"
            };
            var counter = 0;
            foreach(var i in tables){
                counter++;
                Console.WriteLine("Table " + counter + ": " + tableNames[counter]);
                Console.WriteLine(i);
            }
        }

        //-----------------------------------------------------------
        public SemanticAnalyzer() {
            Table = new SymbolTable();
            globalSymbolTable = new SymbolTable();
            globalFunctionTable = new FunctionTable();
            globalConstTable = new SymbolTable();
            currentLocalSymbolTable = new SymbolTable();
            currentLocalConstTable = new SymbolTable();
            currentFunctionParamTable = new SymbolTable();
            inLoop = 0;
            localscope = null;

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
        public Type Visit(Program node) {
            Visit((dynamic) node[0]);
            Visit((dynamic) node[1]);
            return Type.VOID;
        }

        public void Visit(Expression node){
            VisitChildren(node);
        }

        public Type Visit(DeclarationList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(StatementList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(ProcDeclaration node) {
            localscope = node[0].AnchorToken.Lexeme;
            currentLocalConstTable = new SymbolTable();
            currentLocalSymbolTable = new SymbolTable();
            Visit((dynamic) node[1]);
            globalSymbolTable[localscope] = Visit((dynamic) node[2]);
            Visit((dynamic) node[3]);
            Visit((dynamic) node[4]);
            Visit((dynamic) node[5]);
            globalFunctionTable[localscope] = currentFunctionParamTable.Size();
            localSymbolTables.Add(localscope, currentLocalSymbolTable);
            localConstTables.Add(localscope, currentLocalConstTable);
            localscope = null;
            return Type.VOID;
        }
        public Type Visit(ProcParam node) {
            VisitChildren(node);
            return Type.VOID;
        }
        public Type Visit(ProcType node) {
            foreach(var n in node) {
                return Visit((dynamic) n);
            }
            return Type.VOID;
        }
        public Type Visit(ProcConst node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(ProcVar node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(ProcStatement node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(ParamDeclaration node) {
           var idList = node[0];
           var type = node[1].AnchorToken.Category;
           foreach( var n in idList) {
               var name = n.AnchorToken.Lexeme;
               if (currentFunctionParamTable.Contains(name)) {
                    throw new SemanticError(
                    "Parameter " + name 
                    + " already exists in function "+localscope,                   
                    n.AnchorToken);
               }
               currentFunctionParamTable[name] = typeMapper[type];
           }
           return Type.VOID;
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

        public Type Visit(If node){
           if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        public Type Visit(ElseIf node){
           if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        public Type Visit(Else node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(Then node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(Assignment node){
            var variableName = node[0].AnchorToken.Lexeme;
            if (localscope != null && currentLocalSymbolTable.Contains(variableName)) {
                var expectedType = currentLocalSymbolTable[variableName];

                if (expectedType != Visit((dynamic) node[1])) {
                    throw new SemanticError(
                        "Expecting type " + expectedType 
                        + " in assignment statement",
                        node.AnchorToken);
                }
            } else if (globalSymbolTable.Contains(variableName)) {

                var expectedType = globalSymbolTable[variableName];

                if (expectedType != Visit((dynamic) node[1])) {
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

        public Type Visit(Loop node){
            inLoop++;
            VisitChildren(node);
            inLoop--;
            return Type.VOID;
        }

        public Type Visit(For node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(Do node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(List node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(ChimeraType node){
            return typeMapper[node.AnchorToken.Category];
            //VisitChildren(node);
            //return Type.VOID;
        }

        public Type Visit(ListType node){
            Console.WriteLine(node.ToStringTree());
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(Return node){
            Type t = Visit((dynamic) node[0]) || Type.VOID;
            if (t != globalSymbolTable[localscope]) {
                throw new SemanticError("Expected "+globalSymbolTable[localscope]+" as return but got instead "+t , node.AnchorToken);
            }
            return t;
        }

        public Type Visit(Exit node){
            VisitChildren(node);
            if (inLoop > 0) {
                return Type.VOID;
            } else {
                throw new SemanticError("Unexpected {0} outside loop" , node.AnchorToken);
            }
        }

        public Type Visit(IntLiteral node){
            var intStr = node.AnchorToken.Lexeme;
            try {
                Convert.ToInt32(intStr);
            }catch (OverflowException){
                throw new SemanticError("Integer literal exceeds 32 bits (too large): " + intStr, node.AnchorToken);
            }
            return Type.INT;
        }

         public Type Visit(StrLiteral node){
            var str = node.AnchorToken.Lexeme;
            return Type.STR;
        }

        public Type Visit(VarDeclaration node){
            VisitChildren(node);
            return Type.VOID;
        }

        public Type Visit(Var node) {
            foreach(var n in node[0]){
                TokenCategory t = node[1].AnchorToken.Category;
                var varName = n.AnchorToken.Lexeme;
                if(localscope != null){
                    if(currentLocalSymbolTable.Contains(varName)){
                        throw new SemanticError("Duplicated variable: " + varName, n.AnchorToken);
                    }
                    currentLocalSymbolTable[varName] = typeMapper[t];
                }else{
                    if (globalSymbolTable.Contains(varName)){
                        throw new SemanticError("Duplicated variable: " + varName, n.AnchorToken);
                    }
                    globalSymbolTable[varName] = typeMapper[t];
                }
            }
            return Type.VOID;
        }

        public Type Visit(ConstDeclaration node) {
            printTables();
            if(node.AnchorToken.Category != TokenCategory.ASSIGN){
                var varName = node[0].AnchorToken.Lexeme;
                var type = node[1].AnchorToken.Category;
                Console.WriteLine(node[1].ToString());
                if (localscope != null) {
                    if (currentLocalConstTable.Contains(varName)) {
                        throw new SemanticError("Duplicated constant: " + varName, node[0].AnchorToken);
                    }
                    currentLocalConstTable[varName] = typeMapper[type];
                } else {
                    if (globalConstTable.Contains(varName)) {
                        throw new SemanticError("Duplicated constant: " + varName, node[0].AnchorToken);
                    } else if (globalSymbolTable.Contains(varName)) {
                        throw new SemanticError("Constant and variable cannot have the same name: " + varName, node[0].AnchorToken);
                    } else  {
                        globalSymbolTable[varName] = typeMapper[type];
                    }
                }
            }
            VisitChildren(node);
            return Type.VOID;
        }

    //Esto no se encarga del call...
        public Type Visit(Identifier node){
            var varName = node.AnchorToken.Lexeme;
            if (localscope != null) {
                if (currentFunctionParamTable.Contains(varName)) {
                    return currentFunctionParamTable[varName];
                } else if (currentLocalConstTable.Contains(varName)) {
                    return currentLocalConstTable[varName];
                } else if (currentLocalSymbolTable.Contains(varName)) {
                    return currentLocalSymbolTable[varName];
                }
            }
            if (globalConstTable.Contains(varName)) {
                return globalConstTable[varName];
            } else if (globalSymbolTable.Contains(varName)){
                return globalSymbolTable[varName];
            } else {
                throw new SemanticError("No defined variable: " + varName, node.AnchorToken);
            }
        }

        //operators
        //-----------------------------------------------------------
        void VisitBinaryOperator(string op, Node node, Type type) {
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

        void VisitChildren(Node node) {
            foreach (var n in node) {
                Visit((dynamic) n);
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
            if(node[1] != null){//two operands -> substraction
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
    }
}
