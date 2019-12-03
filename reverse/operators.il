.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'operators'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module operators.exe // GUID = {D39A3016-DD8F-4A18-9413-091AC4547DDB}


  .class public auto ansi beforefieldinit Operators
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
    } // end of method Operators::.ctor

    // method line 2
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2058
	.entrypoint
	// Code size 13 (0xd)
	.maxstack 2
	.locals init (
		int32	V_0,
		int32	V_1,
		bool	V_2)
	IL_0000:  ldc.i4.5 
	IL_0001:  stloc.0 
	IL_0002:  ldc.i4.8 
	IL_0003:  stloc.1 
	IL_0004:  ldloc.0 
	IL_0005:  ldloc.1 
	IL_0006:  clt 
	IL_0008:  ldc.i4.0 
	IL_0009:  ceq 
	IL_000b:  stloc.2 
	IL_000c:  ret 
    } // end of method Operators::Main

  } // end of class Operators

