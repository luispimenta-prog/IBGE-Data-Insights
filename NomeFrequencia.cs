using System.Text.Json.Serialization;
namespace IbgeNomes.Models;

public class NomeFrequencia
{
    [JsonPropertyName("nome")] public string Nome { get; set; } = "";
    [JsonPropertyName("frequencia")] public long Frequencia { get; set; }
    [JsonIgnore] public int Posicao { get; set; } // calculada pela ordem recebida
    public string Descricao => $"{Posicao}º  {Nome}  —  {Frequencia:N0}";
}

// Mapeia o JSON de /api/v3/nomes/2022/localidade/{id}
public class ResultadoNomes
{
    [JsonPropertyName("localidade")] public int Localidade { get; set; }
    [JsonPropertyName("homens")] public List<NomeFrequencia> Homens { get; set; } = new();
    [JsonPropertyName("mulheres")] public List<NomeFrequencia> Mulheres { get; set; } = new();
    [JsonPropertyName("sobrenomes")] public List<NomeFrequencia> Sobrenomes { get; set; } = new();
}
