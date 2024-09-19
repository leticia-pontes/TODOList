using System;
using System.Collections.Generic;
using System.Linq;

namespace TODOList
{
    class Tarefa
    {
        private static int _idCounter = 1;

        public int Id { get; private set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Entregue { get; set; }
        public DateOnly DataCriacao { get; set; }
        public DateOnly? DataEntrega { get; set; }

        public Tarefa(string nome, string? descricao, DateOnly dataCriacao)
        {
            this.Id = _idCounter++;
            this.Nome = nome;
            this.Descricao = descricao;
            this.Entregue = false;
            this.DataCriacao = dataCriacao;
            this.DataEntrega = null;
        }
    }

    internal class Program
    {
        static void MostrarMenu()
        {
            Console.WriteLine("\nTODO List Manager");
            Console.WriteLine("1. Mostrar Lista");
            Console.WriteLine("2. Criar Tarefa");
            Console.WriteLine("3. Concluir Tarefa");
            Console.WriteLine("4. Excluir Tarefa");
            Console.WriteLine("5. Sair");
        }

        static void MostrarLista(List<Tarefa> listaTarefas)
        {
            if (listaTarefas.Count == 0)
            {
                Console.WriteLine("A lista de tarefas está vazia.\n");
                return;
            }

            foreach (var item in listaTarefas)
            {
                Console.WriteLine($"ID: {item.Id}");
                Console.WriteLine($"Nome: {item.Nome}");
                Console.WriteLine($"Descrição: {item.Descricao ?? "N/A"}");
                Console.WriteLine($"Data de Criação: {item.DataCriacao:dd/MM/yyyy}");
                Console.WriteLine($"Status: {(item.Entregue ? "Entregue" : "Não Entregue")}");
                Console.WriteLine($"Data de Entrega: {(item.DataEntrega.HasValue ? item.DataEntrega.Value.ToString("dd/MM/yyyy") : "N/A")}");
                Console.WriteLine();
            }
        }

        static void CriarTarefa(List<Tarefa> listaTarefas)
        {
            Console.Write("\nNome: ");
            string nome = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("O nome da tarefa não pode ser vazio!\n");
                return;
            }

            Console.Write("Descrição: ");
            string? descricao = Console.ReadLine();

            DateOnly dataCriacao = DateOnly.FromDateTime(DateTime.Now);
            Tarefa tarefa = new Tarefa(nome, descricao, dataCriacao);

            listaTarefas.Add(tarefa);

            Console.WriteLine("\nTarefa criada com sucesso!\n");
        }

        static Tarefa? BuscarTarefa(List<Tarefa> listaTarefas)
        {
            Console.Write("\nInforme o nome ou ID da tarefa: ");
            string? input = Console.ReadLine();

            Tarefa? tarefaEncontrada = null;

            if (int.TryParse(input, out int id))
            {
                tarefaEncontrada = listaTarefas.FirstOrDefault(tarefa => tarefa.Id == id);
            }
            else
            {
                tarefaEncontrada = listaTarefas.FirstOrDefault(tarefa => tarefa.Nome.Equals(input, StringComparison.OrdinalIgnoreCase));
            }

            return tarefaEncontrada;
        }

        static void ConcluirTarefa(List<Tarefa> listaTarefas)
        {
            Tarefa? tarefaEncontrada = BuscarTarefa(listaTarefas);

            if (tarefaEncontrada != null)
            {
                if (!tarefaEncontrada.Entregue)
                {
                    tarefaEncontrada.Entregue = true;
                    tarefaEncontrada.DataEntrega = DateOnly.FromDateTime(DateTime.Now);
                    Console.WriteLine("\nTarefa concluída com sucesso!\n");
                }
                else
                {
                    Console.WriteLine("\nA tarefa já foi concluída anteriormente!\n");
                }
            }
            else
            {
                Console.WriteLine("\nTarefa não encontrada!\n");
            }
        }

        static void ExcluirTarefa(List<Tarefa> listaTarefas)
        {
            Tarefa? tarefaEncontrada = BuscarTarefa(listaTarefas);

            if (tarefaEncontrada != null)
            {
                listaTarefas.Remove(tarefaEncontrada);
                Console.WriteLine("\nTarefa excluída com sucesso!\n");
            }
            else
            {
                Console.WriteLine("\nTarefa não encontrada!\n");
            }
        }

        static void Main(string[] args)
        {
            List<Tarefa> listaTarefas = new List<Tarefa>();

            bool rodar = true;

            while (rodar)
            {
                MostrarMenu();

                try
                {
                    int opcao = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine();

                    switch (opcao)
                    {
                        case 1:
                            MostrarLista(listaTarefas);
                            break;
                        case 2:
                            CriarTarefa(listaTarefas);
                            break;
                        case 3:
                            ConcluirTarefa(listaTarefas);
                            break;
                        case 4:
                            ExcluirTarefa(listaTarefas);
                            break;
                        case 5:
                            rodar = false;
                            break;
                        default:
                            Console.WriteLine("Opção inválida! Tente novamente.\n");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Por favor, insira um número válido.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}\n");
                }
            }
        }
    }
}
