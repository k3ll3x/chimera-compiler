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
        private static int currentId = 0;
        private static Dictionary<int, List<int>> handles = new Dictionary<int, List<int>>();

        public static int WrInt(int n){
            Console.Write(n);
            return 0;
        }

        public static int WrStr(string s){
            /*chHandle(s);
            StringBuilder builder = new StringBuilder();
            for(int i = 0, n = LenStr(s); i < n; i++){
                builder.Append(char.ConvertFromUtf32(Get(s,i)));
            }
            Console.Write(builder.ToString());
            return 0;*/
            Console.WriteLine(s);
            return 0;
        }

        public static int WrBool(bool n){
            Console.Write(n ? "true" : "false");
            return 0;
        }

        public static int WrLn(){
            Console.WriteLine();
            return 0;
        }

        public static int RdInt(){
            string userInput;
            int n;
            do {
                userInput = Console.ReadLine();
            }while(!int.TryParse(userInput, out n));
            return n;
        }

        public static int RdStr(){
            string userInput = Console.ReadLine();
            return userInput;
        }

        public static int LenStr(string s){
            return s.Length;
        }
     }
 }