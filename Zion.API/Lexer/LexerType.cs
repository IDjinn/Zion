using System.Runtime.CompilerServices;
using Zion.API.Parser;

namespace Zion.API.Lexer;

[Flags]
public enum LexerType : ulong
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
}

public static class LexerTypeUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool MayIsFunctionDeclaration(LexerType type)
    {
        switch (type)
        {
            case LexerType.Struct:
            case LexerType.ValueTypeArray:
            case LexerType.Void:
            case LexerType.ValueType:
            case LexerType.ReferenceType:
                return true;

            default:
                return false;
        }
    }

    public static string IdentifierKeyword(Token token) => token.Value!;

    public static LexerType KeywordToLexerType(string identifier)
    {
        return identifier.ToLower() switch
        {
            "void" => LexerType.Void,
            "struct" => LexerType.Struct,
            "object" => LexerType.Object,
            "int8" => LexerType.Int8,
            "uint8" => LexerType.Uint8,
            "int16" => LexerType.Int16,
            "uint16" => LexerType.Uint16,
            "uint32" => LexerType.Uint32,
            "int64" => LexerType.Int64,
            "uint64" => LexerType.Uint64,
            "int128" => LexerType.Int128,
            "uint128" => LexerType.Uint128,
            "float" => LexerType.Float,
            "double" => LexerType.Double,
            "decimal" => LexerType.Decimal,
            "bool" => LexerType.Bool,
            _ => LexerType.Unknown
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static LexerType GetType(this Token token)
    {
        return KeywordToLexerType(token.Value!);
    }
}