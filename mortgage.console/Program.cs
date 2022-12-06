global using Mortgage.Console;

const string loanMessage = "Please supply the total loan amount:";
const string yearMessage = "Please supply the total years of the loan:";
const string interestMessage = "Please supply the intrest percentage (%):";

Action<int, int, double> Display = (loan, years, interest) =>
{
    double remainingLoan = loan;
    double percentage = (interest / 100) / 12;

    var totalMonths = years * 12;
    var annuity = Calculation.CalculateAnnuity(loan, years, interest);

    for (int i = 0; i < totalMonths; i++)
    {
        var interestValue = Math.Round(remainingLoan * percentage, 2);
        var liquidate = annuity - interestValue;

        remainingLoan = Math.Round(remainingLoan - liquidate, 2);

        Console.WriteLine($"Month: {i + 1}, Liquidate: {Math.Round(liquidate, 2)}, Interest: {interestValue}, Remaining loan: {remainingLoan}");
    }

    Console.WriteLine($"Monthly: {Math.Round(annuity, 2)}, Total: {annuity * years * 12}");
};

Func<string, double> RetrieveUserInputFunc = (message) =>
{
    var output = 0.0;

    while (output == 0)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();

        if (!double.TryParse(input, out output))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            Console.WriteLine($"Invalid input.. '{input}'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else break;
    }

    return output;
};

Console.WriteLine("Welcome");
Console.WriteLine("Let's calculate the annuity of a loan.");

int loan = (int)RetrieveUserInputFunc(loanMessage);
int years = (int)RetrieveUserInputFunc(yearMessage);
double interest = RetrieveUserInputFunc(interestMessage);

Display(loan, years, interest);
