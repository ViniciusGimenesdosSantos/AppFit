using MauiAppFit.ViewModels;

namespace MauiAppFit.Views;

public partial class CadastroAtividade : ContentPage
{
	public CadastroAtividade()
	{
		InitializeComponent();

		BindingContext = new CadastroAtividadeViewModel();
	}

	protected override async void OnAppearing()
	{
		var vm = (CadastroAtividadeViewModel) BindingContext; 

		if(vm.Id == null)
		{
			vm.NovaAtividade.Execute(null);
        }
	}


}