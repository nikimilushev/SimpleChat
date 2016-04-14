using Newtonsoft.Json;
using NUnit.Framework;
using SimpleChat.Client;
using SimpleChat.DI;
using SimpleChat.Model;
using SimpleChat.Server;
using SimpleChat.ChatServer;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleChat.Test
{
    [TestFixture]
    class ChatServiceTest
    {
        #region private vars
        private const string BASE_ADDRESS = "http://localhost:1234/ChatService/";        
        private const string NICK1 = "shrek";
        private const string NICK2 = "fiona";
        private const string TEST_MESSAGE = @"Hello my friend!";

        private ServiceRunner _server;
        private EventWaitHandle _serverStarted;
        #endregion private

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ObjectFactory.ReconfigureContainer(testContainer);
        }

        private static Container testContainer()
        {
            var container = new Container(_ =>
            {
                _.For<AuditRepository>().Singleton().Use<TestAuditRepository>();
                _.For<ChatBase>().Singleton().Use<AuditingChat>();
            });

            return container;
        }

        private ChatBase _chat;

        [SetUp]
        public void SetUp()
        {
            _chat = ObjectFactory.Container.GetInstance<ChatBase>();

            _chat.Init();

            _serverStarted = new AutoResetEvent(false);

            _server = new ServiceRunner(_serverStarted);
            
            _server.StartService();

            _serverStarted.WaitOne();
        }

        [TearDown]
        public void TearDown()
        {
            _server.StopService();
        }
                
        [Test]
        public void TestOneParticipantJoinSuccess()
        {
            var participant = new Participant(NICK1);
            
            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);

            var expected = string.Format(ConstantStrings.HELLO_MESSAGE_FORMAT, NICK1);

            Assert.IsFalse(result.Error);

            Assert.AreEqual(expected, result.Message);
        }

        [Test]
        public void TestSecondParticipantJoinFailIfExists()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            chatChannel.Join(participant);

            var result = chatChannel.Join(participant);

            var expected = string.Format(ConstantStrings.JOIN_ERROR_MESSAGE_FORMAT, NICK1);

            Assert.AreEqual(expected, result.Message);

            Assert.IsTrue(result.Error);
        }
        
        [Test]
        public void TestSecondParticipantJoinSucceedsIfNotExists()
        {            
            var participant1 = new Participant(NICK1);

            var participant2 = new Participant(NICK2);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant1);

            var expected = string.Format(ConstantStrings.HELLO_MESSAGE_FORMAT, NICK1);

            Assert.AreEqual(expected, result.Message);

            result = chatChannel.Join(participant2);

            expected = string.Format(ConstantStrings.HELLO_MESSAGE_FORMAT, NICK2);
        }

        [Test]
        public void TestOneParticipantJoinsThenLeaves()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);

            var expected = string.Format(ConstantStrings.HELLO_MESSAGE_FORMAT, NICK1);

            Assert.AreEqual(expected, result.Message);

            result = chatChannel.Kick(participant);

            expected = string.Format(ConstantStrings.BYE_MESSAGE_FORMAT, NICK1);

            Assert.AreEqual(expected, result.Message);
        }
        
        [Test]
        public void TestOneParticipantJoinsThenLeavesTwiceFails()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);
                        
            result = chatChannel.Kick(participant);

            result = chatChannel.Kick(participant);

            var expected = string.Format(ConstantStrings.MISSING_USER_ERROR_MESSAGE_FORMAT, NICK1);

            Assert.AreEqual(expected, result.Message);

            Assert.IsTrue(result.Error);
        }

        [Test]
        public void TestOneParticipantJoinsSecondPartcipantJoinsAndFirstOneReceivesNotification()
        {
            var participant1 = new Participant(NICK1);

            var participant2 = new Participant(NICK2);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result1 = chatChannel.Join(participant1);

            var result2 = chatChannel.Join(participant2);

            var result3 = chatChannel.Receive(participant1);

            var expected = string.Format(ConstantStrings.OTHER_USER_JOINED_MESSAGE_FORMAT, NICK2);

            Assert.AreEqual(expected, result3.Message);
        }

        [Test]
        public void TestOneParticipantJoinsSecondPartcipantJoinsThenLeavesAndFirstOneReceivesNotification()
        {
            var participant1 = new Participant(NICK1);

            var participant2 = new Participant(NICK2);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result1 = chatChannel.Join(participant1);

            var result2 = chatChannel.Join(participant2);

            var result3 = chatChannel.Kick(participant2);

            var result4 = chatChannel.Receive(participant1); //has joined

            var result5 = chatChannel.Receive(participant1);  //has left

            var expected = string.Format(ConstantStrings.OTHER_USER_LEFT_MESSAGE_FORMAT, NICK2);

            Assert.AreEqual(expected, result5.Message);
        }

        [Test]
        public void TestOneParticipantsSendsMessageOtherRecives()
        {
            var participant1 = new Participant(NICK1);

            var participant2 = new Participant(NICK2);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result1 = chatChannel.Join(participant1);

            var result2 = chatChannel.Join(participant2);

            var message = new MessageParameter
            {
                Content = TEST_MESSAGE,
                Sender = NICK2
            };

            var result3 = chatChannel.Send(message);

            var result4 = chatChannel.Receive(participant1); //has joined

            var result5 = chatChannel.Receive(participant1);  //Hi!

            var expected = string.Format("{0}: {1}", NICK2, TEST_MESSAGE);

            Assert.AreEqual(expected, result5.Message);

            Assert.AreEqual(expected, result3.Message);            
        }

        [Test]
        public void TestParticipantJoinAudit()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);

            var expected = string.Format(TestFormattingStrings.JOIN_SESSION_AUDIT_FORMAT, participant.Nickname, DateTimeProvider.Instance.GetTime());

            var actual = FakeSessionTable.LastAuditRecord;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestParticipantLeftAudit()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);

            result = chatChannel.Kick(participant);

            var expected = string.Format(TestFormattingStrings.KICK_SESSION_AUDIT_FORMAT, participant.Nickname, DateTimeProvider.Instance.GetTime());

            var actual = FakeSessionTable.LastAuditRecord;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestParticipantSendMessageAudit()
        {
            var participant = new Participant(NICK1);

            var chatChannel = new ChatChannel(BASE_ADDRESS);

            var result = chatChannel.Join(participant);

            var message = new MessageParameter
            {
                Content = "Hello",
                Sender = NICK1
            };

            result = chatChannel.Send(message);

            var expected = string.Format(TestFormattingStrings.SEND_MESSAGE_AUDIT_FORMAT, participant.Nickname, message.Content, DateTimeProvider.Instance.GetTime());

            var actual = FakeMessageTable.LastAuditRecord;

            Assert.AreEqual(expected, actual);
        }
    }
}
