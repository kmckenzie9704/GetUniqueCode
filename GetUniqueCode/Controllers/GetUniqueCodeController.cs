using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetUniqueCode.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GetUniqueCode.Controllers
{
    [Route("api/GetUniqueCode")]
    public class GetUniqueCodeController : Controller
    {

        private DatabaseContext _context;

        public GetUniqueCodeController(DatabaseContext context)
        {
            _context = context;
        }
        
        // GET api/GetUniqueCode
        [HttpGet]
        public string Get()
        {
            bool blnValidCode = false;
            string strUniqueCode = string.Empty;
            while (!blnValidCode)
            {
                strUniqueCode = GetNewCode();
                blnValidCode = ValidateCode(strUniqueCode);
            }
            return strUniqueCode;
        }

        private string GetNewCode()
        {
            string strUniqueCode = Guid.NewGuid().ToString();
            strUniqueCode = strUniqueCode.Substring(0, 8).ToUpper();
            return strUniqueCode;
        }
        public bool Post([FromBody]Applicant applicant)
        {
            bool blnUniqueCodeIsValid = ValidateCode(applicant.appUniqueCode);

            return blnUniqueCodeIsValid;
        }

 
        private bool ValidateCode(string strUniqueCode)
        {
            bool blnCodeIsAvailable = false;
            using (_context)
            {
                var code = _context.UniqueCodes.FirstOrDefault(c => c.uniCode == strUniqueCode && c.uniAccepted == false);
                blnCodeIsAvailable = (code is null) ? true : false;
            }

            return blnCodeIsAvailable;
        }
    }

}
