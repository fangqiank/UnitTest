using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestingControllersSample.Api;
using TestingControllersSample.ClientModels;
using TestingControllersSample.Controllers;
using TestingControllersSample.Core.Interfaces;
using TestingControllersSample.Core.Model;
using TestingControllersSample.ViewModels;
using Xunit;

namespace TestMain
{
    public class ControllerShould
    {
        private Mock<IBrainstormSessionRepository> mockRepo = new Mock<IBrainstormSessionRepository>();
        private HomeController controller;

        public ControllerShould()
        {
            mockRepo.Setup(x => x.ListAsync())
                .ReturnsAsync(GetTestSessions);

            controller = new HomeController(mockRepo.Object);
        }
        
        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2,model.Count());

        }

        [Fact]
        public async Task IndexPost_ReturnBadRequest_WhenModelStateIsInvalid()
        {
            controller.ModelState.TryAddModelError("SessionName", "Required");
            var newSession = new HomeController.NewSessionModel();

            var result = await controller.Index(newSession);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequest.Value);
        }

        [Fact]
        public async Task Index_ReturnAsRedirectAndAddSession_WhenModelStateIsValid()
        {
            mockRepo.Setup(x=>x.AddAsync(It.IsAny<BrainstormSession>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new HomeController(mockRepo.Object);
            var newSession = new HomeController.NewSessionModel()
            {
                SessionName = "Test Name"
            };

            var result = await controller.Index(newSession);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index",redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task IndexReturnRedirectToIndexHomeWhenIdIsNull()
        {
            var controller = new SessionController(sessionRepository:null);
            var result = await controller.Index(id: null);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home",redirectToAction.ControllerName);
            Assert.Equal("Index",redirectToAction.ActionName);
        }

        [Fact]
        public async Task IndexReturnContentWithSessionNotFoundWhenSessionNotFound()
        {
            int testSessionId = 1;
            var mockRespo = new Mock<IBrainstormSessionRepository>();
            mockRespo.Setup(x => x.GetByIdAsync(testSessionId))
                .ReturnsAsync((BrainstormSession)null);
            var controller = new SessionController(mockRespo.Object);

            var result = await controller.Index(testSessionId);

            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Session not found.",contentResult.Content);
        }

        [Fact]
        public async Task IndexReturnsViewResultWithStormSessionViewModel()
        {
            int testsessionId = 1;
            var mockRespo = new Mock<IBrainstormSessionRepository>();
            mockRespo.Setup(x => x.GetByIdAsync(testsessionId))
                .ReturnsAsync(GetTestSessions().FirstOrDefault(b => b.Id == testsessionId));
            var controller = new SessionController(mockRespo.Object);

            var result = await controller.Index(testsessionId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<StormSessionViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Test One", model.Name);
            Assert.Equal(9,model.DateCreated.Day);
            Assert.Equal(testsessionId,model.Id);
        }

        [Fact]
        public async Task ForSession_ResturnsHttpNotFound_ForInvalidSession()
        {
            int testSessionId = 123;
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(testSessionId))
                .ReturnsAsync((BrainstormSession) null);
            var controller = new IdeasController(mockRepo.Object);

            var result = await controller.ForSession(testSessionId);

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(testSessionId,notFoundObjectResult.Value);
        }

        [Fact]
        public async Task ForSession_ResturnsIdeasForSession()
        {
            int testSessionId = 1;
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(testSessionId))
                .ReturnsAsync((BrainstormSession)null);
            var controller = new IdeasController(mockRepo.Object);

            var result = await controller.ForSession(testSessionId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<IdeaDTO>>(okResult.Value);
            var idea = returnValue.FirstOrDefault();
            Assert.Equal("One",idea.Name);

        }

        [Fact]
        public async Task Create_ResturnsBadRequest_GivenInvalidMidel()
        {
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            var controller = new IdeasController(mockRepo.Object);
            controller.ModelState.AddModelError("error","some error");

            var result = await controller.Create(model:null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnHttpNotFound_ForInvalidSession()
        {
            int testSessionId = 123;
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(testSessionId))
                .ReturnsAsync((BrainstormSession) null);
            var controller = new IdeasController(mockRepo.Object);

            var result = await controller.Create(new NewIdeaModel());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnNewCreatedIdeaForSession()
        {
            int testSessionId = 123;
            string testName = "test name";
            string testDescription = "test description";
            var testSession = GetTestSession();
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testSessionId))
                .ReturnsAsync(testSession);
            var controller = new IdeasController(mockRepo.Object);

            var newIdea = new NewIdeaModel()
            {
                Description = testDescription,
                Name = testName,
                SessionId = testSessionId
            };
            mockRepo.Setup(x=>x.UpdateAsync(testSession))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await controller.CreateActionResult(newIdea);

            var actionResult = Assert.IsType<ActionResult<BrainstormSession>>(result);
            var createAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<BrainstormSession>(createAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(2,returnValue.Ideas.Count);
            Assert.Equal(testName,returnValue.Ideas.LastOrDefault().Name);
            Assert.Equal(testDescription,returnValue.Ideas.LastOrDefault().Description);

        }
        
        private List<BrainstormSession> GetTestSessions()
        {
            var sessions = new List<BrainstormSession>();
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2021,02,9),
                Id = 1,
                Name = "Test One"
            });

            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2021, 02, 10),
                Id = 2,
                Name = "Test Two"
            });

            return sessions;
        }

        private BrainstormSession GetTestSession()
        {
            var session = new BrainstormSession()
            {
                Name = "Test Session 1",
                DateCreated = new DateTime(2016, 8, 1)
            };
            var idea = new Idea()
            {
                DateCreated = new DateTime(2016, 8, 1),
                Description = "Totally awesome idea",
                Name = "Awesome idea"
            };
            session.AddIdea(idea);
            return session;
        }
    }
}
