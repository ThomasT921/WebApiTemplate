using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Common.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewController : ControllerBase
    {
        private readonly IViewDataAccess _viewDataAccess;

        public ViewController(IViewDataAccess viewDataAccess)
        {
            _viewDataAccess = viewDataAccess;
        }

        [Route("GetView")]
        [HttpGet]
        public async Task<IEnumerable<ViewModel>> Get()
        {
            return await _viewDataAccess.GetViewModelsAsync();
        }
    }
}
