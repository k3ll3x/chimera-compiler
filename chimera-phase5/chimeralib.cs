/* 
* Siegfried Paul Keller Schippner A01375356
* José Javier Rodríguez Mota A01372812
* Ana Paula Mejía Quiroz A01371880
*/

using System;
using System.Text;
using System.Collections.Generic;

namespace Chimera {
    public class Utils {
        //I/O
        public static void WrInt(int n){
            Console.Write(n);
        }

        public static void WrStr(string s){
            Console.Write(s);
        }

        public static void WrBool(bool n){
            Console.Write(n ? "true" : "false");
        }

        public static void WrBool(int n){
            Console.Write(n == 1 ? "true" : "false");
        }

        public static void WrLn(){
            Console.WriteLine();
        }

        public static int RdInt(){
            string userInput;
            int n;
            do {
                userInput = Console.ReadLine();
            }while(!int.TryParse(userInput, out n));
            return n;
        }

        public static string RdStr(){
            string userInput = Console.ReadLine();
            return userInput;
        }

        //String operations
        public static string AtStr(string s, int i){
            string ns = "";
            if(i>=s.Length){
                throw new IndexOutOfRangeException();
            }else{
                for(int j=i; j<s.Length; j++){
                    ns += s[j];
                }
            }
            return ns;
        }

        public static int LenStr(string s){
            return s.Length;
        }

        public static int CmpStr(string s1, string s2){
            return string.Compare(s1,s2);
        }

        public static string CatStr(string s1, string s2){
            return s1+s2;
        }

        //list operations
        public static int LenLstInt(int[] loi){
            return loi.Length;
        }

        public static int LenLstInt(string[] los){
            return los.Length;
        }

        public static int LenLstBool(int[] lob){
            return lob.Length;
        }

        public static int[] NewLstInt(int size){
            int[] array = new int[size];
            for(int i = 0; i < array.Length; i++){
                array[i] = 0;
            }
            return array;
        }

        public static string[] NewLstStr(int size){
            string[] array = new string[size];
            for(int i = 0; i < array.Length; i++){
                array[i] = "";
            }
            return array;
        }

        public static int[] NewLstBool(int size){
            int[] array = new int[size];
            for(int i = 0; i < array.Length; i++){
                array[i] = 0;
            }
            return array;
        }

        //Conversion operations
        public static string IntToStr(int i){
            return i.ToString();
        }

        public static int StrToInt(string s){
            int i=0;
            if(!Int32.TryParse(s, out i)){
                throw new Exception("Convertsion cannot be carried out!\n");
            }
            return i;
        }
     }
 }