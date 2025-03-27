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

// POST: api/carros/cadastrar = Cadastrar carros e salvar
app.MapPost("/api/carros/cadastrar", 
    ([FromBody] Carro carro, [FromServices] AppDataContext ctx) => {

    ctx.Carros.Add(carro);
    ctx.SaveChanges();
    return Results.Created("", carro);
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

// PUT: Atualiza os dados de um carro pelo ID
app.MapPut("/api/carros/{id}", 
    ([FromRoute] int Id, [FromBody] Carro carro, [FromServices] AppDataContext ctx) => {

    Carro? entidade = ctx.Carros.Find(Id);

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

app.Run();
