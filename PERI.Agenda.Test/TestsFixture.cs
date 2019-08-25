using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PERI.Agenda.Test
{
    [CollectionDefinition("MyCollection")]
    public class CollectionClass : ICollectionFixture<TestsFixture> { }

    /// <summary>
    /// Initializes AutoMapper and prevents it from having initialization error
    /// <see href="https://stackoverflow.com/questions/12976319/xunit-net-global-setup-teardown"/>
    /// </summary>
    public class TestsFixture : IDisposable
    {
        public TestsFixture()
        {
            // Do "global" initialization here; Called before every test method.

            // Reset Mapper
            // https://github.com/AutoMapper/AutoMapper/issues/2607
            //Mapper.Reset();

            // Initialize Mapper
            // https://stackoverflow.com/questions/14108080/automapper-enum-to-byte-with-implemention-imapperconfigurator/14150006#14150006
            //Mapper.Initialize(m => m.AddProfile<Web.AutoMapperProfileConfiguration>());
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.

            //Mapper.Reset();
        }
    }
}
