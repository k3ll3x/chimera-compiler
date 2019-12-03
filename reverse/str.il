.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'str'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module str.exe // GUID = {377E896D-F63F-457F-9417-0ABD3D17DD39}


  .class public auto ansi beforefieldinit Str
  	extends [mscorlib]System.Object
  {
    .field  public static  int32 g

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
    } // end of method Str::.ctor

    // method line 2
    .method public static hidebysig 
           default void localVars ()  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 7 (0x7)
	.maxstack 2
	.locals init (
		int32	V_0,
		int32	V_1)
	IL_0000:  ldc.i4.3 
	IL_0001:  stloc.0 
	IL_0002:  ldloc.0 
	IL_0003:  ldloc.0 
	IL_0004:  add 
	IL_0005:  stloc.1 
	IL_0006:  ret 
    } // end of method Str::localVars

    // method line 3
    .method public static hidebysig 
           default void localVars2 ()  cil managed 
    {
        // Method begins at RVA 0x206c
	// Code size 4 (0x4)
	.maxstack 1
	.locals init (
		int32	V_0)
	IL_0000:  ldc.i4.s 0x20
	IL_0002:  stloc.0 
	IL_0003:  ret 
    } // end of method Str::localVars2

    // method line 4
    .method public static hidebysig 
           default void localVars3 ()  cil managed 
    {
        // Method begins at RVA 0x207c
	// Code size 7 (0x7)
	.maxstack 1
	.locals init (
		string	V_0)
	IL_0000:  ldstr "yeah"
	IL_0005:  stloc.0 
	IL_0006:  ret 
    } // end of method Str::localVars3

    // method line 5
    .method public static hidebysig 
           default void localVars4 (int32 z)  cil managed 
    {
        // Method begins at RVA 0x2090
	// Code size 5 (0x5)
	.maxstack 2
	.locals init (
		int32	V_0)
	IL_0000:  ldarg.0 
	IL_0001:  ldc.i4.2 
	IL_0002:  add 
	IL_0003:  stloc.0 
	IL_0004:  ret 
    } // end of method Str::localVars4

    // method line 6
    .method public static hidebysig 
           default void callGlobal ()  cil managed 
    {
        // Method begins at RVA 0x20a4
	// Code size 7 (0x7)
	.maxstack 1
	.locals init (
		int32	V_0)
	IL_0000:  ldsfld int32 Str::g
	IL_0005:  stloc.0 
	IL_0006:  ret 
    } // end of method Str::callGlobal

    // method line 7
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x20b7
	.entrypoint
	// Code size 6 (0x6)
	.maxstack 8
	IL_0000:  call void class Str::callGlobal()
	IL_0005:  ret 
    } // end of method Str::Main

    // method line 8
    .method private static hidebysig specialname rtspecialname 
           default void '.cctor' ()  cil managed 
    {
        // Method begins at RVA 0x20be
	// Code size 8 (0x8)
	.maxstack 8
	IL_0000:  ldc.i4.s 0x2a
	IL_0002:  stsfld int32 Str::g
	IL_0007:  ret 
    } // end of method Str::.cctor

  } // end of class Str

