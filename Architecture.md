# Arquitectura de MockForge

## Resumen
MockForge es una librería para .NET 8 enfocada en generar datos simulados (fakes) con soporte de localización, determinismo por semilla y fácil integración vía DI. La solución se organiza en capas: núcleo (abstracciones/utilidades), proveedores, datos de locales y composición/fachada.

## Proyectos y responsabilidades
- MockForge.Core
  - Abstracciones: `IMockForge`, `IProvider`, `IRandomizer`, `ILocaleStore`, `ITemplateEngine`.
  - Implementaciones:
    - `EmbeddedLocaleStore`: carga JSONs embebidos (p. ej., `MockForge.Locales.es.json`) con caché en memoria y cadena de fallback: solicitado ? base (antes del guion) ? `"en"`.
    - `SimpleTemplateEngine`: reemplazo de tokens `{{token}}`.
    - Random: `ThreadSafeRandomizer` (thread-safe y determinista con semilla).
  - Opciones: `MockForgeOptions` (`Locale`, `Seed`).

- MockForge
  - Fachada/Composición:
    - `MockForgeImpl` (implementa `IMockForge`): mantiene `Locale`, un `IRandomizer`, un `ILocaleStore` (leyendo el ensamblado `MockForge.Locales`) y un caché de proveedores por tipo. Crea proveedores por reflexión con ctor `(IRandomizer, ILocaleStore, string locale)`.
    - `ServiceCollectionExtensions.AddMockForge(...)`: registro en DI.
    - `MockForgeFactory.Create(...)`: fábrica para uso sin DI.

- MockForge.Providers
  - Proveedores concretos (p. ej., `NameProvider`) que implementan `IProvider` y consumen `IRandomizer` + `ILocaleStore` para generar valores (nombres, etc.).

- MockForge.Locales
  - Datos de localización en JSON embebidos como recursos (`Locales/en.json`, `Locales/es.json`) con nombres lógicos `MockForge.Locales.<loc>.json`.

- MockForge.Tests
  - Pruebas (xUnit + FluentAssertions) que validan recursos embebidos y que los proveedores generan resultados (p. ej., `NameProvider.FullAsync()`).

## Flujo de trabajo (alto nivel)
1. Configuras `MockForgeOptions` (locale y semilla opcional) vía DI o fábrica.
2. Obtienes un proveedor: `forge.Get<NameProvider>()` (instancia cacheada por tipo).
3. El proveedor usa `EmbeddedLocaleStore` para leer listas (p. ej., `"name.first"`) aplicando la cadena de fallback.
4. `IRandomizer` selecciona elementos; `ITemplateEngine` puede componer cadenas a partir de plantillas.

## Decisiones de diseño
- Separación clara de responsabilidades (núcleo, proveedores, datos, composición).
- Determinismo opcional por semilla y random seguro para múltiples hilos.
- Caché de proveedores y de datos de locales para rendimiento.
- DI-first con una API simple vía `IMockForge`.

## Uso rápido
```
// DI
services.AddMockForge(o =>
{
    o.Locale = "es";
    o.Seed = 123; // opcional, para resultados reproducibles
});

// Consumo
var forge = provider.GetRequiredService<IMockForge>();
var nombreCompleto = await forge.Get<NameProvider>().FullAsync();
```

## Extender con nuevos proveedores
- Crea una clase que implemente `IProvider` y exponga un `Name`.
- Implementa un constructor `(IRandomizer rnd, ILocaleStore store, string locale)`.
- Usa `store.GetListAsync(locale, "mi.clave")` y `rnd.Pick(...)` para generar valores.
- Consúmela vía `forge.Get<MiProveedor>()`.
