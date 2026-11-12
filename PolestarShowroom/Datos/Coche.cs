using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PolestarShowroom.Datos
{
    /// <summary>
    /// Un modelo de coche del catálogo. Las propiedades "...Texto" y "...Visibilidad"
    /// existen para poder enlazarlas (Binding) directamente desde el XAML.
    /// </summary>
    public class Coche
    {
        private static readonly CultureInfo Es = new CultureInfo("es-ES");

        public string Nombre { get; set; }
        public string Tipo { get; set; }            // Berlina, SUV, Coupé, Gran Turismo
        public decimal Precio { get; set; }         // Precio actual (con oferta si la hay)
        public decimal PrecioAnterior { get; set; } // 0 si no hay oferta
        public int AutonomiaKm { get; set; }
        public int PotenciaCV { get; set; }
        public double Aceleracion { get; set; }     // 0-100 km/h en segundos
        public string Traccion { get; set; }
        public string Descripcion { get; set; }
        public bool EsNovedad { get; set; }
        public string Etiqueta { get; set; }        // Texto de la etiqueta de la tarjeta ("Novedad", "-10 %"...)
        public Color ColorBase { get; set; }        // Color del degradado si no hay foto
        public string ArchivoImagen { get; set; }   // Nombre del archivo dentro de la carpeta Imagenes

        public bool TieneOferta { get { return PrecioAnterior > Precio; } }

        public string PrecioTexto { get { return Precio.ToString("N0", Es) + " €"; } }
        public string PrecioAnteriorTexto { get { return TieneOferta ? PrecioAnterior.ToString("N0", Es) + " €" : ""; } }
        public string Resumen { get { return AutonomiaKm + " km · " + PotenciaCV + " CV · 0-100 en " + Aceleracion.ToString("0.0", Es) + " s"; } }
        public string AutonomiaTexto { get { return AutonomiaKm + " km"; } }
        public string PotenciaTexto { get { return PotenciaCV + " CV"; } }
        public string AceleracionTexto { get { return Aceleracion.ToString("0.0", Es) + " s"; } }

        public Visibility DescuentoVisibilidad { get { return TieneOferta ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility EtiquetaVisibilidad { get { return string.IsNullOrEmpty(Etiqueta) ? Visibility.Collapsed : Visibility.Visible; } }

        /// <summary>Degradado que se ve detrás de la silueta cuando no hay foto.</summary>
        public Brush FondoTarjeta
        {
            get
            {
                Color oscuro = Color.FromRgb((byte)(ColorBase.R * 0.45), (byte)(ColorBase.G * 0.45), (byte)(ColorBase.B * 0.45));
                var degradado = new LinearGradientBrush(ColorBase, oscuro, new Point(0, 0), new Point(1, 1));
                degradado.Freeze();
                return degradado;
            }
        }

        private ImageSource _imagen;
        private bool _imagenBuscada;

        /// <summary>
        /// Foto del coche. Busca Imagenes\ArchivoImagen junto al .exe.
        /// Si no existe devuelve null y se ve la silueta vectorial.
        /// </summary>
        public ImageSource Imagen
        {
            get
            {
                if (!_imagenBuscada)
                {
                    _imagenBuscada = true;
                    _imagen = CargarImagen(ArchivoImagen);
                }
                return _imagen;
            }
        }

        public static ImageSource CargarImagen(string archivo, int anchoMaximo = 600)
        {
            if (string.IsNullOrEmpty(archivo)) return null;
            try
            {
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", archivo);
                if (!File.Exists(ruta)) return null;

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(ruta, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.DecodePixelWidth = anchoMaximo; // las tarjetas se cargan pequeñas; las campañas, grandes
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
            catch
            {
                return null; // Si la imagen está dañada, se usa la silueta
            }
        }
    }
}
