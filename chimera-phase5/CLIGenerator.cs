/*
  Chimera compiler - Common Intermediate Language (CIL) code generator.
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
using System.Text;
using System.Collections.Generic;

namespace Chimera {

    class CILGenerator {

        FunctionTable params;
        SymbolTable globalFunctions;
        SymbolTable globalVariables;
        SymbolTable localVariables;

        int labelCounter = 0;

        //-----------------------------------------------------------
        string GenerateLabel() {
            return String.Format("${0:000000}", labelCounter++);
        }    

        //-----------------------------------------------------------
        static readonly IDictionary<Type, string> CILTypes =
            new Dictionary<Type, string>() {
                { Type.BOOL, "bool" },
                { Type.INT, "int32" }
                { Type.STR, "someStrThingi" }
            };

        public CILGenerator(Object[] tables) {
            globalFunctions = new SymbolTable();
            globalVariables = new SymbolTable();
            localVariables = new SymbolTable();

            globalFunctions.Add(WrInt);
            globalFunctions.Add(WrStr);
            globalFunctions.Add(WrBool);
            globalFunctions.Add(WrLn);
            globalFunctions.Add(RdInt);
            globalFunctions.Add(RdStr);
            globalFunctions.Add(AtStr);
            globalFunctions.Add(LenStr);
            globalFunctions.Add(CmpStr);
            globalFunctions.Add(CatStr);
            globalFunctions.Add(LenLstInt);
            globalFunctions.Add(LenLstStr);
            globalFunctions.Add(LenLstBool);
            globalFunctions.Add(NewLstInt);
            globalFunctions.Add(NewLstStr);
            globalFunctions.Add(NewLstBool);
            globalFunctions.Add(IntToStr);
            globalFunctions.Add(StrToInt);
        }

        public string Visit(Program node) {
            Visit((dynamic) node[0]);
            Visit((dynamic) node[1]);
            return Type.VOID;
        }

        public string Visit(Expression node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(DeclarationList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(StatementList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(ProcDeclaration node) {
            localscope = node[0].AnchorToken.Lexeme;
            currentLocalConstTable = new SymbolTable();
            currentLocalSymbolTable = new SymbolTable();
            currentFunctionParamTable = new SymbolTable();
            Visit((dynamic) node[1]);
            globalSymbolTable[localscope] = Visit((dynamic) node[2]);
            Visit((dynamic) node[3]);
            Visit((dynamic) node[4]);
            Visit((dynamic) node[5]);//ProcStatement
            globalFunctionTable[localscope] = currentFunctionParamTable.Size();
            localSymbolTables.Add(localscope, currentLocalSymbolTable);
            localConstTables.Add(localscope, currentLocalConstTable);
            localscope = null;
            return Type.VOID;
        }
        public string Visit(ProcParam node) {
            VisitChildren(node);
            return Type.VOID;
        }
        public string Visit(ProcType node) {
            foreach(var n in node) {
                return Visit((dynamic) n);
            }
            return Type.VOID;
        }
        public string Visit(ProcConst node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(ProcVar node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(ProcStatement node) {
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(CallStatement node){
            //check if function exists in tables
            var name = node.AnchorToken.Lexeme;
            if(globalFunctionTable.Contains(name)){
                //call function
                if(globalFunctionTableTypes.Contains(name)){
                    return globalFunctionTableTypes[name];
                }
                if(globalSymbolTable.Contains(name)){
                    return globalSymbolTable[name];
                }
                return Type.VOID;
            }else{
                throw new SemanticError("Function/Procedure " + name + "not declared!", node.AnchorToken);
            }
        }

        public string Visit(ParamDeclaration node) {
            var idList = node[0];
            var type = Visit((dynamic)node[1]);
            foreach( var n in idList) {
                var name = n.AnchorToken.Lexeme;
                if (currentFunctionParamTable.Contains(name)) {
                    throw new SemanticError(
                    "Parameter " + name 
                    + " already exists in function "+localscope,                   
                    n.AnchorToken);
                }
                currentFunctionParamTable[name] = type;
            }
            return Type.VOID;
        }

        //types
        //-----------------------------------------------------------
        public string Visit(True node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public string Visit(False node) {
            return Type.BOOL;
        }

        public string Visit(If node){
           if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        public string Visit(ElseIf node){
           if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        public string Visit(Else node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(Then node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(Assignment node){
            var variableName = node[0].AnchorToken.Lexeme;
            var type = Visit((dynamic) node[1]);
            if (localscope != null && currentLocalSymbolTable.Contains(variableName)) {
                var expectedType = currentLocalSymbolTable[variableName];
                if (expectedType != type) {
                    throw new SemanticError(
                        "Expecting type " + expectedType 
                        + " in assignment statement but got "+ type,
                        node.AnchorToken);
                }
            } else if (globalSymbolTable.Contains(variableName)) {
                var expectedType = globalSymbolTable[variableName];
                if (expectedType != type) {
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

        public string Visit(Loop node){
            inLoop++;
            VisitChildren(node);
            inLoop--;
            return Type.VOID;
        }

        public string Visit(For node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(Do node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(List node) {
            Type expectedType = Visit((dynamic) node[0]);
            foreach( var n  in node) {
                Type type = Visit((dynamic) n);
                if (type != expectedType) {
                    throw new SemanticError("Expected "+expectedType+" as return but got instead "+ type, node.AnchorToken);
                }
            }
            switch (expectedType) {
                case Type.INT:
                    return Type.LIST_INT;
                case Type.BOOL:
                    return Type.LIST_BOOL;
                case Type.STR:
                    return Type.LIST_STR;
                default:
                    return Type.VOID;
            };
        }

        public string Visit(ChimeraType node){
            return typeMapper[node.AnchorToken.Category];
            //VisitChildren(node);
            //return Type.VOID;
        }

        public string Visit(ListType node){
            Type type = Type.VOID;
            if (node[0] is ChimeraType) {
                type = Visit((dynamic) node[0]);
            } 
            switch (type) {
                case Type.INT:
                    return Type.LIST_INT;
                case Type.BOOL:
                    return Type.LIST_BOOL;
                case Type.STR:
                    return Type.LIST_STR;
                default:
                    return Type.VOID;
            }
        }

        public string Visit(Return node){
            Type t = Visit((dynamic) node[0]);// || Type.VOID;
            if (t != globalSymbolTable[localscope]) {
                throw new SemanticError("Expected "+globalSymbolTable[localscope]+" as return but got instead "+t , node.AnchorToken);
            }
            return t;
        }

        public string Visit(Exit node){
            VisitChildren(node);
            if (inLoop > 0) {
                return Type.VOID;
            } else {
                throw new SemanticError("Unexpected {0} outside loop" , node.AnchorToken);
            }
        }

        public string Visit(IntLiteral node){
            var intStr = node.AnchorToken.Lexeme;
            try {
                Convert.ToInt32(intStr);
            }catch (OverflowException){
                throw new SemanticError("Integer literal exceeds 32 bits (too large): " + intStr, node.AnchorToken);
            }
            return Type.INT;
        }

         public string Visit(StrLiteral node){
            var str = node.AnchorToken.Lexeme;
            return Type.STR;
        }

        public string Visit(VarDeclaration node){
            VisitChildren(node);
            return Type.VOID;
        }

        public string Visit(Var node) {
            foreach(var n in node[0]){
                var type = Visit((dynamic) node[1]);
                var varName = n.AnchorToken.Lexeme;
                if(localscope != null){
                    if(currentLocalSymbolTable.Contains(varName)){
                        throw new SemanticError("Duplicated variable: " + varName, n.AnchorToken);
                    }
                    currentLocalSymbolTable[varName] = type;
                }else{
                    if (globalSymbolTable.Contains(varName)){
                        throw new SemanticError("Duplicated variable: " + varName, n.AnchorToken);
                    }
                    globalSymbolTable[varName] = type;
                }
            }
            return Type.VOID;
        }

        public string Visit(ConstDeclaration node) {
            if(node.AnchorToken.Category == TokenCategory.ASSIGN){
                var varName = node[0].AnchorToken.Lexeme;
                var type = Visit((dynamic) node[1]);
                if (localscope != null) {
                    if (currentLocalConstTable.Contains(varName)) {
                        throw new SemanticError("Duplicated constant: " + varName, node[0].AnchorToken);
                    }
                    currentLocalConstTable[varName] = type;
                } else {
                    if (globalConstTable.Contains(varName)) {
                        throw new SemanticError("Duplicated constant: " + varName, node[0].AnchorToken);
                    } else if (globalSymbolTable.Contains(varName)) {
                        throw new SemanticError("Constant and variable cannot have the same name: " + varName, node[0].AnchorToken);
                    } else  {
                        globalSymbolTable[varName] = type;
                    }
                }
            }
            VisitChildren(node);
            return Type.VOID;
        }

    //Esto no se encarga del call...
        public string Visit(Identifier node){
            var varName = node.AnchorToken.Lexeme;
            Type type = Type.VOID;
            var found = false;
            if (localscope != null) {
                if (currentFunctionParamTable.Contains(varName)) {
                    type = currentFunctionParamTable[varName];
                    found = true;
                } else if (currentLocalConstTable.Contains(varName)) {
                    type =  currentLocalConstTable[varName];
                    found = true;
                } else if (currentLocalSymbolTable.Contains(varName)) {
                    type =  currentLocalSymbolTable[varName];
                    found = true;
                }
            }
            if (!found) {
                if (globalConstTable.Contains(varName)) {
                    type =  globalConstTable[varName];
                } else if (globalSymbolTable.Contains(varName)){
                    type =  globalSymbolTable[varName];
                } else {
                    throw new SemanticError("No defined variable: " + varName, node.AnchorToken);
                }
            }
            switch (type) {
                case Type.LIST_INT:
                    foreach (var n in node) {
                        type = Type.INT;
                    }
                    return type;
                case Type.LIST_STR:
                     foreach (var n in node) {
                        type = Type.STR;
                    }
                    return type;
                case Type.LIST_BOOL:
                     foreach (var n in node) {
                        type = Type.BOOL;
                    }
                    return type;
                default:
                    return type;
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

        public string Visit(And node){
            VisitBinaryOperator("and",node, Type.BOOL);
            return Type.BOOL;
        }

        public string Visit(Or node){
            VisitBinaryOperator("or",node, Type.BOOL);
            return Type.BOOL;
        }

        public string Visit(Xor node){
            VisitBinaryOperator("xor",node, Type.BOOL);
            return Type.BOOL;
        }

        public string Visit(Equal node){
            VisitBinaryOperator("=",node, Type.INT);
            return Type.BOOL;
        }

        public string Visit(BoolIneq node){//also for int ineq
            if(Visit((dynamic) node[0]) == Type.INT){
                VisitBinaryOperator("<>",node, Type.INT);
            }else{
                VisitBinaryOperator("<>",node, Type.BOOL);
            }
            return Type.BOOL;
        }

        public string Visit(Less node){
            VisitBinaryOperator("<",node,Type.INT);
            return Type.BOOL;
        }

        public string Visit(More node){
            VisitBinaryOperator(">",node,Type.INT);
            return Type.BOOL;
        }

        public string Visit(LessEq node){
            VisitBinaryOperator("<=",node,Type.INT);
            return Type.BOOL;
        }

        public string Visit(MoreEq node){
            VisitBinaryOperator(">=",node,Type.INT);
            return Type.BOOL;
        }

        public string Visit(Plus node){
            VisitBinaryOperator("+",node,Type.INT);
            return Type.INT;
        }

        public string Visit(Neg node){//also for substraction?
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

        public string Visit(Mul node){
            VisitBinaryOperator("*",node,Type.INT);
            return Type.INT;
        }

        public string Visit(Div node){
            VisitBinaryOperator("div",node,Type.INT);
            return Type.INT;
        }

        public string Visit(Rem node){
            VisitBinaryOperator("rem",node,Type.INT);
            return Type.INT;
        }

        public string Visit(Not node){
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
