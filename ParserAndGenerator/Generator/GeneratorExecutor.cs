using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser.Methods;
using Parser.Models;

namespace Generator
{
    [Generator]
    public class GeneratorExecutor : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Generator.Execute(context);
        }


        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}