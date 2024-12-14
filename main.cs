
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
                var time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond / 1000.0;
                var state = new RobotState
                {
                    X = Math.Sin(time) * 0.5,
                    Y = Math.Cos(time * 0.5) * 0.3,
                    Z = Math.Sin(time * 0.7) * 0.4,
                    Roll = Math.Sin(time * 0.3) * Math.PI * 0.25,
                    Pitch = Math.Cos(time * 0.4) * Math.PI * 0.25,
                    Yaw = time * 0.5
                };
                await hub.Clients.All.SendAsync("ReceiveState", state);
                await Task.Delay(50);
            }
        });

        app.Run("http://0.0.0.0:3000");
    }
}
