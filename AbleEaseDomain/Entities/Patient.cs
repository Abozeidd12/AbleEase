using AbleEaseDomain.Enums;
using System;
using System.Collections.Generic;

namespace AbleEaseDomain.Entities
{
    public class Patient : User
    {
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        // Reference to Relative (One-to-One)
        public Guid? RelativeSSN { get; set; }
        public Relative? Relative { get; set; }

        // Reference to Caregiver (Many-to-One)
        public Guid? CaregiverSSN { get; set; }
        public Caregiver? Caregiver { get; set; }

        // Reference to Program (WEAK ENTITY) - Need composite FK
        public Guid? ProgramOrganizationSSN { get; set; }
        public int? ProgramId { get; set; }
        public Program? Program { get; set; }
        public List<Report> reports { get; set; } = new List<Report>();
        public List<PhysicalTherapy> Therapies { get; set; } = new List<PhysicalTherapy>();
        public List<FinancialAid> financialAids { get; set; } = new List<FinancialAid>();
        public List<Payment> payments { get; set; } = new List<Payment>();

        // WEAK ENTITY: MedicalInfo depends on Patient
        public List<MedicalInfo> MedicalInfo { get; set; } = new List<MedicalInfo>();
        public List<AssessmentPatient> assessmentPatients { get; set; } = new List<AssessmentPatient>();
        public PatientWorksAtOrganization? worksAt { get; set; }
        public List<PatientDisability> patientDisabilities { get; set; } = new List<PatientDisability>();
    }
}