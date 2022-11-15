using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;
using Moq;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace hypotheek.azure.test;

[TestClass]
public class CalculateAnnuityTests
{
    private Mock<ILogger>? _mockedLogger;
    private Mock<HttpRequest>? _mockedRequest;

    [TestInitialize]
    public void Initialize()
    {
        _mockedLogger = new Mock<ILogger>();
        _mockedRequest = new Mock<HttpRequest>();
    }

    [TestMethod]
    public void CalculateAnnuity_ShouldReturn_InputParameters()
    {
        //Arrange
        var interest = 4;
        var years = 10;
        var loan = 20000;
        var expectedMonths = years * 12;

        var queryParams = new Dictionary<string, StringValues>();
        queryParams.Add("loan", new StringValues(loan.ToString()));
        queryParams.Add("years", new StringValues(years.ToString()));
        queryParams.Add("interest", new StringValues(interest.ToString()));

        _mockedRequest?.Setup(m => m.Query)
                       .Returns(new QueryCollection(queryParams));

        //Act
        var httpResponse = CalculateAnnuity.Run(_mockedRequest?.Object, _mockedLogger?.Object) as JsonResult;
        var response = httpResponse!.Value as Response;


        //Assert
        response.ShouldNotBeNull();
        response.Interest.ShouldBe(interest);
        response.Years.ShouldBe(years);
        response.Loan.ShouldBe(loan);
        response.Process.Count().ShouldBe(expectedMonths);
    }

    [TestMethod]
    public void CalculateAnnuity_WithInvalidParameters_ShouldReturn_BadRequestObject()
    {
        //Arrange
        var interest = "wrongparameter";
        var years = 10;
        var loan = 20000;
        var expectedMonths = years * 12;

        var queryParams = new Dictionary<string, StringValues>();
        queryParams.Add("loan", new StringValues(loan.ToString()));
        queryParams.Add("years", new StringValues(years.ToString()));
        queryParams.Add("interest", new StringValues(interest));

        _mockedRequest?.Setup(m => m.Query)
                       .Returns(new QueryCollection(queryParams));

        //Act
        var response = CalculateAnnuity.Run(_mockedRequest?.Object, _mockedLogger?.Object);


        //Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<BadRequestResult>();
    }
}
