using Cw10.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTOs;

namespace Cw10.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IDbService _dbService;
        public HospitalController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet("patient")]
        public async Task<IActionResult> GetPatient(GetPatientRequest request)
        {
            return await _dbService.GetPatient(request);
        }
        [HttpPost("prescription")]
        public async Task<IActionResult> AddPrescription(AddPrescription request)
        {
            return await _dbService.AddPrescription(request);
        }
    }
}