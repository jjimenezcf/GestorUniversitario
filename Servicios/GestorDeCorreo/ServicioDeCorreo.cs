﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Utilidades;

namespace ServicioDeCorreos
{

    public class ServicioDeCorreo
    {
        public static bool EnviandoCorreo { get; set; } = false;

        private SmtpClient SmtpCliente;

        private IConfigurationSection ServidorDeCorreo { get; set; }


        private string Sistema => ServidorDeCorreo["Sistema"].ToUpper();
        public string Emisor => ServidorDeCorreo["usuario"];
        private string Servidor => ServidorDeCorreo["servidor"];
        private bool SSL => ServidorDeCorreo["sslActivo"] == "true";
        private int Puerto => ServidorDeCorreo["puerto"].Entero();
        private string Password => ServidorDeCorreo["clave"];

        public ServicioDeCorreo(string servidor)
        {
            InicializaConfiguracion(servidor);

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
                    Credentials = new NetworkCredential(Emisor, Password)
                };
            }
            else
                throw new Exception($"Sistema de correo {Sistema} no definido");
        }

        private void InicializaConfiguracion(string servidor)
        {
            var generador = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var configuration = generador.Build();
            ServidorDeCorreo = configuration.GetSection("ServidorDeCorreo").GetSection(servidor);
        }

        internal void EnviarPara(List<string> receptores, string asunto, string mensaje, bool esHtlm, List<string> archivos, ManejadorDeCorreo manejador)
        {
            EnviarDe(Emisor, receptores, asunto, mensaje, esHtlm, archivos, manejador);
        }

        internal void EnviarDe(string emisor, List<string> receptores, string asunto, string mensaje, bool esHtlm, List<string> archivos, ManejadorDeCorreo manejador)
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

            if (archivos != null)
                foreach (var archivo in archivos)
                {
                    var attach = new Attachment(archivo);
                    email.Attachments.Add(attach);
                }

            SmtpCliente.SendCompleted += new SendCompletedEventHandler(despuesDeEnviarElCorreo);

            SmtpCliente.SendAsync(email, manejador);
        }

        private static void despuesDeEnviarElCorreo(object sender, AsyncCompletedEventArgs e)
        {
            EnviandoCorreo = true;
            var manejador = (ManejadorDeCorreo)e.UserState;
            try
            {
                if (e.Cancelled)
                {
                    manejador.GestorDeCorreo.InvokeMember("AnotarTraza", BindingFlags.InvokeMethod, null, null, new object[] { manejador.Contexto, manejador.CorreoDtm, "Se ha cancelado el envío de correos" });
                }
                if (e.Error != null)
                {
                    manejador.GestorDeCorreo.InvokeMember("AnotarExcepcion", BindingFlags.InvokeMethod, null, null, new object[] { manejador.Contexto, manejador.CorreoDtm, e.Error });
                }
                else
                {
                    manejador.GestorDeCorreo.InvokeMember("IndicarQueElCorreoHaSidoEnviado", BindingFlags.InvokeMethod, null, null, new object[] { manejador.Contexto, manejador.CorreoDtm });
                }
            }
            finally
            {
                EnviandoCorreo = false;
            }
        }


        public static void EnviarCorreoPara(string servidor, List<string> receptores, string asunto, string mensaje, bool esHtlm = true, List<string> archivos = null, ManejadorDeCorreo manejador = null)
        {
            var servicio = new ServicioDeCorreo(servidor);
            servicio.EnviarPara(receptores, asunto, mensaje, esHtlm, archivos, manejador);
        }

        public static void EnviarCorreoDe(string servidor, string emisor, List<string> receptores, string asunto, string mensaje, bool esHtlm = true, List<string> archivos = null, ManejadorDeCorreo manejador = null)
        {
            var servicio = new ServicioDeCorreo(servidor);
            servicio.EnviarDe(emisor, receptores, asunto, mensaje, esHtlm, archivos, manejador);
        }


        public class ManejadorDeCorreo
        {
            public Type GestorDeCorreo;
            public object Contexto;
            public object CorreoDtm;
        }
    }


}
