using System.Net.Mail;
using System.Net;
using System.Text;
using CompanyDetailsWebApp.Models;

namespace CompanyDetailsWebApp
{
    /// <summary>
    /// This class contains methods to send email using SmtpClient
    /// </summary>
    public class EmailService
    {

        #region private variables
        /// <summary>
        /// This member is used to get values from appsettings.json.
        /// </summary>
        IConfiguration _configuration;

        /// <summary>
        /// Name or IP address of the host used for SMTP transaction taken from configuration settings.
        /// </summary>
        private readonly string _sHost;

        /// <summary>
        /// Email address of the sender taken from configuration settings.
        /// </summary>
        private readonly string _sUsername;

        /// <summary>
        /// Email password of the sender.
        /// </summary>
        private readonly string _sEmailPassword;

        /// <summary>
        /// <para>587 for TLS. The preferred port for secure email submission from clients.</para>
        /// </summary>
        private readonly int _iPort;
        #endregion


        public EmailService(IConfiguration config)
        {
            _configuration = config;

            _sHost = _configuration["Email:Host"];
            _sUsername = _configuration["Email:Username"];
            _sEmailPassword = _configuration["Email:Password"];
            _iPort = Convert.ToInt32(_configuration["Email:Port"]); // 465 for SSL; 587 for TLS; 25 for insecure communication,
            // email not sent if port 25 is used
        }

        /// <summary>
        /// This method is used to send the email with respect to different templates.
        /// </summary>
        /// <param name="specificTemplateFolder">The specific folder name for template to be used.</param>
        public void SendMailWithTemplate(string specificTemplateFolder)
        {
            MailMessage mailMessage = null;
            SmtpClient smtpClient = null;
            List<string> lstToEmails = null;
            try
            {
                // get values of recipient email addresses from appsettings.json
                lstToEmails = _configuration["Email:ToEmails"].Split(';').ToList();

                string sEmailSubject = string.Empty;
                string sHtmlBody = string.Empty;

                ReadTemplateFiles(specificTemplateFolder, ref sEmailSubject, ref sHtmlBody);

                // Create MailMessage object to provide various entities regarding email
                #region MailMessage

                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_sUsername);
                foreach (var emailItem in lstToEmails)
                {
                    mailMessage.To.Add(emailItem);
                }
                mailMessage.Subject = sEmailSubject;
                mailMessage.Body = sHtmlBody; // html content from file
                mailMessage.IsBodyHtml = true; // set this to true if MailMessage.Body is to be rendered as html.

                #endregion

                // Create SmtpClient for sending email
                #region Smtp client creation

                smtpClient = new SmtpClient(_sHost); // create smtp instance with host
                smtpClient.Port = _iPort;
                smtpClient.Credentials = new NetworkCredential(_sUsername, _sEmailPassword);

                smtpClient.EnableSsl = _iPort == 587 ? true : false;

                smtpClient.Send(mailMessage);

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
                if (smtpClient != null)
                {
                    smtpClient.Dispose();
                }
            }
        }


        /// <summary>
        /// This function is used to pick specific template files of subject and html email content by providing specific template folder name
        /// </summary>
        /// <param name="specificTemplateFolder">Folder name of specific email template to be used.</param>
        /// <param name="emailSubject">Email subject to be populated after reading subject text file.</param>
        /// <param name="emailBody">Html content to be populated after reading the Html content text file.</param>
        private void ReadTemplateFiles(string specificTemplateFolder, ref string emailSubject, ref string emailBody)
        {
            try
            {
                string sSubjectFileName = "Subject.txt";
                string sHtmlFileName = "HtmlContent.txt";

                var subjectFilePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", specificTemplateFolder, sSubjectFileName);
                if (File.Exists(subjectFilePath))
                {
                    emailSubject = File.ReadAllText(subjectFilePath); //read subject line file separately if file exists
                }

                var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", specificTemplateFolder, sHtmlFileName);
                if (File.Exists(htmlFilePath))
                {
                    using (StreamReader readerHTML = new StreamReader(htmlFilePath))
                    {
                        StringBuilder sbBuilder = new StringBuilder();
                        string line;
                        while ((line = readerHTML.ReadLine()) != null)// read html file line by line if file exists
                        {
                            sbBuilder.AppendLine(line);
                           
                        }
                        emailBody = sbBuilder.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private string DynamicHtml(CompanyDetails companyDetails, string selectedMachine)
        {
            try
            {
                StringBuilder htmlBuilder = new StringBuilder();

                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html lang=\"en\">");
                htmlBuilder.AppendLine("<head>");
                htmlBuilder.AppendLine("    <meta charset=\"UTF-8\">");
                htmlBuilder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                htmlBuilder.AppendLine("    <style>");
                htmlBuilder.AppendLine("        #email-header {");
                htmlBuilder.AppendLine("            color: #eaecee;");
                htmlBuilder.AppendLine("            background-color: #2c3e50;");
                htmlBuilder.AppendLine("        }");
                htmlBuilder.AppendLine("    </style>");
                htmlBuilder.AppendLine("</head>");
                htmlBuilder.AppendLine("<body>");
                htmlBuilder.AppendLine("    <header>");
                htmlBuilder.AppendLine("        <h1 id=\"email-header\">Email Header</h1>");
                htmlBuilder.AppendLine("    </header>");
                htmlBuilder.AppendLine("");
                htmlBuilder.AppendLine("    <main>");
                htmlBuilder.AppendLine("        <h2 style=\"background-color: lightblue;\">Template One Heading</h2>");
                htmlBuilder.AppendLine($"        <p>Company Name: {companyDetails.CompanyName}.</p>");
                htmlBuilder.AppendLine($"        <p>Company Name: {selectedMachine}.</p>");
                htmlBuilder.AppendLine("    </main>");
                htmlBuilder.AppendLine("");
                htmlBuilder.AppendLine("    <footer>");
                htmlBuilder.AppendLine("        <div>");
                htmlBuilder.AppendLine("            <hr>");
                htmlBuilder.AppendLine("            <p style=\"color: blue\">Regards,</p>");
                htmlBuilder.AppendLine("            <p style=\"font-weight: bold;\">xyz Name,</p>");
                htmlBuilder.AppendLine("            <p>Department | WASP3D | Beehive Systems</p>");
                htmlBuilder.AppendLine("            <p>B 37, Sector 1 | Noida 201301 | India</p>");
                htmlBuilder.AppendLine("            <p>www.wasp3d.com</p>");
                htmlBuilder.AppendLine("        </div>");
                htmlBuilder.AppendLine("    </footer>");
                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                return htmlBuilder.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
