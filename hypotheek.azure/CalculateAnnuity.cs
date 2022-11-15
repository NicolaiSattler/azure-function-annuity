using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace hypotheek.azure
{
    public static class CalculateAnnuity
    {
        private const string InvalidParameterError = "Invalid parameters";
        private static ILogger _logger;

        [FunctionName("CalculateAnnuity")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            _logger = log;

            return HandleRequest(req);

        }

        private static IActionResult HandleRequest(HttpRequest req)
        {
            _logger.LogInformation("Received request for Annuity");

            if (req.Query.ContainsKey("loan") && double.TryParse(req.Query["loan"], out var loan)
                && int.TryParse(req.Query["years"], out var years)
                && double.TryParse(req.Query["interest"], out var interest))
            {

                var response = CreateResponse(loan, years, interest);
                return new JsonResult(response);
            }
            else
            {
                _logger.LogError(InvalidParameterError, req.QueryString);
                return new BadRequestResult();
            }
        }

        private static Response CreateResponse(double loan, int years, double interest)
        {
            var annuity = CalculateMonthlyAnnuity(loan, years, interest);
            var records = CreateRecords(loan, years, interest, annuity);

            return new Response
            {
                Years = years,
                Loan = loan,
                Interest = interest,
                Annuity = annuity,
                Process = records
            };
        }

        private static IEnumerable<Record> CreateRecords(double loan, int years, double interest, double annuity)
        {
            var result = new List<Record>();
            var remainingLoan = loan;
            var percentage = (interest / 100) / 12;
            var totalMonths = years * 12;

            for (int i = 0; i < totalMonths; i++)
            {
                var interestValue = Math.Round(remainingLoan * percentage, 2);
                var liquidate = annuity - interestValue;

                remainingLoan = Math.Round(remainingLoan - liquidate, 2);

                result.Add(new Record
                {
                    Liquidate = liquidate,
                    Month = i + 1,
                    Remainer = remainingLoan,
                    PayedInterest = interestValue

                });
            }

            return result;
        }

        private static double CalculateMonthlyAnnuity(double loan, double years, double interest)
        {
            var rate = (interest / 12) / 100;
            var exponent = years * 12;
            var factor = (rate + (rate / (Math.Pow(rate + 1, exponent) - 1)));
            var payment = loan * factor;
            var result = Math.Round(payment, 2);

            return result;
        }
    }
}
