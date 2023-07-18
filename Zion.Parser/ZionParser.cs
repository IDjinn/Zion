#define DISALLOW_IDENTIFIER_START_WITH_NUMBER

using Zion.API.Parser;

namespace Zion.Parser;

public class ZionParser : IParser
{
    private Token? _contextToken = null;
    private TokenType _contextTokenType = TokenType.Unknown;

    public IEnumerable<Token> ParseTokens(string source)
    {
        var reader = new SourceReader(source);
        var tokens = new List<Token>();
        do
        {
            _contextToken = null;
            _contextTokenType = TokenType.Unknown;

            var current = reader.Peek();
            switch (current)
            {
                case ' ':
                    reader.Advance();
                    continue;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    _contextTokenType = TokenType.Identifier;
                    TryParseIdentifier(ref reader);
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _contextTokenType = TokenType.NumberLiteral;
                    TryParseNumber(ref reader);
                    break;

                case '+':
                    if (ParserUtils.IsNumeric(reader.Peek(1)))
                        goto case '0';

                    if (reader.Peek(1) == '+')
                    {
                        MakeToken(reader.Location(), TokenType.Increment);
                        reader.Advance();
                    }
                    else if (reader.Peek(1) == '=')
                    {
                        MakeToken(reader.Location(), TokenType.PlusEqual);
                        reader.Advance();
                    }

                    reader.Advance();
                    break;

                case '-':
                    if (ParserUtils.IsNumeric(reader.Peek(1)))
                        goto case '0';

                    if (reader.Peek(1) == '-')
                    {
                        MakeToken(reader.Location(), TokenType.Decrement);
                        reader.Advance();
                    }
                    else if (reader.Peek(1) == '=')
                    {
                        MakeToken(reader.Location(), TokenType.MinusEqual);
                        reader.Advance();
                    }

                    reader.Advance();
                    break;

                case '{':
                    MakeToken(reader.Location(), TokenType.OpenBrace);
                    reader.Advance();
                    break;

                case '}':
                    MakeToken(reader.Location(), TokenType.CloseBrace);
                    reader.Advance();
                    break;

                case '>':
                    if (reader.Peek(1, '=')) // >=
                    {
                        MakeToken(reader.Location(), TokenType.GreaterThanOrEqual);
                        reader.Advance();
                    }
                    else if (reader.Peek(1, '>')) // >>
                    {
                        if (reader.Peek(2, '=')) // >>=
                        {
                            MakeToken(reader.Location(), TokenType.ShiftRightEqual);
                            reader.Advance(3);
                        }
                        else // >>
                        {
                            MakeToken(reader.Location(), TokenType.ShiftRight);
                            reader.Advance(2);
                        }
                    }
                    else // >
                    {
                        MakeToken(reader.Location(), TokenType.GreaterThan);
                        reader.Advance();
                    }

                    break;
                case '<':
                    if (reader.Peek(1, '=')) // <=
                    {
                        MakeToken(reader.Location(), TokenType.LessThanOrEqual);
                        reader.Advance();
                    }
                    else if (reader.Peek(1, '<')) // <<
                    {
                        if (reader.Peek(2, '=')) // <<=
                        {
                            MakeToken(reader.Location(), TokenType.ShiftLeftEqual);
                            reader.Advance(3);
                        }
                        else // <<
                        {
                            MakeToken(reader.Location(), TokenType.ShiftLeft);
                            reader.Advance(2);
                        }
                    }
                    else // <
                    {
                        MakeToken(reader.Location(), TokenType.LessThan);
                        reader.Advance();
                    }

                    break;

                case '=':
                    if (reader.Peek(1, '=')) // ==
                    {
                        MakeToken(reader.Location(), TokenType.Equal);
                        reader.Advance();
                    }
                    else // =
                    {
                        MakeToken(reader.Location(), TokenType.Assignment);
                    }

                    reader.Advance();
                    break;

                case '!':
                    if (reader.Peek(1, '=')) // !=
                    {
                        MakeToken(reader.Location(), TokenType.NotEqual);
                        reader.Advance();
                    }
                    else // !
                    {
                        MakeToken(reader.Location(), TokenType.Not);
                    }

                    reader.Advance();
                    break;


                case '\n': // TODO /R/t
                    reader.AdvanceColumn();
                    reader.Advance();
                    continue;

                case '"':
                    _contextTokenType = TokenType.StringLiteral;
                    TryParseStringLiteral(ref reader);
                    break;

                case '\t':
                case '\r':
                    reader.Advance();
                    break;

                default:
                    reader.Advance();
                    break;
            }

            if (_contextTokenType != TokenType.Unknown && _contextToken is not null)
            {
                tokens.Add(_contextToken);
            }
        } while (!reader.EOF);

        return tokens;
    }

    public bool TryParseStringLiteral(ref SourceReader reader)
    {
        var location = reader.Location(); // start of string
        if (reader.Peek('"'))
        {
            reader.Advance(); // skip "
            var length = reader.PeekWhile('"');
            var value = reader.PeekValue(length);
            _contextTokenType = TokenType.StringLiteral;
            location = location with { Length = length + 1 };
            _contextToken = new Token(TokenType.StringLiteral, location, value);
            reader.Advance(length + 1); // value"
            return true;
        }

        return false;
    }

    public void MakeToken(Location location, TokenType tokenType, string? value = null)
    {
        _contextTokenType = tokenType;
        _contextToken = new Token(tokenType, location, value);
    }

    public bool TryParseNumber(ref SourceReader reader)
    {
        var offset = 0;
        do
        {
            switch (reader.Peek(offset))
            {
                case '0':
                {
                    if (offset == 0 && reader.Peek(1) == 'x') // Allow 0x[0-9]
                    {
                        offset += 2;
                    }

                    goto case '1';
                }
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '_':
                    offset++;
                    break;

                case '-':
                case '+':
                {
                    if (offset != 0) // JUT AT START
                        return false;

                    offset++;
                    break;
                }

                case ',':
                case '.':
                {
                    if (offset == 0) // NEVER AT START
                        return false;

                    offset++;
                    break;
                }


                default:
                    goto OutWhile;
            }
        } while (true);

        OutWhile:
        _contextToken = new Token(TokenType.NumberLiteral, reader.Location(offset), reader.PeekValue(offset));
        reader.Advance(offset);
        return true;
    }

    public bool TryParseIdentifier(ref SourceReader reader)
    {
        var offset = 0;
        do
        {
            var current = reader.Peek(offset);
            switch (current)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
#if DISALLOW_IDENTIFIER_START_WITH_NUMBER
                    if (offset == 0)
                        return false;
#endif
                    break;


                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    break;

                default:
                    goto OutWhile;
            }

            offset++;
        } while (true);

        OutWhile:
        _contextToken = new Token(TokenType.Identifier, reader.Location(offset), reader.PeekValue(offset));
        reader.Advance(offset);
        return true;
    }
}