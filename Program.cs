var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//criar uma validação de usuario por senha
List<Pessoa> listaUsuario = new List<Pessoa>();
List<Msg> mensagens = new List<Msg>();

app.MapPost("/mandarMensagem", (Msg mensagem) =>
{
    Console.WriteLine(mensagem.chave);
    Console.WriteLine(listaUsuario.Any(u => u.chave == mensagem.chave));
    if (listaUsuario.Any(u => u.chave == mensagem.chave))
    {
        mensagens.Add(mensagem);
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest("Erro");
    }
});

app.MapGet("/listarMensagens", () =>
{
    Msg[] lista = mensagens.ToArray();
    return Results.Ok(lista);
});

app.MapPost("/enviarDados", (Pessoa pessoa) =>
{
    criarUsuario(pessoa);
    return Results.Ok(pessoa);
});

void criarUsuario(Pessoa pessoa)
{
    Console.WriteLine($"{pessoa.nome} - {pessoa.descricao}");
    listaUsuario.Add(new Pessoa { chave = pessoa.chave, nome = pessoa.nome, descricao = pessoa.descricao });
}

app.MapGet("/", () => "API rodando");

app.MapGet("/usuarios", () =>
{
    foreach (Pessoa n in listaUsuario)
    {
        Console.WriteLine(n.nome);
    }
});
app.Run();

