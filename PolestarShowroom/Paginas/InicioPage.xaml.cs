using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using PolestarShowroom.Datos;

namespace PolestarShowroom.Paginas
{
    /// <summary>
    /// Pantalla de inicio: campañas, novedades, mejor precio, oferta de temporada
    /// y una calculadora de ahorro.
    /// </summary>
    public partial class InicioPage : Page
    {
        // Carrusel de campañas
        private readonly Grid[] _campanas;
        private readonly Border[] _marcas;
        private int _actual = 0;
        private bool _pausado = false;
        private readonly DispatcherTimer _temporizador = new DispatcherTimer();

        // Supuestos de la calculadora de ahorro (por cada 100 km)
        private const double CosteGasolina100 = 6.5 * 1.55;   // litros x €/litro
        private const double CosteElectrico100 = 17 * 0.20;   // kWh x €/kWh

        private readonly bool _cargada;

        public InicioPage()
        {
            InitializeComponent();

            _campanas = new[] { campana0, campana1, campana2 };
            _marcas = new[] { marcaPunto0, marcaPunto1, marcaPunto2 };

            // Fotos opcionales de las campañas (Imagenes\campana1.jpg, campana2.jpg, campana3.jpg)
            imgCampana0.Source = Coche.CargarImagen("campana1.jpg");
            imgCampana1.Source = Coche.CargarImagen("campana2.jpg");
            imgCampana2.Source = Coche.CargarImagen("campana3.jpg");

            listaNovedades.ItemsSource = Catalogo.Novedades();
            listaMejorPrecio.ItemsSource = Catalogo.MejorPrecio();

            // Cambia de campaña cada 7 segundos (tiempo suficiente para leerla)
            _temporizador.Interval = TimeSpan.FromSeconds(7);
            _temporizador.Tick += (s, e) => MostrarCampana((_actual + 1) % _campanas.Length);
            _temporizador.Start();

            _cargada = true;
            CalcularAhorro();
        }

        // ---------------------------------------------------------------
        // CARRUSEL
        // ---------------------------------------------------------------
        private void MostrarCampana(int indice)
        {
            if (indice == _actual) return;

            Grid saliente = _campanas[_actual];
            Grid entrante = _campanas[indice];

            // Fundido de salida y, al terminar, se oculta (así no recibe el foco con Tab)
            var salida = new DoubleAnimation(0, TimeSpan.FromMilliseconds(500));
            salida.Completed += (s, e) =>
            {
                if (saliente != _campanas[_actual]) saliente.Visibility = Visibility.Hidden;
            };
            saliente.BeginAnimation(OpacityProperty, salida);

            // Fundido de entrada
            entrante.Visibility = Visibility.Visible;
            entrante.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500)));

            _actual = indice;
            ActualizarIndicadores();
        }

        private void ActualizarIndicadores()
        {
            for (int i = 0; i < _marcas.Length; i++)
            {
                bool activa = i == _actual;
                _marcas[i].Width = activa ? 26 : 8;
                _marcas[i].Background = activa
                    ? (Brush)FindResource("AcentoBrush")
                    : new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));
            }
        }

        private void ReiniciarTemporizador()
        {
            _temporizador.Stop();
            if (!_pausado) _temporizador.Start();
        }

        private void Punto_Click(object sender, RoutedEventArgs e)
        {
            int indice = int.Parse(((Button)sender).Tag.ToString());
            MostrarCampana(indice);
            ReiniciarTemporizador();
        }

        private void Pausa_Click(object sender, RoutedEventArgs e)
        {
            _pausado = !_pausado;
            iconoPausa.Text = _pausado ? "" : ""; // Play / Pausa
            btnPausa.ToolTip = _pausado ? "Reanudar el cambio automático" : "Pausar el cambio automático";
            AutomationProperties_SetName(btnPausa, _pausado ? "Reanudar carrusel" : "Pausar carrusel");
            ReiniciarTemporizador();
        }

        private static void AutomationProperties_SetName(DependencyObject elemento, string nombre)
        {
            System.Windows.Automation.AutomationProperties.SetName(elemento, nombre);
        }

        // Mientras el ratón está encima de la campaña no cambia (da tiempo a leer y pulsar)
        private void Hero_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _temporizador.Stop();
        }

        private void Hero_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_pausado) _temporizador.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _temporizador.Stop();
        }

        // ---------------------------------------------------------------
        // NAVEGACIÓN
        // ---------------------------------------------------------------
        private void Campana_Click(object sender, RoutedEventArgs e)
        {
            string nombre = ((Button)sender).Tag as string;
            Navegacion.IrA(Seccion.Galeria, Catalogo.Buscar(nombre));
        }

        private void VerModelos_Click(object sender, RoutedEventArgs e)
        {
            Navegacion.IrA(Seccion.Galeria);
        }

        // Clic en cualquier tarjeta (evento enrutado desde el ItemsControl)
        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            var boton = e.OriginalSource as Button;
            var coche = boton == null ? null : boton.DataContext as Coche;
            if (coche != null) Navegacion.IrA(Seccion.Galeria, coche);
        }

        // ---------------------------------------------------------------
        // CALCULADORA DE AHORRO
        // ---------------------------------------------------------------
        private void SldKm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_cargada) CalcularAhorro();
        }

        private void CalcularAhorro()
        {
            var es = new CultureInfo("es-ES");
            double km = sldKm.Value;
            double ahorro = km / 100.0 * (CosteGasolina100 - CosteElectrico100);
            txtKm.Text = km.ToString("N0", es) + " km";
            txtAhorro.Text = ahorro.ToString("N0", es) + " €";
        }
    }
}
