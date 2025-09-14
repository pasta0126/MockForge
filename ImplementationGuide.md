# Gu�a r�pida para implementar nuevas funcionalidades

Objetivo
- Explicar de forma simple c�mo a�adir m�todos, proveedores y datos de localizaci�n a MockForge.

Antes de empezar
- Proyectos clave:
  - `MockForge.Core`: contratos/utilidades (`IProvider`, `IRandomizer`, `ILocaleStore`, `ITemplateEngine`, `MockForgeOptions`).
  - `MockForge`: fachada/DI (`IMockForge`, `MockForgeImpl`, `AddMockForge`).
  - `MockForge.Providers`: proveedores concretos (p. ej., `NameProvider`).
  - `MockForge.Locales`: JSONs embebidos por idioma.
  - `MockForge.Tests`: pruebas (xUnit + FluentAssertions).

A�adir un m�todo a un proveedor existente
1. Ubica la clase en `MockForge.Providers` (por ejemplo, `NameProvider`).
2. Si accede a datos localizados, usa `ILocaleStore` y `IRandomizer` inyectados por el ctor.
3. Implementa el m�todo (idealmente async si lee del store):
   ```csharp
   public async Task<string> NickAsync()
   {
       var list = await _store.GetListAsync(_locale, "name.nick");
       return list.Count == 0 ? string.Empty : _rnd.Pick(list);
   }
   ```
4. Crea/actualiza claves en los JSON de `MockForge.Locales` si el m�todo las necesita (ver m�s abajo).
5. A�ade pruebas en `MockForge.Tests` que validen el comportamiento (no nulo/no vac�o, etc.).

Crear un nuevo proveedor
1. Crea una carpeta/namescape en `MockForge.Providers.<Feature>` y a�ade la clase:
   - Debe implementar `IProvider` y exponer `Name`.
   - Debe tener ctor `(IRandomizer rnd, ILocaleStore store, string locale)`.
   ```csharp
   public sealed class AddressProvider : IProvider
   {
       private readonly IRandomizer _rnd;
       private readonly ILocaleStore _store;
       private readonly string _locale;
       public string Name => nameof(AddressProvider);

       public AddressProvider(IRandomizer rnd, ILocaleStore store, string locale)
       { _rnd = rnd; _store = store; _locale = locale; }

       public async Task<string> CityAsync()
       {
           var cities = await _store.GetListAsync(_locale, "address.city");
           return _rnd.Pick(cities);
       }
   }
   ```
2. Consumo desde clientes: `forge.Get<AddressProvider>().CityAsync()`.
3. A�ade pruebas en `MockForge.Tests`.

A�adir nuevas claves de localizaci�n
1. Edita `src/MockForge.Locales/Locales/<idioma>.json` (p. ej., `es.json` y `en.json`).
2. Usa claves en ingl�s y estructura jer�rquica por dominio (p. ej., `"address.city"`).
3. Los archivos ya est�n marcados como `EmbeddedResource`; no necesitas tocar el `.csproj`.
4. El store aplica fallback: solicitado ? base (antes del guion) ? `en`.

Usar plantillas con `ITemplateEngine`
- Para componer textos con tokens `{{token}}`:
  ```csharp
  var engine = new SimpleTemplateEngine();
  var result = engine.Render("Hola {{name}}", new Dictionary<string, Func<string>>
  {
      ["name"] = () => _rnd.Pick(names)
  });
  ```

Opciones y DI
- Para nuevas opciones, a�ade propiedades a `MockForgeOptions` y �salas en `MockForgeImpl`.
- Registro DI: `services.AddMockForge(o => { o.Locale = "es"; o.Seed = 123; });`.

Buenas pr�cticas
- Mant�n m�todos orientados a tareas concretas y con nombres claros.
- Usa `IRandomizer.Pick` para seleccionar elementos de listas.
- Evita bloquear; prefiere m�todos `async` cuando uses `ILocaleStore`.
- A�ade pruebas por cada nuevo m�todo/clave.

Plantilla de prueba (xUnit + FluentAssertions)
```csharp
[Fact]
public async Task CityAsync_NotEmpty()
{
    var asm = Assembly.Load("MockForge.Locales");
    var store = new EmbeddedLocaleStore(asm);
    var rnd = new ThreadSafeRandomizer(123);
    var p = new AddressProvider(rnd, store, "es");

    var city = await p.CityAsync();
    city.Should().NotBeNullOrWhiteSpace();
}
```

Lista de verificaci�n r�pida
- [ ] Implementaci�n del m�todo/proveedor.
- [ ] Claves a�adidas a `en.json` y `es.json` (u otros locales necesarios).
- [ ] Pruebas unitarias pasando.
- [ ] Uso validado v�a `IMockForge.Get<T>()` cuando proceda.
