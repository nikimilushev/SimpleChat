using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Test
{
    public class TestAuditRepository : AuditRepository
    {
        public void AuditJoin(Participant participant)
        {
            FakeSessionTable.LastAuditRecord = string.Format(TestFormattingStrings.JOIN_SESSION_AUDIT_FORMAT, participant.Nickname, DateTimeProvider.Instance.GetTime());
        }

        public void AuditKick(Participant participant)
        {
            FakeSessionTable.LastAuditRecord = string.Format(TestFormattingStrings.KICK_SESSION_AUDIT_FORMAT, participant.Nickname, DateTimeProvider.Instance.GetTime());
        }

        public void AuditSendMessage(MessageParameter message)
        {
            FakeMessageTable.LastAuditRecord = string.Format(TestFormattingStrings.SEND_MESSAGE_AUDIT_FORMAT, message.Sender, message.Content, DateTimeProvider.Instance.GetTime());
        }
    }
}
