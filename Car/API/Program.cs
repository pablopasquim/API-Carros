using API.Models;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

// Swagger

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Carro",
        Description = "Api cara cadastro e listagem de carros",
        Contact = new OpenApiContact
        {
            Name = "Pablo",
            Email = "pablo.pasquim7@gmail.com",
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// Endpoints = relacionados ao recursos de Carros

// GET: api/carros = Lista de todos os carros cadastrados
app.MapGet("/api/carros", 
    ([FromServices] AppDataContext ctx) => {

    if(ctx.Carros.Any()){
        return Results.Ok(ctx.Carros.ToList());
    }
    return Results.NotFound();
});

// GET: Buscar carro pelo ID
app.MapGet("/api/carros/{id}", 
    ([FromRoute] int Id, [FromServices] AppDataContext ctx) => {

    Carro? carro = ctx.Carros.Find(Id);

    if(carro != null){
        return Results.Ok(carro);
    }

    return Results.NotFound();
});

// POST: Cadastrar carros e salvar
app.MapPost("/api/carros/cadastrar", 
    ([FromBody] Carro carro, [FromServices] AppDataContext ctx) => {

    carro.Modelo = ctx.Modelos.Find(carro.Modelo.Id);

    if(carro.Modelo == null){
        return Results.BadRequest("Modelo not found!");
    }

    if(carro.Name == null || carro.Name.Length < 3){
        return Results.BadRequest("O nome do modelo precisa ter mais de 3char");
    }

    ctx.Carros.Add(carro);
    ctx.SaveChanges();
    return Results.Created("", carro);
});

// PUT: Atualiza os dados de um carro pelo ID
app.MapPut("/api/carros/{id}", 
    ([FromRoute] int Id, [FromBody] Carro carro, [FromServices] AppDataContext ctx) => {

    Carro? entidade = ctx.Carros.Find(Id);

    if (entidade == null) {
    return Results.NotFound();  
    }
    
    entidade.Modelo = ctx.Modelos.Find(Id);

    

    if(entidade != null){
        entidade.Name = carro.Name;
        ctx.Carros.Update(entidade);
        ctx.SaveChanges();
        return Results.Ok(entidade);
    }

    return Results.NotFound();
});

// DELETE: Remover um carro pelo ID
app.MapDelete("/api/carros/{id}", 
    ([FromRoute] int Id, [FromServices] AppDataContext ctx) => {
    
    Carro? carro = ctx.Carros.Find(Id);

    if(carro == null){
        return Results.NotFound();
    }

    ctx.Carros.Remove(carro);
    ctx.SaveChanges();
    return Results.NoContent();
});

// GET: Lista todos os modelos cadastrados
app.MapGet("/api/modelos", 
    ([FromQuery] string name, [FromServices] AppDataContext ctx) => {

    var query = ctx.Modelos.AsQueryable();

    if(!string.IsNullOrWhiteSpace(name)){
        query = query.Where(m => EF.Functions.Like(m.Name, "%" + name + "%"));
    }
      
    var modelos = query.ToList();

    if(modelos == null || modelos.Count == 0)
    {   
        return Results.NotFound();
    }
        return Results.Ok(modelos);
});

// GET: Listar o modelo buscando pelo Id
app.MapGet("/api/modelos/{id}", 
    ([FromRoute] int Id, [FromServices] AppDataContext ctx) => {

    var modelos = ctx.Modelos.Find(Id);

    if(modelos != null){
        return Results.Ok(modelos);
    }

    return Results.NotFound();
});

app.Run();
