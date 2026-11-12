using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PolestarShowroom.Datos
{
    /// <summary>
    /// Catálogo de modelos del prototipo.
    /// Precios ORIENTATIVOS tomados de prensa del motor española (noviembre 2026).
    /// En una versión real vendrían de una base de datos o servicio web.
    /// </summary>
    public static class Catalogo
    {
        private static List<Coche> _coches;

        public static List<Coche> Todos
        {
            get
            {
                if (_coches == null) _coches = Crear();
                return _coches;
            }
        }

        /// <summary>Modelos marcados como novedad.</summary>
        public static List<Coche> Novedades()
        {
            return Todos.Where(c => c.EsNovedad).Take(3).ToList();
        }

        /// <summary>Los 3 modelos más baratos.</summary>
        public static List<Coche> MejorPrecio()
        {
            return Todos.OrderBy(c => c.Precio).Take(3).ToList();
        }

        public static Coche Buscar(string nombre)
        {
            return Todos.FirstOrDefault(c => c.Nombre == nombre);
        }

        private static List<Coche> Crear()
        {
            return new List<Coche>
            {
                new Coche
                {
                    Nombre = "Polestar 2", Tipo = "Berlina", Precio = 42900, PrecioAnterior = 48200,
                    AutonomiaKm = 655, PotenciaCV = 299, Aceleracion = 6.2, Traccion = "Trasera",
                    Etiqueta = "Black Friday",
                    Descripcion = "La berlina fastback con la que empezó todo. Interior vegano, Google integrado y la mejor relación precio/autonomía de la gama.",
                    ColorBase = Color.FromRgb(0x4B, 0x6C, 0x8C), ArchivoImagen = "polestar2.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 2 Dual Motor", Tipo = "Berlina", Precio = 58650,
                    AutonomiaKm = 590, PotenciaCV = 421, Aceleracion = 4.5, Traccion = "Total",
                    Descripcion = "Dos motores y tracción total para quien busca más prestaciones sin renunciar al uso diario.",
                    ColorBase = Color.FromRgb(0x5A, 0x5F, 0x68), ArchivoImagen = "polestar2-dual.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 3", Tipo = "SUV", Precio = 81900,
                    AutonomiaKm = 635, PotenciaCV = 489, Aceleracion = 5.0, Traccion = "Total",
                    Descripcion = "SUV de gran tamaño con suspensión neumática y la arquitectura de 800 V para cargas más rápidas.",
                    ColorBase = Color.FromRgb(0x2F, 0x4F, 0x4F), ArchivoImagen = "polestar3.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 4 SUV", Tipo = "SUV", Precio = 55600, EsNovedad = true,
                    AutonomiaKm = 630, PotenciaCV = 272, Aceleracion = 7.1, Traccion = "Trasera",
                    Etiqueta = "Novedad",
                    Descripcion = "La versión más práctica del Polestar 4: más altura, 530 litros de maletero y el mismo diseño escandinavo.",
                    ColorBase = Color.FromRgb(0x6B, 0x5B, 0x4B), ArchivoImagen = "polestar4-suv.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 4 Coupé", Tipo = "Coupé", Precio = 65900,
                    AutonomiaKm = 620, PotenciaCV = 272, Aceleracion = 7.1, Traccion = "Trasera",
                    Descripcion = "SUV coupé sin luneta trasera: la cámara sustituye al retrovisor interior y el techo de cristal recorre toda la cabina.",
                    ColorBase = Color.FromRgb(0x3A, 0x3D, 0x5C), ArchivoImagen = "polestar4-coupe.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 4 Coupé Dual Motor", Tipo = "Coupé", Precio = 72900,
                    AutonomiaKm = 590, PotenciaCV = 544, Aceleracion = 3.8, Traccion = "Total",
                    Descripcion = "La variante más rápida del Polestar 4, con dos motores y tracción total.",
                    ColorBase = Color.FromRgb(0x24, 0x2A, 0x33), ArchivoImagen = "polestar4-dual.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 5", Tipo = "Gran Turismo", Precio = 121899, EsNovedad = true,
                    AutonomiaKm = 670, PotenciaCV = 749, Aceleracion = 3.8, Traccion = "Total",
                    Etiqueta = "Novedad",
                    Descripcion = "El gran turismo de cuatro plazas y buque insignia de la marca, con chasis de aluminio y 800 V.",
                    ColorBase = Color.FromRgb(0x8C, 0x6A, 0x3E), ArchivoImagen = "polestar5.jpg"
                },
                new Coche
                {
                    Nombre = "Polestar 5 Performance", Tipo = "Gran Turismo", Precio = 145299, EsNovedad = true,
                    AutonomiaKm = 650, PotenciaCV = 885, Aceleracion = 3.2, Traccion = "Total",
                    Etiqueta = "Novedad",
                    Descripcion = "La versión más potente jamás fabricada por Polestar, con 885 CV y frenos de alto rendimiento.",
                    ColorBase = Color.FromRgb(0x7A, 0x2E, 0x2E), ArchivoImagen = "polestar5-performance.jpg"
                }
            };
        }
    }
}
