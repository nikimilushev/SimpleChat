using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleChat.UI
{
    public static class SimpleChatCommands
    {
        public static RoutedCommand JoinCommand = new RoutedCommand();
        public static RoutedCommand LeaveCommand = new RoutedCommand();
        public static RoutedCommand SendCommand = new RoutedCommand();
    }
}
