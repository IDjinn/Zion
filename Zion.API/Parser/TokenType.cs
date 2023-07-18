namespace Zion.API.Parser;

public enum TokenType
{
    Unknown, // this is our default value

    Identifier,

    Plus, // +
    Minus, // -
    Multiply, // *
    Divide, // /
    Modulo, // %
    Power, // ** or ^
    Assignment, // =
    Equal, // ==
    NotEqual, // !=
    GreaterThan, // >
    GreaterThanOrEqual, // >=
    LessThan, // <
    LessThanOrEqual, // <=
    Question, // ?
    QuestionEqual, // ?=
    And, // &&
    Or, // ||
    Not, // !
    DotDot, // ..
    Increment, // ++
    Decrement, // --
    PlusEqual, // +=
    MinusEqual, // -=
    MultiplyEqual, // *=
    DivideEqual, // /=
    ModuloEqual, // %=
    PowerEqual, // **=


    // MEMBER ACCESSING
    Dot, // .
    PointerAccess, // ->
    Comma, // ,
    Colon, // :
    ColonColon, // ::
    Semicolon, // ;


    //BLOCKS
    OpenBracket, // {
    CloseBracket, // }
    OpenBrace, // (
    CloseBrace, // )
    OpenSquareBracket, // [
    CloseSquareBracket, // ]

    // KEYWORDS (all keywords are contextual)
    If, //
    Else, //
    While, //
    Return, //
    Do, //
    For, //
    Break, //
    Continue, //
    ForEach, //
    Import, //
    Class, //
    Function, //
    Struct,
    Enum,
    Const,
    Static,
    Virtual,
    Override,
    Public,
    Private,
    Protected,
    Internal,
    External,
    Abstract,


    NumberLiteral,
    StringLiteral,
    CharLiteral,
    BooleanLiteral,


    //BITWISE OPERATORS
    BitwiseAnd, // &
    BitwiseOr, // |
    BitwiseXor, // ^
    BitwiseNot, // ~
    ShiftRight, // >>
    ShiftLeft, // <<
    BitwiseAndEqual, // &=
    BitwiseOrEqual, // |=
    BitwiseXorEqual, // ^=
    ShiftRightEqual, // >>=
    ShiftLeftEqual, // <<=


    // built-in types
    Void,
    Bool,
    Byte,
    Char,
    Int8,
    UInt8,
    Int16,
    Int32,
    Int64,
    Int128,
    Uint16,
    Uint32,
    Uint64,
    Uint128,
    Float32,
    Float64,
    Float128,
    String,


    Invalid,
    EndOfFile,
}