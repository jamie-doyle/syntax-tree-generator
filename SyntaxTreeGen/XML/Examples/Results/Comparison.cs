/* Generated from Comparison.xml by Syntax Tree Generator on 08/05/2016 23:07:31*/
public class Comparison
{

  public static void Main (string[] args)
  {
    int number1;
    int number2;
    System.Console.WriteLine("Enter first integer: ");
    number1 = Convert.ToInt32(Console.ReadLine());
    System.Console.WriteLine("Enter second integer: ");
    number2 = Convert.ToInt32(Console.ReadLine());
    if ((number1 == number2))
    {
      System.Console.WriteLine("{0} == {1}",number1,number2); 
    }
    if ((number1 != number2))
    {
      System.Console.WriteLine("{0} != {1}",number1,number2); 
    }
    if ((number1 < number2))
    {
      System.Console.WriteLine("{0} < {1}",number1,number2); 
    }
    if ((number1 > number2))
    {
      System.Console.WriteLine("{0} > {1}",number1,number2); 
    }
    if ((number1 <= number2))
    {
      System.Console.WriteLine("{0} <= {1}",number1,number2); 
    }
    if ((number1 >= number2))
    {
      System.Console.WriteLine("{0} >= {1}",number1,number2); 
    }
  }

}
