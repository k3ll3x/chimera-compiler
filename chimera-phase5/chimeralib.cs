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

        public static int WrStr(int s){
            chHandle(s);
            StringBuilder builder = new StringBuilder();
            for(int i = 0, n = Size(s); i < n; i++){
                builder.Append(char.ConvertFromUtf32(Get(s,i)));
            }
            Console.Write(builder.ToString());
            return 0;
        }

        public static int WrBool(bool n){
            Console.Write(n ? "true" : "false");
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
            int n = New(0);
            foreach(int i in AsCodePoints(userInput)){
                Add(n,i);
            }
            return n;
        }

        public static int LenStr(int h){
            chHandle(h);
            return handles[h].Count;
        }

        public static int New(int n){
            if(n < 0){
                throw new Exception("Error, negative size array!");
            }
            int h = currentId++;
            handles.Add(h, new List<int>());
            for(int i = 0; i < n; i++){
                Add(h,0);
            }
            return h;
        }

        public static int Add(int h, int i){
            chHandle(h);
            handles[h].Add(i);
            return 0;
        }

        private static void chHandle(int h){
            if(!handles.ContainsKey(h)){
                throw new Exception("Not valid array handle!");
            }
        }

        public static int Size(int h){
            chHandle(h);
            return handles[h].Count;
        }

        public static int Get(int h, int i){
            chHandle(h);
            return handles[h][i];
        }

        private static IEnumerable<int> AsCodePoints(string str){
            for(int i = 0; i < str.Length; i++){
                yield return char.ConvertToUtf32(str, i);
                if(char.IsHighSurrogate(str,i)){
                    i++;
                }
            }
        }
     }
 }