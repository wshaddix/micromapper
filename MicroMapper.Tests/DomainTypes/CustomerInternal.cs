using System;
using System.Collections.Generic;

namespace MicroMapper.Tests.DomainTypes
{
    internal class CustomerInternal
    {
        internal int AgeInYears { get; set; }
        internal DateTime CreatedOnUtc { get; set; }
        internal string FirstName { get; set; }
        internal bool IsPreferredMember { get; set; }
        internal string LastName { get; set; }
        internal List<string> Nicknames { get; set; }
    }
}