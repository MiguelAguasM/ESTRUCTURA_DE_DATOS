using System;
using System.Collections.Generic;

namespace SimuladorNavegador
{
    // Clase que representa una página web visitada
    public class PaginaWeb
    {
        public string Url { get; set; }
        public DateTime FechaHoraVisita { get; set; }

        public PaginaWeb(string url)
        {
            Url = url;
            FechaHoraVisita = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{FechaHoraVisita:HH:mm:ss}] - {Url}";
        }
    }

    // Clase que gestiona la lógica del navegador usando una Pila
    public class Navegador
    {
        // Utilizamos la estructura Stack (Pila) de C#
        private Stack<PaginaWeb> historialPila;
        private PaginaWeb paginaActual;

        public Navegador()
        {
            historialPila = new Stack<PaginaWeb>();
            paginaActual = null;
        }

        // Método para acceder a una nueva URL (Push)
        public void VisitarUrl(string url)
        {
            if (paginaActual != null)
            {
                // Guardamos la página actual en el historial antes de ir a la nueva
                historialPila.Push(paginaActual);
            }
            paginaActual = new PaginaWeb(url);
            Console.WriteLine($"\nNavegando a: {paginaActual.Url}");
        }

        // Método para retroceder en el historial (Pop)
        public void Retroceder()
        {
            if (historialPila.Count > 0)
            {
                // Extraemos la última página visitada del tope de la pila
                paginaActual = historialPila.Pop();
                Console.WriteLine($"\n Ahora estás en: {paginaActual.Url}");
            }
            else
            {
                Console.WriteLine("\n No hay más historial para retroceder.");
            }
        }

        // Reportería: Visualizar y consultar los elementos de la estructura
        public void MostrarHistorial()
        {
            Console.WriteLine("\nREPORTERÍA: HISTORIAL DE NAVEGACIÓN");
            
            if (paginaActual != null)
            {
                Console.WriteLine($"Página Actual (No está en la pila): {paginaActual.Url}");
            }

            if (historialPila.Count == 0)
            {
                Console.WriteLine("El historial está vacía.");
                return;
            }

            Console.WriteLine("\nHistorial de navegación (Del más reciente al más antiguo)");
            // Iteramos sobre la pila sin modificarla
            int index = 1;
            foreach (var pagina in historialPila)
            {
                Console.WriteLine($"{index}. {pagina.ToString()}");
                index++;
            }
            Console.WriteLine("---------------------------");
        }
    }

    // Clase Principal con el Menú
    class Program
    {
        static void Main(string[] args)
        {
            Navegador miNavegador = new Navegador();
            int opcion = 0;

            do
            {
                Console.WriteLine("\nMENÚ DEL NAVEGADOR");
                Console.WriteLine("1. Acceder a una URL");
                Console.WriteLine("2. Botón Retroceder");
                Console.WriteLine("3. Ver Historial de Navegación (Reportería)");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");
                
                string entrada = Console.ReadLine();
                
                if (int.TryParse(entrada, out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            Console.Write("Ingrese la URL (ej. www.uea.gob.ec): ");
                            string url = Console.ReadLine();
                            miNavegador.VisitarUrl(url);
                            break;
                        case 2:
                            miNavegador.Retroceder();
                            break;
                        case 3:
                            miNavegador.MostrarHistorial();
                            break;
                        case 4:
                            Console.WriteLine("Cerrando navegador...");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ingrese un número válido.");
                }

            } while (opcion != 4);
        }
    }
}
