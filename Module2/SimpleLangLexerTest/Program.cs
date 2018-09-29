using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLangLexer;

namespace SimpleLangLexerTest
{
    class Program
    {
        public static void Main()
        {
            string fileContents = @"begin 
id23 := 24;  
cycle ; 2 id258 id29 ; 
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }

            Console.WriteLine("\nTask 1-------------------------------------------------------------------------\n");
            fileContents = @"begin 
hgh, jjh
jkkj:hjj
h := 1;                        
2 + 4; 3 - 2; 2 * 2; 4 / 2; 12 div 5; 6 mod 6; 1 and a; a or b; not 0;
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }

            Console.WriteLine("\nTask 2-------------------------------------------------------------------------\n");
            fileContents = @"begin 
h += 1;
h -=9;
h *= 8;
h /= 5;
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }

            Console.WriteLine("\nTask 3-------------------------------------------------------------------------\n");
            fileContents = @"begin 
h<a;
a > h;
h <= a;
a >= h;
h = 5;
h<> a;
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }

            Console.WriteLine("\nTask 4-------------------------------------------------------------------------\n");
            fileContents = @"begin 
h<a;
//sfsfsfsfsdfd
1234
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }

            Console.WriteLine("\nTask 5-------------------------------------------------------------------------\n");
            fileContents = @"begin 
h<a;
{sfsfsfsfsdfd
1234}
{
dsd
}
{asds
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
        }
    }
 }


    