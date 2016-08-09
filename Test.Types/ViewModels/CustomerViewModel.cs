using System;
using System.Collections.Generic;

namespace Test.Types.ViewModels
{
    public class CustomerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int AgeInYears { get; set; }

        public bool IsPreferredMember { get; set; }

        public List<string> Nicknames { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
