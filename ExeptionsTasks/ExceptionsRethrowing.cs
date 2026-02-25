using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExeptionsTasks
{
    public static class ExceptionsRethrowing
    {
        public static int GetMaxValue(List<int> numbers)
        {
            try
            {
                return numbers.Max();
            } 
            catch (ArgumentNullException ex)
            {

                throw new ArgumentNullException("The numbers list cannot be null.", ex);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("The numbers list cannot be empty.");
                throw;
            }
        }
    }
}
