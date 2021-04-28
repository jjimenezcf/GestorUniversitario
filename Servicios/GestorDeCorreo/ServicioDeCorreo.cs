using System;
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
        private static string Password => ServidorDeCorreo["contraseña"];

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
        public void Enviar(string destinatario, string asunto, string mensaje, bool esHtlm = true)
        {

            if (Sistema != "EMUASA" && Sistema != "GMAIL")
                throw new Exception($"Sistema de correo {Sistema} no definido");

            MailMessage email = new MailMessage(new MailAddress(Usuario), new MailAddress(destinatario))
            {
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = esHtlm,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Subject = asunto,
                Body = mensaje.Replace("\n", "<br/>")
            };

            SmtpCliente.Send(email);
        }


        public static void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            var servicio = new ServicioDeCorreo();
            servicio.Enviar(destinatario, asunto, mensaje, esHtlm);
        }


    }
}
