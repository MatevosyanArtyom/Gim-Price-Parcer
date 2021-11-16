using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Gim.PriceParser.Bll.Mail
{
    public class MailClient: IMailClient
    {
        private readonly MailSettings _mailSettings;
        private readonly IPriceListDao _priceListDao;
        private readonly ISchedulerTaskDao _schedulerTaskDao;

        public MailClient(IOptions<MailSettings> options, ISchedulerTaskDao schedulerTaskDao,
            IPriceListDao priceListDao)
        {
            _schedulerTaskDao = schedulerTaskDao;
            _priceListDao = priceListDao;
            _mailSettings = options.Value;
        }

        public async Task ReceiveMessagesAsync()
        {
            var priceLists = new List<PriceList>();

            using (var client = new ImapClient())
            {
                await client.ConnectAsync(_mailSettings.ImapHost, _mailSettings.ImapPort);
                await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);

                var folder = client.Inbox;
                await folder.OpenAsync(FolderAccess.ReadWrite);

                var uids = await folder.SearchAsync(SearchQuery.NotSeen);

                if (!uids.Any())
                {
                    return;
                }

                var filter = new SchedulerTaskFilter
                {
                    StartBy = SchedulerTaskStartBy.Email,
                    Status = SchedulerTaskStatus.Active
                };
                var tasks = await _schedulerTaskDao.GetManyAsync(filter);
                var emails = tasks.Entities.SelectMany(x => x.Emails.Split(';'));

                var envelopes = await folder.FetchAsync(uids, MessageSummaryItems.Envelope);
                envelopes = envelopes
                    // check if any sender's mailbox address intersects with task's mails
                    .Where(e => e.Envelope.From.Mailboxes.Select(m => m.Address).Intersect(emails).Any())
                    .ToList();

                foreach (var envelope in envelopes)
                {
                    var msg = await folder.GetMessageAsync(envelope.UniqueId);
                    foreach (var attachment in msg.Attachments)
                        // if FileName is not empty, it should be a file
                    {
                        if (attachment is MimePart mimePart && !string.IsNullOrWhiteSpace(mimePart.FileName))
                        {
                            // find task which email's intersects with any of mail's emails
                            var task = tasks.Entities.FirstOrDefault(t =>
                                t.Emails.Split(';')
                                    .Intersect(envelope.Envelope.From.Mailboxes.Select(m => m.Address))
                                    .Any());

                            GimFile file; // minimize stream's memory use time
                            await using (var stream = new MemoryStream())
                            {
                                await mimePart.Content.DecodeToAsync(stream);

                                file = new GimFile
                                {
                                    Data = Convert.ToBase64String(stream.ToArray()),
                                    Name = mimePart.FileName,
                                    Size = (int) stream.Length
                                };
                            }

                            var priceList = new PriceList
                            {
                                PriceListFile = file,
                                SchedulerTaskId = task?.Id,
                                SupplierId = task?.SupplierId
                            };
                            priceLists.Add(priceList);
                        }
                    }
                }

                await folder.AddFlagsAsync(uids, MessageFlags.Seen, false);

                await client.DisconnectAsync(true);
            }

            await _priceListDao.AddManyAsync(priceLists);
        }

        public async Task SendMessageAsync(string email, string subject, string text)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_mailSettings.UserName, _mailSettings.UserName));
            msg.To.Add(new MailboxAddress(email, email));
            msg.Subject = subject;
            msg.Body = new TextPart(TextFormat.Html) {Text = text};

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort);
            await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);

            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
