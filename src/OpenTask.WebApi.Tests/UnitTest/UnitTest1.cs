// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Moq;
using OpenTask.WebApi.Controllers;

namespace OpenTask.WebApi.Tests.UnitTest
{
    public class UserControllerTests
    {
        #region snippet_Index_ReturnsAViewResult_WithAListOfBrainstormSessions
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {

            // Arrange
            Mock<IMediator> mediator = new();

            //mediator.Setup(repo => repo.ListAsync())
            //    .ReturnsAsync(GetTestSessions());

            //        mediator
            //.Setup(m => m.Send(It.IsAny<TransferNotificationCommand>(), It.IsAny<CancellationToken>()))
            //.ReturnsAsync(new Notification()) //<-- return Task to allow await to continue
            //.Verifiable("Notification was not sent.");

            //        ...other code removed for brevity

            //        mediator.Verify(x => x.Send(It.IsAny<CreateIsaTransferNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once());


            UserController controller = new(mediator.Object);

            // Act
            Application.Base.BaseResponse<Application.User.Login.LoginResponse> result = await controller.Login(new Application.User.Login.LoginCommand
            {
                UserName = "admin",
                Password = "admin"
            });

            Assert.Equal(200, result.Code);
        }
        #endregion

        //#region snippet_ModelState_ValidOrInvalid
        //[Fact]
        //public async Task IndexPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IBrainstormSessionRepository>();
        //    mockRepo.Setup(repo => repo.ListAsync())
        //        .ReturnsAsync(GetTestSessions());
        //    var controller = new UserController(mockRepo.Object);
        //    controller.ModelState.AddModelError("SessionName", "Required");
        //    var newSession = new UserController.NewSessionModel();

        //    // Act
        //    var result = await controller.Index(newSession);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.IsType<SerializableError>(badRequestResult.Value);
        //}

        //[Fact]
        //public async Task IndexPost_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IBrainstormSessionRepository>();
        //    mockRepo.Setup(repo => repo.AddAsync(It.IsAny<BrainstormSession>()))
        //        .Returns(Task.CompletedTask)
        //        .Verifiable();
        //    var controller = new UserController(mockRepo.Object);
        //    var newSession = new UserController.NewSessionModel()
        //    {
        //        SessionName = "Test Name"
        //    };

        //    // Act
        //    var result = await controller.Index(newSession);

        //    // Assert
        //    var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.Null(redirectToActionResult.ControllerName);
        //    Assert.Equal("Index", redirectToActionResult.ActionName);
        //    mockRepo.Verify();
        //}
        //#endregion

        //#region snippet_GetTestSessions
        //private List<BrainstormSession> GetTestSessions()
        //{
        //    var sessions = new List<BrainstormSession>();
        //    sessions.Add(new BrainstormSession()
        //    {
        //        DateCreated = new DateTime(2016, 7, 2),
        //        Id = 1,
        //        Name = "Test One"
        //    });
        //    sessions.Add(new BrainstormSession()
        //    {
        //        DateCreated = new DateTime(2016, 7, 1),
        //        Id = 2,
        //        Name = "Test Two"
        //    });
        //    return sessions;
        //}
        //#endregion
    }
}
