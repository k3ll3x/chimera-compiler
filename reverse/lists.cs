using System;
using System.Text;
using System.Collections.Generic;

public class Lists {
	public static int strlen(string s){
		return s.Length;
	}
	
	public static string newstr(string s){
		return "new_" + s;
	}

	public static int len(int[] lst){
		return lst.Length;
	}

	public static void Main(){
		int[] lst = {2,4,6,7,34};
		Console.WriteLine(len(lst));
		string s = "Yeah";
		Console.WriteLine(strlen(s));
		Console.WriteLine(newstr(s));
	}
}
