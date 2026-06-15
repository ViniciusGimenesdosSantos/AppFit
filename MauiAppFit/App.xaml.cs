using MauiAppFit.Helpers;

namespace MauiAppFit
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper? database;

        public static SQLiteDatabaseHelper? Database
        {
            get
            {
                if (database == null)
                {
                    var local_instalacao = Environment.SpecialFolder.ApplicationData;

                    var caminho_instalacao = Environment.GetFolderPath(local_instalacao);

                    string arquivo_sqlite = Path.Combine(caminho_instalacao, "Fit.db3");

                    database = new SQLiteDatabaseHelper(arquivo_sqlite);
                }

                return database;
            }
        }

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var w = new Window(new AppShell());
            
            w.Height = 700;
            w.Width = 350;

            return w;
        }
    }
}