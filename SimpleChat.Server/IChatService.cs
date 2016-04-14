using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimpleChat.Server
{
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "Join", Method = "POST")]
        ChatResult Join(Participant participant);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "Kick", Method = "POST")]
        ChatResult Kick(Participant participant);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "Receive", Method = "POST")]
        ChatResult Receive(Participant participant);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "Send", Method = "POST")]
        ChatResult Send(MessageParameter message);
    }
}
