using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Test
{
    class DateTimeProvider
    {
        private readonly DateTime _testTime;
        
        private static readonly DateTimeProvider instance = new DateTimeProvider();

        private DateTimeProvider() 
        { 
            _testTime = DateTime.Now;
        }

        public static DateTimeProvider Instance
        {
            get
            {
                return instance;
            }
        }

        public DateTime GetTime()
        {
            return _testTime;    
        }
    }
}
