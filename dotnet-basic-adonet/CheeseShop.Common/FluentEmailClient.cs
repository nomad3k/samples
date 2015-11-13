using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CheeseShop.Common
{
    public class FluentEmailClient : IFluentEmailClient
    {
        private class FluentEmail : IFluentEmail
        {
            private string _fromAddress;
            private string[] _to = new string[0];
            private string[] _cc = new string[0];
            private string[] _bcc = new string[0];
            private string[] _exclusions = new string[0];
            private Func<string> _subject;
            private Func<string> _body;
            private MailPriority _priority;
            private string _replyTo;

            public FluentEmail()
            {
                _priority = MailPriority.Normal;
            }

            public IFluentEmail From(string fromAddress)
            {
                _fromAddress = fromAddress;
                return this;
            }

            public IFluentEmail To(params string[] to)
            {
                _to = _to.Concat(to)
                         .Distinct()
                         .ToArray();
                return this;
            }

            public IFluentEmail Cc(params string[] cc)
            {
                _cc = _cc.Concat(cc)
                         .Distinct()
                         .ToArray();
                return this;
            }

            public IFluentEmail Bcc(params string[] bcc)
            {
                _bcc = _bcc.Concat(bcc)
                           .Distinct()
                           .ToArray();
                return this;
            }

            public IFluentEmail Exclude(params string[] exclusions)
            {
                _exclusions = exclusions;
                return this;
            }

            public IFluentEmail Subject(string subject, params object[] args)
            {
                return Subject(() => string.Format(subject, args));
            }

            public IFluentEmail Subject(Func<string> subject)
            {
                _subject = subject;
                return this;
            }

            public IFluentEmail Body(string body, params object[] args)
            {
                return Body(() => string.Format(body, args));
            }

            public IFluentEmail Body(Func<string> body)
            {
                _body = body;
                return this;
            }

            public IFluentEmail Priority(MailPriority priority)
            {
                _priority = priority;
                return this;
            }

            public IFluentEmail ReplyTo(string replyTo)
            {
                _replyTo = replyTo;
                return this;
            }

            public void SendAsync()
            {
                Task.Factory.StartNew(() =>
                                          {
                                              try
                                              {
                                                  Send();
                                              }
                                              catch (Exception e)
                                              {
                                                  // LOG the error!
                                              }
                                          });
            }

            public void Send()
            {
                try
                {
                    var to = _to.Except(_exclusions).Distinct().OrderBy(x => x).ToArray();
                    if (!to.Any()) return;

                    var cc = _cc.Except(_exclusions).Distinct().OrderBy(x => x).ToArray();
                    var bcc = _bcc.Except(_exclusions).Distinct().OrderBy(x => x).ToArray();

                    var subject = _subject();
                    var body = _body();

                    var message = new MailMessage
                        {
                            IsBodyHtml = true,
                            Priority = _priority,
                            From = new MailAddress(_fromAddress),
                            Subject = subject,
                            Body = body
                        };
                    foreach (var address in to)
                        message.To.Add(address);
                    foreach (var address in cc)
                        message.CC.Add(address);
                    foreach (var address in bcc)
                        message.Bcc.Add(address);
                    if (!string.IsNullOrWhiteSpace(_replyTo))
                        message.ReplyToList.Add(_replyTo);

                    var client = new SmtpClient("somewebserver", 12324);

                    const string path = @"c:\temp\cheeseshop";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    client.PickupDirectoryLocation = path;
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

                    client.Send(message);
                }
                catch (Exception e)
                {
                    // TODO LOG the error
                    throw;
                }
            }
        }

        public IFluentEmail Prepare()
        {
            return new FluentEmail();
        }
    }
}