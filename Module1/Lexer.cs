using System;
using System.Collections.Generic;

public class LexerException : System.Exception
{
    public LexerException(string msg)
        : base(msg)
    {
    }

}

public class Lexer
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

    public virtual void Parse()
    {

    }
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
            result += int.Parse(currentCh.ToString()) ;
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

public class IdentifierLexer: Lexer
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

public class NonZeroInStartIntegerLexer: Lexer
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
        {
            Error();
        }

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

//public class StripedLexerStartingWithLetter: Lexer
//{
    
//}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("\nIntLexer\n");
        List<string> input = new List<string>{ "154216" ,  "-9908", "88al",  "08", "", "1", "8+"} ;
        foreach(var i in input)
        {
            Lexer L = new IntLexer(i);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        Console.WriteLine("\nIdentifierLexer\n");
        input = new List<string> { "", "1", "9a", "aaaa2", "A87d787a", "aa7J", "_", "1_", "a_", "a9_", "as_d_8_j_7_8", "+h8", "hh-" };
        foreach (var i in input)
        {
            Lexer L = new IdentifierLexer(i);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        Console.WriteLine("\nNonZeroInStartIntegerLexer\n");
        input = new List<string> { "", "1", "9a", "aaaa2",  "_", "1_", "+h8", "-9", "0", "09", "00", "70", "-08" };
        foreach (var i in input)
        {
            Lexer L = new NonZeroInStartIntegerLexer(i);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}