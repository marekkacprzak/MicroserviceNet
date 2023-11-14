using Actio.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RawRabbit;

namespace Actio.Api.Tests.Unit.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void Home_Controller_Get_Returns_String_Content()
    {
        var mock = new Mock<IBusClient>();
        var result = (new HomeController(mock.Object)).Get();

        var contentResult = (ContentResult)result.Result;

        contentResult.Should().NotBeNull();
        contentResult.Content.Should().Be("Hello from Api");
    }
}