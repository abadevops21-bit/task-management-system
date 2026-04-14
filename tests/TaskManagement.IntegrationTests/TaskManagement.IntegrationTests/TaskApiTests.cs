using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TaskManagement.IntegrationTests;
using Xunit;

public class TaskApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TaskApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_Should_Return_200()
    {
        var response = await _client.GetAsync("/api/tasks");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}