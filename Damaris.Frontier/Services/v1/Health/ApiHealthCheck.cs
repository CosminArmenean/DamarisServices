using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace Damaris.Frontier.Services.v1.Health;

public class ApiHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var url = "https://api-baseball.p.rapidapi.com/timezone";
        var client = new RestClient();
        var request = new RestRequest(url, Method.Get);
        request.AddHeader("X-RapidAPI-Key", "cb11d7b4a9msh0022a9755317d69p14d30fjsn15c4585122d8");
        request.AddHeader("X-RapidAPI-Host", "api-baseball.p.rapidapi.com");

        var response = client.Execute(request);
        if (response.IsSuccessful)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        else
        {
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }    
    }
}

