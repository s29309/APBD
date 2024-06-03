
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTOs;
using Cw10.Models;

namespace Cw10.Services
{
    public interface IDbService
    {
        public Task<IActionResult> GetPatient(GetPatientRequest request);
        public Task<IActionResult> AddPrescription(AddPrescription request);
    }
}
