.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'const'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module const.exe // GUID = {FAAA79EF-F51A-4EA1-85CC-87F1B870B57A}


  .class public auto ansi beforefieldinit Const
  	extends [mscorlib]System.Object
  {
    .field  private static  int32 i3
    .field  private static  string hi
    .field  private static  int32 i4

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
    } // end of method Const::.ctor

    // method line 2
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2058
	.entrypoint
	// Code size 68 (0x44)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.s 0x0b
	IL_0002:  stloc.0 
	IL_0003:  ldc.i4.s 0x0c
	IL_0005:  stsfld int32 Const::i3
	IL_000a:  ldstr "hola"
	IL_000f:  stsfld string Const::hi
	IL_0014:  ldc.i4.s 0x0a
	IL_0016:  ldstr "hola"
	IL_001b:  call int32 class Const::constMethodInt(int32, string)
	IL_0020:  call void class [mscorlib]System.Console::Write(int32)
	IL_0025:  ldsfld string Const::hi
	IL_002a:  call string class Const::constMethodStr(string)
	IL_002f:  call void class [mscorlib]System.Console::Write(string)
	IL_0034:  ldstr "hola"
	IL_0039:  call string class Const::constMethodStr(string)
	IL_003e:  call void class [mscorlib]System.Console::Write(string)
	IL_0043:  ret 
    } // end of method Const::Main

    // method line 3
    .method public static hidebysig 
           default int32 constMethodInt (int32 a, string b)  cil managed 
    {
        // Method begins at RVA 0x20a8
	// Code size 41 (0x29)
	.maxstack 2
	.locals init (
		int32	V_0,
		int32	V_1,
		string	V_2)
	IL_0000:  ldc.i4.s 0x0b
	IL_0002:  stloc.0 
	IL_0003:  ldc.i4.s 0x0b
	IL_0005:  stloc.1 
	IL_0006:  ldstr "ho"
	IL_000b:  stloc.2 
	IL_000c:  ldc.i4.s 0x0a
	IL_000e:  ldloc.0 
	IL_000f:  add 
	IL_0010:  call void class [mscorlib]System.Console::Write(int32)
	IL_0015:  ldarg.1 
	IL_0016:  call void class [mscorlib]System.Console::Write(string)
	IL_001b:  ldloc.1 
	IL_001c:  ldsfld int32 Const::i3
	IL_0021:  add 
	IL_0022:  call void class [mscorlib]System.Console::Write(int32)
	IL_0027:  ldc.i4.0 
	IL_0028:  ret 
    } // end of method Const::constMethodInt

    // method line 4
    .method public static hidebysig 
           default string constMethodStr (string c)  cil managed 
    {
        // Method begins at RVA 0x20dd
	// Code size 2 (0x2)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ret 
    } // end of method Const::constMethodStr

  } // end of class Const

