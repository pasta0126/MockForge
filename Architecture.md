# Arquitectura de MockForge

## Resumen
MockForge es una librer�a para .NET 8 enfocada en generar datos simulados (fakes) con soporte de localizaci�n, determinismo por semilla y f�cil integraci�n v�a DI. La soluci�n se organiza en capas: n�cleo (abstracciones/utilidades), proveedores, datos de locales y composici�n/fachada.

## Proyectos y responsabilidades
- MockForge.Core
  - Abstracciones: `IMockForge`, `IProvider`, `IRandomizer`, `ILocaleStore`, `ITemplateEngine`.
  - Implementaciones:
    - `EmbeddedLocaleStore`: carga JSONs embebidos (p. ej., `MockForge.Locales.es.json`) con cach� en memoria y cadena de fallback: solicitado ? base (antes del guion) ? `"en"`.
    - `SimpleTemplateEngine`: reemplazo de tokens `{{token}}`.
    - Random: `ThreadSafeRandomizer` (thread-safe y determinista con semilla).
  - Opciones: `MockForgeOptions` (`Locale`, `Seed`).

- MockForge
  - Fachada/Composici�n:
    - `MockForgeImpl` (implementa `IMockForge`): mantiene `Locale`, un `IRandomizer`, un `ILocaleStore` (leyendo el ensamblado `MockForge.Locales`) y un cach� de proveedores por tipo. Crea proveedores por reflexi�n con ctor `(IRandomizer, ILocaleStore, string locale)`.
    - `ServiceCollectionExtensions.AddMockForge(...)`: registro en DI.
    - `MockForgeFactory.Create(...)`: f�brica para uso sin DI.

- MockForge.Providers
  - Proveedores concretos (p. ej., `NameProvider`) que implementan `IProvider` y consumen `IRandomizer` + `ILocaleStore` para generar valores (nombres, etc.).

- MockForge.Locales
  - Datos de localizaci�n en JSON embebidos como recursos (`Locales/en.json`, `Locales/es.json`) con nombres l�gicos `MockForge.Locales.<loc>.json`.

- MockForge.Tests
  - Pruebas (xUnit + FluentAssertions) que validan recursos embebidos y que los proveedores generan resultados (p. ej., `NameProvider.FullAsync()`).

## Flujo de trabajo (alto nivel)
1. Configuras `MockForgeOptions` (locale y semilla opcional) v�a DI o f�brica.
2. Obtienes un proveedor: `forge.Get<NameProvider>()` (instancia cacheada por tipo).
3. El proveedor usa `EmbeddedLocaleStore` para leer listas (p. ej., `"name.first"`) aplicando la cadena de fallback.
4. `IRandomizer` selecciona elementos; `ITemplateEngine` puede componer cadenas a partir de plantillas.

## Decisiones de dise�o
- Separaci�n clara de responsabilidades (n�cleo, proveedores, datos, composici�n).
- Determinismo opcional por semilla y random seguro para m�ltiples hilos.
- Cach� de proveedores y de datos de locales para rendimiento.
- DI-first con una API simple v�a `IMockForge`.

## Uso r�pido
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
- Cons�mela v�a `forge.Get<MiProveedor>()`.
