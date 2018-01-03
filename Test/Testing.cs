using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Cotizaciones.Data;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Collections.Generic;
using System;

namespace Cotizaciones.Models 
{
    public class MainServicioTest : IDisposable  {

        IMainServicio Servicio {get; set;}

        ILogger Logger {get; set;}

        public MainServicioTest(){

            // Logger Factory
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            Logger = loggerFactory.CreateLogger<MainServicioTest>();

            Logger.LogInformation("Initializing Test ..");

            // SQLite en memoria
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Opciones de inicializacion del CotizacionesContext
            var options = new DbContextOptionsBuilder<CotizacionesContext>()
            .UseSqlite(connection)
            .Options;

            // inicializacion del CotizacionesContext
            CotizacionesContext backend = new CotizacionesContext(options);
            // Crear la base de datos
            backend.Database.EnsureCreated();

            // Servicio a probar
            Servicio = new MainServicio(backend, loggerFactory);

            Logger.LogInformation("Initialize Test ok.");

        }
        [Fact]
        public void InitializeTest()
        {
            Logger.LogInformation("Testing IMainService.Initialize() ..");
            Servicio.Initialice();

            // No se puede inicializar 2 veces
            Assert.Throws<Exception>( () => { Servicio.Initialice(); });

            // Personas en la BD
            List<Persona> personas = Servicio.Personas();

            // Debe ser !=  de null
            Assert.True(personas != null);

            // Debe haber 5 persona
            Assert.True(personas.Count == 5);

            // Print de la persona
            foreach(Persona persona in personas) {
                Logger.LogInformation("Persona: {0}", persona);
            }

            Logger.LogInformation("Test IMainService.Initialize() ok");
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}


