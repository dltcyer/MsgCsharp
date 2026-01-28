using Microsoft.Data.Sqlite;

public static class UserService
{
    public static bool UsuarioExiste(string chave)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT 1 FROM users WHERE chave = $chave;
        ";
        cmd.Parameters.AddWithValue("$chave", chave);

        using var reader = cmd.ExecuteReader();
        return reader.Read();
    }

    public static bool ValidarUsuario(string chave, string password)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT password FROM users WHERE chave = $chave;
        ";
        cmd.Parameters.AddWithValue("$chave", chave);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return false;

        return password == reader.GetString(0);
    }

    public static List<Pessoa> ListarUsuarios()
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT chave, nome, descricao FROM users;
        ";

        using var reader = cmd.ExecuteReader();
        var lista = new List<Pessoa>();

        while (reader.Read())
        {
            lista.Add(new Pessoa
            {
                chave = reader.GetString(0),
                nome = reader.GetString(1),
                descricao = reader.IsDBNull(2) ? null : reader.GetString(2)
            });
        }

        return lista;
    }
}
