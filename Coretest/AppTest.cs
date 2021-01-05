using System;
using System.Collections.Generic;
using Core;
using Database;
using MessageBroker;
using Models;
using Moq;
using Xunit;

namespace CoreTest {
    public class AppTest {
      

        [Fact]
        public void RunTest() {
            var app = new Mock<IApp>();
            var interpreterPath = "python";
            var interpreterPath2 = "ruby";
            var interpreterPath3 = "node";
          
            int count = 15;
            //Act
            app.Setup(x => x.Run(interpreterPath, count)).Returns(0);
            app.Setup(x => x.Run(interpreterPath2, count)).Returns(0);
            app.Setup(x => x.Run(interpreterPath3, count)).Returns(0);
            //Assert
            Assert.Equal(0, app.Object.Run(interpreterPath, count));
            Assert.Equal(0, app.Object.Run(interpreterPath2, count));
            Assert.Equal(0, app.Object.Run(interpreterPath3, count));
        }
    }

 }