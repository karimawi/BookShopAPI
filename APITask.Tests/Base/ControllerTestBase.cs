using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITask.Tests.Base
{
    public abstract class ControllerTestBase<TController> where TController : ControllerBase
    {
        protected TController Controller { get; set; }

        protected ControllerTestBase(TController controller)
        {
            Controller = controller;
            SetupControllerContext();
        }

        private void SetupControllerContext()
        {
            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        protected void AddModelError(string key, string errorMessage)
        {
            Controller.ModelState.AddModelError(key, errorMessage);
        }

        protected void ClearModelState()
        {
            Controller.ModelState.Clear();
        }
    }
}