
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    [Produces("text/json")]
    public abstract class ControllerBases : ControllerBase
    {
    }
}
