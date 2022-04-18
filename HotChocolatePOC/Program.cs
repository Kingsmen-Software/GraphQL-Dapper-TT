using HotChocolatePOC.Data.Interfaces;
using HotChocolatePOC.Data.Services;
using HotChocolatePOC.Domain.Classes;
using HotChocolatePOC.Domain.Entities;
using HotChocolatePOC.Domain.Interfaces;
using HotChocolatePOC.GraphQL.Query;

var builder = WebApplication.CreateBuilder(args);

//Setup Database Connection String
var contextData = new ContextData
{
    DatabaseConnectionString = builder.Configuration.GetConnectionString("DatabaseConnection")
};

builder.Services.AddSingleton<IContextData>(contextData);

builder.Services.AddTransient<ISqlConnectionService, SqlConnectionService>();
builder.Services.AddTransient<IDomainReflectionService, DomainReflectionService>();
builder.Services.AddTransient<IQueryBuilderService, QueryBuilderService>();
builder.Services.AddTransient<IQueryExecuteService, QueryExecuteService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<User>()
    .AddType<UserRole>()
    .AddType<RoleAction>()
    .InitializeOnStartup();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapGraphQL();

app.Run();
