using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Test
{
    public static class TestFormattingStrings
    {
        public static readonly string JOIN_SESSION_AUDIT_FORMAT = "JOINED: UserName: {0}, timestamp: {1:MM/dd/yy H:mm:ss zzz}";
        public static readonly string KICK_SESSION_AUDIT_FORMAT = "LEFT: UserName: {0}, timestamp: {1:MM/dd/yy H:mm:ss zzz}";
        public static readonly string SEND_MESSAGE_AUDIT_FORMAT = "SEND_MESSAGE: UserName: {0}, Content: {1} timestamp: {2:MM/dd/yy H:mm:ss zzz}";

    }
}
