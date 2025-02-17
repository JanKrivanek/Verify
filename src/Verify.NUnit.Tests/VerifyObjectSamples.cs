﻿using Argon;

// Non-nullable field is uninitialized
#pragma warning disable CS8618

[TestFixture]
public class VerifyObjectSamples
{
    async Task ChangeDefaultsPerVerification(object target)
    {
        #region ChangeDefaultsPerVerification

        await Verify(target)
            .DontIgnoreEmptyCollections()
            .DontScrubGuids()
            .DontScrubDateTimes();

        #endregion
    }

    [Test]
    public Task ScopedSerializer()
    {
        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Dob = new(2000, 10, 1, 0, 0, 0, TimeSpan.Zero)
        };

        var settings = new VerifySettings();
        settings.DontScrubDateTimes();
        settings.AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);
        return Verify(person, settings);
    }

    [Test]
    public Task ScopedSerializerFluent()
    {
        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Dob = new(2000, 10, 1, 0, 0, 0, TimeSpan.Zero)
        };

        return Verify(person)
            .DontScrubDateTimes()
            .AddExtraSettings(_ => _.DefaultValueHandling = DefaultValueHandling.Include);
    }

    async Task Before()
    {
        #region Before

        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Spouse = "Jill",
            Address = new()
            {
                Street = "1 Puddle Lane",
                Country = "USA"
            }
        };

        await Verify(person);

        #endregion
    }

    #region AnonNUnit

    [Test]
    public Task Anon()
    {
        var person1 = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith"
        };
        var person2 = new Person
        {
            GivenNames = "Marianne",
            FamilyName = "Aguirre"
        };

        return Verify(
            new
            {
                person1,
                person2
            });
    }

    #endregion

    async Task After()
    {
        #region After

        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Spouse = "Jill",
            Address = new()
            {
                Street = "1 Puddle Lane",
                Suburb = "Gotham",
                Country = "USA"
            }
        };

        await Verify(person);

        #endregion
    }

    class Person
    {
        public string GivenNames;
        public string FamilyName;
        public string Spouse;
        public Address Address;
        public DateTimeOffset Dob;
    }

    class Address
    {
        public string Street;
        public string Country;
        public string Suburb;
    }
}