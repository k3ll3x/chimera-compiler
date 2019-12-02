using System;
using System.Text;

public class Const {
    int i3 ;
	public static void Main(){
		const int i = 10;
        const string s = "hola";
        const bool b = false;
        int i2 = 11;

        i2 = 12;

        Console.Write(i);
        Console.Write(i +i2);
        Console.Write(i2);
        Console.Write(s);

        Console.Write(b);

	}

    public int constMethodInt(int a, string b) {
        const int i = 10;
        int i2 = 11;
        int i4 = 11;
        string i5 = "ho";
        Console.Write(i+i2);

        Console.Write(b);
        return 0;
    }

    public string constMethodStr(int c){
        return "";
    }
}
