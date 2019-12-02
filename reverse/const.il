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
.module const.exe // GUID = {18E0DE9D-9C0A-410B-834B-3DD3BF177AE4}


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
	// Code size 60 (0x3c)
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
	IL_0025:  ldc.i4.1 
	IL_0026:  call bool class Const::constMethodStr(bool)
	IL_002b:  call void class [mscorlib]System.Console::Write(bool)
	IL_0030:  ldc.i4.1 
	IL_0031:  call bool class Const::constMethodStr(bool)
	IL_0036:  call void class [mscorlib]System.Console::Write(bool)
	IL_003b:  ret 
    } // end of method Const::Main

    // method line 3
    .method public static hidebysig 
           default int32 constMethodInt (int32 a, string b)  cil managed 
    {
        // Method begins at RVA 0x20a0
	// Code size 39 (0x27)
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
	IL_000c:  ldloc.0 
	IL_000d:  neg 
	IL_000e:  call void class [mscorlib]System.Console::Write(int32)
	IL_0013:  ldarg.1 
	IL_0014:  call void class [mscorlib]System.Console::Write(string)
	IL_0019:  ldloc.1 
	IL_001a:  ldsfld int32 Const::i3
	IL_001f:  add 
	IL_0020:  call void class [mscorlib]System.Console::Write(int32)
	IL_0025:  ldc.i4.0 
	IL_0026:  ret 
    } // end of method Const::constMethodInt

    // method line 4
    .method public static hidebysig 
           default bool constMethodStr (bool c)  cil managed 
    {
        // Method begins at RVA 0x20d3
	// Code size 5 (0x5)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldc.i4.0 
	IL_0002:  ceq 
	IL_0004:  ret 
    } // end of method Const::constMethodStr

  } // end of class Const

