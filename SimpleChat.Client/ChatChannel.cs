using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using SimpleChat.Model;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace SimpleChat.Client
{
    public class ChatChannel : BaseChannel
    {
        #region private vars
        private string _baseAddress;
        private readonly string _joinAddress;
        private readonly string _kickAddress;
        private readonly string _receiveAddress;
        private readonly string _sendAddress;
        #endregion private vars

        public ChatChannel(string baseAddress)
        {
            _baseAddress = baseAddress;
            _joinAddress = string.Format("{0}Join",_baseAddress);
            _kickAddress = string.Format("{0}Kick", _baseAddress);
            _receiveAddress = string.Format("{0}Receive", _baseAddress);
            _sendAddress = string.Format("{0}Send", _baseAddress);
        }

        public ChatResult Join(Participant participant)
        {
            return ProcessSessionRequest<Participant>(_joinAddress, participant);            
        }

        public ChatResult Kick(Participant participant)
        {
            return ProcessSessionRequest<Participant>(_kickAddress, participant);
        }

        public ChatResult Receive(Participant participant)
        {
            return ProcessSessionRequest<Participant>(_receiveAddress, participant);
        }

        public ChatResult Send(MessageParameter message)
        {
            return ProcessSessionRequest<MessageParameter>(_sendAddress, message);
        }

        private ChatResult ProcessSessionRequest<T>(string address, T payload)
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync<T>(address, payload).Result;

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<ChatResult>().Result;
            }
        }
    }
}
