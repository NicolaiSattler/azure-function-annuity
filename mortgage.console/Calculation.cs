namespace Mortgage.Console;

public class Calculation
{
    public static double CalculateAnnuity(int loan, int years, double interest)
    {
        double rate = (interest / 12) / 100;
        double exponent = years * 12;
        double factor = (rate + (rate / (Math.Pow(rate + 1, exponent) - 1)));
        var payment = loan * factor;
        var result = Math.Round(payment, 2);

        return result;
    }
}
