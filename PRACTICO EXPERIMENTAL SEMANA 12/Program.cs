using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace BibliotecaApp
{
    class Program
    {
        // Uso un diccionario para buscar libros rápido usando su ISBN (que es único)
        // En C#, Dictionary es la forma de implementar Mapas.
        static Dictionary<string, Libro> catalogoLibros = new Dictionary<string, Libro>();
        
        // Uso un HashSet para tener las categorías sin repetidos
        static HashSet<string> categoriasDisponibles = new HashSet<string>();

        const string ArchivoJson = "libros.json";

        static void Main(string[] args)
        {
            // Cargo los datos al iniciar el programa para recuperar lo guardado antes
            CargarDatos();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n--- SISTEMA DE BIBLIOTECA ---");
                Console.WriteLine("1. Registrar nuevo libro");
                Console.WriteLine("2. Mostrar todos los libros");
                Console.WriteLine("3. Mostrar categorías disponibles");
                Console.WriteLine("4. Buscar libro por Código de Barras");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarLibroInteractuando();
                        break;
                    case "2":
                        MostrarTodosLosLibros();
                        break;
                    case "3":
                        MostrarCategorias();
                        break;
                    case "4":
                        BuscarLibroInteractivo();
                        break;
                    case "5":
                        salir = true;
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
        }

        static void CargarDatos()
        {
            // Verifico si el archivo ya existe para leerlo, si no, empezamos con catálogo vacío
            if (File.Exists(ArchivoJson))
            {
                try
                {
                    string jsonString = File.ReadAllText(ArchivoJson);
                    // Uso System.Text.Json para convertir el texto JSON a una lista de libros
                    var libros = JsonSerializer.Deserialize<List<Libro>>(jsonString);
                    
                    if (libros != null)
                    {
                        foreach (var libro in libros)
                        {
                            // Vuelvo a llenar mi Diccionario y mi Conjunto con los datos del JSON
                            catalogoLibros[libro.CodigoBarras] = libro;
                            categoriasDisponibles.Add(libro.Categoria);
                        }
                    }
                    Console.WriteLine("Datos cargados correctamente desde libros.json.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hubo un error al cargar los datos: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No se encontró archivo previo de libros. Se iniciará un catálogo nuevo.");
            }
        }

        static void GuardarDatos()
        {
            try
            {
                // Convierto los valores del diccionario (los libros) a una lista para guardarlos más fácil en JSON
                var listaLibros = catalogoLibros.Values.ToList();
                var opciones = new JsonSerializerOptions { WriteIndented = true }; // Para que el JSON se vea bonito y legible
                string jsonString = JsonSerializer.Serialize(listaLibros, opciones);
                
                File.WriteAllText(ArchivoJson, jsonString);
                // No imprimo mensaje aquí para no saturar la consola cada vez que agregan un libro
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hubo un error al guardar los datos: {ex.Message}");
            }
        }

        static void RegistrarLibroInteractuando()
        {
            Console.WriteLine("\n--- REGISTRAR LIBRO ---");
            Console.Write("Ingrese Codigo de Barras: ");
            string codigo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("El Codigo de Barras no puede estar vacío.");
                return;
            }

            if (catalogoLibros.ContainsKey(codigo))
            {
                Console.WriteLine($"Error: El Código de Barras {codigo} ya está registrado.");
                return;
            }

            Console.Write("Ingrese Título: ");
            string titulo = Console.ReadLine();
            
            Console.Write("Ingrese Autor: ");
            string autor = Console.ReadLine();
            
            Console.Write("Ingrese Categoría: ");
            string categoria = Console.ReadLine();

            Libro nuevoLibro = new Libro(codigo, titulo, autor, categoria);
            
            // Guardo en mis estructuras de datos
            catalogoLibros.Add(codigo, nuevoLibro);
            categoriasDisponibles.Add(categoria);
            
            Console.WriteLine($"Libro '{titulo}' agregado al catálogo.");
            
            // Guardo los cambios en el archivo JSON
            GuardarDatos();
        }

        static void MostrarTodosLosLibros()
        {
            Console.WriteLine("\n--- LISTA DE LIBROS ---");
            if (catalogoLibros.Count == 0)
            {
                Console.WriteLine("No hay libros registrados aún.");
                return;
            }

            foreach (KeyValuePair<string, Libro> par in catalogoLibros)
            {
                Console.WriteLine($"- {par.Value.Titulo} (Código de Barras: {par.Key}) - Autor: {par.Value.Autor} - Categoría: {par.Value.Categoria}");
            }
        }

        static void MostrarCategorias()
        {
            Console.WriteLine("\n--- CATEGORÍAS DISPONIBLES ---");
            if (categoriasDisponibles.Count == 0)
            {
                Console.WriteLine("No hay categorías registradas.");
                return;
            }

            // Recorro el conjunto de categorías para listarlas
            foreach (string categoria in categoriasDisponibles)
            {
                Console.WriteLine($"- {categoria}");
            }
        }

        static void BuscarLibroInteractivo()
        {
            Console.WriteLine("\n--- BÚSQUEDA DE LIBRO ---");
            Console.Write("Ingrese el Código de Barras a buscar: ");
            string codigo = Console.ReadLine();

            // TryGetValue sigue siendo la forma más rápida y segura (O(1)) para buscar en el diccionario (Mapa)
            if (catalogoLibros.TryGetValue(codigo, out Libro libroEncontrado))
            {
                Console.WriteLine($"\nLibro Encontrado:");
                Console.WriteLine($"Título: {libroEncontrado.Titulo}");
                Console.WriteLine($"Autor: {libroEncontrado.Autor}");
                Console.WriteLine($"Categoría: {libroEncontrado.Categoria}");
            }
            else
            {
                Console.WriteLine($"No se encontró ningún libro con el Código de Barras: {codigo}");
            }
        }
    }

    class Libro
    {
        public string CodigoBarras { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }

        // Constructor vacío necesario para que JsonSerializer pueda reconstruir el objeto al leer el archivo
        public Libro() { }

        public Libro(string codigoBarras, string titulo, string autor, string categoria)
        {
            CodigoBarras = codigoBarras;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }
    }
}
