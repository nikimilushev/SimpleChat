using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace SimpleChat.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                 ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {
        private ChatBase _chat;

        public ChatService(ChatBase chat)
        {
            _chat = chat;
        }

        public ChatResult Join(Participant participant)
        {
            var result = _chat.Add(participant);
            return result;
        }

        public ChatResult Kick(Participant participant)
        {
            var result = _chat.Remove(participant);            
            return result;
        }

        public ChatResult Receive(Participant participant)
        {
            return _chat.GetMessage(participant);            
        }

        public ChatResult Send(MessageParameter message)
        {
            var result = _chat.SendMessage(message);
            return result;
        }
    }
}
