using System.Runtime.CompilerServices;
using Zion.API.Parser;

namespace Zion.API.Lexer;

[Flags]
public enum SyntaxKind : ulong
{
    Unknown = 0,

    ValueType = 1 << 0,
    Void = 1 << 1 | ValueType,
    Struct = 1 << 3 | ValueType,
    ValueTypeArray = 1 << 2 | ValueType,

    Int8 = 3 << 2 | ValueType,
    Uint8 = 3 << 3 | ValueType,
    Int16 = 3 << 4 | ValueType,
    Uint16 = 3 << 5 | ValueType,
    Int32 = 3 << 6 | ValueType,
    Uint32 = 3 << 7 | ValueType,
    Int64 = 3 << 8 | ValueType,
    Uint64 = 3 << 9 | ValueType,
    Int128 = 3 << 10 | ValueType,
    Uint128 = 3 << 11 | ValueType,
    Float = 3 << 12 | ValueType,
    Double = 3 << 13 | ValueType,
    Decimal = 3 << 14 | ValueType,
    Bool = 3 << 15 | ValueType,

    ReferenceType = 4 << 1,
    Object = 5 << 1 | ReferenceType,

    // VisibilityModifier = 6 << 1,
    // PublicModifier = 7 << 2 | VisibilityModifier,
    // PrivateModifier = 7 << 3 | VisibilityModifier,
    // StaticModifier = 7 << 4 | VisibilityModifier,
    // InternalModifier = 7 << 5 | VisibilityModifier,
    // PrivateStaticModifier = PrivateModifier | StaticModifier
    Import = 8 << 1,
    Variable = 8 << 2
}

public static class LexerTypeUtilities
{
    public static readonly IReadOnlyDictionary<string, SyntaxKind> KeywordToKind = new Dictionary<string, SyntaxKind>()
    {
        ["import"] = SyntaxKind.Import,
        ["struct"] = SyntaxKind.Struct,
        ["object"] = SyntaxKind.Object,
        ["var"] = SyntaxKind.Variable,
        ["void"] = SyntaxKind.Void,


        ["int8"] = SyntaxKind.Int8,
        ["uint8"] = SyntaxKind.Uint8,
        ["int16"] = SyntaxKind.Int16,
        ["uint16"] = SyntaxKind.Uint16,
        ["int32"] = SyntaxKind.Int32,
        ["uint32"] = SyntaxKind.Uint32,
        ["int64"] = SyntaxKind.Int64,
        ["uint64"] = SyntaxKind.Uint64,
        ["int128"] = SyntaxKind.Int128,
        ["uint128"] = SyntaxKind.Uint128,
        ["float"] = SyntaxKind.Float,
        ["double"] = SyntaxKind.Double,
        ["decimal"] = SyntaxKind.Decimal,
        ["bool"] = SyntaxKind.Bool,
    };

    public static readonly IReadOnlyDictionary<SyntaxKind, string> KindToKeyword = new Dictionary<SyntaxKind, string>()
    {
        [SyntaxKind.Import] = "import",
        [SyntaxKind.Struct] = "struct",
        [SyntaxKind.Object] = "class",
        [SyntaxKind.Variable] = "var",
        [SyntaxKind.Void] = "void",
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool IsItImport(Token token)
    {
        // TODO: IT MAY BE AT PARSER NOT IN LEXER
        return token.TokenType == TokenType.Identifier && token.Value == KindToKeyword[SyntaxKind.Import];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool IsItClass(Token token)
    {
        // TODO: IT MAY BE AT PARSER NOT IN LEXER
        return token.TokenType == TokenType.Identifier && (
            token.Value == KindToKeyword[SyntaxKind.Object]
            || token.Value == KindToKeyword[SyntaxKind.Struct]
        );
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool MayIsFunctionDeclaration(SyntaxKind type)
    {
        switch (type)
        {
            case SyntaxKind.Struct:
            case SyntaxKind.ValueTypeArray:
            case SyntaxKind.Void:
            case SyntaxKind.ValueType:
            case SyntaxKind.ReferenceType:
                return true;

            default:
                return false;
        }
    }

    public static string IdentifierKeyword(Token token) => token.Value!;

    public static SyntaxKind KeywordToLexerType(string? identifier)
    {
        return identifier?.ToLower() switch
        {
            "void" => SyntaxKind.Void,
            "struct" => SyntaxKind.Struct,
            "object" => SyntaxKind.Object,
            "int8" => SyntaxKind.Int8,
            "uint8" => SyntaxKind.Uint8,
            "int16" => SyntaxKind.Int16,
            "uint16" => SyntaxKind.Uint16,
            "int32" => SyntaxKind.Int32,
            "uint32" => SyntaxKind.Uint32,
            "int64" => SyntaxKind.Int64,
            "uint64" => SyntaxKind.Uint64,
            "int128" => SyntaxKind.Int128,
            "uint128" => SyntaxKind.Uint128,
            "float" => SyntaxKind.Float,
            "double" => SyntaxKind.Double,
            "decimal" => SyntaxKind.Decimal,
            "bool" => SyntaxKind.Bool,
            "import" => SyntaxKind.Import,
            _ => SyntaxKind.Unknown
        };
    }

    public static TV? SafeIndexOf<TK, TV>(this IReadOnlyDictionary<TK, TV> dict, TK key)
    {
        return dict.TryGetValue(key, out var value) ? value : default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static SyntaxKind GetType(this Token token)
    {
        return KeywordToLexerType(token.Value!);
    }
}