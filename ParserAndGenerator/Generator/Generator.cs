using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Generator;

public static class Generator
{
    public static StatementSyntax[] CreateStreamWriterSyntaxBlock(List<ArgumentDto> arguments)
    {
        return arguments.SelectMany(item => new[]
        {
            ExpressionStatement(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("streamWriter"),
                                IdentifierName("Write")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        IdentifierName(item.Name))))))
        }).ToArray();

    }
    public static SyntaxNodeOrToken[] ConvertModelsToSyntaxNode(List<ArgumentDto> arguments) =>
        arguments.SelectMany(item => new [] {(SyntaxNodeOrToken) Parameter(
                    Identifier(item.Name))
                .WithType(
                    PredefinedType(
                        Token(SyntaxKind.StringKeyword))),
            Token(SyntaxKind.CommaToken)}).SkipLast(1).ToArray();

    public static CompilationUnitSyntax Execute(GeneratorExecutionContext context)
    {
            //using System
            var systemUsing = UsingDirective(
                IdentifierName("System"));

            //using System.IO
            var systemUsing2 = UsingDirective(
                QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("IO")));

            //using System.Net
            var systemUsing3 = UsingDirective(
                QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("Net")));

            // namespace 
            var currentNamespace = FileScopedNamespaceDeclaration(
                IdentifierName("Generator"));

            // class
            var classDeclaration = ClassDeclaration("HttpHandler");

            // URL 
            var urlField = FieldDeclaration(
                    VariableDeclaration(
                            PredefinedType(
                                Token(SyntaxKind.StringKeyword)))
                        .WithVariables(
                            SingletonSeparatedList<VariableDeclaratorSyntax>(
                                VariableDeclarator(
                                        Identifier("Url"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal("task")))))))
                .WithModifiers(
                    TokenList(
                        new[]
                        {
                            Token(SyntaxKind.PrivateKeyword),
                            Token(SyntaxKind.ConstKeyword)
                        }));


            List<MethodDto> methods = Parser.Methods.Parser.GetMethods();

            // List<MethodDeclarationSyntax> methodsToGenerate = new List<MethodDeclarationSyntax>();

            MemberDeclarationSyntax[] methodsToGenerate = new MemberDeclarationSyntax[methods.Count];
            var i = 0;

            foreach (var methodDto in methods)
            {
                var method = methodDto.HttpMethodName switch
                {
                    "Get" =>
                        MethodDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)),
                                Identifier(methodDto.MethodName))
                            .WithModifiers(TokenList(new[]
                            {
                                Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)
                            }))
                            .WithBody(Block(
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("HttpWebRequest"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("req"))
                                            .WithInitializer(EqualsValueClause(
                                                InvocationExpression(MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("WebRequest"),
                                                        IdentifierName("CreateHttp")))
                                                    .WithArgumentList(ArgumentList(
                                                        SingletonSeparatedList<ArgumentSyntax>(
                                                            Argument(
                                                                InterpolatedStringExpression(
                                                                        Token(SyntaxKind.InterpolatedStringStartToken))
                                                                    .WithContents(List<InterpolatedStringContentSyntax>(
                                                                        new InterpolatedStringContentSyntax[]
                                                                        {
                                                                            InterpolatedStringText()
                                                                                .WithTextToken(Token(TriviaList(),
                                                                                    SyntaxKind
                                                                                        .InterpolatedStringTextToken,
                                                                                    "http://localhost:7000 + ",
                                                                                    "http://localhost:7000 + ",
                                                                                    TriviaList())),
                                                                            Interpolation(IdentifierName("Url"))
                                                                        }))))))))))),
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("WebResponse"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("resp"))
                                            .WithInitializer(EqualsValueClause(InvocationExpression(
                                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("req"), IdentifierName("GetResponse")))))))),
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("Stream"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("stream"))
                                            .WithInitializer(EqualsValueClause(InvocationExpression(
                                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("resp"), IdentifierName("GetResponseStream")))))))),
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("StreamReader"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("sr"))
                                            .WithInitializer(EqualsValueClause(
                                                ObjectCreationExpression(IdentifierName("StreamReader"))
                                                    .WithArgumentList(ArgumentList(
                                                        SingletonSeparatedList<ArgumentSyntax>(
                                                            Argument(IdentifierName("stream")))))))))),
                                LocalDeclarationStatement(
                                    VariableDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)))
                                        .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            VariableDeclarator(Identifier("output"))
                                                .WithInitializer(EqualsValueClause(
                                                    InvocationExpression(MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression, IdentifierName("sr"),
                                                        IdentifierName("ReadToEnd")))))))),
                                ExpressionStatement(InvocationExpression(MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression, IdentifierName("sr"),
                                    IdentifierName("Close")))), ReturnStatement(IdentifierName("output")))),
                    "Post" =>
                        MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)),
                                Identifier(methodDto.MethodName))
                            .WithModifiers(TokenList(new[]
                            {
                                Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)
                            }))
                            .WithParameterList(ParameterList(
                                SeparatedList<ParameterSyntax>(ConvertModelsToSyntaxNode(methodDto.ArgDeclarations))))
                            .WithBody(Block(
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("HttpWebRequest"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("httpWebRequest"))
                                            .WithInitializer(EqualsValueClause(CastExpression(
                                                IdentifierName("HttpWebRequest"),
                                                InvocationExpression(MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("WebRequest"),
                                                        IdentifierName("Create")))
                                                    .WithArgumentList(ArgumentList(
                                                        SingletonSeparatedList<ArgumentSyntax>(
                                                            Argument(
                                                                InterpolatedStringExpression(
                                                                        Token(SyntaxKind.InterpolatedStringStartToken))
                                                                    .WithContents(
                                                                        SingletonList<InterpolatedStringContentSyntax>(
                                                                            Interpolation(
                                                                                IdentifierName("Url")))))))))))))),
                                ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("httpWebRequest"), IdentifierName("ContentType")),
                                    LiteralExpression(SyntaxKind.StringLiteralExpression,
                                        Literal("application/json")))),
                                ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("httpWebRequest"), IdentifierName("Method")),
                                    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("POST")))),
                                SyntaxFactory
                                    .UsingStatement(SyntaxFactory.Block()
                                        .AddStatements(CreateStreamWriterSyntaxBlock(methodDto.ArgDeclarations)))
                                    .WithDeclaration(
                                        VariableDeclaration(IdentifierName(Identifier(TriviaList(),
                                                SyntaxKind.VarKeyword,
                                                "var", "var", TriviaList())))
                                            .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                VariableDeclarator(Identifier("streamWriter"))
                                                    .WithInitializer(EqualsValueClause(
                                                        ObjectCreationExpression(IdentifierName("StreamWriter"))
                                                            .WithArgumentList(ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(InvocationExpression(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            IdentifierName("httpWebRequest"),
                                                                            IdentifierName(
                                                                                "GetRequestStream")))))))))))),
                                LocalDeclarationStatement(VariableDeclaration(IdentifierName("HttpWebResponse"))
                                    .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(Identifier("httpResponse"))
                                            .WithInitializer(EqualsValueClause(CastExpression(
                                                IdentifierName("HttpWebResponse"),
                                                InvocationExpression(MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("httpWebRequest"),
                                                    IdentifierName("GetResponse"))))))))),
                                UsingStatement(Block(SingletonList<StatementSyntax>(LocalDeclarationStatement(
                                        VariableDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)))
                                            .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                VariableDeclarator(Identifier("result"))
                                                    .WithInitializer(EqualsValueClause(
                                                        InvocationExpression(MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("streamReader"),
                                                            IdentifierName("ReadToEnd")))))))))))
                                    .WithDeclaration(
                                        VariableDeclaration(IdentifierName(Identifier(TriviaList(),
                                                SyntaxKind.VarKeyword,
                                                "var", "var", TriviaList())))
                                            .WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                VariableDeclarator(Identifier("streamReader"))
                                                    .WithInitializer(EqualsValueClause(
                                                        ObjectCreationExpression(IdentifierName("StreamReader"))
                                                            .WithArgumentList(ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(InvocationExpression(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            IdentifierName("httpResponse"),
                                                                            IdentifierName(
                                                                                "GetResponseStream")))))))))))))),
                    _ => null
                };

                methodsToGenerate[i] = method;
                ++i;
            }

            CompilationUnitSyntax compilationUnit = CompilationUnit().WithUsings(
                    List<UsingDirectiveSyntax>(new UsingDirectiveSyntax[]
                        {systemUsing, systemUsing2, systemUsing3}))
                .AddMembers(currentNamespace.AddMembers(classDeclaration.AddMembers(urlField).AddMembers(methodsToGenerate)))
                .NormalizeWhitespace();
           
            context.AddSource("Generator.g.cs", compilationUnit.ToString());

            return compilationUnit;
        }
}