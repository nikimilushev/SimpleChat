using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Model
{
    public interface BaseChannel
    {
        ChatResult Join(Participant participant);

        ChatResult Kick(Participant participant);

        ChatResult Receive(Participant participant);

        ChatResult Send(MessageParameter message);
    }
}
