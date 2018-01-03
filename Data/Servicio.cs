using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Cotizaciones.Models;
using Cotizaciones.Data;

namespace Cotizaciones.Data{

    /// <summary>
    /// Métodos de la interface
    /// </summary>
    public interface IMainServicio
    {
        void add(Persona persona);
        List<Persona> FindPersonas(String nombre);
        List<Persona> Personas();

        void add(Cotizacion cotizacion);
        List<Cotizacion> FindCotizacion(Double monto);
        List<Cotizacion> Cotizaciones();

        void Initialice();
    }

    /// <summary>
    /// Implementanción de la interface
    /// </summary>

    public class MainServicio : IMainServicio
    {

        private CotizacionesContext CotizacionesContext {get; set;}
        private ILogger Logger {get; set;}
        private Boolean Initialized {get; set;}

        /// <summary>
        /// Constructor via DI
        /// </summary>
        /// <param name="CotizacionesContext"></param>
        /// <param name="loggerFactory"></param>

        public MainServicio(CotizacionesContext cotizacionesContext, ILoggerFactory loggerFactory){
            // Inicialización del Logger

            Logger = loggerFactory?.CreateLogger <MainServicio> ();
            if(Logger == null){
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            // Obtener el contexto

            CotizacionesContext = cotizacionesContext;
            if(cotizacionesContext == null){
                throw new ArgumentNullException("MainServicio requiere del contexto != null");
            }

            // No se ha inicializado
            Initialized = false;
            
            Logger.LogInformation("MainServicio created");
        }
        

        

        public void add(Persona persona)
        {
            // Guardo a la persona en el contexto
            CotizacionesContext.Personas.Add(persona);
            //guardo los cambios
            CotizacionesContext.SaveChanges();
        }

        public void add(Cotizacion cotizacion)
        {
            // Guardo cotizaciones en el contexto
            CotizacionesContext.Cotizaciones.Add(cotizacion);
            // Guardo los cambios
            CotizacionesContext.SaveChanges();
        }

        public List<Cotizacion> Cotizaciones()
        {
            return CotizacionesContext.Cotizaciones.ToList();
        }

        public List<Cotizacion> FindCotizacion(double monto)
        {
            return null;
        }

        public List<Persona> FindPersonas(string nombre)
        {
            return CotizacionesContext.Personas
            .Where(p => p.Nombre.Contains(nombre))
            .OrderBy(p => p.Nombre).ToList();
        }

        public void Initialice()
        {
            if(Initialized){
                throw new Exception("Solo se permite llamar este metodo una vez");
            }

            Logger.LogDebug("Realizando inicialización..");

            //Persona por defecto

            Persona persona = new Persona();
            persona.Rut = "185181340";
            persona.Nombre = "Fabian";
            persona.Paterno = "Castillo";
            persona.Materno = "Rojas";

            // Agrego la persona al contexto

            this.add(persona);

            foreach(Persona p in CotizacionesContext.Personas){
                Logger.LogDebug("Persona: {0}",p);
            }
            Initialized = true;

            Logger.LogDebug("Inicializacion terminada:)");
        }

        public List<Persona> Personas()
        {
            return CotizacionesContext.Personas.ToList();
        }
    }

}