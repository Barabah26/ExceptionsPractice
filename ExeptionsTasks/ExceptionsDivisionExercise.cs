using System;
public static class ExceptionsDivisionExercise
{
    public static int DivideNumbers(int a, int b)
    {
        try
        {
            return a / b;
        }
        catch
        {
            Console.WriteLine("Division by zero.");
        }
        finally
        {
            Console.WriteLine("The DivideNumbers method ends.");
        }
        return 0;
    }
}
