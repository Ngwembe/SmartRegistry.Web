using System;

namespace SmartRegistry.Web.ViewModels
{
    public class PatientViewModel
    {
        public int CreatorType { get; set; }
        public int PatientType_Id { get; set; }
        public bool IsActive { get; set; }

        public int PatientId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Allergies { get; set; } // i think will have to be a new table on its own
        public string Type { get; set; }
        public int PersonId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdNumber { get; set; }
        public string PassportNumber { get; set; }
        public string CauseOfDeath { get; set; }
        public DateTime? DeathDate { get; set; }
        public int? CreatorId { get; set; }
        public string MiddleName { get; set; }
        public string Initials { get; set; }
        public string ResAddress1 { get; set; }
        public string ResAddress2 { get; set; }
        public string ResPostCode { get; set; }
        public string Title { get; set; }
        public string Occupation { get; set; }
        public string Religion { get; set; }
        public string Email { get; set; }
        public string BusAddress { get; set; }
        public string BusPostCode { get; set; }
        public string BusName { get; set; }
        public string HomeTel { get; set; }
        public string WorkTel { get; set; }
        public string ResSurbub { get; set; }
        public string Country { get; set; }
        public int GenderId { get; set; }
        public int DeadTypeId { get; set; }

        //  Next Of Kin
        public string NokName { get; set; }
        public string NokSurname { get; set; }
        public string NokContact { get; set; }
        public string NokBuildingHome { get; set; }
        public string NokStreetName { get; set; }
        public string NokCountry { get; set; }
        public string NokSurburbTown { get; set; }
        public string NokPostalCode { get; set; }
        public string NokEmail { get; set; }
        public string NokRelationship { get; set; }
        public string Ethnicity { get; set; }
        public string IdentityType { get; set; }
        public string MaritalStatus { get; set; }
        public string Relation { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }

    }
}
