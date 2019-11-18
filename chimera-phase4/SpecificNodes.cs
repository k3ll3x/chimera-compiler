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
 
namespace Chimera {    

    class Program: Node {}

    class DeclarationList: Node {}

    class VarDeclaration: Node {}

    class Var: Node {}
    class ConstList: Node {}
    class ConstDeclaration: Node {}
    class ParamDeclaration: Node{}

    class ProcDeclaration: Node {}
    class ProcStatement: Node {}
    class ProcVar: Node {}
    class ProcConst: Node {}
    class ProcParam: Node {}

    class StatementList: Node {}

    class Assignment: Node {}

    class If: Node {}
    class Then: Node {}
    class ElseIf: Node {}
    class Else: Node {}
    
    class Loop: Node {}

    class For: Node {}
    class Do: Node {}

    class CallStatement: Node {}

    class CallExpression: Node {}

    class Identifier: Node {}
    class IdentifierList: Node {}

    class IntLiteral: Node {}
    
    class StrLiteral: Node {}
    class List: Node {}
    class ChimeraType: Node {}
    class ListType: Node {}

    class True: Node {}
    class False: Node {}

    class Neg: Node {}
    class Not: Node {}

    class And: Node {}
    class Or: Node {}
    class Xor: Node {}
    class Equal: Node {}
    class BoolIneq: Node {}
    class LessEq: Node {}
    class Less: Node {}
    class More: Node {}
    class MoreEq: Node {}

    class Plus: Node {}

    class Mul: Node {}
    class Div: Node {}
    class Rem: Node {}
    class Exit: Node {}
    class Return: Node {}
    class Expression: Node {}
}