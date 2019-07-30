using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSLR_DeliveryMessagingSystem.ViewModels
{
    public class ReceptionVM
    {
        public Models.StudentDetail StudentDetail { get; set; }
        public Models.Company Company { get; set; }
        public Models.Message Message { get; set; }

        public IEnumerable<SelectListItem> ListCompany { get; set; }
        public IEnumerable<SelectListItem> ListMessage { get; set; }
        public IEnumerable<SelectListItem> ListStudentDetail { get; set; }
    }
}