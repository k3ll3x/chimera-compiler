using System;
using System.Text;
using System.Collections.Generic;

public class CallStatement {
	public static void y(){}
	public static void yy(int i){}
	public static int yyy(int i){
		return i;
	}
	public static string yyyy(string s){
		return s;
	}
	
	public static void Main(){
		y();
		yy(5);
		yyy(4);
		yyyy("hello");
	}
}
