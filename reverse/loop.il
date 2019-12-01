.assembly extern mscorlib
{
  .ver 4:0:0:0
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 ) // .z\V.4..
}
.assembly 'loop'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module loop.exe // GUID = {F99BD107-4CB6-47E0-B445-FF0DEABBE599}


  .class public auto ansi beforefieldinit Loop
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
    } // end of method Loop::.ctor

    // method line 2
    .method public static hidebysig 
           default void Main ()  cil managed 
    {
        // Method begins at RVA 0x2058
	.entrypoint
	// Code size 85 (0x55)
	.maxstack 2
	.locals init (
		class [mscorlib]System.Collections.Generic.List`1<int32>	V_0,
		int32	V_1,
		valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>	V_2)
	IL_0000:  newobj instance void class [mscorlib]System.Collections.Generic.List`1<int32>::'.ctor'()
	IL_0005:  stloc.0 
	IL_0006:  ldloc.0 
	IL_0007:  ldc.i4.1 
	IL_0008:  callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Add(!0)
	IL_000d:  ldloc.0 
	IL_000e:  ldc.i4.1 
	IL_000f:  callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Add(!0)
	IL_0014:  ldloc.0 
	IL_0015:  ldc.i4.1 
	IL_0016:  callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Add(!0)
	IL_001b:  ldloc.0 
	IL_001c:  callvirt instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<int32>::GetEnumerator()
	IL_0021:  stloc.2 
	.try { // 0
	  IL_0022:  br IL_0035

	  IL_0027:  ldloca.s 2
	  IL_0029:  call instance !0 valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::get_Current()
	  IL_002e:  stloc.1 
	  IL_002f:  ldloc.1 
	  IL_0030:  call void class [mscorlib]System.Console::WriteLine(int32)
	  IL_0035:  ldloca.s 2
	  IL_0037:  call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()
	  IL_003c:  brtrue IL_0027

	  IL_0041:  leave IL_0054

	} // end .try 0
	finally  { // 0
	  IL_0046:  ldloca.s 2
	  IL_0048:  constrained. valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>
	  IL_004e:  callvirt instance void class [mscorlib]System.IDisposable::Dispose()
	  IL_0053:  endfinally 
	} // end handler 0
	IL_0054:  ret 
    } // end of method Loop::Main

  } // end of class Loop

