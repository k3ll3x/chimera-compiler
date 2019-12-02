using System;
using System.Text;

public class Const {
    static int i3 ;
    static string hi;
    static int i4;
	public static void Main(){
		const int i = 10;
        const string s = "hola";
        const bool b = true;
        int i2 = 11;

        i3 = 12;
        hi = s;

        Console.Write(constMethodInt(i,s));
        Console.Write(constMethodStr(b));

                Console.Write(constMethodStr(b));

	}

    public static int constMethodInt(int a, string b) {
        const int i = 10;


        int i2 = 11;
        int i4 = 11;
        string s5 = "ho";

        Console.Write(-i2);
        Console.Write(b);
        Console.Write(i4 + i3);
        return 0;
    }

    public static bool constMethodStr(bool c){
        return !c;
    }
}
