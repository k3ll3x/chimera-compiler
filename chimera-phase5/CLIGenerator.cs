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
        public SymbolTable globalSymbolTable = new SymbolTable();
        public SymbolTable globalFunctionTableTypes = new SymbolTable();
        public FunctionTable globalFunctionTable = new FunctionTable();
        public SymbolTable globalConstTable = new SymbolTable();
        public IDictionary<string, SymbolTable> localSymbolTables = new SortedDictionary<string, SymbolTable>();
        public IDictionary<string, SymbolTable> localConstTables = new SortedDictionary<string, SymbolTable>();
        public IDictionary<string, SymbolTable> functionParamTables = new SortedDictionary<string, SymbolTable>();
        public SymbolTable currentLocalSymbolTable = new SymbolTable();
        public SymbolTable currentLocalConstTable = new SymbolTable();
        public SymbolTable currentFunctionParamTable = new SymbolTable();
        public Dictionary<string, string> ilasmApiFunction = new Dictionary<string, string>();

        public Dictionary<string, string> constTable = new Dictionary<string, string>();

         public IDictionary<string, int> currentLocalVar = new Dictionary<string, int>();

        int labelCounter = 0;
        bool insideFunction = false;
        string endLoop = "";
        string ifLabel = "";

        string localscope = null;

        //-----------------------------------------------------------
        string GenerateLabel() {
            return String.Format("${0:000000}", labelCounter++);
        }    

        //-----------------------------------------------------------
        static readonly IDictionary<Type, string> CILTypes =
            new Dictionary<Type, string>() {
                { Type.BOOL, "int32" },
                { Type.INT, "int32" },
                { Type.STR, "string" },
                { Type.VOID, "void" },
                { Type.LIST_INT, "int32[]" },
                { Type.LIST_BOOL, "int32[]" },
                { Type.LIST_STR, "string[]" }
            };

        public CILGenerator(Object[] tables) {
            //assign tables given by Semantic Analyzer
            globalSymbolTable = (SymbolTable) tables[0];
            globalFunctionTable = (FunctionTable) tables[1];
            globalConstTable = (SymbolTable) tables[2];
            localSymbolTables =  (IDictionary<string, SymbolTable>) tables[3];
            localConstTables = (IDictionary<string, SymbolTable>) tables[4];
            functionParamTables = (IDictionary<string, SymbolTable>) tables[5];
            currentLocalSymbolTable = (SymbolTable) tables[6];
            currentLocalConstTable = (SymbolTable) tables[7];
            currentFunctionParamTable = (SymbolTable) tables[8];
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
            ilasmApiFunction.Add("WrInt","call void class ['chimeralib']'Chimera'.'Utils'::'WrInt'(int32)\n");
            ilasmApiFunction.Add("WrStr","call void class ['chimeralib']'Chimera'.'Utils'::'WrStr'(string)\n");
            ilasmApiFunction.Add("WrBool","call void class ['chimeralib']'Chimera'.'Utils'::'WrBool'(int32)\n");
            ilasmApiFunction.Add("WrLn","call void class ['chimeralib']'Chimera'.'Utils'::'WrLn'()\n");
            ilasmApiFunction.Add("RdInt","call int32 class ['chimeralib']'Chimera'.'Utils'::'RdInt'()\npop\n");
            ilasmApiFunction.Add("RdStr","call string class ['chimeralib']'Chimera'.'Utils'::'RdStr'()\npop\n");
            ilasmApiFunction.Add("AtStr","call string class ['chimeralib']'Chimera'.'Utils'::'AtStr'(string, int32)\npop\n");
            ilasmApiFunction.Add("LenStr","call int32 class ['chimeralib']'Chimera'.'Utils'::'LenStr'(string)\npop\n");
            ilasmApiFunction.Add("CmpStr","call int32 class ['chimeralib']'Chimera'.'Utils'::'CmpStr'(string, string)\npop\n");
            ilasmApiFunction.Add("CatStr","call string class ['chimeralib']'Chimera'.'Utils'::'CatStr'(string, string)\npop\n");
            ilasmApiFunction.Add("LenLstInt","call int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstInt'(int32[])\npop\n");
            ilasmApiFunction.Add("LenLstStr","call int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstStr'(string[])\npop\n");
            ilasmApiFunction.Add("LenLstBool","call int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstBool'(int32[])\npop\n");
            ilasmApiFunction.Add("NewLstInt","call int32[] class ['chimeralib']'Chimera'.'Utils'::'NewLstInt'(int32)\npop\n");
            ilasmApiFunction.Add("NewLstStr","call string[] class ['chimeralib']'Chimera'.'Utils'::'NewLstStr'(int32)\npop\n");
            ilasmApiFunction.Add("NewLstBool","call int32[] class ['chimeralib']'Chimera'.'Utils'::'NewLstBool'(int32)\npop\n");
            ilasmApiFunction.Add("IntToStr","call string class ['chimeralib']'Chimera'.'Utils'::'IntToStr'(int32)\npop\n");
            ilasmApiFunction.Add("StrToInt","call int32 class ['chimeralib']'Chimera'.'Utils'::'StrToInt'(string)\npop\n");
        }

        string VisitChildren(Node node) {
            var sb = new StringBuilder();
            foreach (var n in node) {
                sb.Append(Visit((dynamic) n));
            }
            return sb.ToString();
        }

        public string Visit(Program node) {
            return "//Code generated by The Chimera Compiler.\n"
            + ".assembly 'chimera' {}\n"
            + ".assembly extern 'chimeralib' {}\n"
            + ".class public 'ChimeraProgram' extends "
            + "['mscorlib']'System'.'Object' {\n"
            + Visit((dynamic) node[0]) //DeclarationList
            + Visit((dynamic) node[1]) //StatementList
            + "}\n";
           
        }

        public string Visit(Expression node){
            return VisitChildren(node);
        }

        public string Visit(DeclarationList node) {
            //Var declaration, Const Declaration
            return VisitChildren(node);
        }

        public string Visit(StatementList node) {
            return VisitChildren(node);
        }


//Constant declaration missing
        public string Visit(ProcDeclaration node) {
            insideFunction = true;
            localscope = node[0].AnchorToken.Lexeme;
            currentLocalConstTable = localConstTables[localscope];
            currentLocalSymbolTable = localSymbolTables[localscope];
            currentFunctionParamTable = functionParamTables[localscope];
            
            var sb = new StringBuilder();
            sb.Append("instance default ");
            sb.Append(CILTypes[globalFunctionTableTypes[localscope]]);
            sb.Append(" ");
            sb.Append(localscope);
            sb.Append(" (");
            var flag = false;
            int count = 1;
            foreach (KeyValuePair<string, Type> kvp in currentFunctionParamTable) {
                if (flag) {
                    sb.Append(", ");
                } else {
                    flag = true;
                }
                sb.Append(CILTypes[kvp.Value]);
                sb.Append(" ");
                sb.Append(kvp.Key);
                currentLocalVar.Add(kvp.Key, count);
                count ++;
            }
            sb.Append(") cil managed \n{");
            sb.Append(".locals init (");
            count = 0;
            foreach (KeyValuePair<string, Type> kvp in currentLocalSymbolTable) {
                if (flag) {
                    sb.Append(", ");
                } else {
                    flag = true;
                }
                sb.Append(CILTypes[kvp.Value]);
                sb.Append(" ");
                sb.Append("V_");
                sb.Append(count);
                currentLocalVar.Add(kvp.Key, count);
                count ++;
            }
            sb.Append(")\n");           
            sb.Append(Visit((dynamic) node[5]));
            sb.Append("} // end of method Const::");
            sb.Append(localscope);
            localscope = null;
            insideFunction = false;
            return sb.ToString();
        }
        public string Visit(ProcParam node) {
            return VisitChildren(node);
        }
        public string Visit(ProcType node) {
            return VisitChildren(node);
            /*foreach(var n in node) {
                return Visit((dynamic) n);
            }
            return Type.VOID;*/
        }
        public string Visit(ProcConst node) {
            return VisitChildren(node);
        }

        public string Visit(ProcVar node) {
            return VisitChildren(node);
        }

        public string Visit(ProcStatement node) {
            return VisitChildren(node);
        }

        public string Visit(CallStatement node){
            //check if function exists in tables
            var result = VisitChildren(node);
            foreach(KeyValuePair<string, string> kvp in ilasmApiFunction){
                if(node.AnchorToken.Lexeme == kvp.Key){
                    result += kvp.Value;
                    return result;
                    //return VisitChildren(node)
                    //+ ilasmApiFunction[apiFunction];
                }
            }
            var cliType = CILTypes[globalSymbolTable[node.AnchorToken.Lexeme]];
            result += "call " + cliType
            + " class 'ChimeraProgram'::'" + node.AnchorToken.Lexeme
            + "'(";

            result += ")\n";
            if(cliType != "void"){
                result += "pop\n";
            }
            return result;
        }

        public string Visit(ParamDeclaration node) {
            var result = "";
            var idList = node[0];
            var type = Visit((dynamic)node[1]);
            var counter = 0;
            foreach( var n in idList) {
                counter++;
                var name = n.AnchorToken.Lexeme;
                result += CILTypes[type] + " " + name;
                if(counter >= globalFunctionTable[localscope]){
                    result += "";
                }
            }
            return result;
        }

        //types
        //-----------------------------------------------------------
        public string Visit(True node) {
            return "\tldc.i4 1\n";
        }

        //-----------------------------------------------------------
        public string Visit(False node) {
            return "\tldc.i4 0\n";
        }

        public string Visit(If node){
            var elseBody = GenerateLabel();
            var prevEndIf = ifLabel;
            ifLabel = GenerateLabel();
            var result = Visit(((dynamic) node[0]))
            + "ldc.i4.1\n"
            + "bne.un '" + elseBody + "'\n"
            + Visit(((dynamic) node[1]))
            + "br " + ifLabel + "\n"
            + "'" + elseBody + "':\n"
            + Visit(((dynamic) node[2]))
            + "'" + ifLabel + "':\n";

            ifLabel = prevEndIf;
            return result;
        }

        public string Visit(ElseIf node){
            var label = GenerateLabel();
            return Visit(((dynamic) node[0]))
            + "ldc.i4.1\n"
            + "bne.un '" + label + "'\n"
            + Visit(((dynamic) node[1]))
            + "'" + label + "':\n";
        }

        public string Visit(Else node){
            return VisitChildren(node);
        }

        public string Visit(Then node){
            return VisitChildren(node);
        }

        public string Visit(Assignment node) {
            /*if(functionParamTables.Contains(node.AnchorToken.Lexeme)){
                return VisitChildren(node) + "\tstarg.s" + params[node.AnchorToken.Lexeme] + "\n";
            } else if(localSymbolTables.Contains(node.AnchorToken.Lexeme)){
                return VisitChildren(node) + "\tstloc '" + node.AnchorToken.Lexeme + "'\n";
            } else{
                return VisitChildren(node) + "\tstsfld int32 'ChimeraProgram'::'" + node.AnchorToken.Lexeme + "'\n";
            }*/
            return "Assignment node code\n";
        }

        public string Visit(Loop node) {
            var startLoop = GenerateLabel();
            var currentEndLoop = GenerateLabel();
            endLoop = currentEndLoop;
            var result =  "\t"
            + startLoop + ":\n"
            + VisitChildren(node) + "\n"
            + "br " + startLoop
            + currentEndLoop + "\n";
            return result;
        }

        public string Visit(For node){
            return VisitChildren(node);
        }

        public string Visit(Do node){
            return VisitChildren(node);
        }

        public string Visit(List node) {
            /*Type expectedType = Visit((dynamic) node[0]);
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
            };*/
            return "List node code\n";
        }

        public string Visit(ChimeraType node){
            return null;
        }

        public string Visit(ListType node){
            /*Type type = Type.VOID;
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
            }*/
            return "ListType node code\n";
        }

        public string Visit(Return node){
            return VisitChildren(node) + "\tret\n";
        }

        public string Visit(Exit node){
            VisitChildren(node);
            return "brfalse " + endLoop + "\n";
        }

        public string Visit(IntLiteral node){
            var value = Convert.ToInt32(node.AnchorToken.Lexeme);
            if(value <= 8){
                return "\tldc.i4." + value + "\n";
            }else if(value <= 127){
                return "\tldc.i4.s " + value + "\n";
            }else{
                return "\tldc.i4 " + value + "\n";
            }
        }

         public string Visit(StrLiteral node){
            return "ldstr " + node.AnchorToken.Lexeme + "\n";
        }

        public string Visit(VarDeclaration node){
            var sb = new StringBuilder();
            foreach (var entry in globalSymbolTable) {
                //If not a function
                if (!globalFunctionTable.Contains(entry.Key)) {
                    sb.Append(
                        String.Format(
                            "\t\t.locals init ({0} '{1}')\n",                              
                            CILTypes[entry.Value],
                                entry.Key
                        )
                    );
                }
                
            }
            return sb.ToString();
        }

        public string Visit(Var node) {
            var result = "";
            if(insideFunction){
                foreach(var n in node[0]){
                    result = result + "\t.locals init (int32 '" + n.AnchorToken.Lexeme + "')\n";
                    //localSymbolTables[n.AnchorToken.Lexeme];
                }
            }else{
                foreach(var n in node[0]){
                    result = result + "\t.field public static int32 '" + n.AnchorToken.Lexeme + "'\n";
                    //globalSymbolTable[n.AnchorToken.Lexeme];
                }
            }
            return result;
        }

        public string Visit(ConstDeclaration node) {
            /*if(node.AnchorToken.Category == TokenCategory.ASSIGN){
                var varName = node[0].AnchorToken.Lexeme;
                var type = Visit((dynamic) node[1]);
                if (localscope) {
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
            return Type.VOID;*/
            return "ConstDecl node code\n";
        }

        public string Visit(Identifier node){
            return "Identifier node code\n";
            /*if(functionParamTables.Contains(node.AnchorToken.Lexeme)){
                return "\tldarg." + functionParamTables[node.AnchorToken.Lexeme] + "\n";
            } else if(localSymbolTables.Contains(node.AnchorToken.Lexeme)){
                return "\tldloc '" + node.AnchorToken.Lexeme + "'\n";
            }else{
                return "\tldsfld int32 'ChimeraProgram'::'" + node.AnchorToken.Lexeme + "'\n";
            }*/
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

        public string Visit(And node){
            /*VisitBinaryOperator("and",node, Type.BOOL);
            return Type.BOOL;*/
            return "And node code\n";
        }

        public string Visit(Or node){
            /*VisitBinaryOperator("or",node, Type.BOOL);
            return Type.BOOL;*/
            return "Or node code\n";
        }

        public string Visit(Xor node){
            /*VisitBinaryOperator("xor",node, Type.BOOL);
            return Type.BOOL;*/
            return "Xor node code\n";
        }

        public string Visit(Equal node){
            /*VisitBinaryOperator("=",node, Type.INT);
            return Type.BOOL;*/
            return "Equal node code\n";
        }

        public string Visit(BoolIneq node){//also for int ineq
            /*if(Visit((dynamic) node[0]) == Type.INT){
                VisitBinaryOperator("<>",node, Type.INT);
            }else{
                VisitBinaryOperator("<>",node, Type.BOOL);
            }
            return Type.BOOL;*/
            return "BoolIneq node code\n";
        }

        public string Visit(Less node){
            /*VisitBinaryOperator("<",node,Type.INT);
            return Type.BOOL;*/
            return "Less node code\n";
        }

        public string Visit(More node){
            /*VisitBinaryOperator(">",node,Type.INT);
            return Type.BOOL;*/
            return "More node code\n";
        }

        public string Visit(LessEq node){
            /*VisitBinaryOperator("<=",node,Type.INT);
            return Type.BOOL;*/
            return "LessEq node code\n";
        }

        public string Visit(MoreEq node){
            /*VisitBinaryOperator(">=",node,Type.INT);
            return Type.BOOL;*/
            return "MoreEq node code\n";
        }

        public string Visit(Plus node){
            /*VisitBinaryOperator("+",node,Type.INT);
            return Type.INT;*/
            return "Plus node code\n";
        }

        public string Visit(Neg node){//also for substraction?
            /*if(node[1] != null){//two operands -> substraction
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
            return Type.INT;*/
            return "Neg node code\n";
        }

        public string Visit(Mul node){
            /*VisitBinaryOperator("*",node,Type.INT);
            return Type.INT;*/
            return "Mul node code\n";
        }

        public string Visit(Div node){
            /*VisitBinaryOperator("div",node,Type.INT);
            return Type.INT;*/
            return "Div node code\n";
        }

        public string Visit(Rem node){
            /*VisitBinaryOperator("rem",node,Type.INT);
            return Type.INT;*/
            return "Rem node code\n";
        }

        public string Visit(Not node){
            /*if(Visit((dynamic) node[0]) != Type.BOOL){
                throw new SemanticError(
                    String.Format(
                        "Operator not requires one operand of type {1}",
                        Type.BOOL),
                    node.AnchorToken);
            }
            return Type.BOOL;*/
            return "Not node code\n";
        }
    }
}
