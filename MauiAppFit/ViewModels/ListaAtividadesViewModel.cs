using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MauiAppFit.Models;
using System.Windows.Input;

namespace MauiAppFit.ViewModels
{
    public class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /* Pegar o que foi digitado na SearchBar */
        public string ParametroBusca { get; set; }

        /* Gerenciar a RefreshView */
        bool estaAtualizando = false;
        public bool EstaAtualizando
        {
            get => estaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizando"));
            }
        }

        /* Coleção que armazena as atividades cadastradas. */
        ObservableCollection<Atividade> listaAtividades = new();
        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividades;
            set => listaAtividades = value;
        }

        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;

                        EstaAtualizando = true;
                        List<Atividade> tmp = await App.Database.GetAllRows();
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                    } finally
                    {
                        EstaAtualizando = false;
                    } // fecha try-catch-finally
                }); // fecha return
            } // fecha get
        } // fecha atualizar lista

        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;

                        EstaAtualizando = true;
                        List<Atividade> tmp = await App.Database.Search(ParametroBusca);
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));   
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        } // fecha buscar

        public ICommand AbrirDetalhes
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    string uri = $"//CadastroAtividade?parametro_id{id}";
                    await Shell.Current.GoToAsync(uri);
                });
            }
        }

        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Shell.Current.DisplayAlertAsync(
                            "Tem certeza?", "Excluir?", "Sim", "Não");

                        if(conf)
                        {
                            await App.Database.Delete(id);
                            AtualizarLista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    } // fecha classe
} // fecha namespace
