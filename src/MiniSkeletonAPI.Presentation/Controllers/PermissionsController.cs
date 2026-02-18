//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MiniSkeletonAPI.Infrastructure.Identity.Permission;
//using System.Threading.Tasks;

//namespace MiniSkeletonAPI.Presentation.Controllers
//{
//  /*  [ApiController]
//    [Route("api/[controller]")]*/
//    //[Authorize] // Tambahkan otorisasi untuk seluruh controller jika perlu
//    public class PermissionsController : ControllerBases
//    {
       
//        [HttpGet]
//        public async Task<IActionResult> GetPermissions()
//        {
//            var json = StaticSerialization.GetFieldFromStaticClass(typeof(Permissions));
//            return Ok(json);
//        }
//    }
//}
