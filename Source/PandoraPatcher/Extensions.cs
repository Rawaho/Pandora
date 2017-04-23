using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;

namespace PandoraPatcher
{
    public static class Extensions
    {
        public static TypeDefinition GetTypeDefinition(this AssemblyDefinition assemblyDefinition, string type)
        {
            TypeDefinition typeDefinition = assemblyDefinition.MainModule.GetType(type);
            if (typeDefinition == null)
            {
                Console.WriteLine($"Failed to find type \"{type}\" in assembly! An update to Faeria probably broke this.");
                Environment.Exit(0);
            }

            return typeDefinition;
        }

        public static MethodDefinition GetMethodDefinition(this TypeDefinition typeDefinition, string method)
        {
            MethodDefinition methodDefinition = typeDefinition.GetMethod(method);
            if (methodDefinition == null)
            {
                Console.WriteLine($"Failed to find method \"{method}\" in assembly! An update to Faeria probably broke this.");
                Environment.Exit(0);
            }

            return methodDefinition;
        }

        public static void ReplaceString(this AssemblyDefinition assemblyDefinition, string original, string replacement)
        {
            foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
                foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
                    IterateType(typeDefinition, original, replacement);
        }

        public static void IterateType(TypeDefinition typeDefinition, string original, string replacement)
        {
            foreach (TypeDefinition nestedTypeDefinition in typeDefinition.NestedTypes)
                IterateType(nestedTypeDefinition, original, replacement);

            foreach (MethodDefinition md in typeDefinition.Methods)
            {
                if (!md.HasBody)
                    continue;

                for (int i = 0; i < md.Body.Instructions.Count - 1; i++)
                {
                    Instruction inst = md.Body.Instructions[i];
                    if (inst.OpCode == OpCodes.Ldstr)
                        if (inst.Operand.ToString().Equals(original))
                            inst.Operand = replacement;
                }
            }
        }
    }
}
