using Cw10.Models;

namespace Cw10.DTOs
{
    public class GetPatientResponse
    {
        public int IdPatient { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Medicament> Medicaments { get; set; }

    }
}
