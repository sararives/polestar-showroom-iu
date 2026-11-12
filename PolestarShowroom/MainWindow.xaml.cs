using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using PolestarShowroom.Datos;
using PolestarShowroom.Paginas;

namespace PolestarShowroom
{
    /// <summary>
    /// Ventana principal. Contiene los elementos comunes a todas las pantallas
    /// (logotipo, menú de navegación y pie) y un Frame donde se cargan las páginas.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            marco.Navigated += Marco_Navigated;
            IrA(Seccion.Inicio, null);
        }

        /// <summary>Carga la página pedida en el Frame y marca la opción activa del menú.</summary>
        public void IrA(Seccion seccion, Coche cocheSeleccionado)
        {
            Page pagina;
            switch (seccion)
            {
                case Seccion.Galeria:
                    pagina = new GaleriaPage(cocheSeleccionado);
                    break;
                case Seccion.Login:
                    pagina = new LoginPage();
                    break;
                default:
                    pagina = new InicioPage();
                    break;
            }
            marco.Navigate(pagina);

            // Indicador de sección actual en el menú
            navInicio.Tag = seccion == Seccion.Inicio ? "activo" : null;
            navGaleria.Tag = seccion == Seccion.Galeria ? "activo" : null;

            // En la propia pantalla de login no tiene sentido el botón "Iniciar sesión"
            navLogin.Visibility = (seccion == Seccion.Login && !Sesion.Iniciada) ? Visibility.Hidden : Visibility.Visible;
            ActualizarSesion();
        }

        /// <summary>Muestra el saludo y cambia "Iniciar sesión" por "Cerrar sesión".</summary>
        public void ActualizarSesion()
        {
            if (Sesion.Iniciada)
            {
                txtSaludo.Text = "Hola, " + Sesion.NombreVisible;
                txtSaludo.Visibility = Visibility.Visible;
                navLogin.Content = "Cerrar sesión";
                navLogin.ToolTip = "Salir de tu cuenta";
            }
            else
            {
                txtSaludo.Visibility = Visibility.Collapsed;
                navLogin.Content = "Iniciar sesión";
                navLogin.ToolTip = "Accede a tu área de cliente";
            }
        }

        /// <summary>Muestra un aviso breve en la parte superior (feedback al usuario).</summary>
        public void MostrarAviso(string texto)
        {
            txtAviso.Text = texto;
            Storyboard sb = (Storyboard)this.Resources["SbAviso"];
            sb.Begin(this);
        }

        // Evita que se acumule el historial del Frame (la navegación se hace con el menú)
        private void Marco_Navigated(object sender, NavigationEventArgs e)
        {
            while (marco.CanGoBack) marco.RemoveBackEntry();
        }

        private void Logo_Click(object sender, RoutedEventArgs e)
        {
            IrA(Seccion.Inicio, null);
        }

        private void NavInicio_Click(object sender, RoutedEventArgs e)
        {
            IrA(Seccion.Inicio, null);
        }

        private void NavGaleria_Click(object sender, RoutedEventArgs e)
        {
            IrA(Seccion.Galeria, null);
        }

        private void NavLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Sesion.Iniciada)
            {
                Sesion.Cerrar();
                IrA(Seccion.Inicio, null);
                MostrarAviso("Has cerrado sesión correctamente.");
            }
            else
            {
                IrA(Seccion.Login, null);
            }
        }
    }
}
