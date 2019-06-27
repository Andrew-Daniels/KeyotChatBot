using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KeyotBotCoreApp.Context.Entities
{
    [Table("Candidate_Crew212")]
    public class Candidate
    {
        [Key]
        [Column("candidate_id")]
        public int CandidateId { get; set; }
        [Column("college_degree")]
        public string CollegeDegree { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("gpa")]
        public string GPA { get; set; }
        [Column("grad_year")]
        public string GradYear { get; set; }
        [Column("leadership")]
        public int Leadership { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("numInternships")]
        public int NumInternships { get; set; }
        [Column("numOrganizations_expand")]
        public string NumOrganizations { get; set; }
        [Column("numTechnicalSkills")]
        public int NumTechnicalSkills { get; set; }
        [Column("relocate")]
        public string Relocate { get; set; }
        [Column("school_name")]
        public string SchoolName { get; set; }
        [Column("stem_degree")]
        public string StemDegree { get; set; }
        [Column("technicalSkills")]
        public string TechnicalSkills { get; set; }
        [Column("usAuthorized")]
        public string USAuthorized { get; set; }
        [Column("work_experience")]
        public int WorkExperience { get; set; }
    }
}