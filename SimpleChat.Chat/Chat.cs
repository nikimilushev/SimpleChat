using SimpleChat.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SimpleChat.ChatServer
{    
    public class Chat : ChatBase
    {
        #region private vars
        private ConcurrentDictionary<string, Participant> _participants;
        #endregion private vars

        public Chat() 
        {
            Init();
        }

        public void Init()
        {
            _participants = new ConcurrentDictionary<string, Participant>();
        }

        public Participant GetParticipant(string nickname)
        {
            Participant participantToGet;
            if (_participants.TryGetValue(nickname, out participantToGet))
            {
                return participantToGet;
            }
            return null;
        }

        private ChatResult SendMessageToOthers(MessageParameter message)
        {
            var result = new ChatResult();
            
            var messageText = message.IsSystemMessage ? string.Format("{0}", message.Content):string.Format("{0}: {1}", message.Sender, message.Content);

            foreach (KeyValuePair<string, Participant> entry in _participants)
            {
                if (entry.Key != message.Sender)
                {
                    entry.Value.NewMessage(messageText);
                }
            }
            result.Message = messageText;
            return result;
        }

        private ChatResult GetNewMessage(Participant participantToCheck)
        {
            Participant participant = GetParticipant(participantToCheck.Nickname);

            if (participant == null)
            {
                throw new ArgumentException("participant");
            }

            var timeout = new TimeSpan(0, 0, 15);

            var wait = new EventWaitHandle(false, EventResetMode.ManualReset);

            EventHandler waiter = (s, e) => wait.Set();

            participant.GotNewMessage += waiter;

            if (participant.HasNewMessages() == false)
            {
                wait.WaitOne(timeout);
            }

            participant.GotNewMessage -= waiter;

            return new ChatResult
            {
                Message = participant.GetFirstNewMessage()
            };
        }

        public ChatResult Add(Participant participantToAdd)
        {
            Participant participant = GetParticipant(participantToAdd.Nickname);
            if (participant == null) 
            {
                var message = new MessageParameter{
                    Content = string.Format(ConstantStrings.OTHER_USER_JOINED_MESSAGE_FORMAT, participantToAdd.Nickname),
                    Sender = participantToAdd.Nickname,
                    IsSystemMessage = true
                };

                _participants.TryAdd(participantToAdd.Nickname, new Participant(participantToAdd.Nickname));
                
                SendMessageToOthers(message);
                         
                return new ChatResult
                {
                    Message = string.Format(ConstantStrings.HELLO_MESSAGE_FORMAT, participantToAdd.Nickname)
                };

            }
            else 
            {
                return new ChatResult
                {
                    Message = string.Format(ConstantStrings.JOIN_ERROR_MESSAGE_FORMAT, participantToAdd.Nickname),
                    Error = true
                };
            }
        }

        public ChatResult Remove(Participant participantToRemove)
        {
            Participant participant = GetParticipant(participantToRemove.Nickname);
            if (participant != null)
            {
                var message = new MessageParameter
                {
                    Content = string.Format(ConstantStrings.OTHER_USER_LEFT_MESSAGE_FORMAT, participantToRemove.Nickname),
                    Sender = participantToRemove.Nickname,
                    IsSystemMessage = true
                };

                Participant dummy;

                _participants.TryRemove(participantToRemove.Nickname, out dummy);

                SendMessageToOthers(message);

                return new ChatResult
                {
                    Message = string.Format(ConstantStrings.BYE_MESSAGE_FORMAT, participantToRemove.Nickname)
                };

            }
            else
            {
                return new ChatResult
                {
                    Message = string.Format(ConstantStrings.MISSING_USER_ERROR_MESSAGE_FORMAT, participantToRemove.Nickname),
                    Error = true
                };
            }            
        }

        public ChatResult GetMessage(Participant participantToGet)
        {
            Participant participant = GetParticipant(participantToGet.Nickname);
            if (participant!=null)
            {
                return GetNewMessage(participant);                
            }
            else
            {
                return new ChatResult
                {
                    Message = string.Format(ConstantStrings.MISSING_USER_ERROR_MESSAGE_FORMAT, participant.Nickname),
                    Error = true
                };
            }
        }

        public ChatResult SendMessage(MessageParameter message)
        {
            Participant participant = GetParticipant(message.Sender);
            if (participant != null) 
            {
                return SendMessageToOthers(message);
            }
            else
            {
                return new ChatResult{
                    Message = string.Format(ConstantStrings.MISSING_USER_ERROR_MESSAGE_FORMAT, message.Sender),
                    Error = true
                };
            }
        }
    }
}