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
.module lists.exe // GUID = {089CD59B-76F8-467D-85E2-B82611E71927}


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
           default int32 strlen (string s)  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  callvirt instance int32 string::get_Length()
	IL_0006:  ret 
    } // end of method Lists::strlen

    // method line 3
    .method public static hidebysig 
           default string newstr (string s)  cil managed 
    {
        // Method begins at RVA 0x2060
	// Code size 12 (0xc)
	.maxstack 8
	IL_0000:  ldstr "new_"
	IL_0005:  ldarg.0 
	IL_0006:  call string string::Concat(string, string)
	IL_000b:  ret 
    } // end of method Lists::newstr

    // method line 4
    .method public static hidebysig 
           default int32 len (int32[] lst)  cil managed 
    {
        // Method begins at RVA 0x206d
	// Code size 4 (0x4)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldlen 
	IL_0002:  conv.i4 
	IL_0003:  ret 
    } // end of method Lists::len

    // method line 5
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2074
	.entrypoint
	// Code size 58 (0x3a)
	.maxstack 3
	.locals init (
		int32[]	V_0,
		string	V_1)
	IL_0000:  ldc.i4.5 
	IL_0001:  newarr [mscorlib]System.Int32
	IL_0006:  dup 
	IL_0007:  ldtoken field valuetype '<PrivateImplementationDetails>'/'$ArrayType=20' '<PrivateImplementationDetails>'::'$field-1797E739BC3EBBFD41CC859F7DE7125687A94C56'
	IL_000c:  call void class [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
	IL_0011:  stloc.0 
	IL_0012:  ldloc.0 
	IL_0013:  call int32 class Lists::len(int32[])
	IL_0018:  call void class [mscorlib]System.Console::WriteLine(int32)
	IL_001d:  ldstr "Yeah"
	IL_0022:  stloc.1 
	IL_0023:  ldloc.1 
	IL_0024:  call int32 class Lists::strlen(string)
	IL_0029:  call void class [mscorlib]System.Console::WriteLine(int32)
	IL_002e:  ldloc.1 
	IL_002f:  call string class Lists::newstr(string)
	IL_0034:  call void class [mscorlib]System.Console::WriteLine(string)
	IL_0039:  ret 
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
