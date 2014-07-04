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

            var il = mainMethodBuilder.GetILGenerator();

            // Define the pointer and array
            var arr = il.DeclareLocal(typeof(sbyte[]));
            var ptr = il.DeclareLocal(typeof(int));

            // Program runs in an array of 30000 bytes
            il.Emit(OpCodes.Ldc_I4, 30000);
            il.Emit(OpCodes.Newarr, typeof(sbyte));
            il.Emit(OpCodes.Stloc, arr);

            // Parse each token
            EmitFromTree(il, ast, arr, ptr);

            // Return zero since we assume everything runs fine
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var programClass = programClassBuilder.CreateType();
            assemblyBuilder.SetEntryPoint(mainMethodBuilder, PEFileKinds.ConsoleApplication);
            assemblyBuilder.Save(fileName);
        }

        static void EmitFromTree(ILGenerator il, TreeNode<TokenKind> tree, LocalBuilder arr, LocalBuilder ptr)
        {
            foreach (var token in tree.Children)
            {
                switch (token.Value)
                {
                    // Load the pointer, modify it, and restore it
                    case TokenKind.IncrementPointer:
                    case TokenKind.DecrementPointer:
                        il.Emit(OpCodes.Ldloc, ptr);
                        il.Emit(OpCodes.Ldc_I4_1);
                        if (token.Value == TokenKind.IncrementPointer)
                            il.Emit(OpCodes.Add);
                        else
                            il.Emit(OpCodes.Sub);
                        il.Emit(OpCodes.Stloc, ptr);
                        break;

                    // Load the index in the array, modify it, and restore it
                    case TokenKind.IncrementPointee:
                    case TokenKind.DecrementPointee:
                        // Prepare for storing the modified element into the array
                        il.Emit(OpCodes.Ldloc, arr);
                        il.Emit(OpCodes.Ldloc, ptr);
                        
                        // Load the element from the array
                        il.Emit(OpCodes.Ldloc, arr);
                        il.Emit(OpCodes.Ldloc, ptr);
                        il.Emit(OpCodes.Ldelem_I1);

                        // Load 1 onto the stack and then do the add/subtract
                        il.Emit(OpCodes.Ldc_I4_1);
                        if (token.Value == TokenKind.IncrementPointee)
                            il.Emit(OpCodes.Add);
                        else
                            il.Emit(OpCodes.Sub);

                        // The second arr/ptr were consumed so now we consume the first ones
                        il.Emit(OpCodes.Stelem_I1);
                        break;

                    case TokenKind.GetChar:
                        throw new NotImplementedException();
                        break;

                    case TokenKind.PutChar:
                        il.Emit(OpCodes.Ldloc, arr);
                        il.Emit(OpCodes.Ldloc, ptr);
                        il.Emit(OpCodes.Ldelem_U1);
                        il.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", new Type[] { typeof(char) }));
                        break;

                    // Equivalent to `while (*ptr)`
                    case TokenKind.NonZeroLoopOpen:
                        var loopStart = il.DefineLabel();
                        var loopEnd = il.DefineLabel();

                        // *ptr
                        il.MarkLabel(loopStart);
                        il.Emit(OpCodes.Ldloc, arr);
                        il.Emit(OpCodes.Ldloc, ptr);
                        il.Emit(OpCodes.Ldelem_U1);

                        // Jump to after loop if *ptr == 0
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Beq, loopEnd);

                        // Emit the inside of the loop
                        EmitFromTree(il, token, arr, ptr);

                        // Jump back to the start of the loop
                        il.Emit(OpCodes.Br, loopStart);
                        il.MarkLabel(loopEnd);
                        break;
                }
            }
        }
    }
}
