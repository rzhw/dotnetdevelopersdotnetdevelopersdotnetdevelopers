using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace dotnetdevelopersdotnetdevelopersdotnetdevelopers
{
    public class Generator
    {
        public static void Generate(string source, string name)
        {
            var parser = new Parser(source);
            var ast = parser.Parse();

            string fileName = String.Format("{0}.exe", name);

            var assemblyName = new AssemblyName(name);
            var assemblyDomain = AppDomain.CurrentDomain;

            var assemblyBuilder = assemblyDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, fileName);

            var programClassBuilder = moduleBuilder.DefineType(
                String.Format("{0}.Program", name),
                TypeAttributes.Public | TypeAttributes.Class);
            var mainMethodBuilder = programClassBuilder.DefineMethod(
                "Main",
                MethodAttributes.Public | MethodAttributes.Static,
                typeof(int),
                new Type[] { typeof(string[]) });

            var ilGenerator = mainMethodBuilder.GetILGenerator();

            // ...

            var programClass = programClassBuilder.CreateType();
            assemblyBuilder.SetEntryPoint(mainMethodBuilder, PEFileKinds.ConsoleApplication);
            assemblyBuilder.Save(fileName);
        }
    }
}
