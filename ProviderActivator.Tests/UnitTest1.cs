using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProviderActivators;

namespace ProviderActivators.Tests
{
    [TestClass]
    public class ActivateProvider
    {
        [TestMethod]
        public void TestMethod1()
        {
            var settings = new ProviderSettings("sqlProvider", typeof(SqlMembershipProvider).AssemblyQualifiedName);
            settings.Parameters.Add( "connectionStringName","localserver");
            var membershipProvider = ProviderActivator.InstanciateProvider(settings, typeof (MembershipProvider));
            Assert.IsNotNull(membershipProvider);
            Assert.AreEqual("sqlProvider", membershipProvider.Name);
        }
    }
}
