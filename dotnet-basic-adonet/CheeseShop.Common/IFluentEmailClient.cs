using System;
using System.Net.Mail;

namespace CheeseShop.Common
{
    public interface IFluentEmailClient
    {
        IFluentEmail Prepare();
    }

    public interface IFluentEmail
    {
        IFluentEmail From(string fromAddress);
        IFluentEmail To(params string[] to);
        IFluentEmail Cc(params string[] cc);
        IFluentEmail Bcc(params string[] bcc);
        IFluentEmail Exclude(params string[] exclusions);
        IFluentEmail ReplyTo(string replyTo);
        IFluentEmail Priority(MailPriority priority);
        IFluentEmail Subject(string subject, params object[] args);
        IFluentEmail Subject(Func<string> subject);
        IFluentEmail Body(string body, params object[] args);
        IFluentEmail Body(Func<string> body);
        void Send();
        void SendAsync();
    }
}