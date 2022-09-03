Program.cs
====================
A very simple way of minifying the "noice" often created when setting up the scermonial code of a .Net 5+ project.


Recommendation:
Create extension methods to group service-registrations that belongs togeather to make your code even more readable/"scanable"..
Sample:

```C#
var program = new Program();

program.HostBuilder.UseSerilog(); //Cool we are using Serilog.. but at this stage you dont need to see the 1-(many) lines you might be using to register Serilog and configure it.. we just want to notify the reader that "we are using Serilog".. to dig deeper.. see the extension method..
program.Services.AddMassTransit(); //Cool.. we are adding MassTransit.. 
//Now.. imagine having a bunch more dependencies of different sorts, with different configurations... now also imagine them all "exploded" here as the more traditional way of writing "minimal apis"... thats a mess! And very hard to get a clear view of what we are using..

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


//Different C# file (ServiceCollectionExtensions.cs or whatever..).. this is just an example.. so it might not even run..
public static class ServiceCollectionExtensions
{
    public static void AddMassTransit(this IServiceCollection services) 
    {
        services.AddMassTransit(x => 
        {
            x.AddConsumer<MessageConsumer>();

            x.UsingRabbitMq((context,cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}


//Differecne C# file (HoseBuilderExtensions.cs or whatever..)..  this is just an example.. so it might not even run..
public static class HostBuilderExtensions
{
    public static void UseSerilog(this IHostBuilder hostBuilder) 
    {
        hostBuilder.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console())
    }
}
```