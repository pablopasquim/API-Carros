using API.Models;  
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

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
    ([FromServices] AppDataContext ctx) => {

    var modelos = ctx.Modelos.ToList();

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
