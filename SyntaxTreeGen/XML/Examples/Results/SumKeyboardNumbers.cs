/* Generated from SumKeyboardNumbers.xml by Syntax Tree Generator on 08/05/2016 23:07:12*/
public class Addition
{

  public static void Main (string[] args)
  {
    int number1;
    int number2;
    int sum;
    System.Console.WriteLine("Enter first integer: ");
    number1 = Convert.ToInt32(Console.ReadLine());
    number2 = Convert.ToInt32(Console.ReadLine());
    sum = (number1 + number2);
    System.Console.WriteLine(sum);
  }

}
