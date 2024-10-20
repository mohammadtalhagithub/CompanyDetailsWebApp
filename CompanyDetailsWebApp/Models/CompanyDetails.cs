using System.ComponentModel;

namespace CompanyDetailsWebApp.Models
{
    /// <summary>
    /// This entity represents the database table => WaspCloudTemplates.dbo.Company
    /// </summary>
    public class CompanyDetails
    {
        [DisplayName("Company Id")]
        public int CompanyID { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Company Email")]
        public string CompanyEmail { get; set; }

        [DisplayName("License Id")]
        public string LicenseId { get; set; }

        [DisplayName("Data")]
        public string Data { get; set; }

        [DisplayName("Machines List")]
        public List<MachineDetails> MachinesList { get; set; }
    }

    public class MachineDetails
    {
        [DisplayName("Machine ID")]
        public string MachineID { get; set; }

        [DisplayName("Mac Address")]
        public string MacAddress { get; set; }


        [DisplayName("Modules")]
        public List<Module> Modules { get; set; }
    }

    public class Module
    {
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }

        [DisplayName("Module Id")]
        public string ModuleID { get; set; }
    }
}
