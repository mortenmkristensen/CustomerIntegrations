using System;
using Core;
using Database;
using MessageBroker;
using Moq;
using Xunit;

namespace CoreTest {
    public class AppTest {
        private readonly App app;
        private readonly Mock<IStager> stagerMock;
        private readonly Mock<IScriptRunner> scriptRunnerMock;
        private readonly Mock<IDBAccess> dbAccessMock;
        private readonly Mock<IMessageBroker> messageBrokerMock;
        private readonly Mock<IDataValidation> dataValidationMock;
        public AppTest() {
            app = new App(dbAccessMock.Object, stagerMock.Object, scriptRunnerMock.Object, messageBrokerMock.Object, dataValidationMock.Object);
        }

        [Fact]
        public void RunTest() {
            //Arrange 
            var interpreterPath = "Python";
            var interpreterPath2 = "Ruby";
            var interpreterPath3 = "node";
            int count = 15;
            //Act
            int i = app.Run(interpreterPath, count);
            int i2 = app.Run(interpreterPath2, count);
            int i3 = app.Run(interpreterPath3, count);
            //Assert
            Assert.Equal(0, i);
            Assert.Equal(0, i2);
            Assert.Equal(0, i3);
        }
    }

 }