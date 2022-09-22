global using Microsoft.EntityFrameworkCore;
using ApiCart;
using ApiCart.Configuration;
using ApiCart.Data;
using ApiCart.Model;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("carrinhoDB"));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ViewModelEntidade());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/Carrinho/ListarTodos", async (DataContext context) =>
{
    var carrinhos = await context.Carrinho
    .Include(x => x.CarrinhoProduto)
    .ToListAsync();

    if (carrinhos == null || carrinhos.Count == 0) return Results.NotFound("Nenhum carrinho encontrado!");

    var carrinhoModel = mapper.Map<List<CarrinhoModel>>(carrinhos);

    return Results.Ok(carrinhoModel);
});

app.MapGet("/Carrinho/Obter/{id}", async (DataContext context, int id) =>
{
    var carrinho = await context.Carrinho
                                .Include(x => x.CarrinhoProduto)
                                .FirstOrDefaultAsync(x => x.Id == id);

    if (carrinho == null) return Results.NotFound("Nenhum carrinho encontrado!");

    var carrinhoModel = mapper.Map<CarrinhoModel>(carrinho);

    return Results.Ok(carrinhoModel);
});

app.MapPost("/Carrinho/Adicionar", async (DataContext context, CarrinhoModel carrinhoModel) =>
 {
     try
     {
         var dbCarrinho = await context.Carrinho.FindAsync(carrinhoModel.Id);

         if (dbCarrinho == null)
         {
             foreach (var item in carrinhoModel.CarrinhoProduto)
             {
                 var produto = await context.Produto.FindAsync(item.IdProduto);

                 if (produto == null) Results.NotFound("Produto não encontrado!");

                 else if (item.Quantidade > produto?.Quantidade) return Results.NotFound("Estoque insuficiente!");

                 produto.Quantidade -= item.Quantidade;
                 context.Produto.Update(produto);
             }

             var carrinho = mapper.Map<Carrinho>(carrinhoModel);
             context.Carrinho.Add(carrinho);
             await context.SaveChangesAsync();
             return Results.Ok("Produto adicionado no carrinho com sucesso!");
         }
         else if (dbCarrinho.Finalizado)
         {
             return Results.NotFound("Carrinho já se encontrado finalizado!");
         }
         else
         {
             foreach (var item in carrinhoModel.CarrinhoProduto)
             {

                 var produto = await context.Produto.FindAsync(item.IdProduto);

                 if (produto == null) Results.NotFound("Produto não encontrado!");
                 else if (item.Quantidade > produto?.Quantidade) return Results.NotFound("Estoque insuficiente!");
                 else
                 {
                     produto.Quantidade -= item.Quantidade;
                     context.Produto.Update(produto);
                 }
                 var carrinhoProduto = await context.CarrinhoProduto
                    .Include(x => x.Produto)
                    .Include(x => x.Carrinho)
                    .Where(x => x.IdProduto == item.IdProduto && x.IdCarrinho == item.IdCarrinho).FirstOrDefaultAsync();

                 if (carrinhoProduto != null)
                 {
                     carrinhoProduto.Quantidade += item.Quantidade;
                     context.CarrinhoProduto.Update(carrinhoProduto);
                 }
                 else
                 {
                     var lista = mapper.Map<List<CarrinhoProduto>>(carrinhoModel.CarrinhoProduto);
                     dbCarrinho.CarrinhoProduto.AddRange(lista);
                 }
                 await context.SaveChangesAsync();
             }

             return Results.Ok("Produto adicionado no carrinho com sucesso!");
         }
     }
     catch (Exception ex)
     {
         return Results.BadRequest("Ocorreu um erro" + ex.Message);
     }

 });


app.MapPost("/Produtos/Cadastrar", async (DataContext context, ProdutoModel produtoModel) =>
{
    try
    {
        var produto = mapper.Map<Produto>(produtoModel);
        context.Produto.Add(produto);
        await context.SaveChangesAsync();
        return Results.Ok("Produto cadastrado com sucesso!");
    }
    catch (Exception ex)
    {
        return Results.BadRequest("Ocorreu um erro" + ex.Message);
    }

});

app.MapPut("/Carrinho/FinalizarCompra/{id}", async (DataContext context, int id) =>
{
    try
    {
        var dbCarrinho = await context.Carrinho
                                    .Include(x => x.CarrinhoProduto)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (dbCarrinho == null) return Results.NotFound("Nenhum registro encontrado!");

        dbCarrinho.Finalizado = true;
        var quantidade = new List<int>();
        var valorTotal = new List<decimal>();

        foreach (var item in dbCarrinho.CarrinhoProduto)
        {
            var produto = await context.Produto.FindAsync(item.IdProduto);

            if (produto == null) Results.NotFound("Nenhum registro encontrado!");

            quantidade.Add(item.Quantidade);
            valorTotal.Add(item.Quantidade * produto.ValorUnitario);
        }

        await context.SaveChangesAsync();

        return Results.Ok($"Carrinho finalizado! Quantidade de itens {quantidade.Sum()} e o valor total: R$ {valorTotal.Sum()}");
    }
    catch (Exception ex)
    {
        return Results.BadRequest("Ocorreu um erro" + ex.Message);
    }

});

app.MapDelete("/Carrinho/Deletar/{id}", async (DataContext context, int id) =>
{
    try
    {
        var carrinho = await context.Carrinho.FindAsync(id);
        if (carrinho == null) return Results.NotFound("Nenhum carrinho encontrado!");

        context.Carrinho.Remove(carrinho);
        await context.SaveChangesAsync();

        return Results.Ok("Carrinho removido com sucesso!");
    }
    catch (Exception ex)
    {
        return Results.BadRequest("Ocorreu um erro" + ex.Message);
    }
});

app.MapDelete("/Produto/Deletar/{id}", async (DataContext context, int id) =>
{
    try
    {
        var carrinho = await context.Produto.FindAsync(id);
        if (carrinho == null) return Results.NotFound("Nenhum produto encontrado!");

        context.Produto.Remove(carrinho);
        await context.SaveChangesAsync();

        return Results.Ok("Produto removido com sucesso!");
    }
    catch (Exception ex)
    {
        return Results.BadRequest("Ocorreu um erro" + ex.Message);
    }
});

app.Run();
