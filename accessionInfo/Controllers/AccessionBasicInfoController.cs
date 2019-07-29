using Microsoft.AspNetCore.Mvc;

namespace emailServiceAPITemplate.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccessionBasicInfoController : ControllerBase
    {

        private Services.IBrcFormService _BRCFormService;

    public AccessionBasicInfoController()
        {
            _BRCFormService = new Services.BRCFormService();
    }

        // GET api/accessionBasicInfo
        [HttpGet]
        public ActionResult<Models.AccessionBasicInfo> Get(Models.BRCInfo bRCInfo)
        {

            var accessionBasicInfo = _BRCFormService.extractCareerCodeFromBRCFormInfo(bRCInfo);


            return accessionBasicInfo;
        }

    }
}
