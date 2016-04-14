using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Model
{
    public static class ConstantStrings
    {
        public static readonly string HELLO_MESSAGE_FORMAT = "Hello {0}!";
        public static readonly string BYE_MESSAGE_FORMAT = "Bye {0}!";
        public static readonly string JOIN_ERROR_MESSAGE_FORMAT = "Error: User {0} already exists!";
        public static readonly string MISSING_USER_ERROR_MESSAGE_FORMAT = "Error: User {0} does not exist!";
        public static readonly string OTHER_USER_JOINED_MESSAGE_FORMAT = "User {0} has joined!";
        public static readonly string OTHER_USER_LEFT_MESSAGE_FORMAT = "User {0} has left!";

        public static readonly string JOIN = "join";
        public static readonly string KICK = "kick";
    }
}
