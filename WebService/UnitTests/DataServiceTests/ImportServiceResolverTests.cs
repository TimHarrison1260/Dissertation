using Core.Interfaces.Services;
using Infrastructure.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.DataServiceTests
{
    [TestClass]
    public class ImportServiceResolverTests
    {

        private Mock<ISnhImportService> _mockSnhImportService = null;
        private Mock<IRenUkImportService> _mockRenUkImportService = null;

        [TestInitialize]
        public void Initialise()
        {
            //  Create mock instances of the Import service being resolved to 
            //  avoid having to create all the dependencies.
            _mockSnhImportService = new Mock<ISnhImportService>();
            _mockRenUkImportService = new Mock<IRenUkImportService>();

        }


        [TestMethod]
        [TestCategory("ImportServiceResolver")]
        public void Resolve_OK_ReturnsSnhForId1()
        {
            //  Arrange
            var resolver = new ImportServiceResolver(_mockSnhImportService.Object, _mockRenUkImportService.Object);

            //  Act
            var result = resolver.Resolve(1);

            //  Assert
            Assert.AreSame(_mockSnhImportService.Object, result, "Expected the Snh instance to be returned");
        }

        [TestMethod]
        [TestCategory("ImportServiceResolver")]
        public void Resolve_OK_ReturnsRenUkForId2()
        {
            //  Arrange
            var resolver = new ImportServiceResolver(_mockSnhImportService.Object, _mockRenUkImportService.Object);

            //  Act
            var result = resolver.Resolve(2);

            //  Assert
            Assert.AreSame(_mockRenUkImportService.Object, result, "Expected the RenUk instance to be returned");
        }

        [TestMethod]
        [TestCategory("ImportServiceResolver")]
        public void Resolve_Not_OK_ReturnsNullForId3()
        {
            //  Arrange
            var resolver = new ImportServiceResolver(_mockSnhImportService.Object, _mockRenUkImportService.Object);

            //  Act
            var result = resolver.Resolve(3);

            //  Assert
            Assert.IsNull(result, "Expected a NULL instance to be returned, Id 3 does not exists as a valid datasource");
        }
    }
}
