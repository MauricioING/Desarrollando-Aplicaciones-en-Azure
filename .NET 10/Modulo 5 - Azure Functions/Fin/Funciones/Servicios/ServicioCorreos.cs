using Funciones.Funciones.OptimizadorGrupoDeImagenes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Funciones.Servicios
{
    internal class ServicioCorreos : IServicioCorreos
    {
        private readonly IConfiguration configuration;

        public ServicioCorreos(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarReporteImagenesOptimizadas(ResultadoOptimizarGrupoDeImagenes modelo, string correo)
        {
            var asunto = "Su imágenes fueron optimizadas";

            var listadoImagenes = modelo.Urls.Select(url => $"- {url}").ToList();
            var imagenes = string.Join("\n", listadoImagenes);

            var cuerpo = $"""
                Saludos, 
            
                Ya tenemos listo el resultado del reporte {modelo.JobId}

                Aquí las sus imágenes optimizadas:

                {imagenes}

                Atentamente,
                Felipe
                """;

            await EnviarMensaje(correo, asunto, cuerpo);
        }


        public async Task EnviarReportePersonas(string url, string correo)
        {
            var asunto = "Su reporte ha sido completado";

            var cuerpo = $"""
Saludos, 
            
Puede ver el reporte haciendo click en este enlace: {url}

Atentamente,
Felipe
""";

            await EnviarMensaje(correo, asunto, cuerpo);
        }


        public async Task EnviarMensajeDeCumple(string nombre, string correo)
        {
            var asunto = "Feliz cumpleaños 🎉";

            var cuerpo = $"""
    Saludos, {nombre}, 
            
    Te queremos desear un feliz cumpleaños. 🎂

    Atentamente,
    Felipe
    """;

            await EnviarMensaje(correo, asunto, cuerpo);

        }

        private async Task EnviarMensaje(string emailDestinatario, string asunto, string cuerpo)
        {
            var nuestroEmail = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");

            var smtpCliente = new SmtpClient(host, puerto);
            smtpCliente.EnableSsl = true;
            smtpCliente.UseDefaultCredentials = false;
            smtpCliente.Credentials = new NetworkCredential(nuestroEmail, password);

            var mensaje = new MailMessage(nuestroEmail!, emailDestinatario, asunto, cuerpo);
            await smtpCliente.SendMailAsync(mensaje);
        }

    }
}
