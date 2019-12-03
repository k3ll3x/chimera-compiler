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

         public IDictionary<string, string> globalSymbolAssLoad = new Dictionary<string, string>();
         public IDictionary<string,string> localSymbolAssLoad = new Dictionary<string,string>();

         public IDictionary<string,string> globalSymbolAssAssign = new Dictionary<string, string>();
          public IDictionary<string,string> localSymbolAssAssign = new Dictionary<string, string>();


        int labelCounter = 0;
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
            globalFunctionTableTypes = (SymbolTable) tables[9];
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
            ilasmApiFunction.Add("WrInt","\tcall void class ['chimeralib']'Chimera'.'Utils'::'WrInt'(int32)\n");
            ilasmApiFunction.Add("WrStr","\tcall void class ['chimeralib']'Chimera'.'Utils'::'WrStr'(string)\n");
            ilasmApiFunction.Add("WrBool","\tcall void class ['chimeralib']'Chimera'.'Utils'::'WrBool'(int32)\n");
            ilasmApiFunction.Add("WrLn","\tcall void class ['chimeralib']'Chimera'.'Utils'::'WrLn'()\n");
            ilasmApiFunction.Add("RdInt","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'RdInt'()\npop\n");
            ilasmApiFunction.Add("RdStr","\tcall string class ['chimeralib']'Chimera'.'Utils'::'RdStr'()\npop\n");
            ilasmApiFunction.Add("AtStr","\tcall string class ['chimeralib']'Chimera'.'Utils'::'AtStr'(string, int32)\npop\n");
            ilasmApiFunction.Add("LenStr","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'LenStr'(string)\npop\n");
            ilasmApiFunction.Add("CmpStr","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'CmpStr'(string, string)\npop\n");
            ilasmApiFunction.Add("CatStr","\tcall string class ['chimeralib']'Chimera'.'Utils'::'CatStr'(string, string)\npop\n");
            ilasmApiFunction.Add("LenLstInt","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstInt'(int32[])\npop\n");
            ilasmApiFunction.Add("LenLstStr","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstStr'(string[])\npop\n");
            ilasmApiFunction.Add("LenLstBool","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'LenLstBool'(int32[])\npop\n");
            ilasmApiFunction.Add("NewLstInt","\tcall int32[] class ['chimeralib']'Chimera'.'Utils'::'NewLstInt'(int32)\npop\n");
            ilasmApiFunction.Add("NewLstStr","\tcall string[] class ['chimeralib']'Chimera'.'Utils'::'NewLstStr'(int32)\npop\n");
            ilasmApiFunction.Add("NewLstBool","\tcall int32[] class ['chimeralib']'Chimera'.'Utils'::'NewLstBool'(int32)\npop\n");
            ilasmApiFunction.Add("IntToStr","\tcall string class ['chimeralib']'Chimera'.'Utils'::'IntToStr'(int32)\npop\n");
            ilasmApiFunction.Add("StrToInt","\tcall int32 class ['chimeralib']'Chimera'.'Utils'::'StrToInt'(string)\npop\n");
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
            + ".method private static hidebysig specialname rtspecialname\n"
            + "default void '.cctor' ()  cil managed\n"
            + " {\n"
            + GlobalVarDec()
            + "ret\n}\n}\n";
        }

        public string GlobalVarDec() {
            var sb = new StringBuilder();
            foreach (var entry in globalSymbolTable) {
                //If not a function
                if (!globalFunctionTable.Contains(entry.Key)) {
                    sb.Append(
                        String.Format(
                            "\t\t{2}\n"
                            + "\t\tstsfld {0} 'ChimeraProgram'::'{1}' \n",                              
                            CILTypes[entry.Value],
                                entry.Key,
                                getInitVal(CILTypes[entry.Value])
                        )
                    );
                }
            }
            return sb.ToString();
        }
        public string getInitVal(string type) {
            switch(type) {
                case "string":
                    return "ldstr \" \"";
                case "int32":
                    return "ldc.i4.0 ";
                case "bool":
                    return "ldc.i4.0 ";
            }
            return "";
            
        }

        public string Visit(Expression node){
            return VisitChildren(node);
        }

        public string Visit(DeclarationList node) {
            //Var declaration, Const Declaration
            return VisitChildren(node);
        }

        public string Visit(StatementList node) {
            var result = "";
            result += ".method public static hidebysig default void Main () cil managed\n{\n"
            + "\t.entrypoint\n"
            + VisitChildren(node)
            + "\tret\n"
            + "}\n";
            return result;
        }


//Constant declaration missing
        public string Visit(ProcDeclaration node) {
            localscope = node[0].AnchorToken.Lexeme;
            currentLocalConstTable = localConstTables[localscope];
            currentLocalSymbolTable = localSymbolTables[localscope];
            currentFunctionParamTable = functionParamTables[localscope];
            localSymbolAssAssign = new Dictionary<string, string>();
            localSymbolAssLoad = new Dictionary<string, string>();
            Visit((dynamic) node[3]); //Constants
            var functionLoad = new StringBuilder();
            functionLoad.Append("call ");
            functionLoad.Append(CILTypes[globalSymbolTable[localscope]]);
            functionLoad.Append(" class ChimeraProgram::");
            functionLoad.Append(localscope);
            functionLoad.Append("(");

            var sb = new StringBuilder();
            //sb.Append("instance default ");
            sb.Append(".method public static hidebysig default ");
            sb.Append(CILTypes[globalSymbolTable[localscope]]);
            sb.Append(" ");
            sb.Append(localscope);
            sb.Append(" (");
            var flag = false;
            int count = 0;
            foreach (KeyValuePair<string, Type> kvp in currentFunctionParamTable) {
                if (flag) {
                    sb.Append(", ");
                    functionLoad.Append(", ");
                } else {
                    flag = true;
                }
                sb.Append(CILTypes[kvp.Value]);
                functionLoad.Append(CILTypes[kvp.Value]);
                sb.Append(" ");
                sb.Append(kvp.Key);
                try {
                    currentLocalVar.Add(kvp.Key, count);
                    localSymbolAssLoad.Add(kvp.Key, "ldarg."+count+"\n");
                }catch{

                }
                count ++;
            }
            sb.Append(") cil managed\n{");
            sb.Append("\n.locals init (\n");
            count = 0;
            flag = false;
            foreach (KeyValuePair<string, Type> kvp in currentLocalSymbolTable) {
                if (flag) {
                    sb.Append(",\n");
                } else {
                    flag = true;
                }
                sb.Append(CILTypes[kvp.Value]);
                sb.Append(" ");
                /*sb.Append("V_");
                sb.Append(count);*/
                sb.Append("'" + kvp.Key + "'");
                try{
                    currentLocalVar.Add(kvp.Key, count);
                    localSymbolAssAssign.Add(kvp.Key, "stloc."+count+"\n");
                    localSymbolAssLoad.Add(kvp.Key, "ldloc."+count+"\n");
                }catch{

                }
                count ++; 
            }
            sb.Append(")\n");
            foreach (KeyValuePair<string, Type> kvp in currentLocalSymbolTable) {
                sb.Append(String.Format(
                            "\t\t{0}\n"
                            +localSymbolAssAssign[kvp.Key]+"\n",
                            getInitVal(CILTypes[kvp.Value])
                        ));
            }        
            sb.Append(Visit((dynamic) node[5]));
            sb.Append("\nret\n} // end of method ChimeraProgram::");
            sb.Append(localscope);
            globalSymbolAssLoad.Add(localscope,functionLoad.ToString());
            localscope = null;
            sb.Append("\n");
            return sb.ToString();
        }
        public string Visit(ProcParam node) {
            return VisitChildren(node);
        }
        public string Visit(ProcType node) {
            return VisitChildren(node);
        }
        public string Visit(ProcConst node) {
            return VisitChildren(node);
        }

        public string Visit(ConstList node) {
            return VisitChildren(node);
        }

        public string Visit(ProcVar node) {
            return VisitChildren(node);
        }

        public string Visit(ProcStatement node) {
            return VisitChildren(node);
        }

        public string Visit(CallStatement node){
            var result = VisitChildren(node);
            foreach(KeyValuePair<string, string> kvp in ilasmApiFunction){
                if(node.AnchorToken.Lexeme == kvp.Key){
                    result += kvp.Value;
                    return result;
                }
            }
            var cliType = CILTypes[globalSymbolTable[node.AnchorToken.Lexeme]];
            result += "\tcall " + cliType
            + " class 'ChimeraProgram'::'" + node.AnchorToken.Lexeme
            + "'(";

            var counter = 0;
            foreach(KeyValuePair<string, Type> kvp in functionParamTables[node.AnchorToken.Lexeme]){
                if(counter != 0){
                    result += ", " + CILTypes[kvp.Value];
                }else{
                    result += CILTypes[kvp.Value];
                }
                counter++;
            }

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

        //Aqui hay que revisar si hay else
        public string Visit(If node){
            var elseBody = GenerateLabel();
            var prevEndIf = ifLabel;
            ifLabel = GenerateLabel();
            var result = Visit(((dynamic) node[0]))
            + "ldc.i4.1\n"
            + "bne.un '" + elseBody + "'\n";
           /* + Visit(((dynamic) node[1]))
            + "br " + ifLabel + "\n"
            + "'" + elseBody + "':\n"
            + Visit(((dynamic) node[2]))
            + "'" + ifLabel + "':\n";*/

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
            var result = "";
            if(localscope != null && localSymbolTables.ContainsKey(localscope) && localSymbolTables[localscope].Contains(node[0].AnchorToken.Lexeme)){
                result += Visit((dynamic) node[1])
                + "\tstloc '" + node[0].AnchorToken.Lexeme + "'\n";
                return result;
            }else if (functionParamTables.ContainsKey(node[0].AnchorToken.Lexeme)){
                result += Visit((dynamic) node[1])
                + "\tstarg '" + node[0].AnchorToken.Lexeme + "'\n";
                return result;
            }else{//global
                var isFirst  = true;
                foreach ( var n in node) {
                    if (isFirst){
                        isFirst = false;
                    } else {
                        result += Visit((dynamic)n);
                    }
                }
                result += "\tstsfld " + CILTypes[globalSymbolTable[node[0].AnchorToken.Lexeme]] + " 'ChimeraProgram'::'" + node[0].AnchorToken.Lexeme + "'\n";
                return result;
            }
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
            return VisitChildren(node);
        }

        public string Visit(ChimeraType node){
            return null;
        }

        public string Visit(ListType node){
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
           if(value <= 127){
                return "\tldc.i4.s " + value + "\n";
            }else{
                return "\tldc.i4 " + value + "\n";
            }
        }

         public string Visit(StrLiteral node){
            return "\tldstr " + node.AnchorToken.Lexeme + "\n";
        }

        public string Visit(VarDeclaration node){
            var sb = new StringBuilder();
            foreach (var entry in globalSymbolTable) {
                //If not a function
                if (!globalFunctionTable.Contains(entry.Key)) {
                    sb.Append(
                        String.Format(
                            "\t\t.field public static {0} '{1}'\n",                              
                            CILTypes[entry.Value],
                                entry.Key
                        )
                    );
                    try{
                    globalSymbolAssLoad.Add(entry.Key, String.Format(
                            "\t\tldsfld {0} 'ChimeraProgram'::'{1}'\n",                              
                            CILTypes[entry.Value],
                                entry.Key
                        ));
                    globalSymbolAssAssign.Add(entry.Key, String.Format(
                            "\t\tstsfld {0} 'ChimeraProgram'::'{1}'\n",                              
                            CILTypes[entry.Value],
                                entry.Key
                        ));
                    }catch{

                    }
                }
            }
            /*foreach (var entry in globalConstTable) {
                sb.Append(
                    String.Format(
                        "\t\t.field public static {0} '{1}'\n",                              
                        CILTypes[entry.Value],
                            entry.Key
                    )
                );
            }*/
            return sb.ToString();
        }

        public string Visit(Var node) {
            return "ESTO NO DEBERIA SALIR";
        }

        public string Visit(ConstDeclaration node) {
            var varName = node[0].AnchorToken.Lexeme;
            var type = Visit((dynamic) node[1]);
            if (localscope !=  null ) {
                try {
                    localSymbolAssLoad.Add(varName, type);
                }catch{

                }
            } else {
                try {
                    globalSymbolAssLoad.Add(varName, type);
                }catch{

                }
            }
            return "";
        }

        public string Visit(Identifier node){
            if (globalSymbolAssLoad.ContainsKey(node.AnchorToken.Lexeme)) {
                return globalSymbolAssLoad[node.AnchorToken.Lexeme];
            } 
            if (localSymbolAssLoad.ContainsKey(node.AnchorToken.Lexeme)) {
                return localSymbolAssLoad[node.AnchorToken.Lexeme];
            }
            if(functionParamTables[localscope].Contains(node.AnchorToken.Lexeme)){
                return "\tldarg " + "'" + node.AnchorToken.Lexeme + "'" + "\n";
            } else if(localSymbolTables.ContainsKey(node.AnchorToken.Lexeme)){
                return "\tldloc '" + node.AnchorToken.Lexeme + "'\n";
            }else if(globalSymbolTable.Contains(node.AnchorToken.Lexeme)){
                return "\tldsfld " + CILTypes[globalSymbolTable[node.AnchorToken.Lexeme]] + " 'ChimeraProgram'::'" + node.AnchorToken.Lexeme + "'\n";
            }else{//const table
                return "\tldsfld " + CILTypes[globalConstTable[node.AnchorToken.Lexeme]] + " 'ChimeraProgram'::'" + node.AnchorToken.Lexeme + "'\n";
            }
        }

        //operators
        //-----------------------------------------------------------
        
        public string Visit(And node){
            var result = "";
            var label1 = GenerateLabel();
            var label2 = GenerateLabel();
            foreach(var n in node){
                result += Visit((dynamic) n)
                + "\tldc.i4.1\n"
                + "\tbne.un '" + label1 + "'\n";
            }
            return result
            + "\tldc.i4.1\n"
            + "\tbr " + label2 + "\n"
            + label1 + ":\n"
            + "\tldc.i4.0\n"
            + label2 + ":\n";
        }

        public string Visit(Or node){
            var result = "";
            var trueCond = GenerateLabel();
            var falseCond = GenerateLabel();
            foreach(var n in node){
                result += Visit((dynamic) n)
                + "\tldc.i4.0\n"
                + "\tbne.un '" + trueCond + "'\n";
            }
            return result
            + "\tldc.i4.0\n"
            + "\tbr " + falseCond + "\n"
            + trueCond + ":\n"
            + "\tldc.i4.0\n"
            + falseCond + ":\n";
        }

        public string Visit(Xor node){
            var result = "";
            result += Visit((dynamic) node[0])
            + Visit((dynamic) node[1])
            + "\txor\n";
            return result;
        }

        public string Visit(Equal node){
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tceq\n";
            return result;
        }

        public string Visit(BoolIneq node){//also for int ineq
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tceq\n"
            + "\tldc.i4.0\n"
            + "\tceq\n";
            return result;
        }

        public string Visit(Less node){
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tclt\n";
            return result;
        }

        public string Visit(More node){
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tcgt\n";
            return result;
        }

        public string Visit(LessEq node){
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tcgt\n"
            + "\tldc.i4.0\n"
            + "\tceq\n";
            return result;
        }

        public string Visit(MoreEq node){
            var result = "";
            result +=
              Visit((dynamic) node[0]) + "\n"
            + Visit((dynamic) node[1]) + "\n"
            + "\tclt\n"
            + "\tldc.i4.0\n"
            + "\tceq\n";
            return result;
        }

        public string Visit(Plus node){
            return
             Visit((dynamic) node[0])
            +Visit((dynamic) node[1])+"\tadd\n";
        }

        public string Visit(Neg node){//also for substraction?
            if (node[1] != null ) {
                return Visit((dynamic) node[0]) + Visit((dynamic) node[1])+"sub\n";
            }
            return Visit((dynamic) node[0])+"neg\n";
        }

        public string Visit(Mul node){
            return Visit((dynamic) node[0])+Visit((dynamic) node[1])+"mul\n";
        }

        public string Visit(Div node){
            return Visit((dynamic) node[0])+Visit((dynamic) node[1])+"div\n";
        }

        public string Visit(Rem node){
            return Visit((dynamic) node[0])+Visit((dynamic) node[1])+"rem\n";
        }

        public string Visit(Not node){
            return Visit((dynamic) node[0])+"ldc.i4.0\nceq\n";
        }
    }
}
