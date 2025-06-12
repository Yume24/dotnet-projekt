## ⚙CI/CD – GitHub Actions

W projekcie skonfigurowano workflow `dotnet-ci.yml`, który automatycznie uruchamia proces budowania i testowania aplikacji przy każdym **pushu lub pull requeście na gałęzi `main`**.

### Etapy workflow:
1. **Checkout** – pobranie kodu źródłowego z repozytorium.
2. **Setup .NET** – przygotowanie środowiska z odpowiednią wersją SDK (8.0).
3. **Restore** – pobranie zależności (`dotnet restore`).
4. **Build** – kompilacja aplikacji (`dotnet build`).
5. **Test** – uruchomienie testów jednostkowych (`dotnet test`).

### 📄 Plik workflow:
Zlokalizowany w: .github/workflows/dotnet-ci.yml
