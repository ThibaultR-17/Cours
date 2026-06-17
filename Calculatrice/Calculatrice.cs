using System;

public class Calculatrice
{


    public void run()
    {
        Console.WriteLine("bonjour, je suis une calculette tres basique");
        Console.WriteLine("entrez votre operation (addition fait pas le con)");

        while (true) {

            String input = Console.ReadLine();

            String[] parse = input.Split(new char[] { ' ' });

            if (parse.Length > 2)
            {
                analyse(parse);
            }
            else
            {
                Console.WriteLine("manque des trucs");
            }


        }


    }
    private void analyse(string[] text)
    {
        int result = 0;
            switch (text[1])
            {
                case "+":
                    result = addition(toInt32(text[0]), toInt32(text[2]));
                    break;

                case "-":
                    result = soust(toInt32(text[0]), toInt32(text[2]));
                    break;

                case "*":
                    result = mult(toInt32(text[0]), toInt32(text[2]));
                    break;

                case "/":
                    result = div(toInt32(text[0]), toInt32(text[2]));
                    break;

                default:
                    Console.WriteLine("????? kestufé ??????????");
                    break;
            }
        Console.WriteLine( result);
     }
    private int addition(int a, int b)
    {
        return a + b;
    }

    private int soust(int a, int b)
    {
        return a - b;
    }

    private int mult(int a, int b)
    {
        return a * b;
    }

    private int div(int a, int b)
    {
        return a / b;
    }

    private int toInt32(String a)
    {
        int b = 0;
        try
        {
            b = Int32.Parse(a);
        }
        catch (FormatException) {
            throw new FormatException();
        }
        return b;
    }
}

