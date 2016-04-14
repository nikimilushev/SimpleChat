using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleChat.Model
{
    [DataContract]
    public class ChatResult
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool Error { get; set; }
    }
}
