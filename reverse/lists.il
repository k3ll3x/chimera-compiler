.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'lists'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module lists.exe // GUID = {A324C78A-04E1-4ABF-81F0-EE3F140FCB6E}


  .class public auto ansi beforefieldinit Lists
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
    } // end of method Lists::.ctor

    // method line 2
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2058
	.entrypoint
	// Code size 19 (0x13)
	.maxstack 3
	.locals init (
		int32[]	V_0)
	IL_0000:  ldc.i4.5 
	IL_0001:  newarr [mscorlib]System.Int32
	IL_0006:  dup 
	IL_0007:  ldtoken field valuetype '<PrivateImplementationDetails>'/'$ArrayType=20' '<PrivateImplementationDetails>'::'$field-1797E739BC3EBBFD41CC859F7DE7125687A94C56'
	IL_000c:  call void class [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
	IL_0011:  stloc.0 
	IL_0012:  ret 
    } // end of method Lists::Main

  } // end of class Lists

  .class private auto ansi abstract sealed beforefieldinit '<PrivateImplementationDetails>'
  	extends [mscorlib]System.Object
  {
    .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....

    .field  assembly static initonly  valuetype '<PrivateImplementationDetails>'/'$ArrayType=20' '$field-1797E739BC3EBBFD41CC859F7DE7125687A94C56' at D_00004000

  .class nested private sequential ansi sealed beforefieldinit '$ArrayType=20'
  	extends [mscorlib]System.ValueType
  {
    .pack 1
    .size 20

  } // end of class $ArrayType=20

  } // end of class <PrivateImplementationDetails>

.data D_00004000 = bytearray (
	 02 00 00 00 04 00 00 00 06 00 00 00 07 00 00 00
	 22 00 00 00) // size: 20
