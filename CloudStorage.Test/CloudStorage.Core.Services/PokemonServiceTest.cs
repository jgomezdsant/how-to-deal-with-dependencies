using CloudStorage.Core;
using CloudStorage.Core.Model.DAL;
using CloudStorage.Core.Services;
using CloudStorageAPI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Test.CloudStorage.Core.Services
{
    [TestFixture]
    public class PokemonServiceTest
    {
        private PokemonService _service;
        private Mock<IWebHostEnvironment> _envMock = new();
        private Mock<IConfiguration> _confMock = new();
        private Mock<IConfigurationSection> _confConnectionStringSectionMock = new();
        private Mock<IConfigurationSection> _confContainerSectionMock = new();
        private Mock<IPokemonRepository> _repoMock = new();
        private Mock<IAzureStorageManager> _storageManager = new();

        [SetUp]
        public void Setup()
        {
            _confConnectionStringSectionMock.Setup(c => c.Value).Returns("DefaultEndpointsProtocol=https;AccountName=pokemonstorageaccount;AccountKey=s2NZfDImLBoDShvhqa+XmGcYW3bt6CSvRdbgp/TOLPJxx+/GPi69kyKmQG2n9n7GykyddyDY5RVf+AStv20bOw==;EndpointSuffix=core.windows.net");
            _confContainerSectionMock.Setup(c => c.Value).Returns("pokemon-container-test");
            _confMock.Setup(c => c.GetSection("Azure:StorageConnectionString")).Returns(_confConnectionStringSectionMock.Object);
            _confMock.Setup(c => c.GetSection("Azure:ContainerName")).Returns(_confContainerSectionMock.Object);
            _service = new PokemonService(_repoMock.Object, _envMock.Object, _confMock.Object, _storageManager.Object);
        }

        [Test]
        public void AddPokemon()
        {
            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.Setup(c => c.FileName).Returns("a-file.png");
            var pokemonObj = new PokemonUpsertDAL { Name = "Pokemon", Photo = mockIFormFile.Object};
            var result = _service.AddAsync(pokemonObj);
            Assert.IsNotNull(result);
        }

    }
}
