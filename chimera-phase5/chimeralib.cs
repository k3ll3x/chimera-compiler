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
        public static int WrInt(int n){
            Console.Write(i);
            return 0;
        }

        public static int WrInt(string n){
            Console.Write(i);
            return 0;
        }

        public static int WrBool(int n){
            if(n == 1){
                Console.Write("true");
            }else if(n == 0){
                Console.Write("false")
            }
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
     }
 }