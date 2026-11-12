# Polestar Showroom España — Prototipo WPF

Práctica 1 de **Interfaces de Usuario** (Grado en Ingeniería Informática en Tecnologías de la Información, UMH).
Prototipo de aplicación de escritorio para las tiendas que venden vehículos Polestar en España, hecho con **Microsoft Blend / Visual Studio (WPF, .NET Framework 4.8)**.

## Cómo ejecutarlo
1. Abrir `PolestarShowroom.sln` con Visual Studio o Blend.
2. Pulsar **Iniciar** (F5).

Usuario de prueba: `cliente@polestar.es` · contraseña `Polestar26`

## Pantallas
| Pantalla | Qué contiene |
|---|---|
| **Inicio** | Carrusel de campañas (pausable), novedades, mejor precio, oferta de temporada y calculadora de ahorro |
| **Iniciar sesión** | Validación por campo, error de credenciales con intentos restantes, bloqueo de 30 s tras 3 fallos, aviso de Bloq Mayús, mostrar contraseña |
| **Modelos (galería)** | Todos los modelos con filtros (carrocería, precio, ofertas), búsqueda, orden y ficha de detalle |

Elementos comunes a todas las pantallas: **logotipo**, **menú de navegación** y **pie de página**.

## Estructura
```
PolestarShowroom/
├─ App.xaml                 Carga el diccionario de estilos
├─ MainWindow.xaml          Cabecera (logo + menú), Frame de contenido, aviso flotante y pie
├─ Estilos/Estilos.xaml     Colores, botones, campos, tarjeta de coche y siluetas vectoriales
├─ Paginas/                 InicioPage, LoginPage, GaleriaPage
├─ Datos/                   Coche, Catalogo, Sesion, Navegacion
└─ Imagenes/                Fotos opcionales (ver LEEME.txt)
```

## Uso de inteligencia artificial
Este proyecto se ha desarrollado con ayuda de **Claude (Anthropic)**. El detalle de prompts, resultados y modificaciones propias está en la documentación entregada y en el enlace a la conversación indicado en ella.

> Datos y precios orientativos (prensa del motor española, noviembre 2026). Proyecto académico sin relación oficial con Polestar.
