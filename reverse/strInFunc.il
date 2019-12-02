.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'strInFunc'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module strInFunc.exe // GUID = {DA282F65-8307-41EC-85FA-1C15530B8225}


  .class public auto ansi beforefieldinit Str
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
    } // end of method Str::.ctor

    // method line 2
    .method private hidebysig 
           instance default void writeStr (string s)  cil managed 
    {
        // Method begins at RVA 0x2058
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.1 
	IL_0001:  call void class [mscorlib]System.Console::WriteLine(string)
	IL_0006:  ret 
    } // end of method Str::writeStr

    // method line 3
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2060
	.entrypoint
	// Code size 7 (0x7)
	.maxstack 1
	.locals init (
		string	V_0)
	IL_0000:  ldstr "Siegfried"
	IL_0005:  stloc.0 
	IL_0006:  ret 
    } // end of method Str::Main

  } // end of class Str

