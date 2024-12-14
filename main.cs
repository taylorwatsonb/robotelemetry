
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

public class RobotState
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double Roll { get; set; }
    public double Pitch { get; set; }
    public double Yaw { get; set; }
    public static double SimulationSpeed { get; set; } = 1.0;
}

public class SimulationControl
{
    public double Speed { get; set; }
}

public class RobotHub : Hub
{
    public async Task UpdateState(RobotState state)
    {
        await Clients.All.SendAsync("ReceiveState", state);
    }

    public async Task UpdateSimulationSpeed(SimulationControl control)
    {
        RobotState.SimulationSpeed = control.Speed;
        await Clients.All.SendAsync("SimulationSpeedUpdated", control.Speed);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.Services.AddCors();

        var app = builder.Build();
        
        app.UseCors(builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapHub<RobotHub>("/robotHub");

        app.MapGet("/", async context =>
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/index.html");
        });

        // Simulate robot state updates
        Task.Run(async () =>
        {
            var hub = app.Services.GetService<IHubContext<RobotHub>>();
            var random = new Random();
            while (true)
            {
                var state = new RobotState
                {
                    X = random.NextDouble() * 0.4 - 0.2,
                    Y = random.NextDouble() * 0.4 - 0.2,
                    Z = random.NextDouble() * 0.4 - 0.2,
                    Roll = random.NextDouble() * Math.PI,
                    Pitch = random.NextDouble() * Math.PI,
                    Yaw = random.NextDouble() * Math.PI
                };
                await hub.Clients.All.SendAsync("ReceiveState", state);
                await Task.Delay(500);
            }
        });

        app.Run("http://0.0.0.0:3000");
    }
}
