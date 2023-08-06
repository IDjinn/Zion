using System.Runtime.InteropServices;
using LLVMSharp.Interop;
using Zion.API.Generation;
using Zion.API.Lexer;

namespace Zion.Generation;

public class ZionGenerator : IGeneration
{
    public unsafe void Generate(AbstractSyntaxTree syntaxTree)
    {
        LLVM.InitializeAllTargets();
        LLVM.InitializeAllTargetMCs();
        LLVM.InitializeAllAsmParsers();
        LLVM.InitializeAllAsmPrinters();


        var context = LLVM.ContextCreate();
        var moduleName = Marshal.StringToHGlobalAnsi("ASTModule");
        var module = LLVM.ModuleCreateWithName((sbyte*)moduleName.ToPointer());

        // Define the class structure (not complete, just for demonstration)
        var className = Marshal.StringToHGlobalAnsi("AbstractSyntaxTree");
        var classType = LLVM.StructCreateNamed(context, (sbyte*)className.ToPointer());
        var classTypePointer = LLVM.PointerType(classType, 0);

        // Define the main function
        var paramType = LLVM.PointerType(LLVM.PointerType(LLVM.Int32TypeInContext(context), 0), 0);
        var paramPointer = LLVM.PointerType(paramType, 0);
        var mainFunctionType = LLVM.FunctionType(
            LLVM.VoidTypeInContext(context),
            (LLVMOpaqueType**)paramPointer,
            0,
            1
        );
        var mainFunctionName = Marshal.StringToHGlobalAnsi("AbstractSyntaxTree");
        var mainFunction = LLVM.AddFunction(module, (sbyte*)mainFunctionName.ToPointer(), mainFunctionType);
        var entryBlock = LLVM.AppendBasicBlock(mainFunction, (sbyte*)Marshal.StringToHGlobalAnsi("entry").ToPointer());

        var builder = LLVM.CreateBuilderInContext(context);
        LLVM.PositionBuilderAtEnd(builder, entryBlock);
        LLVM.BuildRetVoid(builder);

        LLVM.DisposeBuilder(builder);
        LLVM.DumpModule(module);
        LLVM.PrintModuleToFile(module, (sbyte*)Marshal.StringToHGlobalAnsi("output").ToPointer(), null);
        LLVM.ContextDispose(context);
    }
}