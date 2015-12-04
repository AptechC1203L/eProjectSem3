using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorForums.Models
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
    }

    public class Topic
    {
        public string Title { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }

    public class Message
    {

    }
}
