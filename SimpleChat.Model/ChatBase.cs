using System;
namespace SimpleChat.Model
{
    public interface ChatBase
    {
        void Init();

        ChatResult Add(Participant participantToAdd);

        ChatResult Remove(Participant participantToRemove);

        ChatResult SendMessage(MessageParameter message);

        ChatResult GetMessage(Participant participantToGet);
        
        Participant GetParticipant(string nickname);
    }
}
