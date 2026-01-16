var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

//criar uma validação de usuario por senha
List<Pessoa> listaUsuario = new List<Pessoa>();
List<Msg> mensagens = new List<Msg>();

app.MapPost("/mandarMensagem", (Msg mensagem) =>
{
    int valor = mensagens.Count;

    if (valor >= 20)
    {
        mensagens.Clear();
    }
    var usuario = listaUsuario.FirstOrDefault(u => u.chave == mensagem.chave);
    if (usuario == null)
        return Results.Unauthorized();

    mensagens.Add(new Msg
    {
        usuario = usuario.nome,
        valor = mensagem.valor
    });

    return Results.Ok();
});


app.MapPost("/listarMensagens", (Msg mensagem) =>
{
    Msg[] lista = mensagens.ToArray();
    if (listaUsuario.Any(u => u.chave == mensagem.chave))
    {
        mensagens.Add(mensagem);
        return Results.Ok(lista);
    }
    else
    {
        return Results.BadRequest("Erro");
    }
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

app.MapPost("/usuarios", (string valor) =>
{
    Console.WriteLine(valor);
    if (valor == "aaa123!")
    {
        Pessoa[] a = listaUsuario.ToArray();
        return Results.Ok(a);
    }
    else
    {
        return Results.BadRequest();
    }
});
app.Run();

