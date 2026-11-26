# IBGE Nomes por Localidade (.NET MAUI)

**Integrantes:** _Luis Pimenta_, _Paulo Henrique_, _Thainá Mariano_

App que encadeia duas APIs do IBGE: Localidades (estados/municípios) → Nomes Censo 2022.

## Arquitetura em camadas
- `Models/` – `Localidade`, `NomeFrequencia`, `ResultadoNomes`
- `Services/IbgeService.cs` – HttpClient, JSON e URLs das APIs
- `Views/MainPage` – Pickers (tipo, estado, município) e listas de homens, mulheres e sobrenomes

## Como executar
1. `dotnet new maui -n IbgeNomes` (.NET 8+ com workload `maui`).
2. Copie as pastas `Models`, `Services` e `Views` para dentro do projeto (apague o `MainPage` original).
3. Em `AppShell.xaml`, aponte o `ShellContent` para `views:MainPage`
   (`xmlns:views="clr-namespace:IbgeNomes.Views"`). Em `App.xaml.cs`/`AppShell` remova referências ao `MainPage` antigo.
4. `dotnet build -t:Run -f net8.0-android` (emulador) ou rode pelo Visual Studio.

## API de Nomes
`GET /api/v3/nomes/2022/localidade/{id}` retorna `{localidade, homens[], mulheres[], sobrenomes[]}`,
cada item com `nome` e `frequencia` (top 10, já ordenado). O app numera a posição pela ordem.
