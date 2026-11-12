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

## Créditos de las imágenes
Fotos recortadas y redimensionadas para el prototipo.

| Archivo | Autor | Fuente | Licencia |
|---|---|---|---|
| polestar2.jpg | Benjamin Brunner | Unsplash | [Licencia Unsplash](https://unsplash.com/license) |
| polestar2-dual.jpg, campana2.jpg | Patrick Langwallner | Unsplash | [Licencia Unsplash](https://unsplash.com/license) |
| login.jpg | Sebastian Wagner | Unsplash | [Licencia Unsplash](https://unsplash.com/license) |
| polestar3.jpg, campana3.jpg | Wiryan Tirtarahardja | Unsplash | [Licencia Unsplash](https://unsplash.com/license) |
| polestar4-coupe.jpg | Alejandro-93 | [Wikimedia Commons](https://commons.wikimedia.org/wiki/File:Polestar_4_IMG_1734.jpg) | CC BY-SA 4.0 |
| polestar4-suv.jpg (interior) | Alejandro-93 | [Wikimedia Commons](https://commons.wikimedia.org/wiki/File:Polestar_4_IMG_1743.jpg) | CC BY-SA 4.0 |
| polestar4-dual.jpg | ver página del archivo | [Wikimedia Commons](https://commons.wikimedia.org/wiki/File:Polestar_4_004.jpg) | CC BY-SA 4.0 |
| polestar5.jpg | Alexander Migl | Wikimedia Commons («Polestar_5_Auto_Zuerich_2025_DSC_3198») | CC BY-SA 4.0 |
| campana1.jpg | Alexander Migl | Wikimedia Commons («Polestar_5_Auto_Zuerich_2025_DSC_3195») | CC BY-SA 4.0 |
| polestar5-performance.jpg | Alexandre Prevot | Wikimedia Commons («Polestar_5_55110544940») | CC BY-SA 4.0 |

Las imágenes de Wikimedia Commons recortadas se comparten bajo la misma licencia CC BY-SA 4.0.

## Uso de inteligencia artificial
Este proyecto se ha desarrollado con ayuda de **Claude (Anthropic)**, modelo Claude Opus 5.5, los días 11 y 12 de noviembre de 2026. La IA me ayudó a generar y me explicó la estructura del proyecto, el XAML de las tres pantallas, los estilos y el código C#. Yo ejecuté y probé la aplicación, detecté errores y pedí corregirlos, busqué y verifiqué las fotos y sus licencias, elegí el color y el eslogan, y gestioné este repositorio. El prompt inicial y el detalle están en la documentación entregada.

> Datos y precios orientativos (prensa del motor española, noviembre 2026). Proyecto académico sin relación oficial con Polestar.
