//Code generated by The Chimera Compiler.
.assembly 'chimera' {}
.assembly extern 'chimeralib' {}
.class public 'ChimeraProgram' extends ['mscorlib']'System'.'Object' {
	ldstr "Hello, World!"
	call void class ['chimeralib']'Chimera'.'Utils'::'WrStr'(string)
	call void class ['chimeralib']'Chimera'.'Utils'::'WrLn'()
}
