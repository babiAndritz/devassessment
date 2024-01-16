using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace Graph
{
    public interface IGraph<T>
    {
        IObservable<IEnumerable<T>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : IGraph<T>
    {
        public IEnumerable<ILink<T>> Links;

        public Graph(IEnumerable<ILink<T>> links)
        {
            this.Links = links;
        }

        public IEnumerable<object> Vertices { get; set; }

        // IObservable é uma interface de uma fonte de dados assíncrona e infinita que gera uma sequência de valores
        // IEnumerable emite sequências
        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            var visited = new HashSet<T>(); // tabela hash para armazenar dados sem repetições dos nós já visitados
            var path = new List<T>(); // lista de nós do caminho atual
            var allPaths = new List<IEnumerable<T>>(); // lista de todos os caminhos encontrados

            FindAllPaths(source, target, visited, path, allPaths);

            return allPaths.ToObservable(); 
        }

        // algoritmo de busca em profundidade
        private void FindAllPaths(T current, T target, HashSet<T> visited, List<T> path, List<IEnumerable<T>> allPaths)
        {
            visited.Add(current); // transforma o nó atual em um nó visitado
            path.Add(current); // adiciona o nó no caminho atual

            if (current.Equals(target))
            {
                // se o nó atual for o nó alvo, o caminho acabou e adiciona esse caminho na lista de todos os caminhos completos
                allPaths.Add(path.ToList());
            }
            else // se o nó não for o alvo final
            { 
                // para cada link dos links em que o de origem é o atual
                foreach (var link in Links.Where(l => l.Source.Equals(current)))
                {
                    if (!visited.Contains(link.Target)) // se o nó visitado não contém um link para o nó alvo
                    {
                        FindAllPaths(link.Target, target, visited, path, allPaths); // repetir recursivamente a função
                        // sendo que o nó atual passa a ser o nó de destino desse link, para dar sequência no caminho
                    }
                }
            }

            path.RemoveAt(path.Count - 1); // remove o caminho atual para uma possibilidade de ter outros caminhos
            visited.Remove(current); // remove os nós visitados para uma possibilidade de ter outros caminhos
        }

    }
}
