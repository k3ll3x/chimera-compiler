using System;
using System.Text;

public class Const {
    static int i3 ;
    static string hi;
    static int i4;
	public static void Main(){
		const int i = 10;
        const string s = "hola";
        const bool b = false;
        int i2 = 11;

        i3 = 12;
        hi = s;

        Console.Write(constMethodInt(i,s));
        Console.Write(constMethodStr(hi));

                Console.Write(constMethodStr(s));

	}

    public static int constMethodInt(int a, string b) {
        const int i = 10;


        int i2 = 11;
        int i4 = 11;
        string s5 = "ho";

        Console.Write(i+i2);
        Console.Write(b);
        Console.Write(i4 + i3);
        return 0;
    }

    public static string constMethodStr(string c){
        return c;
    }
}
