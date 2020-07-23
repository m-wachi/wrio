namespace WrioWebapi

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy;
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

//open Wrio.Logic

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        //let sesfunc options : Action<SessionOptions> =
        //    options.IdleTimeout <- TimeSpan.FromSeconds(10)
        
        services.AddDistributedMemoryCache() |> ignore

        //services.AddSession(sesfunc) |> ignore
        services.AddSession(fun options -> 
                                    options.IdleTimeout <- TimeSpan.FromSeconds(300.0)
                                    options.Cookie.HttpOnly <- false
                                    options.Cookie.Name <- "wriosession") |> ignore
        // Add framework services.
        services.AddControllers() |> ignore

        //services.AddSingleton<IMyConfig, MyConfig>() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseSession() |> ignore

        //app.UseHttpsRedirection() |> ignore
        app.UseRouting() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
            ) |> ignore

    member val Configuration : IConfiguration = null with get, set
