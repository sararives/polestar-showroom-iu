using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using PolestarShowroom.Datos;

namespace PolestarShowroom.Paginas
{
    /// <summary>
    /// Galería con todos los modelos: filtros, búsqueda, orden y ficha de detalle.
    /// </summary>
    public partial class GaleriaPage : Page
    {
        private readonly Coche _cocheInicial;   // Si venimos de Inicio con un coche elegido
        private Coche _cocheAbierto;
        private readonly bool _cargada;

        public GaleriaPage() : this(null) { }

        public GaleriaPage(Coche cocheInicial)
        {
            InitializeComponent();
            _cocheInicial = cocheInicial;
            _cargada = true;
            AplicarFiltros();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_cocheInicial != null && _cocheAbierto == null) AbrirDetalle(_cocheInicial);
        }

        // ---------------------------------------------------------------
        // FILTROS
        // ---------------------------------------------------------------
        private void AplicarFiltros()
        {
            if (!_cargada) return; // Los controles lanzan eventos mientras se crean

            string tipo = TipoSeleccionado();
            bool sinLimite = sldPrecio.Value >= sldPrecio.Maximum;
            decimal precioMax = (decimal)sldPrecio.Value;
            bool soloOfertas = chkOfertas.IsChecked == true;
            string texto = txtBuscar.Text.Trim().ToLowerInvariant();

            IEnumerable<Coche> resultado = Catalogo.Todos.Where(c =>
                (tipo == null || c.Tipo == tipo) &&
                (sinLimite || c.Precio <= precioMax) &&
                (!soloOfertas || c.TieneOferta) &&
                (texto.Length == 0 || c.Nombre.ToLowerInvariant().Contains(texto) || c.Tipo.ToLowerInvariant().Contains(texto)));

            switch (cmbOrden.SelectedIndex)
            {
                case 1: resultado = resultado.OrderByDescending(c => c.Precio); break;
                case 2: resultado = resultado.OrderByDescending(c => c.AutonomiaKm); break;
                case 3: resultado = resultado.OrderByDescending(c => c.PotenciaCV); break;
                default: resultado = resultado.OrderBy(c => c.Precio); break;
            }

            List<Coche> lista = resultado.ToList();
            listaCoches.ItemsSource = lista;
            txtContador.Text = lista.Count == 1 ? "1 modelo" : lista.Count + " modelos";
            panelVacio.Visibility = lista.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

            txtPrecioMax.Text = sinLimite ? "Sin límite" : precioMax.ToString("N0", new CultureInfo("es-ES")) + " €";
            placeholderBuscar.Visibility = txtBuscar.Text.Length == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private string TipoSeleccionado()
        {
            // Todos los RadioButton están en el mismo panel que rbTodos
            var panel = (Panel)rbTodos.Parent;
            foreach (RadioButton rb in panel.Children.OfType<RadioButton>())
            {
                if (rb.IsChecked == true) return rb.Tag as string; // "Todas" no tiene Tag -> null
            }
            return null;
        }

        private void Filtro_Cambiado(object sender, RoutedEventArgs e) { AplicarFiltros(); }
        private void Precio_Cambiado(object sender, RoutedPropertyChangedEventArgs<double> e) { AplicarFiltros(); }
        private void Buscar_Cambiado(object sender, TextChangedEventArgs e) { AplicarFiltros(); }
        private void Orden_Cambiado(object sender, SelectionChangedEventArgs e) { AplicarFiltros(); }

        private void Restablecer_Click(object sender, RoutedEventArgs e)
        {
            rbTodos.IsChecked = true;
            sldPrecio.Value = sldPrecio.Maximum;
            chkOfertas.IsChecked = false;
            txtBuscar.Text = "";
            cmbOrden.SelectedIndex = 0;
            AplicarFiltros();
        }

        // ---------------------------------------------------------------
        // FICHA DE DETALLE
        // ---------------------------------------------------------------
        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            var boton = e.OriginalSource as Button;
            var coche = boton == null ? null : boton.DataContext as Coche;
            if (coche != null) AbrirDetalle(coche);
        }

        private void AbrirDetalle(Coche coche)
        {
            _cocheAbierto = coche;
            panelDetalle.DataContext = coche;
            capaDetalle.Visibility = Visibility.Visible;
            ((Storyboard)this.Resources["SbAbrirDetalle"]).Begin(this);
            btnPrueba.Focus();
        }

        private void CerrarDetalle()
        {
            if (capaDetalle.Visibility != Visibility.Visible) return;
            _cocheAbierto = null;
            ((Storyboard)this.Resources["SbCerrarDetalle"]).Begin(this);
        }

        private void SbCerrarDetalle_Completed(object sender, EventArgs e)
        {
            if (_cocheAbierto == null) capaDetalle.Visibility = Visibility.Collapsed;
        }

        private void CerrarDetalle_Click(object sender, RoutedEventArgs e) { CerrarDetalle(); }

        private void FondoDetalle_Click(object sender, MouseButtonEventArgs e) { CerrarDetalle(); }

        // La tecla Esc cierra la ficha (atajo estándar)
        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && capaDetalle.Visibility == Visibility.Visible)
            {
                CerrarDetalle();
                e.Handled = true;
            }
        }

        private void SolicitarPrueba_Click(object sender, RoutedEventArgs e)
        {
            Coche coche = _cocheAbierto;
            if (coche == null) return;

            if (!Sesion.Iniciada)
            {
                // Se recuerda el coche para volver aquí después de iniciar sesión
                Sesion.CochePendiente = coche;
                Navegacion.IrA(Seccion.Login);
                return;
            }

            CerrarDetalle();
            Navegacion.Aviso("Solicitud enviada. Tu tienda te llamará para la prueba del " + coche.Nombre + ".");
        }
    }
}
