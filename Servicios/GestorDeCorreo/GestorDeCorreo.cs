using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Gestor.Correo
{

    public class GestorDeCorreo
    {
        private SmtpClient cliente;
        private static IConfiguration Configuration { get; set; }
        private MailMessage email;
        public GestorDeCorreo()
        {
            InicializaConfiguracion();
            cliente = new SmtpClient(Configuration["host"], Int32.Parse(Configuration["port"]))
            {
                EnableSsl = Boolean.Parse(Configuration["enableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Configuration["user"], Configuration["password"])
            };
        }
        private static void InicializaConfiguracion()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddXmlFile("configuracionCorreo.xml");
            Configuration = builder.Build();
        }
        public void Enviar(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            email = new MailMessage(Configuration["user"], destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);
        }
        public void Enviar(MailMessage message)
        {
            cliente.Send(message);
        }
        public async Task EnviarAsync(MailMessage message)
        {
            await cliente.SendMailAsync(message);
        }

        public static void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            var correo = new GestorDeCorreo();
            correo.Enviar(destinatario,asunto,mensaje,esHtlm);
        }


    }
}
