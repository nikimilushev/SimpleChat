using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleChat.Model
{
    [DataContract]
    public class Participant
    {
        private ConcurrentQueue<string> _newMessages;
        private object lockMessages = new object(); 

        [DataMember]
        public string Nickname { get; set; }

        public Participant(string nick)
        {
            Nickname = nick;
            _newMessages = new ConcurrentQueue<string>();
        }

        public void NewMessage(string message)
        {
            lock (lockMessages)
            {
                _newMessages.Enqueue(message);
            }

            OnGotNewMessage(EventArgs.Empty);
        }

        public string GetFirstNewMessage()
        {
            string result=null;
            if (HasNewMessages())
            {
                lock (lockMessages)
                {
                    if (!_newMessages.TryDequeue(out result))
                    {
                        result = "";
                    }
                }
            }
            return result;
        }

        public event EventHandler GotNewMessage;

        public void OnGotNewMessage(EventArgs e)
        {
            EventHandler handler = GotNewMessage;
            if (handler != null) handler(this, e);
        }

        public bool HasNewMessages()
        {
            return !_newMessages.IsEmpty;
        }
    }
}
