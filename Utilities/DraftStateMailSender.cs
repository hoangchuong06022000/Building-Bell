using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SitecoreCaseStudy.Utilities
{
    public class DraftStateMailSender
    {
        public void Process(WorkflowPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            ProcessorItem processorItem = args.ProcessorItem;

            if (processorItem == null)
            {
                return;
            }

            Item innerItem = processorItem.InnerItem;
            var newsItem = args.DataItem;

            string from = GetText(innerItem, "from", args);
            string to = GetText(innerItem, "to", args);
            string mailServer = GetText(innerItem, "mail server", args);
            string subject = GetText(innerItem, "subject", args);
            string message = GetText(innerItem, "message", args);

            var senderEmail = new MailAddress(from);
            var receiverEmail = new MailAddress(to);
            var password = "MOYmeoHONG321@";
            var smtp = new SmtpClient
            {
                Host = mailServer,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };

            string contentMail = message;

            using (var messageToSend = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = contentMail
            })
            {
                smtp.Send(messageToSend);
                Sitecore.Diagnostics.Log.Info(string.Format("Send mail Contributor add new news item: From: {0}; To:{1}; Item Name: {2}, DateTime: {3}.", senderEmail, receiverEmail, newsItem.Name, DateTime.Now), this);
            }
        }

        private string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string text = commandItem[field];
            return text.Length > 0 ? ReplaceVariables(text, args) : string.Empty;
        }

        private string ReplaceVariables(string text, WorkflowPipelineArgs args)
        {
            text = text.Replace("$itemName$", args.DataItem.Paths.FullPath);
            text = text.Replace("$itemPath$", args.DataItem.Paths.FullPath);
            text = text.Replace("$itemLanguage$", args.DataItem.Language.ToString());
            text = text.Replace("$itemVersion$", args.DataItem.Version.ToString());
            return text;
        }
    }
}