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
        private static SmtpClient clienteJson;

        private static IConfigurationSection ServidorDeCorreo { get; set; }

        private static string Usuario => ServidorDeCorreo["user"];
        private static string Servidor => ServidorDeCorreo["host"];
        private static bool SSL => ServidorDeCorreo["enableSsl"] == "true";
        private static int Puerto => ServidorDeCorreo["port"].Entero();
        private static string Password => ServidorDeCorreo["password"];

        public GestorDeCorreo()
        {
            InicializaConfiguracion();

            clienteJson = new SmtpClient(Servidor,Puerto)
            {
                EnableSsl =SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Usuario,Password)
            };
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
            var email = new MailMessage(Usuario, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            clienteJson.Send(email);
        }


        public static void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            var correo = new GestorDeCorreo();
            correo.Enviar(destinatario,asunto,mensaje,esHtlm);
        }


    }
}
