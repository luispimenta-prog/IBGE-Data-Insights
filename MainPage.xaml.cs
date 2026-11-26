using System.Collections.ObjectModel;
using IbgeNomes.Models;
using IbgeNomes.Services;
namespace IbgeNomes.Views;

public partial class MainPage : ContentPage
{
    private readonly IbgeService _api = new();
    public ObservableCollection<Localidade> Estados { get; } = new();
    public ObservableCollection<Localidade> Municipios { get; } = new();
    public ObservableCollection<NomeFrequencia> Homens { get; } = new();
    public ObservableCollection<NomeFrequencia> Mulheres { get; } = new();
    public ObservableCollection<NomeFrequencia> Sobrenomes { get; } = new();

    private bool _mostrarMunicipio, _carregando; private string _erro = "";
    public bool MostrarMunicipio { get => _mostrarMunicipio; set { _mostrarMunicipio = value; OnPropertyChanged(); } }
    public bool Carregando { get => _carregando; set { _carregando = value; OnPropertyChanged(); } }
    public string Erro { get => _erro; set { _erro = value; OnPropertyChanged(); } }

    public MainPage() { InitializeComponent(); BindingContext = this; PickerTipo.SelectedIndex = 0; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Estados.Count > 0) return;
        await Executar(async () => { foreach (var e in await _api.GetEstadosAsync()) Estados.Add(e); });
    }

    private void OnTipoChanged(object? s, EventArgs e)
    {
        MostrarMunicipio = PickerTipo.SelectedIndex == 1;
        Limpar();
        if (PickerEstado.SelectedItem is Localidade uf && !MostrarMunicipio) _ = CarregarNomes(uf.Id);
    }

    private async void OnEstadoChanged(object? s, EventArgs e)
    {
        if (PickerEstado.SelectedItem is not Localidade uf) return;
        if (MostrarMunicipio)
        {
            Municipios.Clear(); Limpar();
            await Executar(async () => { foreach (var m in await _api.GetMunicipiosAsync(uf.Id)) Municipios.Add(m); });
        }
        else await CarregarNomes(uf.Id);
    }

    private async void OnMunicipioChanged(object? s, EventArgs e)
    {
        if (PickerMunicipio.SelectedItem is Localidade mun) await CarregarNomes(mun.Id);
    }

    private Task CarregarNomes(int id) => Executar(async () =>
    {
        var r = await _api.GetNomesAsync(id);
        Preencher(Homens, r.Homens); Preencher(Mulheres, r.Mulheres); Preencher(Sobrenomes, r.Sobrenomes);
    });

    private static void Preencher(ObservableCollection<NomeFrequencia> c, IEnumerable<NomeFrequencia> d)
    { c.Clear(); foreach (var i in d) c.Add(i); }

    private void Limpar() { Homens.Clear(); Mulheres.Clear(); Sobrenomes.Clear(); }

    private async Task Executar(Func<Task> acao)
    {
        try { Erro = ""; Carregando = true; await acao(); }
        catch (Exception ex) { Erro = "Falha ao consultar o IBGE: " + ex.Message; }
        finally { Carregando = false; }
    }
}
