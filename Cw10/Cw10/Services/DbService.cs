
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTOs;
using Cw10.Models;
using Cw10.Configurations;

namespace Cw10.Services
{
    public class DbService : IDbService
    {
        private IDbContext _context;
        public DbService(IDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddPrescription(AddPrescription request)
        {
            Patient? patient = _context.Patients.Find(request.Patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient()
                {
                    Name = request.Patient.Name,
                    LastName = request.Patient.LastName,
                    BirthDate = request.Patient.BirthDate
                };
                _context.Patients.Add(patient);
            }
            if (request.PrescriptionMedicaments.Count > 10)
                return new BadRequestObjectResult("Max 10 medicaments");
            if (request.DueDate < request.Date)
                return new BadRequestObjectResult("Invalid date");

            foreach (var medicament in request.PrescriptionMedicaments)
            {
                Medicament? found = _context.Medicaments.Find(medicament.IdMedicament);
                if (found == null)
                    return new BadRequestObjectResult("Medicament not found");
            }

            var prescription = new Prescription()
            {
                IdPrescription = request.IdPrescription,
                IdDoctor = request.Doctor.IdDoctor,
                IdPatient = request.Patient.IdPatient,
                Date = request.Date,
                DueDate = request.DueDate,
                PrescrptionMedicaments = request.PrescriptionMedicaments
            };

            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChanges();
            return new OkObjectResult($"Added perscripttion {request.IdPrescription}");
        }
     
        public async Task<IActionResult> GetPatient(GetPatientRequest request)
        {
           
            Patient? patient = await _context.Patients
                .Where(p => p.IdPatient == request.IdPatient)
                .SingleOrDefaultAsync();

            if (patient == null)
            {
                return new BadRequestObjectResult($"Patient {request.IdPatient} not found");
            }

            var prescriptions = patient.Prescriptions;

            prescriptions.OrderBy(p => p.DueDate);

            var medicaments = new List<Medicament>();

            var doctors = new List<Doctor>();

            foreach (var p in prescriptions)
            {
                foreach(var pm in p.PrescrptionMedicaments)
                {
                    medicaments.Add(pm.Medicament);
                }

                doctors.Add(await _context.Doctors
                    .Where(d => d.IdDoctor == p.IdDoctor)
                    .SingleOrDefaultAsync());

            };

            var response =  new GetPatientResponse
                {
                    Name = patient.Name,
                    LastName = patient.LastName,
                    BirthDate = patient.BirthDate,
                    Prescriptions = patient.Prescriptions,
                    Medicaments = medicaments,
                    Doctors = doctors
                };


            return new OkObjectResult(response);
        }

    }
}
