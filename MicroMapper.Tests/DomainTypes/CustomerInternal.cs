using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMapper.Tests.DomainTypes
{
    internal class CustomerInternal
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }

        internal int AgeInYears { get; set; }

        internal bool IsPreferredMember { get; set; }

        internal List<string> Nicknames { get; set; }

        internal DateTime CreatedOnUtc { get; set; }
    }
}
