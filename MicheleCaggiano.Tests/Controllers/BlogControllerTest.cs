using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicheleCaggiano.Web;
using MicheleCaggiano.Web.Controllers;
using MicheleCaggiano.Web.Models;
using System.Security.Principal;
using Moq;
using MicheleCaggiano.Web.Tests.ExtensionMethods;
using MicheleCaggiano.Models;

namespace MicheleCaggiano.Web.Tests.Controllers
{
  [TestClass]
  public class BlogControllerTest
  {
    [TestMethod]
    public void GetBlogViewModel()
    {
      // Arrange
      BlogController controller = new BlogController();

      controller.SetUserContext(ControllerExtensions.UsersType.Admin);

      // Act
      PrivateObject obj = new PrivateObject(controller);
      var result = obj.Invoke("GetBlogViewModel", BlogViewModel.TipologiaArticoli.Tutti, null, 1);

      // Assert
      Assert.IsNotNull(result);

    }

    //[TestMethod]
    //public void About()
    //{
    //  // Arrange
    //  HomeController controller = new HomeController();

    //  // Act
    //  ViewResult result = controller.About() as ViewResult;

    //  // Assert
    //  Assert.AreEqual("Your application description page.", result.ViewBag.Message);
    //}

    //[TestMethod]
    //public void Contact()
    //{
    //  // Arrange
    //  HomeController controller = new HomeController();

    //  // Act
    //  ViewResult result = controller.Contact() as ViewResult;

    //  // Assert
    //  Assert.IsNotNull(result);
    //}
  }
}
