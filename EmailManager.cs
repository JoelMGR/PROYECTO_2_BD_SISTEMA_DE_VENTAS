using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO;
using System.Collections;

namespace WpfApplication1
{
    class EmailManager
    {
        //Recibe un string dir que es la direccion electronica que recibe el correo y el nombre del pdf que
        //se recibira ******** SIN LA EXTENSION .pdf ***********
        public void SendEmail(string dir, string filename)
        {
            try
            {
                //Construimos el correo 
                MailMessage email = new MailMessage();
                email.To.Add(new MailAddress(dir));
                email.From = new MailAddress("proyecto2bd@outlook.com");
                email.Subject = "Aqui es Jose haciendo SPAM";
                email.Body = "Ya el codigo de enviar correos funciona al fin XD";

                //Debe guardarse el pdf a en bin \\ Debug
                Attachment data = new Attachment(filename + ".pdf", MediaTypeNames.Application.Pdf);
                email.Attachments.Add(data);

                //Esto lo usamos para enviar el correo
                SmtpClient clienteSmtp = new SmtpClient();

                //Host para correos outlook
                clienteSmtp.Host = "Smtp.live.com";

                clienteSmtp.Port = 587;

                //Especificamos no usar el correo propio y usamos proyecto2bd@outlook.com
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                clienteSmtp.Credentials = new System.Net.NetworkCredential("proyecto2bd@outlook.com", "APasarBases");
                clienteSmtp.EnableSsl = true;
                clienteSmtp.Send(email);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}
