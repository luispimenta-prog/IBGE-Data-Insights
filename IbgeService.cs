using System.Net.Http.Json;
using System.Text.Json;
using IbgeNomes.Models;
namespace IbgeNomes.Services;

/// Camada de integração: única classe que conhece as URLs e o JSON do IBGE.
public class IbgeService
{
    private const string Localidades = "https://servicodados.ibge.gov.br/api/v1/localidades";
    private const string Nomes = "https://servicodados.ibge.gov.br/api/v3/nomes/2022/localidade";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };
    private static readonly JsonSerializerOptions Opt = new() { PropertyNameCaseInsensitive = true };

    public Task<List<Localidade>> GetEstadosAsync() =>
        GetListaAsync($"{Localidades}/estados?orderBy=nome");

    public Task<List<Localidade>> GetMunicipiosAsync(int ufId) =>
        GetListaAsync($"{Localidades}/estados/{ufId}/municipios?orderBy=nome");

    private async Task<List<Localidade>> GetListaAsync(string url) =>
        await Http.GetFromJsonAsync<List<Localidade>>(url, Opt) ?? new();

    /// Consulta encadeada: usa o id escolhido na API de Localidades.
    public async Task<ResultadoNomes> GetNomesAsync(int idLocalidade)
    {
        var r = await Http.GetFromJsonAsync<ResultadoNomes>($"{Nomes}/{idLocalidade}", Opt) ?? new();
        Formatar(r.Homens); Formatar(r.Mulheres); Formatar(r.Sobrenomes);
        return r;
    }

    // A API devolve nomes em minúsculas e sem posição: capitaliza e numera pela ordem.
    private static void Formatar(List<NomeFrequencia> lista)
    {
        var ti = System.Globalization.CultureInfo.GetCultureInfo("pt-BR").TextInfo;
        for (int i = 0; i < lista.Count; i++)
        {
            lista[i].Posicao = i + 1;
            lista[i].Nome = ti.ToTitleCase(lista[i].Nome.ToLowerInvariant());
        }
    }
}
