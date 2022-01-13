using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileUseCase _uploadFileUseCase;

        public UploadFileController(IUploadFileUseCase uploadFileUseCase)
        {
            _uploadFileUseCase = uploadFileUseCase;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<UploadFileResponse>> UploadFile()
        {
            var file = Request.Form.Files[0];

            var fileId = await  _uploadFileUseCase.UploadFileAsync(file.OpenReadStream(), file.Name);
            return Ok(new UploadFileResponse()
            {
                FileId = fileId
            });
        }
    }
}