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
.module const.exe // GUID = {B8554B14-8FFA-48C1-8B65-E9FDE75F03B8}


  .class public auto ansi beforefieldinit Const
  	extends [mscorlib]System.Object
  {
    .field  private  int32 i3

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
	// Code size 45 (0x2d)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.s 0x0b
	IL_0002:  stloc.0 
	IL_0003:  ldc.i4.s 0x0c
	IL_0005:  stloc.0 
	IL_0006:  ldc.i4.s 0x0a
	IL_0008:  call void class [mscorlib]System.Console::Write(int32)
	IL_000d:  ldc.i4.s 0x0a
	IL_000f:  ldloc.0 
	IL_0010:  add 
	IL_0011:  call void class [mscorlib]System.Console::Write(int32)
	IL_0016:  ldloc.0 
	IL_0017:  call void class [mscorlib]System.Console::Write(int32)
	IL_001c:  ldstr "hola"
	IL_0021:  call void class [mscorlib]System.Console::Write(string)
	IL_0026:  ldc.i4.0 
	IL_0027:  call void class [mscorlib]System.Console::Write(bool)
	IL_002c:  ret 
    } // end of method Const::Main

    // method line 3
    .method public hidebysig 
           instance default int32 constMethodInt (int32 a, string b)  cil managed 
    {
        // Method begins at RVA 0x2094
	// Code size 29 (0x1d)
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
	IL_0015:  ldarg.2 
	IL_0016:  call void class [mscorlib]System.Console::Write(string)
	IL_001b:  ldc.i4.0 
	IL_001c:  ret 
    } // end of method Const::constMethodInt

    // method line 4
    .method public hidebysig 
           instance default string constMethodStr (int32 c)  cil managed 
    {
        // Method begins at RVA 0x20bd
	// Code size 6 (0x6)
	.maxstack 8
	IL_0000:  ldstr ""
	IL_0005:  ret 
    } // end of method Const::constMethodStr

  } // end of class Const

