.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'chimeraApi'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module chimeraApi.exe // GUID = {F173BB47-E181-4FB5-9584-C3B0E3ED072D}


.namespace Chimera
{
  .class public auto ansi beforefieldinit Utils
  	extends [mscorlib]System.Object
  {

    // method line 1
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x2050
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void object::'.ctor'()
	IL_0006:  ret 
    } // end of method Utils::.ctor

    // method line 2
    .method public static hidebysig 
           default void WrInt (int32 n)  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call void class [mscorlib]System.Console::Write(int32)
	IL_0006:  ret 
    } // end of method Utils::WrInt

    // method line 3
    .method public static hidebysig 
           default void WrStr (string s)  cil managed 
    {
        // Method begins at RVA 0x2060
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call void class [mscorlib]System.Console::Write(string)
	IL_0006:  ret 
    } // end of method Utils::WrStr

    // method line 4
    .method public static hidebysig 
           default void WrBool (bool n)  cil managed 
    {
        // Method begins at RVA 0x2068
	// Code size 27 (0x1b)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  brfalse IL_0010

	IL_0006:  ldstr "true"
	IL_000b:  br IL_0015

	IL_0010:  ldstr "false"
	IL_0015:  call void class [mscorlib]System.Console::Write(string)
	IL_001a:  ret 
    } // end of method Utils::WrBool

    // method line 5
    .method public static hidebysig 
           default void WrLn ()  cil managed 
    {
        // Method begins at RVA 0x2084
	// Code size 6 (0x6)
	.maxstack 8
	IL_0000:  call void class [mscorlib]System.Console::WriteLine()
	IL_0005:  ret 
    } // end of method Utils::WrLn

    // method line 6
    .method public static hidebysig 
           default int32 RdInt ()  cil managed 
    {
        // Method begins at RVA 0x208c
	// Code size 21 (0x15)
	.maxstack 2
	.locals init (
		string	V_0,
		int32	V_1)
	IL_0000:  call string class [mscorlib]System.Console::ReadLine()
	IL_0005:  stloc.0 
	IL_0006:  ldloc.0 
	IL_0007:  ldloca.s 1
	IL_0009:  call bool int32::TryParse(string, [out] int32&)
	IL_000e:  brfalse IL_0000

	IL_0013:  ldloc.1 
	IL_0014:  ret 
    } // end of method Utils::RdInt

    // method line 7
    .method public static hidebysig 
           default string RdStr ()  cil managed 
    {
        // Method begins at RVA 0x20b0
	// Code size 8 (0x8)
	.maxstack 1
	.locals init (
		string	V_0)
	IL_0000:  call string class [mscorlib]System.Console::ReadLine()
	IL_0005:  stloc.0 
	IL_0006:  ldloc.0 
	IL_0007:  ret 
    } // end of method Utils::RdStr

    // method line 8
    .method public static hidebysig 
           default string AtStr (string s, int32 i)  cil managed 
    {
        // Method begins at RVA 0x20c4
	// Code size 68 (0x44)
	.maxstack 3
	.locals init (
		string	V_0,
		int32	V_1)
	IL_0000:  ldstr ""
	IL_0005:  stloc.0 
	IL_0006:  ldarg.1 
	IL_0007:  ldarg.0 
	IL_0008:  callvirt instance int32 string::get_Length()
	IL_000d:  blt IL_0018

	IL_0012:  newobj instance void class [mscorlib]System.IndexOutOfRangeException::'.ctor'()
	IL_0017:  throw 
	IL_0018:  ldarg.1 
	IL_0019:  stloc.1 
	IL_001a:  br IL_0036

	IL_001f:  ldloc.0 
	IL_0020:  ldarg.0 
	IL_0021:  ldloc.1 
	IL_0022:  callvirt instance char string::get_Chars(int32)
	IL_0027:  box [mscorlib]System.Char
	IL_002c:  call string string::Concat(object, object)
	IL_0031:  stloc.0 
	IL_0032:  ldloc.1 
	IL_0033:  ldc.i4.1 
	IL_0034:  add 
	IL_0035:  stloc.1 
	IL_0036:  ldloc.1 
	IL_0037:  ldarg.0 
	IL_0038:  callvirt instance int32 string::get_Length()
	IL_003d:  blt IL_001f

	IL_0042:  ldloc.0 
	IL_0043:  ret 
    } // end of method Utils::AtStr

    // method line 9
    .method public static hidebysig 
           default int32 LenStr (string s)  cil managed 
    {
        // Method begins at RVA 0x2114
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  callvirt instance int32 string::get_Length()
	IL_0006:  ret 
    } // end of method Utils::LenStr

    // method line 10
    .method public static hidebysig 
           default int32 CmpStr (string s1, string s2)  cil managed 
    {
        // Method begins at RVA 0x211c
	// Code size 8 (0x8)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.1 
	IL_0002:  call int32 string::Compare(string, string)
	IL_0007:  ret 
    } // end of method Utils::CmpStr

    // method line 11
    .method public static hidebysig 
           default string CatStr (string s1, string s2)  cil managed 
    {
        // Method begins at RVA 0x2125
	// Code size 8 (0x8)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.1 
	IL_0002:  call string string::Concat(string, string)
	IL_0007:  ret 
    } // end of method Utils::CatStr

    // method line 12
    .method public static hidebysig 
           default int32 LenLstInt (int32[] loi)  cil managed 
    {
        // Method begins at RVA 0x212e
	// Code size 4 (0x4)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldlen 
	IL_0002:  conv.i4 
	IL_0003:  ret 
    } // end of method Utils::LenLstInt

    // method line 13
    .method public static hidebysig 
           default int32 LenLstInt (string[] los)  cil managed 
    {
        // Method begins at RVA 0x2133
	// Code size 4 (0x4)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldlen 
	IL_0002:  conv.i4 
	IL_0003:  ret 
    } // end of method Utils::LenLstInt

    // method line 14
    .method public static hidebysig 
           default int32 LenLstBool (int32[] lob)  cil managed 
    {
        // Method begins at RVA 0x2138
	// Code size 4 (0x4)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldlen 
	IL_0002:  conv.i4 
	IL_0003:  ret 
    } // end of method Utils::LenLstBool

    // method line 15
    .method public static hidebysig 
           default int32[] NewLstInt (int32 size)  cil managed 
    {
        // Method begins at RVA 0x2140
	// Code size 33 (0x21)
	.maxstack 3
	.locals init (
		int32[]	V_0,
		int32	V_1)
	IL_0000:  ldarg.0 
	IL_0001:  newarr [mscorlib]System.Int32
	IL_0006:  stloc.0 
	IL_0007:  ldc.i4.0 
	IL_0008:  stloc.1 
	IL_0009:  br IL_0016

	IL_000e:  ldloc.0 
	IL_000f:  ldloc.1 
	IL_0010:  ldc.i4.0 
	IL_0011:  stelem.i4 
	IL_0012:  ldloc.1 
	IL_0013:  ldc.i4.1 
	IL_0014:  add 
	IL_0015:  stloc.1 
	IL_0016:  ldloc.1 
	IL_0017:  ldloc.0 
	IL_0018:  ldlen 
	IL_0019:  conv.i4 
	IL_001a:  blt IL_000e

	IL_001f:  ldloc.0 
	IL_0020:  ret 
    } // end of method Utils::NewLstInt

    // method line 16
    .method public static hidebysig 
           default string[] NewLstStr (int32 size)  cil managed 
    {
        // Method begins at RVA 0x2170
	// Code size 37 (0x25)
	.maxstack 3
	.locals init (
		string[]	V_0,
		int32	V_1)
	IL_0000:  ldarg.0 
	IL_0001:  newarr [mscorlib]System.String
	IL_0006:  stloc.0 
	IL_0007:  ldc.i4.0 
	IL_0008:  stloc.1 
	IL_0009:  br IL_001a

	IL_000e:  ldloc.0 
	IL_000f:  ldloc.1 
	IL_0010:  ldstr ""
	IL_0015:  stelem.ref 
	IL_0016:  ldloc.1 
	IL_0017:  ldc.i4.1 
	IL_0018:  add 
	IL_0019:  stloc.1 
	IL_001a:  ldloc.1 
	IL_001b:  ldloc.0 
	IL_001c:  ldlen 
	IL_001d:  conv.i4 
	IL_001e:  blt IL_000e

	IL_0023:  ldloc.0 
	IL_0024:  ret 
    } // end of method Utils::NewLstStr

    // method line 17
    .method public static hidebysig 
           default int32[] NewLstBool (int32 size)  cil managed 
    {
        // Method begins at RVA 0x21a4
	// Code size 33 (0x21)
	.maxstack 3
	.locals init (
		int32[]	V_0,
		int32	V_1)
	IL_0000:  ldarg.0 
	IL_0001:  newarr [mscorlib]System.Int32
	IL_0006:  stloc.0 
	IL_0007:  ldc.i4.0 
	IL_0008:  stloc.1 
	IL_0009:  br IL_0016

	IL_000e:  ldloc.0 
	IL_000f:  ldloc.1 
	IL_0010:  ldc.i4.0 
	IL_0011:  stelem.i4 
	IL_0012:  ldloc.1 
	IL_0013:  ldc.i4.1 
	IL_0014:  add 
	IL_0015:  stloc.1 
	IL_0016:  ldloc.1 
	IL_0017:  ldloc.0 
	IL_0018:  ldlen 
	IL_0019:  conv.i4 
	IL_001a:  blt IL_000e

	IL_001f:  ldloc.0 
	IL_0020:  ret 
    } // end of method Utils::NewLstBool

    // method line 18
    .method public static hidebysig 
           default string IntToStr (int32 i)  cil managed 
    {
        // Method begins at RVA 0x21d1
	// Code size 14 (0xe)
	.maxstack 8
	IL_0000:  ldarga.s 0
	IL_0002:  constrained. [mscorlib]System.Int32
	IL_0008:  callvirt instance string object::ToString()
	IL_000d:  ret 
    } // end of method Utils::IntToStr

    // method line 19
    .method public static hidebysig 
           default int32 StrToInt (string s)  cil managed 
    {
        // Method begins at RVA 0x21e0
	// Code size 28 (0x1c)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.0 
	IL_0001:  stloc.0 
	IL_0002:  ldarg.0 
	IL_0003:  ldloca.s 0
	IL_0005:  call bool int32::TryParse(string, [out] int32&)
	IL_000a:  brtrue IL_001a

	IL_000f:  ldstr "Convertsion cannot be carried out!\n"
	IL_0014:  newobj instance void class [mscorlib]System.Exception::'.ctor'(string)
	IL_0019:  throw 
	IL_001a:  ldloc.0 
	IL_001b:  ret 
    } // end of method Utils::StrToInt

    // method line 20
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2208
	.entrypoint
	// Code size 9 (0x9)
	.maxstack 8
	IL_0000:  ldc.i4.s 0x20
	IL_0002:  call string class Chimera.Utils::IntToStr(int32)
	IL_0007:  pop 
	IL_0008:  ret 
    } // end of method Utils::Main

  } // end of class Chimera.Utils
}

