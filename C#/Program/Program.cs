var program = new Program();

//Setup DI and HostBuilder here..

await program.RunAsync();

public partial class Program
{
    public IServiceCollection Services { get; } = new ServiceCollection();
    
    public IHostBuilder HostBuilder { get; } = Host.CreateDefaultBuilder();

    public async Task RunAsync()
    {
        if (Services.Any())
        {
            HostBuilder.ConfigureServices(services =>
            {
                foreach (var service in Services)
                {
                    Services.Add(service);
                }
            });
        }

        var host = HostBuilder.Build();
        await host.RunAsync();
    }
}