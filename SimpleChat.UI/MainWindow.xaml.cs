using SimpleChat.Client;
using SimpleChat.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimpleChat.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private vars
        private string _serverAddress;
        private Participant _participant;
        private ChatChannel _channel;
        private Thread _workerThread;
        private Thread _mainThread;
        private bool _stopUpdateMessages;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            _serverAddress = ConfigurationManager.AppSettings["ServerAddress"];

            lblServerAddress.Text = _serverAddress;

            _channel = new ChatChannel(_serverAddress);

            _mainThread = Thread.CurrentThread;
        }

        private void AddToHistory(string message)
        {
            if (string.IsNullOrEmpty(message) == false)
            {
                History.AppendText(string.Format("{0}\n", message));
                History.ScrollToEnd();
            }
        }
        
        #region join,leave,send commands implementation
        
        private void ExecutedJoinCommand(object sender,   ExecutedRoutedEventArgs e)
        {
            JoinChat();
        }

        private void CanExecuteJoinCommand(object sender,   CanExecuteRoutedEventArgs e)
        {
            if(_participant == null && Nickname.Text!="")
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void ExecutedLeaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            LeaveChat();
        }

        private void CanExecuteLeaveCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_participant != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void ExecutedSendCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SendMessage();
        }

        private void CanExecuteSendCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_participant != null && Message.Text!="")
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        #endregion

        private void JoinChat()
        {
            _participant = new Participant(Nickname.Text);

            var result = _channel.Join(_participant);

            AddToHistory(result.Message);

            if (result.Error)
            {
                _participant = null;
            }
            else
            {
                UpdateMessages();
            }
        }

        private void LeaveChat()
        {
            var result = _channel.Kick(_participant);

            AddToHistory(result.Message);

            _participant = null;

            _stopUpdateMessages = true;
        }

        private void UpdateMessages()
        {
            _workerThread = new Thread(() =>
            {
                if (_participant != null && !_stopUpdateMessages)
                {
                    var receiveResult = _channel.Receive(_participant);

                    if(!string.IsNullOrEmpty(receiveResult.Message))
                    { 
                        Dispatcher.Invoke((Action)(() =>
                                    {
                                            AddToHistory(receiveResult.Message);
                                    }));
                    }

                    if (!_stopUpdateMessages)
                    {
                        UpdateMessages();
                    }
                }
            });
            _workerThread.Start();
        }

        private void SendMessage()
        {
            var message = new SimpleChat.Model.MessageParameter{
                Content=Message.Text,
                Sender=_participant.Nickname
            };

            var result = _channel.Send(message);

            AddToHistory(result.Message);

            Message.Text = "";
        }       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SimpleChatCommands.LeaveCommand.Execute(this, null);
        }
    }
}
