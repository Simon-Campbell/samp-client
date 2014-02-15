using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAMP.Client.Data.Queries;
using SAMP.Client.Data.Models;
using System.Collections.Generic;

namespace SAMP.Client.Services.Test
{
    [TestClass]
    public class ServerDiscoveryServiceTest
    {
        List<Server> _servers;

        [TestInitialize]
        public void Setup()
        {
            _servers = new List<Server>
            {

            };
        }

        [TestMethod]
        public void TestGetsAllServers()
        {
            var queryMock 
                = new Mock<IServerListQuery>();
            var configMock
                = new Mock<IConfigurationService>();

            queryMock
                .Setup(x => x.All())
                .Returns(() => _servers);

            var service = new ServerDiscoveryService(queryMock.Object, configMock.Object);

            Assert.AreEqual(_servers, service.GetServers());
        }

        public void TestGetsFavourites()
        {
        }
    }
}
