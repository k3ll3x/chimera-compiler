.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'callStatement'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module callStatement.exe // GUID = {410E3D23-865B-4AEE-AC75-DB9721EDEB13}


  .class public auto ansi beforefieldinit CallStatement
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
    } // end of method CallStatement::.ctor

    // method line 2
    .method public static hidebysig 
           default void y ()  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 1 (0x1)
	.maxstack 8
	IL_0000:  ret 
    } // end of method CallStatement::y

    // method line 3
    .method public static hidebysig 
           default void yy (int32 i)  cil managed 
    {
        // Method begins at RVA 0x205a
	// Code size 1 (0x1)
	.maxstack 8
	IL_0000:  ret 
    } // end of method CallStatement::yy

    // method line 4
    .method public static hidebysig 
           default int32 yyy (int32 i, int32 ii)  cil managed 
    {
        // Method begins at RVA 0x205c
	// Code size 4 (0x4)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.1 
	IL_0002:  add 
	IL_0003:  ret 
    } // end of method CallStatement::yyy

    // method line 5
    .method public static hidebysig 
           default string yyyy (string s)  cil managed 
    {
        // Method begins at RVA 0x2061
	// Code size 2 (0x2)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ret 
    } // end of method CallStatement::yyyy

    // method line 6
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2064
	.entrypoint
	// Code size 31 (0x1f)
	.maxstack 8
	IL_0000:  call void class CallStatement::y()
	IL_0005:  ldc.i4.5 
	IL_0006:  call void class CallStatement::yy(int32)
	IL_000b:  ldc.i4.4 
	IL_000c:  ldc.i4.5 
	IL_000d:  call int32 class CallStatement::yyy(int32, int32)
	IL_0012:  pop 
	IL_0013:  ldstr "hello"
	IL_0018:  call string class CallStatement::yyyy(string)
	IL_001d:  pop 
	IL_001e:  ret 
    } // end of method CallStatement::Main

  } // end of class CallStatement

