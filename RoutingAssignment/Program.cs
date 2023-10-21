var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

Dictionary<int, string> countries = new Dictionary<int, string>();
countries.Add(1, "India");
countries.Add(2, "United States");
countries.Add(3, "Canada");
countries.Add(4, "UK");
countries.Add(5, "Japan");


app.UseEndpoints(endpoints =>
{
    endpoints.Map("countries", async context =>
    {
        foreach(KeyValuePair<int, string> kvp in countries)
        {
            await context.Response.WriteAsync($"{kvp.Key}, {kvp.Value}\n");
        }
    });

    endpoints.MapGet("/countries/{id:int:Range(1,100)}", async context =>
    {
        var id = Convert.ToInt32(context.Request.RouteValues["id"]);
        if (countries.ContainsKey(id))
        {
            await context.Response.WriteAsync(countries[id]);
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("[No Country]");
        }
    });

    endpoints.MapGet("/countries/{id:int:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The id should be between 1 and 100");
    });
});

app.Run();