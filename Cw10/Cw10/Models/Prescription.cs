using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Cw10.Models
{
    public class Prescription
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public virtual ICollection<PrescriptionMedicament> PrescrptionMedicaments { get; set; }
        public int IdPatient { get; set; }

        public int IdDoctor { get; set; }

    }
}
