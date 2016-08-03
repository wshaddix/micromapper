using System;
using System.Collections.Generic;

namespace MicroMapper.Tests.ViewModels
{
    public class PersonViewModel
    {
        public List<string> Aliases { get; set; }
        public string FullName { get; set; }
        public int HowOld { get; set; }
        public bool Preferred { get; set; }
        public string SomeProperty { get; set; }
        public DateTime WhenCreated { get; set; }
    }
}