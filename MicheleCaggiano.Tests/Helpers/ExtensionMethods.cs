using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MicheleCaggiano.Web.Tests.ExtensionMethods
{
  public static class ControllerExtensions
  {
    public enum UsersType
    {
      Admin = 0,
      Registered = 1,
      Anonymous = 2
    }

    public static void SetUserContext(this Controller controller, ControllerExtensions.UsersType userType)
    {
      var mock = new Mock<ControllerContext>();

      switch (userType)
      {
        case UsersType.Admin:
          var principal = new Moq.Mock<IPrincipal>();
          principal.Setup(p => p.IsInRole("Admin")).Returns(true);
          mock.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
          mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
          break;
        case UsersType.Registered:
          mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
          break;
        case UsersType.Anonymous:
        default:
          mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(false);
          break;
      }

      controller.ControllerContext = mock.Object;
    }
  }
}
