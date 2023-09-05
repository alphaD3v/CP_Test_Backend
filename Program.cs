using api.Data;
using api.Models;
using api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

string keyWord = "JOHN";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

// ioc
services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));

services.AddScoped<DataSeeder>();
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IEmailRepository, EmailRepository>();
services.AddScoped<IDocumentRepository, DocumentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
List<Client> lstClient = new List<Client>();



app.MapGet("/clients", async (IClientRepository clientRepository) =>
{
    //sample data to be used
    //and added to a list
    Client clt1 = new Client()
    {
        Email = "john@gmail.com",
        Id = "xosiosiosdhad",
        FirstName = "John",
        LastName = "Smith",
        PhoneNumber = "8202820232"
    };
    lstClient.Add(clt1);
    Client clt2 = new Client()
    {
        Email = "ja_23@live.com",
        Id = "001",
        FirstName = "John Anderson",
        LastName = "LeBlanc",
        PhoneNumber = "09209641757"
    };
    lstClient.Add(clt2);
    Client clt3 = new Client()
    {
        Email = "ja_23@live.com",
        Id = "002",
        FirstName = "Leonard",
        LastName = "Thompson",
        PhoneNumber = "09199621978"
    };
    lstClient.Add(clt3);
    //call task to create record
    int result = await clientRepository.Create(lstClient);
    //get email of the client
    string oldEmailAddress = clt1.Email;
    //call task to update a record
    string emailAddress = await clientRepository.Update(clt1);
    //send email address once update is complete
    if(emailAddress != oldEmailAddress)
    {
        //await clientRepository.SendEmail(emailAddress);
    }
    return await clientRepository.Get(keyWord);
})
.WithName("get clients");       

app.UseCors();

// seed data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed(lstClient);
}

// run app
app.Run();