using System.Windows;

namespace PolestarShowroom.Datos
{
    public enum Seccion { Inicio, Galeria, Login }

    /// <summary>
    /// Atajos para que cualquier página pueda pedir a la ventana principal
    /// que cambie de sección o muestre un aviso.
    /// </summary>
    public static class Navegacion
    {
        private static MainWindow Ventana
        {
            get { return Application.Current == null ? null : Application.Current.MainWindow as MainWindow; }
        }

        public static void IrA(Seccion seccion)
        {
            IrA(seccion, null);
        }

        public static void IrA(Seccion seccion, Coche cocheSeleccionado)
        {
            MainWindow v = Ventana;
            if (v != null) v.IrA(seccion, cocheSeleccionado);
        }

        public static void Aviso(string texto)
        {
            MainWindow v = Ventana;
            if (v != null) v.MostrarAviso(texto);
        }

        public static void RefrescarCabecera()
        {
            MainWindow v = Ventana;
            if (v != null) v.ActualizarSesion();
        }
    }
}
