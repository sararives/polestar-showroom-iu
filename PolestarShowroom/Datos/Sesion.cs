using System;
using System.Collections.Generic;

namespace PolestarShowroom.Datos
{
    /// <summary>
    /// Gestión muy simple de la sesión del cliente (prototipo: los usuarios
    /// están escritos en el código; en una versión real irían en un servidor
    /// y las contraseñas cifradas).
    /// </summary>
    public static class Sesion
    {
        // Usuarios de prueba: correo -> contraseña
        private static readonly Dictionary<string, string> Usuarios =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "cliente@polestar.es", "Polestar26" },
                { "alumno@umh.es", "1234" }
            };

        public static string Usuario { get; private set; }

        /// <summary>Último correo con "Recordarme" marcado (solo en memoria).</summary>
        public static string UsuarioRecordado { get; set; }

        /// <summary>Coche que el cliente quería probar antes de iniciar sesión.</summary>
        public static Coche CochePendiente { get; set; }

        public static bool Iniciada { get { return Usuario != null; } }

        /// <summary>Parte del correo antes de la @ con la primera letra en mayúscula.</summary>
        public static string NombreVisible
        {
            get
            {
                if (Usuario == null) return "";
                string nombre = Usuario.Split('@')[0];
                if (nombre.Length == 0) return Usuario;
                return char.ToUpper(nombre[0]) + nombre.Substring(1);
            }
        }

        public static bool Validar(string correo, string password)
        {
            string guardada;
            return Usuarios.TryGetValue(correo, out guardada) && guardada == password;
        }

        public static void Iniciar(string correo)
        {
            Usuario = correo.ToLowerInvariant();
        }

        public static void Cerrar()
        {
            Usuario = null;
            CochePendiente = null;
        }
    }
}
