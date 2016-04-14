using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.ChatServer
{
    public class AuditingChat : ChatBase
    {
        #region private vars
        private readonly AuditRepository _auditRepository;
        private readonly ChatBase _chat;
        #endregion 

        public AuditingChat(AuditRepository auditRepository) 
        {
            _auditRepository = auditRepository;
            _chat = new Chat();
        }

        public ChatResult Add(Participant participantToAdd)
        {
            var result = _chat.Add(participantToAdd);
            if (result.Error == false)
            {
                _auditRepository.AuditJoin(_chat.GetParticipant(participantToAdd.Nickname));
            }
            return result;
        }

        public ChatResult GetMessage(Participant participantToGet)
        {
            return _chat.GetMessage(participantToGet);
        }

        public void Init()
        {
            _chat.Init();
        }

        public ChatResult Remove(Participant participantToRemove)
        {
            var participant = _chat.GetParticipant(participantToRemove.Nickname);
            if (participant != null)
            {
                _auditRepository.AuditKick(participant);
            }

            var result =  _chat.Remove(participantToRemove);
            if (result.Error == false)
            {
                
            }
            return result;
        }

        public ChatResult SendMessage(MessageParameter message)
        {
            var result = _chat.SendMessage(message);
            if (result.Error == false)
            {
                _auditRepository.AuditSendMessage(message);
            }
            return result;
        }
        
        public Participant GetParticipant(string nickname)
        {
           return _chat.GetParticipant(nickname);
        }
    }
}
