using System;
using System.Text;
using System.Collections.Generic;

public class Str {
	public void writeStr(string s){
		Console.WriteLine(s);
	}

	public static void Main(){
		string s = "Siegfried";
		Str.writeStr(s);
	}
}
