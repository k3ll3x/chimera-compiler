using System;
using System.Text;
using System.Collections.Generic;

public class Str {
	public static int g = 42;
	public static void localVars(){
		int x = 3;
		int y = x + x;
	}
	
	public static void localVars2(){
		int x = 32;
	}
	
	public static void localVars3(){
		string x = "yeah";
	}
	
	public static void localVars4(int z){
		int x = z + 2;
	}
	
	public static void callGlobal(){
		int x = g;
	}
	
	public static void Main(){
		callGlobal();
	}
}
