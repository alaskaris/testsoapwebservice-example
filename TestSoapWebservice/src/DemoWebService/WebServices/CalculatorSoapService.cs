using System.ServiceModel;
namespace DemoWebService.WebServices;

[ServiceContract]
public interface ICalculatorSoapService
{
    [OperationContract]
    string Sum(int num1,int num2);
}

public class CalculatorSoapService: ICalculatorSoapService
{
    public string Sum(int num1, int num2)
    {
        return $"Sum of two number is: {num1+ num2}";
    }
}