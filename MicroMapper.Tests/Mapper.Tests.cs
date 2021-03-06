﻿using MicroMapper.Tests.DomainTypes;
using System;
using System.Collections.Generic;
using Test.Types.DomainTypes;
using Test.Types.ViewModels;
using Xunit;

namespace MicroMapper.Tests
{
    public class MapperTests
    {
        [Fact]
        public void Can_Ignore_Properties()
        {
            var customer = new Customer
            {
                AgeInYears = 33,
                CreatedOnUtc = DateTime.UtcNow,
                FirstName = "First",
                IsPreferredMember = true,
                LastName = "Last",
                Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
            };

            var customerVm = new CustomerViewModel
            {
                AgeInYears = 0,
                FirstName = "First should be ignored"
            };

            var mapper = new Mapper<Customer, CustomerViewModel>(customer, customerVm);

            mapper
                .ReadOnlyPublicPropertiesFromSource()
                .Ignore(vm => vm.AgeInYears)
                .Ignore(vm => vm.FirstName)
                .Execute();

            Assert.True(customerVm.AgeInYears == 0, "AgeInYears should be 0");
            Assert.True(customer.CreatedOnUtc == customerVm.CreatedOnUtc, "CreatedOnUtc does not match");
            Assert.True(customerVm.FirstName == "First should be ignored", "FirstName should be 'First should be ignored'");
            Assert.True(customer.IsPreferredMember == customerVm.IsPreferredMember, "IsPreferredMember does not match");
            Assert.True(customer.LastName == customerVm.LastName, "LastName does not match");
            Assert.True(customer.Nicknames == customerVm.Nicknames, "Nicknames does not match");
        }

        [Fact]
        public void Can_Map_Properties_From_Internal_Source_To_Public_Destination()
        {
            var customer = new CustomerInternal
            {
                AgeInYears = 45,
                CreatedOnUtc = DateTime.UtcNow,
                FirstName = "First",
                IsPreferredMember = true,
                LastName = "Last",
                Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
            };

            var customerVm = new CustomerViewModel();
            var mapper = new Mapper<CustomerInternal, CustomerViewModel>(customer, customerVm);

            mapper.Execute();

            Assert.True(customer.AgeInYears == customerVm.AgeInYears, "AgeInYears does not match");
            Assert.True(customer.CreatedOnUtc == customerVm.CreatedOnUtc, "CreatedOnUtc does not match");
            Assert.True(customer.FirstName == customerVm.FirstName, "FirstName does not match");
            Assert.True(customer.IsPreferredMember == customerVm.IsPreferredMember, "IsPreferredMember does not match");
            Assert.True(customer.LastName == customerVm.LastName, "LastName does not match");
            Assert.True(customer.Nicknames == customerVm.Nicknames, "Nicknames does not match");
        }

        [Fact]
        public void Can_Map_Properties_From_Public_Source_To_Internal_Destination()
        {
            var customerVm = new CustomerViewModel
            {
                AgeInYears = 45,
                CreatedOnUtc = DateTime.UtcNow,
                FirstName = "First",
                IsPreferredMember = true,
                LastName = "Last",
                Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
            };

            var customer = new CustomerInternal();
            var mapper = new Mapper<CustomerViewModel, CustomerInternal>(customerVm, customer);

            mapper.Execute();

            Assert.True(customer.AgeInYears == customerVm.AgeInYears, "AgeInYears does not match");
            Assert.True(customer.CreatedOnUtc == customerVm.CreatedOnUtc, "CreatedOnUtc does not match");
            Assert.True(customer.FirstName == customerVm.FirstName, "FirstName does not match");
            Assert.True(customer.IsPreferredMember == customerVm.IsPreferredMember, "IsPreferredMember does not match");
            Assert.True(customer.LastName == customerVm.LastName, "LastName does not match");
            Assert.True(customer.Nicknames == customerVm.Nicknames, "Nicknames does not match");
        }

        [Fact]
        public void Can_Map_Properties_From_Public_Source_To_Public_Destination()
        {
            var customer = new Customer
            {
                AgeInYears = 45,
                CreatedOnUtc = DateTime.UtcNow,
                FirstName = "First",
                IsPreferredMember = true,
                LastName = "Last",
                Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
            };

            var customerVm = new CustomerViewModel();
            var mapper = new Mapper<Customer, CustomerViewModel>(customer, customerVm);

            mapper.Execute();

            Assert.True(customer.AgeInYears == customerVm.AgeInYears, "AgeInYears does not match");
            Assert.True(customer.CreatedOnUtc == customerVm.CreatedOnUtc, "CreatedOnUtc does not match");
            Assert.True(customer.FirstName == customerVm.FirstName, "FirstName does not match");
            Assert.True(customer.IsPreferredMember == customerVm.IsPreferredMember, "IsPreferredMember does not match");
            Assert.True(customer.LastName == customerVm.LastName, "LastName does not match");
            Assert.True(customer.Nicknames == customerVm.Nicknames, "Nicknames does not match");
        }

        [Fact]
        public void Can_Map_Properties_With_Public_Setter_With_Different_Name_From_Public_Source()
        {
            var customer = new Customer
            {
                AgeInYears = 45,
                CreatedOnUtc = DateTime.UtcNow,
                FirstName = "First",
                IsPreferredMember = true,
                LastName = "Last",
                Nicknames = new List<string> { "Nickname 1", "Nickname 2" }
            };

            var personVm = new PersonViewModel();
            var mapper = new Mapper<Customer, PersonViewModel>(customer, personVm);

            mapper
                .ReadOnlyPublicPropertiesFromSource()
                .MapProperty(vm => vm.HowOld, c => c.AgeInYears)
                .MapProperty(vm => vm.WhenCreated, c => c.CreatedOnUtc)
                .MapProperty(vm => vm.FullName, c => $"{customer.FirstName} {customer.LastName}")
                .MapProperty(vm => vm.Preferred, c => c.IsPreferredMember)
                .MapProperty(vm => vm.Aliases, c => c.Nicknames)
                .MapProperty(vm => vm.SomeProperty, c => "any value works")
                .Execute();

            Assert.True(personVm.HowOld == customer.AgeInYears, "AgeInYears does not match");
            Assert.True(personVm.WhenCreated == customer.CreatedOnUtc, "CreatedOnUtc does not match");
            Assert.True(personVm.FullName == $"{customer.FirstName} {customer.LastName}", "Name does not match");
            Assert.True(personVm.Preferred == customer.IsPreferredMember, "IsPreferredMember does not match");
            Assert.True(personVm.Aliases == customer.Nicknames, "Nicknames does not match");
            Assert.True(personVm.SomeProperty == "any value works", "SomeProperty does not match");
        }
    }
}