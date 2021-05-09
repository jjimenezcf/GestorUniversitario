using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Utilidades;

namespace ServicioDeCorreos
{

    public class ServicioDeCorreo
    {
        private static SmtpClient SmtpCliente;

        private static IConfigurationSection ServidorDeCorreo { get; set; }


        private static string Sistema => ServidorDeCorreo["Sistema"].ToUpper();
        private static string Usuario => ServidorDeCorreo["usuario"];
        private static string Servidor => ServidorDeCorreo["servidor"];
        private static bool SSL => ServidorDeCorreo["sslActivo"] == "true";
        private static int Puerto => ServidorDeCorreo["puerto"].Entero();
        private static string Password => ServidorDeCorreo["clave"];

        public ServicioDeCorreo()
        {
            InicializaConfiguracion();

            if (Sistema == "EMUASA")
            {
                SmtpCliente = new SmtpClient(Servidor);
            }
            else
            if (Sistema == "GMAIL")
            {
                SmtpCliente = new SmtpClient(Servidor, Puerto)
                {
                    EnableSsl = SSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Usuario, Password)
                };
            }
            else
                throw new Exception($"Sistema de correo {Sistema} no definido");
        }

        private static void InicializaConfiguracion()
        {
            var generador = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var configuration = generador.Build();
            ServidorDeCorreo = configuration.GetSection("ServidorDeCorreo");
        }

        internal void EnviarPara(List<string> receptores, string asunto, string mensaje, bool esHtlm, List<string> archivos, MailPriority prioridad)
        {
            EnviarDe(Usuario, receptores, asunto, mensaje, esHtlm, archivos, prioridad);
        }

        internal void EnviarDe(string emisor, List<string> receptores, string asunto, string mensaje, bool esHtlm, List<string> archivos, MailPriority prioridad)
        {

            if (Sistema != "EMUASA" && Sistema != "GMAIL")
                throw new Exception($"Sistema de correo {Sistema} no definido");

            var destinos = receptores.ToString(";");
            if (destinos.IsNullOrEmpty())
            {
                throw new Exception($"No se ha definido el destinatario del correo {asunto}");
            }

            MailMessage email = new MailMessage(new MailAddress(emisor), new MailAddress(destinos))
            {
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = esHtlm,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Subject = asunto,
                Body = mensaje.Replace("\n", "<br/>")

            };

            if (archivos != null) foreach (var archivo in archivos)
                {
                    var attach = new Attachment(archivo);
                    email.Attachments.Add(attach);
                }

                SmtpCliente.Send(email);
        }

        public static void EnviarCorreoPara(List<string> receptores, string asunto, string mensaje, bool esHtlm = true, List<string> archivos = null, MailPriority prioridad = MailPriority.Normal)
        {
            var servicio = new ServicioDeCorreo();
            servicio.EnviarPara(receptores, asunto, mensaje, esHtlm, archivos, prioridad);
        }

        public static void EnviarCorreoDe(string emisor, List<string> receptores, string asunto, string mensaje, bool esHtlm = true, List<string> archivos = null, MailPriority prioridad = MailPriority.Normal)
        {
            var servicio = new ServicioDeCorreo();
            servicio.EnviarDe(emisor, receptores, asunto, mensaje, esHtlm, archivos, prioridad);
        }

    }
}
