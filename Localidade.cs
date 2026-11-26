using System.Text.Json.Serialization;
namespace IbgeNomes.Models;

// Estado ou município (mesmo formato básico: id + nome)
public class Localidade
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("nome")] public string Nome { get; set; } = "";
    [JsonPropertyName("sigla")] public string? Sigla { get; set; } // só estados
    public override string ToString() => Sigla is null ? Nome : $"{Nome} ({Sigla})";
}
