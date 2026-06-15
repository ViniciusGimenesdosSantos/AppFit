# MauiAppFit

Aplicativo desenvolvido em **.NET MAUI** para a disciplina de Desenvolvimento Mobile da FATEC Jahu. É um diário de atividades físicas: o usuário registra atividades (descrição, data, peso e observações), com persistência local em **SQLite**, navegação por abas (**TabBar**) e arquitetura **MVVM** completa.

## Funcionamento do App

O app utiliza **Shell** com `TabBar`, dividido em três abas:

### 1. Nova Atividade (`CadastroAtividade`)
Tela de cadastro/edição, com `BindingContext` para `CadastroAtividadeViewModel`:

- Campos vinculados via *data binding*: Descrição (`Entry`), Data (`DatePicker`), Peso (`Entry` numérico) e Observações (`Entry`).
- Toolbar com dois botões: **"Nova"** (limpa o formulário através do comando `NovaAtividade`) e **"Salvar"** (executa `SalvarAtividade`).
- Ao salvar: se `Id` for nulo, insere um novo registro no banco; caso contrário, atualiza o registro existente. Depois exibe um alerta de sucesso e navega de volta para a lista (`Shell.Current.GoToAsync("//ListaAtividades")`).
- Quando aberta a partir da lista (clicando em "Detalhes"), recebe o `Id` da atividade via **navegação com parâmetros de query** (`[QueryProperty]`), carrega os dados do banco e preenche o formulário (modo edição).

### 2. Minhas Atividades (`ListaAtividades`)
Tela com `ListaAtividadesViewModel`:

- Exibe as atividades cadastradas em uma `ListView` dentro de um `RefreshView` (pull-to-refresh).
- `SearchBar` + botão "Buscar" filtram atividades pela descrição (consulta `LIKE` no banco).
- Cada item da lista tem ações de contexto (`ContextActions`): **"Detalhes"** (navega para `CadastroAtividade` passando o `Id` via rota) e **"Excluir"** (remove o registro após confirmação).
- Toolbar com botão **"Atualizar"** que recarrega a lista do banco.

### 3. Sobre (`Sobre`)
Tela simples de informações sobre o app.

### Persistência (SQLite)
A classe `SQLiteDatabaseHelper` (em `Helpers/`) encapsula o acesso ao banco com `SQLiteAsyncConnection`, oferecendo `Insert`, `Update`, `Delete`, `GetAllRows`, `GetById` e `Search`, acessada globalmente via `App.Database`. O banco `Fit.db3` é armazenado em `Environment.SpecialFolder.ApplicationData`.

## Conteúdos novos aprendidos

- Arquitetura **MVVM completa**: ViewModels (`CadastroAtividadeViewModel`, `ListaAtividadesViewModel`) implementando `INotifyPropertyChanged`, com propriedades que notificam a View sobre alterações (`PropertyChanged`).
- **ICommand / Command<T>** para vincular ações (botões, toolbar items, menus de contexto) diretamente às ViewModels via `{Binding}`, sem usar eventos `Clicked` no code-behind.
- **Navegação por rotas no Shell** (`Shell.Current.GoToAsync`) e **passagem de parâmetros entre páginas** com `[QueryProperty]`.
- **TabBar** no `AppShell.xaml` para organizar o app em múltiplas abas.
- **RefreshView** combinado com `ListView` e binding de `IsRefreshing` para pull-to-refresh controlado pela ViewModel.
- `ViewCell.ContextActions` com `Command` apontando para a ViewModel da página através de `{Binding Source={x:Reference Pagina}, Path=BindingContext.Comando}`.
- Continuação do uso de **SQLite** (CRUD assíncrono) já com o padrão MVVM, separando completamente a lógica de dados da interface.
- Uso de `OnAppearing()` para disparar comandos da ViewModel ao exibir uma página (ex: carregar lista, resetar formulário).

## Tecnologias

- .NET 10 (MAUI)
- C# / XAML
- SQLite (sqlite-net-pcl)
- Padrão MVVM
- Plataformas suportadas: Android, iOS, MacCatalyst, Windows

## Como executar

1. Clone o repositório.
2. Abra o arquivo de solução (`.slnx`/`.sln`) no Visual Studio 2022 (ou superior) com a workload **.NET Multi-platform App UI** instalada.
3. Selecione o destino de execução (ex: Android Emulator ou Windows Machine).
4. Pressione **F5** ou clique em **Run** para compilar e executar o app. O banco de dados SQLite é criado automaticamente na primeira execução.
