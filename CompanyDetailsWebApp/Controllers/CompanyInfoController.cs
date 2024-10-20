using CompanyDetailsWebApp.DataAccessLogic;
using CompanyDetailsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection.PortableExecutable;

namespace CompanyDetailsWebApp.Controllers
{
    /// <summary>
    /// ASP.NET Core's controller factory creates an instance of the controller and injects any dependencies listed in the controller’s constructor.
    /// Controllers are Scoped services, A new instance of the controller is created for each incoming HTTP request.
    /// </summary>
    public class CompanyInfoController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        private CompanyDetails GenerateDefaultData()
        {
            CompanyDetails companyDetails = new CompanyDetails();

            companyDetails.CompanyID = Convert.ToInt32("32");
            companyDetails.CompanyName = Convert.ToString("Default Company_Name");
            companyDetails.CompanyEmail = Convert.ToString("Default Company_Email");
            companyDetails.LicenseId = Convert.ToString("Default License_Id");
            companyDetails.Data = Convert.ToString("Default Data");


            return companyDetails;
        }


        public CompanyInfoController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //_WCoreKCSqlDatabase = new WCoreKCSqlDatabase(WCoreKCSqlDatabase.GetConnectionString(_configuration));
        }

        //[Route("mainmethod")]
        public IActionResult Index()
        {
            return View();
        }

        

        //[HttpGet("CompanyDetails")]
        [ActionName("CompanyDetails")]
        public IActionResult GetCompanyDetails(string companyId)
        {
            // http://localhost:5081/CompanyInfo/CompanyDetails?companyId=32
            // 5187
            CompanyDetails companyDetails = new CompanyDetails();
            companyDetails.MachinesList = new List<MachineDetails>();

            DataTable dataTable = null;

            DataUtil dataUtil = new DataUtil(_configuration);

            //dataTable = dataUtil.FetchCompanyByID(companyId);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                try
                {
                    DataRow dataRow = dataTable.Rows[0];
                    companyDetails.CompanyID = Convert.ToInt32(dataRow["Company_Id"]);
                    companyDetails.CompanyName = Convert.ToString(dataRow["Company_Name"]);
                    companyDetails.CompanyEmail = Convert.ToString(dataRow["Company_Email"]);
                    companyDetails.LicenseId = Convert.ToString(dataRow["License_Id"]);
                    companyDetails.Data = Convert.ToString(dataRow["Data"]);

                    MachineDetails machine_1 = new MachineDetails();
                    machine_1.MacAddress = "00:1A:2B:3C:4D:5E";
                    machine_1.MachineID = "JF75";

                    MachineDetails machine_2 = new MachineDetails();
                    machine_2.MacAddress = "01:23:45:67:89";
                    machine_1.MachineID = "FF8W";

                    MachineDetails machine_3 = new MachineDetails();
                    machine_3.MacAddress = "5C:4D:3E:2F:1A:0B";
                    machine_1.MachineID = "VA83";

                    MachineDetails machine_4 = new MachineDetails();
                    machine_4.MacAddress = "AA:BB:CC:DD:EE";
                    machine_1.MachineID = "PM8F";

                    companyDetails.MachinesList.Add(machine_1);
                    companyDetails.MachinesList.Add(machine_2);
                    companyDetails.MachinesList.Add(machine_3);
                    companyDetails.MachinesList.Add(machine_4);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetCompanyDetails()" + ex.Message);

                    return RedirectToAction("Error");
                }
            }
            else
            {
                // Unable to connect to database, trying HardCoded Data

                companyDetails = GenerateDefaultData();
                companyDetails.MachinesList = new List<MachineDetails>();

                {
                    MachineDetails machine_1 = new MachineDetails();
                    machine_1.MacAddress = "00:1A:2B:3C:4D:5E";
                    machine_1.MachineID = "JF75";

                    machine_1.Modules = new List<Module>();
                    Module mod1 = new Module()
                    {
                        ModuleID = "1",
                        ModuleName = "Module x"
                    };
                    machine_1.Modules.Add(mod1);

                    Module mod2 = new Module()
                    {
                        ModuleID = "2",
                        ModuleName = "Module 2"
                    };
                    machine_1.Modules.Add(mod2);

                    companyDetails.MachinesList.Add(machine_1);
                }

                {
                    MachineDetails machine_2 = new MachineDetails();
                    machine_2.MacAddress = "01:23:45:67:89";
                    machine_2.MachineID = "FF8W";
                    machine_2.Modules = new List<Module>();
                    Module mod1 = new Module()
                    {
                        ModuleID = "1",
                        ModuleName = "Module 1"
                    };
                    machine_2.Modules.Add(mod1);

                    companyDetails.MachinesList.Add(machine_2);

                }

                {

                    MachineDetails machine_3 = new MachineDetails();
                    machine_3.MacAddress = "5C:4D:3E:2F:1A:0B";
                    machine_3.MachineID = "VA83";
                    machine_3.Modules = new List<Module>();
                    Module mod1 = new Module()
                    {
                        ModuleID = "1",
                        ModuleName = "Module 1"
                    };
                    machine_3.Modules.Add(mod1);

                    companyDetails.MachinesList.Add(machine_3);
                }

                {

                    MachineDetails machine_4 = new MachineDetails();
                    machine_4.MacAddress = "AA:BB:CC:DD:EE";
                    machine_4.MachineID = "PM8F";

                    machine_4.Modules = new List<Module>();
                    Module mod1 = new Module()
                    {
                        ModuleID = "1",
                        ModuleName = "Module 1"
                    };
                    machine_4.Modules.Add(mod1);

                    companyDetails.MachinesList.Add(machine_4);
                }

                if (companyDetails.MachinesList == null)
                {
                    companyDetails.MachinesList = new List<MachineDetails>();
                }
                
            }

            ViewData["Title"] = "Company Details";
            Console.WriteLine("No error...");
            return View("CompanyDetailsV2", companyDetails);
            //return View(companyDetails);
        }


        public IActionResult SendEmail(string machineID)
        {
            try
            {
                EmailService emailService = new EmailService(_configuration);
                emailService.SendMailWithTemplate("TemplateOne");

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            return View();
        }


    }
}
