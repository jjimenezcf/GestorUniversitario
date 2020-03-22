using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Utilidades;

namespace Gestor.Correo
{

    public class GestorDeCorreo
    {
        private static SmtpClient SmtpCliente;

        private static IConfigurationSection ServidorDeCorreo { get; set; }


        private static string Sistema => ServidorDeCorreo["Sistema"].ToUpper();
        private static string Usuario => ServidorDeCorreo["usuario"];
        private static string Servidor => ServidorDeCorreo["servidor"];
        private static bool SSL => ServidorDeCorreo["sslActivo"] == "true";
        private static int Puerto => ServidorDeCorreo["puerto"].Entero();
        private static string Password => ServidorDeCorreo["contraseña"];

        public GestorDeCorreo()
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
        public void Enviar(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            MailMessage email;

            if (Sistema == "EMUASA")
                email = new MailMessage(Usuario, destinatario, asunto, mensaje);
            else
            if (Sistema == "GMAIL")
                email = new MailMessage(Usuario, destinatario, asunto, mensaje);
            else
                throw new Exception($"Sistema de correo {Sistema} no definido");

            email.IsBodyHtml = esHtlm;
            //SmtpCliente.Send(email);
        }


        public static void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            var correo = new GestorDeCorreo();
            correo.Enviar(destinatario, asunto, mensaje, esHtlm);
        }


    }
}
