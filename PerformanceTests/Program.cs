using System.Net.Http;
using NBomber.CSharp;

var httpClient = new HttpClient();

var scenario = Scenario.Create("get_service_orders", async context =>
{
    var response = await httpClient.GetAsync("https://localhost:7226/ServiceOrders");

    if (response.IsSuccessStatusCode)
        return Response.Ok();

    return Response.Fail(); 
})
.WithoutWarmUp() 
.WithLoadSimulations(
    Simulation.Inject(rate: 50, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(2))
);

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();
