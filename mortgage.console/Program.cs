global using Mortgage.Console;

int loan = 0;
int years = 0;
double interest = 0;

const string loanMessage = "Please supply the total loan amount:";
const string yearMessage = "Please supply the total years of the loan:";
const string interestMessage = "Please supply the intrest percentage (%):";

Console.WriteLine("Welcome");
Console.WriteLine("Let's calculate the annuity of a loan.");

Func<string, double> RetrieveUserInputFunc = (message) =>
{
    var output = 0.0;

    while (output == 0)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();

        if (!double.TryParse(input, out output))
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 2);
            Console.WriteLine("Invalid input..");
        }
        else break;
    }

    return output;
};

loan = (int)RetrieveUserInputFunc(loanMessage);
years = (int)RetrieveUserInputFunc(yearMessage);
interest = RetrieveUserInputFunc(interestMessage);

var annuity = Calculation.CalculateAnnuity(loan, years, interest);
Console.WriteLine($"Monthly: {annuity}, Total: {annuity * years * 12}");

Display();

void Display()
{
    double remainingLoan = loan;
    double percentage = (interest / 100) / 12;
    var totalMonths = years * 12;

    for (int i = 0; i < totalMonths; i++)
    {
        var interestValue = Math.Round(remainingLoan * percentage, 2);
        var liquidate = annuity - interestValue;

        remainingLoan = Math.Round(remainingLoan - liquidate, 2);

        Console.WriteLine($"Month: {i + 1}, Liquidate: {liquidate}, Interest: {interestValue}, Remaining loan: {remainingLoan}");
    }
}
