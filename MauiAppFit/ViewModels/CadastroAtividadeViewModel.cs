using MauiAppFit.Models;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace MauiAppFit.ViewModels
{
    [QueryProperty("PegarIdNavegacao", "parametro_id")]
    public class CadastroAtividadeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        string? descricao;
        string? observacoes;
        int? id;
        DateTime data = DateTime.Now;
        double? peso;

        public string PegarIdNavegacao
        {
            set
            {
                int id = Convert.ToInt32(Uri.UnescapeDataString(value));
                VerAtividade.Execute(id);
            }
        }

        public ICommand VerAtividade
        {
            get => new Command<int>(async (int id) =>
            {
                try
                {
                    Atividade model = await App.Database.GetById(id);
                    this.Id = model.Id;
                    this.Descricao = model.Descricao;
                    this.Observacoes = model.Observacoes;
                    this.Data = model.Data;
                    this.Peso = model.Peso;
                } catch(Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                }
            });
        } // fecha command VerAtividade

        public ICommand NovaAtividade
        {
            get => new Command(() =>
            {
                this.Id = null;
                this.Descricao = "";
                this.Data = DateTime.Now;
                this.Peso = null;
                this.Observacoes = string.Empty;
            });
        }

        public string? Descricao
        {
            get => descricao;
            set
            {
                descricao = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Descricao"));
            }
        }

        public string? Observacoes
        {
            get => observacoes;
            set
            {
                observacoes = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Observacoes"));
            }
        }

        public int? Id
        {
            get => id;
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public DateTime? Data
        {
            get => data;
            set
            {
                data = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Data)));
            }
        }

        public double? Peso
        {
            get => peso;
            set
            {
                peso = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Peso"));
            }
        }

        public ICommand SalvarAtividade
        {
            get => new Command(async () =>
            {
                try
                {
                    Atividade model = new()
                    {
                        Descricao = this.Descricao,
                        Data = this.Data,
                        Peso = this.Peso,
                        Observacoes = this.Observacoes
                    };

                    if(this.Id == null)
                    {
                        Debug.WriteLine("---------------------------------");
                        Debug.WriteLine("Vou inserir");

                        await App.Database.Insert(model);
                    } else
                    {
                        model.Id = this.Id;
                        await App.Database.Update(model);
                    }

                    await Shell.Current.DisplayAlertAsync("Beleza","Dados salvos com sucesso!", "OK");
                    await Shell.Current.GoToAsync("//ListaAtividades");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                }
            });
        }
    }
}
