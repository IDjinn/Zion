using Zion.API;
using Zion.Generation;
using Zion.Lexer;

namespace Zion.Core;

public class Program
{
    public static void Main(string[] args)
    {
        var program = """
                      		void hello() {
                      			print("World 123456789");
                      		}
                      """;

        var compiler = new ZionCompiler(new Parser.ZionParser(), new ZionLexer(), new ZionGenerator());
        compiler.Compile(program);
    }
}