﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetdevelopersdotnetdevelopersdotnetdevelopers
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
++++++++
[
>++++
[
>++
>+++
>+++
>+
<<<<-
]
>+
>+
>-
>>+
[<]
<-
]
             */

            string source = @"
Developers Developers Developers Developers Developers Developers Developers Developers
DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers
DevelopersDevelopersDevelopers Developers Developers Developers Developers
DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers
DevelopersDevelopersDevelopers Developers Developers
DevelopersDevelopersDevelopers Developers Developers Developers
DevelopersDevelopersDevelopers Developers Developers Developers
DevelopersDevelopersDevelopers Developers
DevelopersDevelopersDevelopersDevelopers DevelopersDevelopersDevelopersDevelopers DevelopersDevelopersDevelopersDevelopers DevelopersDevelopersDevelopersDevelopers DevelopersDevelopers
DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers
DevelopersDevelopersDevelopers Developers
DevelopersDevelopersDevelopers Developers
DevelopersDevelopersDevelopers DevelopersDevelopers
DevelopersDevelopersDevelopers DevelopersDevelopersDevelopers Developers
DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers DevelopersDevelopersDevelopersDevelopers DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers
DevelopersDevelopersDevelopersDevelopers DevelopersDevelopers
DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers";

            try
            {
                Generator.Generate(source, "helloworld");

                Console.ReadLine();
            }
            catch (ParseException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
