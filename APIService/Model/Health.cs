using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace APIService.Model
{
    public class Health
    {
        public string Comments { get; set; }
        public string CompanyName { get; set; }
        public string FileDescription { get; set; }
        public string FileVersion { get; set; }
        public string Language { get; set; }
        public string LegalCopyright { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
    }
}
