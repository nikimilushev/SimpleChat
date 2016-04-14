using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Model
{
    public interface AuditRepository
    {
        void AuditJoin(Participant participant);

        void AuditKick(Participant participant);

        void AuditSendMessage(MessageParameter message);
    }
}
