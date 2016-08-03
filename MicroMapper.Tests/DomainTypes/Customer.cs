using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMapper.Tests.DomainTypes
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int AgeInYears { get; set; }

        public bool IsPreferredMember { get; set; }

        public List<string> Nicknames { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
