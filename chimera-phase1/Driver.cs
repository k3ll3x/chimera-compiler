/*
  Chimera compiler - Program driver.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/* 
 * Siegfried Paul Keller Schippner A01375356
 * José Javier Rodríguez Mota A01372812
 * Ana Paula Mejía Quiroz A01371880
 */

using System;
using System.IO;
using System.Text;

namespace Chimera1 {

    public class Driver {

        const string VERSION = "0.1";

        //-----------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis"
        };

        //-----------------------------------------------------------
        void PrintAppHeader() {
            Console.WriteLine("Chimera compiler, version " + VERSION);
            Console.WriteLine("Copyright \u00A9 2013 by A. Ortiz, ITESM CEM."                
            );
            Console.WriteLine("This program is free software; you may "
                + "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public License version 3 or "
                + "later.");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        //-----------------------------------------------------------
        void PrintReleaseIncludes() {
            Console.WriteLine("Included in this release:");            
            foreach (var phase in ReleaseIncludes) {
                Console.WriteLine("   * " + phase);
            }
        }

        //-----------------------------------------------------------
        void Run(string[] args) {

            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if (args.Length != 1) {
                Console.Error.WriteLine(
                    "Please specify the name of the input file.");
                Environment.Exit(1);
            }

            try {            
                var inputPath = args[0];                
                var input = File.ReadAllText(inputPath);
                
                Console.WriteLine(String.Format(
                    "===== Tokens from: \"{0}\" =====", inputPath)
                );
                var count = 1;
                foreach (var tok in new Scanner(input).Start()) {
                    Console.WriteLine(String.Format("[{0}] {1}", 
                                                    count++, tok)
                    );
                }
                
            } catch (FileNotFoundException e) {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }                
        }

        //-----------------------------------------------------------
        public static void Main(string[] args) {
            new Driver().Run(args);
        }
    }
}
