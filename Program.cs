var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

List<Msg> mensagens = new List<Msg>();

app.MapPost("/mandarMensagem", (Msg mensagem) =>
{
    int valor = mensagens.Count;

    if (valor >= 20)
    {
        mensagens.Clear();
    }
    if (!UserService.UsuarioExiste(mensagem.chave))
        return Results.Unauthorized();

    mensagens.Add(new Msg
    {
        chave = mensagem.chave,
        usuario = mensagem.usuario,
        valor = mensagem.valor
    });

    return Results.Ok();
});


app.MapPost("/listarMensagens", (Msg mensagem) =>
{
    using var conn = Database.GetConnection();
    conn.Open();

    var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        SELECT 1
        FROM users
        WHERE chave = $chave
        LIMIT 1;
    ";

    cmd.Parameters.AddWithValue("$chave", mensagem.chave);

    var existeUsuario = cmd.ExecuteScalar();

    if (existeUsuario == null)
        return Results.Unauthorized();

    mensagens.Add(mensagem);

    var lista = mensagens.Select(m => new Msg
    {
        usuario = m.usuario,
        valor = m.valor
    }).ToList();

    return Results.Ok(lista);
});


app.MapPost("/enviarDados", (Pessoa pessoa) =>
{
    try
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
           INSERT INTO users (chave, nome, descricao, password)
           VALUES ($chave, $nome, $descricao, $password);
";

        cmd.Parameters.AddWithValue("$chave", pessoa.chave);
        cmd.Parameters.AddWithValue("$nome", pessoa.nome);
        cmd.Parameters.AddWithValue("$descricao", pessoa.descricao);
        cmd.Parameters.AddWithValue("$password", pessoa.password);

        cmd.ExecuteNonQuery();

        return Results.Ok("Criado");
    }
    catch
    {
        return Results.BadRequest("Erro em criar");
    }
});

app.MapGet("/", () => "API rodando");

app.MapPost("/usuarios", (string valor) =>
{
    Console.WriteLine(valor);
    if (valor == "aaa123!")
    {
        var usuarios = UserService.ListarUsuarios();
        return Results.Ok(usuarios);

    }
    else
    {
        return Results.BadRequest();
    }
});
app.Run();

