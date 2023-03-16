using GT.Trace.Common.CleanArch;
using Microsoft.AspNetCore.Mvc;

namespace Gtt.Packaging.PalletRelease.UI.WebAPI.Controllers
{
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly ILogger<InitController> _logger;

        private readonly ResultViewModel<InitController> _viewModel;

        public InitController(ILogger<InitController> logger, ResultViewModel<InitController> viewModel)
        {
            _logger = logger;
            _viewModel = viewModel;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(500, _viewModel.Fail("Not implemented."));
        }
    }
}