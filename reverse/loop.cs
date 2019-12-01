using System;
using System.Text;
using System.Collections.Generic;

public class Loop {
	public static void Main(){
		var n = new List<int>();
		n.Add(1);
		n.Add(1);
		n.Add(1);
		
		foreach(var i in n){
			Console.WriteLine(i);
		}
	}
}
