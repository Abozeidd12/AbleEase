using Microsoft.EntityFrameworkCore;
using AbleEaseDomain.Entities;
using AbleEaseDomain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AbleEaseInfrastructure.Identity;

namespace AbleEaseInfrastructure.Data
{
    public class AbleEaseDbContext : IdentityDbContext<ApplicationUser>
    {
        public AbleEaseDbContext(DbContextOptions<AbleEaseDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Relative> Relatives { get; set; }
        public DbSet<Caregiver> Caregivers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ReceivedMessages> ReceivedMessages { get; set; }
        public DbSet<SentMessages> SentMessages { get; set; }
        public DbSet<MedicalInfo> MedicalInfos { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<FinancialAid> FinancialAids { get; set; }
        public DbSet<PhysicalTherapy> PhysicalTherapies { get; set; }
        public DbSet<PhysiotherapyCenter> PhysiotherapyCenters { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<AssessmentPatient> AssessmentPatients { get; set; }
        public DbSet<PatientWorksAtOrganization> PatientWorksAtOrganizations { get; set; }
        public DbSet<PatientDisability> PatientDisabilities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // USER INHERITANCE CONFIGURATION (TPT)
            // ============================================
            modelBuilder.Entity<User>()
                .HasKey(u => u.SSN);

            modelBuilder.Entity<User>()
                .UseTptMappingStrategy();


            modelBuilder.Entity<Caregiver>()
    .HasOne<User>()
    .WithOne()
    .HasForeignKey<Caregiver>(c => c.SSN)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Relative>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Relative>(r => r.SSN)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Patient>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Patient>(p => p.SSN)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Organization>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Organization>(o => o.SSN)
                .OnDelete(DeleteBehavior.NoAction);



            // ============================================
            // PATIENT CONFIGURATION
            // ============================================
            modelBuilder.Entity<Patient>()
                .Property(p => p.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Relative)
                .WithOne(r => r.Patient)
                .HasForeignKey<Patient>(p => p.RelativeSSN)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Caregiver)
                .WithMany(c => c.patients)
                .HasForeignKey(p => p.CaregiverSSN)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Program)
                .WithMany(pr => pr.patients)
                .HasForeignKey(p => new { p.ProgramOrganizationSSN, p.ProgramId })
                .OnDelete(DeleteBehavior.NoAction);

            // ============================================
            // RELATIVE CONFIGURATION
            // ============================================
            modelBuilder.Entity<Relative>()
                .Property(r => r.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<SentMessages>()
               .Property(r => r.messageType)
               .HasConversion<string>();
            modelBuilder.Entity<ReceivedMessages>()
               .Property(r => r.messageType)
               .HasConversion<string>();

            // ============================================
            // CAREGIVER CONFIGURATION
            // ============================================
            modelBuilder.Entity<Caregiver>()
                .Property(c => c.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Caregiver>()
                .HasOne(c => c.organization)
                .WithMany(o => o.caregivers)
                .HasForeignKey(c => c.OrganizationSSN)
                .OnDelete(DeleteBehavior.SetNull);

            // ============================================
            // WEAK ENTITY: MESSAGE
            // ============================================
            modelBuilder.Entity<SentMessages>()
                .HasKey(m => new { m.SenderSSN, m.Id, m.ReceiverSSN });

            modelBuilder.Entity<SentMessages>()
                .Property(m => m.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<SentMessages>()
                .HasOne(m => m.sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderSSN)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ReceivedMessages>()
             .HasKey(m => new { m.SenderSSN, m.Id, m.ReceivedSSN });

            modelBuilder.Entity<ReceivedMessages>()
                .Property(m => m.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ReceivedMessages>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceivedSSN)
                .OnDelete(DeleteBehavior.Cascade);



            // ============================================
            // WEAK ENTITY: MEDICAL INFO
            // ============================================
            modelBuilder.Entity<MedicalInfo>()
                .HasKey(m => new { m.PatientSSN, m.Id });

            modelBuilder.Entity<MedicalInfo>()
                .Property(m => m.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<MedicalInfo>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalInfo)
                .HasForeignKey(m => m.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalInfo>()
                .HasOne(m => m.Relative)
                .WithMany(r => r.medicalInfo)
                .HasForeignKey(m => m.RelativeSSN)
                .OnDelete(DeleteBehavior.NoAction);

            // ============================================
            // WEAK ENTITY: PROGRAM
            // ============================================
            modelBuilder.Entity<Program>()
                .HasKey(p => new { p.OrganizationSSN, p.Id });

            modelBuilder.Entity<Program>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Program>()
                .Property(p => p.status)
                .HasConversion<string>();

            modelBuilder.Entity<Program>()
                .HasOne(p => p.organization)
                .WithMany(o => o.Programs)
                .HasForeignKey(p => p.OrganizationSSN)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // WEAK ENTITY: ASSESSMENT
            // ============================================
            modelBuilder.Entity<Assessment>()
                .HasKey(a => new { a.ProgramOrganizationSSN, a.ProgramId, a.Id });

            modelBuilder.Entity<Assessment>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.program)
                .WithOne(p => p.Assessment)
                .HasForeignKey<Assessment>(a => new { a.ProgramOrganizationSSN, a.ProgramId })
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // FINANCIAL AID
            // ============================================
            modelBuilder.Entity<FinancialAid>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FinancialAid>()
                .Property(f => f.ApprovalStatus)
                .HasConversion<string>();

            modelBuilder.Entity<FinancialAid>()
                .HasOne(f => f.patient)
                .WithMany(p => p.financialAids)
                .HasForeignKey(f => f.PatientSSN)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<FinancialAid>()
                .HasOne(f => f.organization)
                .WithMany(o => o.financialAids)
                .HasForeignKey(f => f.OrganizationSSN)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // PHYSICAL THERAPY
            // ============================================
            modelBuilder.Entity<PhysicalTherapy>()
                .HasKey(pt => pt.Id);

            modelBuilder.Entity<PhysicalTherapy>()
                .HasOne(pt => pt.patient)
                .WithMany(p => p.Therapies)
                .HasForeignKey(pt => pt.PatientSSN)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PhysicalTherapy>()
                .HasOne(pt => pt.center)
                .WithMany()
                .HasForeignKey(pt => pt.CenterID)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // PHYSIOTHERAPY CENTER
            // ============================================
            modelBuilder.Entity<PhysiotherapyCenter>()
                .HasKey(pc => pc.SSN);

            // ============================================
            // PAYMENT
            // ============================================
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Payment>()
                .Property(p => p.approvalStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.patient)
                .WithMany(pat => pat.payments)
                .HasForeignKey(p => p.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.financialAid)
                .WithOne(f => f.payment)
                .HasForeignKey<Payment>(p => p.FinancialId)
                .OnDelete(DeleteBehavior.SetNull);

            // ============================================
            // REPORT
            // ============================================
            modelBuilder.Entity<Report>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.caregiver)
                .WithMany(c => c.reports)
                .HasForeignKey(r => r.CaregiverSSN)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.patient)
                .WithMany(p => p.reports)
                .HasForeignKey(r => r.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.program)
                .WithMany(p => p.reports)
                .HasForeignKey(r => new { r.ProgramOrganizationSSN, r.ProgramId })
                .OnDelete(DeleteBehavior.SetNull);

            // ============================================
            // DISABILITY
            // ============================================
            modelBuilder.Entity<Disability>()
                .HasKey(d => d.SSN);

            // ============================================
            // ASSESSMENT-PATIENT
            // ============================================
            modelBuilder.Entity<AssessmentPatient>()
                .HasKey(ap => new {
                    ap.AssessmentProgramOrganizationSSN,
                    ap.AssessmentProgramId,
                    ap.AssessmentId,
                    ap.PatientSSN
                });

            modelBuilder.Entity<AssessmentPatient>()
                .HasOne(ap => ap.Assessment)
                .WithMany(a => a.assessmentPatients)
                .HasForeignKey(ap => new {
                    ap.AssessmentProgramOrganizationSSN,
                    ap.AssessmentProgramId,
                    ap.AssessmentId
                })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssessmentPatient>()
                .HasOne(ap => ap.patient)
                .WithMany(p => p.assessmentPatients)
                .HasForeignKey(ap => ap.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // PATIENT-ORGANIZATION
            // ============================================
            modelBuilder.Entity<PatientWorksAtOrganization>()
                .HasKey(pw => new { pw.PatientSSN });

            modelBuilder.Entity<PatientWorksAtOrganization>()
                .HasOne(pw => pw.patient)
                .WithOne(p => p.worksAt)
                .HasForeignKey<PatientWorksAtOrganization>(pw => pw.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientWorksAtOrganization>()
                .HasOne(pw => pw.organization)
                .WithMany(o => o.patientWorksAts)
                .HasForeignKey(pw => pw.OrganizationSSN)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // PATIENT-DISABILITY
            // ============================================
            modelBuilder.Entity<PatientDisability>()
                .HasKey(pd => new { pd.PatientSSN, pd.DisabilityID });

            modelBuilder.Entity<PatientDisability>()
                .HasOne(pd => pd.patient)
                .WithMany(p => p.patientDisabilities)
                .HasForeignKey(pd => pd.PatientSSN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientDisability>()
                .HasOne(pd => pd.disability)
                .WithMany(d => d.patientDisabilities)
                .HasForeignKey(pd => pd.DisabilityID)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // INDEXES FOR PERFORMANCE
            // ============================================
            

            modelBuilder.Entity<SentMessages>()
                .HasIndex(m => m.SentDate);

            modelBuilder.Entity<ReceivedMessages>()
    .HasIndex(m => m.SentDate);

            modelBuilder.Entity<MedicalInfo>()
                .HasIndex(m => m.RelativeSSN);

            modelBuilder.Entity<Program>()
                .HasIndex(p => p.status);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.CaregiverSSN);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => new { p.ProgramOrganizationSSN, p.ProgramId });

            modelBuilder.Entity<PhysicalTherapy>()
                .HasIndex(pt => pt.PatientSSN);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PatientSSN);

            modelBuilder.Entity<FinancialAid>()
                .HasIndex(f => f.PatientSSN);

            modelBuilder.Entity<Report>()
                .HasIndex(r => r.PatientSSN);

            modelBuilder.Entity<Report>()
                .HasIndex(r => r.CaregiverSSN);

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.ProgramOrganizationSSN, r.ProgramId });
        }
    }
}