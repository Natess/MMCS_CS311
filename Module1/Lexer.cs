using System;
using System.Collections.Generic;
using System.Reflection;

public class LexerException : System.Exception
{
    public LexerException(string msg)
        : base(msg)
    {
    }

}

public abstract class Lexer
{
    protected int position;
    protected char currentCh;       // очередной считанный символ
    protected int currentCharValue; // целое значение очередного считанного символа
    protected System.IO.StringReader inputReader;
    protected string inputString;

    public Lexer(string input)
    {
        inputReader = new System.IO.StringReader(input);
        inputString = input;
    }

    public void Error()
    {
        System.Text.StringBuilder o = new System.Text.StringBuilder();
        o.Append(inputString + '\n');
        o.Append(new System.String(' ', position - 1) + "^\n");
        o.AppendFormat("Error in symbol {0}", currentCh);
        throw new LexerException(o.ToString());
    }

    protected void NextCh()
    {
        this.currentCharValue = this.inputReader.Read();
        this.currentCh = (char)currentCharValue;
        this.position += 1;
    }

    public abstract void Parse();
}

public class IntLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        var result = 0;
        var sign_p = true;

        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            if (currentCh == '-')
                sign_p = false;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            result += int.Parse(currentCh.ToString());
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            result *= 10;
            result += int.Parse(currentCh.ToString());
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        if (!sign_p)
            result *= -1;
        System.Console.WriteLine("Integer is recognized");
        Console.WriteLine("The number is {0}.", result);
        //return result;
    }
}

public class IdentifierLexer : Lexer
{
    protected System.Text.StringBuilder identifierString;

    public IdentifierLexer(string input) : base(input)
    {
        identifierString = new System.Text.StringBuilder();

    }

    public override void Parse()
    {
        var result = "";
        NextCh();
        if (char.IsLetter(currentCh))
        {
            result += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh) || currentCh == '_')
        {
            result += currentCh;
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("Identifier {0} is recognized", result);
    }
}

public class NonZeroInStartIntegerLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public NonZeroInStartIntegerLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            resString.Append(currentCh);
            NextCh();
        }

        if (char.IsDigit(currentCh) && int.Parse(currentCh.ToString()) != 0)
        {
            resString.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            resString.Append(currentCh);
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class StripedLexerStartingWithLetter : Lexer
{
    protected System.Text.StringBuilder resString;

    public StripedLexerStartingWithLetter(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            resString.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            resString.Append(currentCh);
            NextCh();
            if (currentCharValue == -1) break;
            if (char.IsLetter(currentCh))
            {
                resString.Append(currentCh);
                NextCh();
            }
            else
                Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class SeparationLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public SeparationLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string originalString = "";
        NextCh();

        if (char.IsLetter(currentCh))
        {
            originalString += currentCh;
            resString.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsLetter(currentCh) || currentCh == ';' || currentCh == ',')
        {
            originalString += currentCh;
            var f = false || (currentCh == ';' || currentCh == ',');
            if (!f)
            {
                resString.Append(currentCh);
                NextCh();
                continue;
            }
            NextCh();
            if (currentCharValue == -1) break;
            if (char.IsLetter(currentCh))
            {
                resString.Append(currentCh);
                NextCh();
            }
            else
                Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", originalString);
        System.Console.WriteLine("Result of work is {0}", resString);

    }
}

public class DigitListWithSpacesLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public DigitListWithSpacesLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string originalString = "";
        NextCh();

        if (char.IsDigit(currentCh))
        {
            resString.Append(currentCh);
            originalString += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh) || currentCh == ' ')
        {
            originalString += currentCh;
            NextCh();
            if (char.IsDigit(currentCh))
            {
                resString.Append(currentCh);
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", originalString);
        System.Console.WriteLine("Result of work is {0}", resString);

    }
}

public class StripedGroupLettersAndDigitsLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public StripedGroupLettersAndDigitsLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        var isLetter = true;
        int counter = 0;

        if (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            ++counter;
            if (char.IsDigit(currentCh))
                isLetter = false;
            resString.Append(currentCh);
            NextCh();
            if (char.IsLetter(currentCh) && !isLetter || char.IsDigit(currentCh) && isLetter)
            {
                counter = 0;
                isLetter = !isLetter;
            }
        }
        else
            Error();

        while ((char.IsDigit(currentCh) && !isLetter || char.IsLetter(currentCh) && isLetter) && counter < 2)
        {
            ++counter;
            resString.Append(currentCh);
            NextCh();
            if (char.IsLetter(currentCh) && !isLetter || char.IsDigit(currentCh) && isLetter)
            {
                counter = 0;
                isLetter = !isLetter;
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class RealWithDecimalDotLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public RealWithDecimalDotLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        var wasPoint = false;
        NextCh();
        if (char.IsDigit(currentCh) && currentCh != '0')
        {
            resString.Append(currentCh);
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh) || currentCh == '.' && !wasPoint)
        {
            if (currentCh == '.')
                wasPoint = true;
            resString.Append(currentCh);
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class StringWithApostrophesLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public StringWithApostrophesLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '\'')
        {
            resString.Append(currentCh);
            NextCh();
        }
        else
            Error();

        var closeApostrophe = false;
        while (char.IsLetter(currentCh) || currentCh == '\'')
        {
            resString.Append(currentCh);
            if (currentCh == '\'')
            {
                NextCh();
                closeApostrophe = true;
                break;
            }
            NextCh();
        }

        if (currentCharValue != -1 || !closeApostrophe) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class CommentLexer : Lexer
{
    protected System.Text.StringBuilder resString;

    public CommentLexer(string input)
        : base(input)
    {
        resString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '/')
        {
            resString.Append(currentCh);
            NextCh();
            if (currentCh == '*')
            {
                resString.Append(currentCh);
                NextCh();
            }
            else
                Error();
        }
        else
            Error();

        var closeApostrophe = false;
        while (char.IsLetterOrDigit(currentCh) || char.IsPunctuation(currentCh) || currentCh == '/' || currentCh == '*')
        {
            resString.Append(currentCh);
            if (currentCh == '*')
            {
                NextCh();
                if (currentCh == '/')
                {
                    resString.Append(currentCh);
                    NextCh();
                    closeApostrophe = true;
                    break;
                }
            }
            else
            {
                NextCh();
            }
        }

        if (currentCharValue != -1 || !closeApostrophe) // StringReader вернет -1 в конце строки
            Error();
        System.Console.WriteLine("String {0} is recognized", resString);
    }
}

public class LexerFabric
{
    private string type;
    public LexerFabric(string t)
    {
        type = t;
    }

    public Lexer GetUnit(string input)
    {
        switch (type)
        {
            case "IntLexer":
                return new IntLexer(input);
            case "IdentifierLexer":
                return new IdentifierLexer(input);
            case "NonZeroInStartIntegerLexer":
                return new NonZeroInStartIntegerLexer(input);
            case "StripedLexerStartingWithLetter":
                return new StripedLexerStartingWithLetter(input);
            case "SeparationLexer":
                return new SeparationLexer(input);
            case "DigitListWithSpacesLexer":
                return new DigitListWithSpacesLexer(input);
            case "StripedGroupLettersAndDigitsLexer":
                return new StripedGroupLettersAndDigitsLexer(input);
            case "RealWithDecimalDotLexer":
                return new RealWithDecimalDotLexer(input);
            case "StringWithApostrophesLexer":
                return new StringWithApostrophesLexer(input);
            case "CommentLexer":
                return new CommentLexer(input);
            default:
                throw new Exception(type + " - incorrect type");
        }
    }
}

public class Program
{
    public static void RunLexer(List<string> test, string lexerType)
    {
        foreach (var i in test)
        {
            try
            {
                var lexer = new LexerFabric(lexerType).GetUnit(i);
                lexer.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void Main()
    {
        Console.WriteLine("\nIntLexer-----------------------------------------\n");
        List<string> input = new List<string> { "154216", "-9908", "88al", "08", "", "1", "8+" };
        RunLexer(input, "IntLexer");

        Console.WriteLine("\nIdentifierLexer-----------------------------------------\n");
        input = new List<string> { "", "1", "9a", "aaaa2", "A87d787a", "aa7J", "_", "1_", "a_", "a9_", "as_d_8_j_7_8", "+h8", "hh-" };
        RunLexer(input, "IdentifierLexer");

        Console.WriteLine("\nNonZeroInStartIntegerLexer-----------------------------------------\n");
        input = new List<string> { "", "1", "9a", "aaaa2", "_", "1_", "+h8", "-9", "0", "09", "00", "70", "-08" };
        RunLexer(input, "NonZeroInStartIntegerLexer");

        Console.WriteLine("\nStripedLexerStartingWithLetter-----------------------------------------\n");
        input = new List<string> { "", "1", "9a", "aaaa2", "_", "1_", "+h8", "a", "a_", "i8i8i8i8i", "i9i8" };
        RunLexer(input, "StripedLexerStartingWithLetter");

        Console.WriteLine("\nSeparationLexer-----------------------------------------\n");
        input = new List<string> { "", "1", "9a", "aaaa2", "_", "1_", "a", "a_", "iii;i", ";i9i8", "jj,k", "k;", "j,", ",j", "m;,j", "dl,l;", "l;,k", "lk;;" };
        RunLexer(input, "SeparationLexer");

        //Дополнительные задания
        Console.WriteLine("\nDigitListWithSpacesLexer-----------------------------------------\n");
        input = new List<string> { "", "9a", "aaaa2", "_", "1_", "a", " 9", "iii i", " i9i8", "1", "1    7", "18 8  8    " };
        RunLexer(input, "DigitListWithSpacesLexer");

        Console.WriteLine("\nStripedGroupLettersAndDigitsLexer-----------------------------------------\n");
        input = new List<string> { "1", "a", "9a", "a9", "aa9", "99a", "aa00a9aa0a99", "8a_a", "", " ", "178", "aa888a8" };
        RunLexer(input, "StripedGroupLettersAndDigitsLexer");

        Console.WriteLine("\nRealWithDecimalDotLexer-----------------------------------------\n");
        input = new List<string> { "", ".", "a", "1s", ".0", "1..9", "1.9.7", "1.2.", "01", "1", "10", "1.9", "1.0", "123.45678" };
        RunLexer(input, "RealWithDecimalDotLexer");

        Console.WriteLine("\nStringWithApostrophesLexer-----------------------------------------\n");
        input = new List<string> { "", ".", "a", "'n", "n'", "'a'a'", "'1'", "'.'", "''", "'a'", "'asas'" };
        RunLexer(input, "StringWithApostrophesLexer");

        Console.WriteLine("\nCommentLexer-----------------------------------------\n");
        input = new List<string> { "", ".", "a", "1", "", "'a'a'", "/*", "/*a", "s*/", "/*ss*/d*/", "/sdd/", "/*sdsd*", "/*sas*/", "/*sdds*sd*/", "/*sdsd/dsds*/", "/*sdsd/*ds*/" };
        RunLexer(input, "CommentLexer");
    }
}