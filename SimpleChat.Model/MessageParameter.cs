using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SimpleChat.Model
{
    [DataContract]
    public class MessageParameter
    {
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public string Sender { get; set; }
        [DataMember]
        public bool   IsSystemMessage { get; set; }
    }
}
