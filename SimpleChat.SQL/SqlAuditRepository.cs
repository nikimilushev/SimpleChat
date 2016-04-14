using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.SQL
{
    public class SqlAuditRepository : AuditRepository
    {
        private SimpleChatEntities _entities;
        private object _lockEntities = new object();

        public SqlAuditRepository()
        {
            _entities = new SimpleChatEntities();
            _entities.Configuration.ProxyCreationEnabled = false;
        }

        public void AuditJoin(Participant participant)
        {
            var auditJoin = new Session
            {
                Nickname = participant.Nickname,
                Event = ConstantStrings.JOIN
            };

            lock (_lockEntities)
            {
                _entities.Session.Add(auditJoin);
                _entities.SaveChanges();
            }
        }

        public void AuditKick(Participant participant)
        {
            var auditKick = new Session
            {
                Nickname = participant.Nickname,
                Event = ConstantStrings.KICK
            };

            lock (_lockEntities)
            {
                _entities.Session.Add(auditKick);
                _entities.SaveChanges();
            }
        }

        public void AuditSendMessage(MessageParameter message)
        {
            var auditMessage = new Message{
                Sender = message.Sender,
                Content = message.Content
            };

            lock (_lockEntities)
            {
                _entities.Message.Add(auditMessage);
                _entities.SaveChanges();
            }
        }
    }
    public abstract class BaseDomainContext : DbContext
    {
        static BaseDomainContext()
        {
            // This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
