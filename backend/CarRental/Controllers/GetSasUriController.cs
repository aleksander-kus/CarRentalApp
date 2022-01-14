using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class GetSasUriController : Controller
    {
        private readonly IGetSasUriUseCase _getSasUriUseCase;

        public GetSasUriController(IGetSasUriUseCase getSasUriUseCase)
        {
            _getSasUriUseCase = getSasUriUseCase;
        }

        [Route("download/{fileName}")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<FileSasResponse>> GetSasUriForFile(string fileName)
        {
            var uri = await _getSasUriUseCase.GetFileSasAsync(fileName, DateTimeOffset.Now.AddMinutes(1));
            if (uri == null) return NotFound(null);
            return Ok(new FileSasResponse
            {
                Uri = uri
            });
        }
    }
}