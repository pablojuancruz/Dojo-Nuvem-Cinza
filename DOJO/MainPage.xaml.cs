using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DOJO.ServicoCalculo;

namespace DOJO
{
    public partial class MainPage : UserControl
    {
        int _vez = 0;
        public MainPage()
        {
            InitializeComponent();

            var cliente = new CalculosClient();
            cliente.PrimeiroDiaCompleted += new EventHandler<PrimeiroDiaCompletedEventArgs>(PrimeiroDia);
            cliente.PrimeiroDiaAsync();


            cliente.PrimeiroAeroportoCompleted += new EventHandler<PrimeiroAeroportoCompletedEventArgs>(PrimeiroAeroporto);
            cliente.PrimeiroAeroportoAsync();


            cliente.TodosAeroportosCompleted += new EventHandler<TodosAeroportosCompletedEventArgs>(TodosAeroportos);
            cliente.TodosAeroportosAsync();

            cliente.DiasPrimeiroAeroportoCompleted += new EventHandler<DiasPrimeiroAeroportoCompletedEventArgs>(DiasPrimeiroAeroporto);
            cliente.DiasPrimeiroAeroportoAsync();


            cliente.DiasTodosAeroportosCompleted += new EventHandler<DiasTodosAeroportosCompletedEventArgs>(DiasTodosAeroportos);
            cliente.DiasTodosAeroportosAsync();

        }

        private void DiasTodosAeroportos(object sender, DiasTodosAeroportosCompletedEventArgs e)
        {
            lblTodos.Content = String.Format("Todos os aeroportos estarão cobertos em {0} ", e.Result);
        }

        private void DiasPrimeiroAeroporto(object sender, DiasPrimeiroAeroportoCompletedEventArgs e)
        {
            lblPrimeiro.Content = String.Format("O primeiro Aeroporto estará coberto em {0} dias", e.Result);
        }

        private void TodosAeroportos(object sender, TodosAeroportosCompletedEventArgs e)
        {
            var Mapa = BiDimensionalMatriz(e.Result);
            BindDataGrid(Mapa, gridTodos);
        }

        private void PrimeiroDia(object sender, PrimeiroDiaCompletedEventArgs e)
        {
            var Mapa = BiDimensionalMatriz(e.Result);
            BindDataGrid(Mapa, gridInicio);
        }

        private void PrimeiroAeroporto(object sender, PrimeiroAeroportoCompletedEventArgs e)
        {
            var Mapa = BiDimensionalMatriz(e.Result);
            BindDataGrid(Mapa, gridPrimeiro);
        }


        char[,] BiDimensionalMatriz(string resultado)
        {

            char[,] rollback = new char[10, 10];
            var rows = resultado.Split(';');

            for (int i = 0; i < 10; i++)
            {
                var columns = rows[i].Split('|');

                for (int j = 0; j < 10; j++)
                {
                    Char.TryParse(columns[j].ToString(), out rollback[i, j]);
                }
            }
            return rollback;

        }
        void BindDataGrid(char[,] _array, DataGrid grid)
        {
            ObservableCollection<ObservableCollection<char>> dataSource = new ObservableCollection<ObservableCollection<char>>();
            for (int i = 0; i < 10; i++)
            {
                ObservableCollection<char> row = new ObservableCollection<char>();
                for (int j = 0; j < 10; j++)
                {
                    row.Add(_array[i, j]);
                }
                dataSource.Add(row);
            }
            for (int i = 0; i < 10; i++)
            {
                DataGridTextColumn dataColumn = new DataGridTextColumn();
                dataColumn.Header = "       ";
                dataColumn.Binding = new Binding("[" + i.ToString() + "]");
                grid.Columns.Add(dataColumn);
            }
            grid.ItemsSource = dataSource;
        }
    }
}
