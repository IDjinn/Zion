namespace Zion.API.Parser;

public record Location(
    int Column,
    int Row,
    int Length,
    int SourceIndex
);