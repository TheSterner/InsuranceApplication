using InsuranceApplication.Models;
using InsuranceApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceApplication.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
			//GET: Admin
			using (InsuranceEntities db = new InsuranceEntities())
			{
			
				var customerInfos = db.CustomerInfoes.ToList();
				var customerInfoVms = new List<CustomerInfoVm>();
				foreach (var customerInfo in customerInfos)
				{
					var customerInfoVm = new CustomerInfoVm();
					customerInfoVm.Id = customerInfo.Id;
					customerInfoVm.FirstName = customerInfo.FirstName;
					customerInfoVm.LastName = customerInfo.LastName;
					customerInfoVm.EmailAddress = customerInfo.EmailAddress;
					customerInfoVm.Quote = customerInfo.Quote.ToString();
					customerInfoVms.Add(customerInfoVm);
				}
				return View(customerInfoVms);
			}
		}
    }
}