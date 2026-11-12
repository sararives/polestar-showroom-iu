using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using PolestarShowroom.Datos;

namespace PolestarShowroom.Paginas
{
    /// <summary>
    /// Pantalla de identificación con gestión de errores:
    ///  - campos vacíos y formato de correo (errores junto a cada campo)
    ///  - credenciales incorrectas (mensaje general que no revela qué dato falla)
    ///  - máximo 3 intentos y bloqueo temporal de 30 segundos
    ///  - aviso de Bloq Mayús y opción de mostrar la contraseña
    /// </summary>
    public partial class LoginPage : Page
    {
        private const int MaxIntentos = 3;
        private const int SegundosBloqueo = 30;

        // Se guardan fuera de la página para que el bloqueo no se salte cambiando de pantalla
        private static int _intentosFallidos = 0;
        private static DateTime _bloqueadoHasta = DateTime.MinValue;

        private readonly DispatcherTimer _relojBloqueo = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        private bool _passwordVisible = false;

        public LoginPage()
        {
            InitializeComponent();
            _relojBloqueo.Tick += RelojBloqueo_Tick;
            imgLateral.Source = Coche.CargarImagen("login.jpg");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Sesion.UsuarioRecordado))
            {
                txtUsuario.Text = Sesion.UsuarioRecordado;
                chkRecordar.IsChecked = true;
                txtPassword.Focus();
            }
            else
            {
                txtUsuario.Focus();
            }

            if (Sesion.CochePendiente != null)
                Navegacion.Aviso("Inicia sesión para solicitar la prueba del " + Sesion.CochePendiente.Nombre + ".");

            if (EstaBloqueado()) EmpezarBloqueo();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _relojBloqueo.Stop();
        }

        // ---------------------------------------------------------------
        // ENTRAR
        // ---------------------------------------------------------------
        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            if (EstaBloqueado()) return;

            LimpiarErrores();
            string correo = txtUsuario.Text.Trim();
            string password = LeerPassword();

            // 1) Validación de los campos (antes de comprobar credenciales)
            bool correcto = true;
            if (correo.Length == 0)
            {
                MarcarError(txtUsuario, errUsuario, "Escribe tu correo electrónico.");
                correcto = false;
            }
            else if (!FormatoCorreoValido(correo))
            {
                MarcarError(txtUsuario, errUsuario, "El correo no parece válido. Ejemplo: nombre@correo.es");
                correcto = false;
            }

            if (password.Length == 0)
            {
                MarcarError(CampoPasswordActivo(), errPassword, "Escribe tu contraseña.");
                correcto = false;
            }

            if (!correcto)
            {
                Sacudir();
                if (errUsuario.Visibility == Visibility.Visible) txtUsuario.Focus();
                else CampoPasswordActivo().Focus();
                return;
            }

            // 2) Comprobación de credenciales
            if (!Sesion.Validar(correo, password))
            {
                _intentosFallidos++;
                int restantes = MaxIntentos - _intentosFallidos;

                if (restantes <= 0)
                {
                    _bloqueadoHasta = DateTime.Now.AddSeconds(SegundosBloqueo);
                    EmpezarBloqueo();
                }
                else
                {
                    MostrarErrorGeneral("El correo o la contraseña no son correctos. " +
                        (restantes == 1 ? "Te queda 1 intento." : "Te quedan " + restantes + " intentos."), true);
                }

                BorrarPassword();
                CampoPasswordActivo().Focus();
                return;
            }

            // 3) Acceso correcto
            _intentosFallidos = 0;
            Sesion.UsuarioRecordado = chkRecordar.IsChecked == true ? correo : null;
            Sesion.Iniciar(correo);

            Coche pendiente = Sesion.CochePendiente;
            Sesion.CochePendiente = null;

            if (pendiente != null)
                Navegacion.IrA(Seccion.Galeria, pendiente);   // vuelve a donde estaba
            else
                Navegacion.IrA(Seccion.Inicio);

            Navegacion.Aviso("¡Hola, " + Sesion.NombreVisible + "! Has iniciado sesión.");
        }

        private static bool FormatoCorreoValido(string correo)
        {
            int arroba = correo.IndexOf('@');
            int punto = correo.LastIndexOf('.');
            return arroba > 0 && punto > arroba + 1 && punto < correo.Length - 1 && !correo.Contains(" ");
        }

        // ---------------------------------------------------------------
        // BLOQUEO TRAS 3 INTENTOS
        // ---------------------------------------------------------------
        private static bool EstaBloqueado()
        {
            return DateTime.Now < _bloqueadoHasta;
        }

        private void EmpezarBloqueo()
        {
            btnEntrar.IsEnabled = false;
            ActualizarMensajeBloqueo();
            _relojBloqueo.Start();
        }

        private void RelojBloqueo_Tick(object sender, EventArgs e)
        {
            if (EstaBloqueado())
            {
                ActualizarMensajeBloqueo();
                return;
            }

            _relojBloqueo.Stop();
            _intentosFallidos = 0;
            btnEntrar.IsEnabled = true;
            avisoError.Visibility = Visibility.Collapsed;
            CampoPasswordActivo().Focus();
        }

        private void ActualizarMensajeBloqueo()
        {
            int segundos = (int)Math.Ceiling((_bloqueadoHasta - DateTime.Now).TotalSeconds);
            txtAvisoError.Text = "Has superado el número de intentos. Por seguridad, espera " + segundos +
                                 " s antes de volver a intentarlo.";
            avisoError.Visibility = Visibility.Visible;
        }

        // ---------------------------------------------------------------
        // MOSTRAR / OCULTAR CONTRASEÑA y BLOQ MAYÚS
        // ---------------------------------------------------------------
        private void VerPassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;
            if (_passwordVisible)
            {
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                btnVerPassword.Content = "Ocultar";
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
                btnVerPassword.Content = "Mostrar";
            }
            // Copiamos también el estado de error al campo que queda visible
            txtPasswordVisible.Tag = txtPassword.Tag;
            CampoPasswordActivo().Focus();
        }

        private string LeerPassword()
        {
            return _passwordVisible ? txtPasswordVisible.Text : txtPassword.Password;
        }

        private void BorrarPassword()
        {
            txtPassword.Password = "";
            txtPasswordVisible.Text = "";
        }

        private Control CampoPasswordActivo()
        {
            return _passwordVisible ? (Control)txtPasswordVisible : txtPassword;
        }

        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            ComprobarMayusculas();
        }

        private void Password_Foco(object sender, KeyboardFocusChangedEventArgs e)
        {
            ComprobarMayusculas();
        }

        private void ComprobarMayusculas()
        {
            avisoMayus.Visibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Collapsed;
        }

        // ---------------------------------------------------------------
        // ERRORES
        // ---------------------------------------------------------------
        private void MarcarError(Control campo, TextBlock mensaje, string texto)
        {
            campo.Tag = "error";
            mensaje.Text = texto;
            mensaje.Visibility = Visibility.Visible;
        }

        private void MostrarErrorGeneral(string texto, bool animar)
        {
            txtAvisoError.Text = texto;
            avisoError.Visibility = Visibility.Visible;
            if (animar) Sacudir();
        }

        private void LimpiarErrores()
        {
            txtUsuario.Tag = null;
            txtPassword.Tag = null;
            txtPasswordVisible.Tag = null;
            errUsuario.Visibility = Visibility.Collapsed;
            errPassword.Visibility = Visibility.Collapsed;
            avisoError.Visibility = Visibility.Collapsed;
        }

        private void Sacudir()
        {
            Storyboard sb = (Storyboard)this.Resources["SbError"];
            sb.Begin(this);
        }

        // Al escribir de nuevo en un campo con error, el error desaparece (feedback inmediato)
        private void Campo_Cambiado(object sender, TextChangedEventArgs e)
        {
            QuitarErrorDe(sender as Control);
        }

        private void Password_Cambiada(object sender, RoutedEventArgs e)
        {
            QuitarErrorDe(txtPassword);
        }

        private void QuitarErrorDe(Control campo)
        {
            if (campo == null || !"error".Equals(campo.Tag)) return;
            campo.Tag = null;
            if (campo == txtUsuario) errUsuario.Visibility = Visibility.Collapsed;
            else errPassword.Visibility = Visibility.Collapsed;
        }

        private void Olvido_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Te enviaremos un enlace para crear una contraseña nueva al correo con el que te registraste.\n\n" +
                "(En este prototipo la recuperación de contraseña no está implementada.)",
                "Recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
