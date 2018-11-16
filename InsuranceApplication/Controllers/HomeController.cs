using InsuranceApplication.Models;
using InsuranceApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceApplication.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		//public ActionResult TestVm()
		//{
		//	return View();
		//}


		private InsuranceEntities db = new InsuranceEntities();

		[HttpPost]
		public ActionResult CustomerInfo(string firstName, string lastName, string emailAddress, string dateOfBirth, int carYear,
										   string carMake, string carModel, bool dui, string speedingTicket, bool fullCoverage)
		{
			//CHECK FOR EMPTY FORM
			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(dateOfBirth.ToString()) ||
				string.IsNullOrEmpty(carYear.ToString()) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(speedingTicket.ToString()))
			{
				return View("~/Views/Shared/Error.cshtml");

			}
			else
			{
				//DB INSTANSIATION 
				using (InsuranceEntities db = new InsuranceEntities())
				{
					var customerInfo = new CustomerInfo();

					customerInfo.FirstName = firstName;
					customerInfo.LastName = lastName;
					customerInfo.EmailAddress = emailAddress;
					customerInfo.CustomerDOB = dateOfBirth;
					customerInfo.CarYear = carYear;
					customerInfo.CarMake = carMake;
					carMake = carMake.ToLower();
					customerInfo.CarModel = carModel;
					carModel = carModel.ToLower();
					customerInfo.DUI = dui;
					customerInfo.SpeedingTicket = Convert.ToInt32(speedingTicket);
					customerInfo.FullCoverage = fullCoverage;


					//LOGIC OPPERATIONS
					DateTime grab = DateTime.Parse(dateOfBirth);
					int year = grab.Year;
					int age = DateTime.Now.Year - year;

					double quote;
					quote = 50.00;

					//AGE CHECK
					if (age < 18)
					{
						quote = quote + 100.00;
					}
					else if (age < 25 || age > 100)
					{
						quote = quote + 25.00;
					}


					//CAR YEAR CHECK
					if (carYear == 1999 || carYear > 2015)
					{
						quote = quote + 25.00;
					}

					//PORCHE CHECK 
					if (carMake == "porsche")
					{
						quote = quote + 25.00;
						if (carModel == "911 carrera")
						{
							quote = quote + 25.00;
						}
					}

					//SPEEDING TICKET
					int ticketCount = Convert.ToInt32(speedingTicket);
					if (ticketCount < 0)
					{
						return View("~/Views/Shared/Error.cshtml");
					}
					for (int i = 0; i < ticketCount; i++)
					{
						quote = quote + 10.00;
					}

					//DUI
					if (dui == true)
					{
						quote = quote + (quote * .25);
					}

					//FULL COVERAGE
					if (fullCoverage == true)
					{
						quote = quote + (quote * .50);
					}

					customerInfo.Quote = quote;

					db.CustomerInfoes.Add(customerInfo);
					db.SaveChanges();

					int userId = customerInfo.Id;

					//   return View(Quoter(userId));



					return RedirectToAction("Quote", new { Id = userId });
				}
			}
		}


		//public ActionResult Quoter(int Id)
		//{
		//    if (Id == null)
		//    {
		//        return View("~/Views/Shared/Error.cshtml");
		//    }
		//    CustomerTable customer = db.CustomerTables.Find(Id);
		//    if (customer == null)
		//    {
		//        return View("~/Views/Shared/Error.cshtml");
		//    }
		//    return View(customer);
		//}




		//public ActionResult Quote(int? Id)
		//{
		//	using (InsuranceEntities db = new InsuranceEntities())
		//	{
		//		var returner = new QuoteVm();
		//		returner.Id = returner.Id;
		//		returner.FirstName = returner.FirstName;
		//		returner.LastName = returner.LastName;
		//		returner.Quote = returner.Quote;
		//		return View(returner);
		//	}


		//}


		//public ActionResult Quote(int? id)
		//{

		//    CustomerTable customer = db.CustomerTables.Find(id);

		//    return View(customer);
		//}


		//[HttpPost]
		//public ActionResult Quote(int Id)
		//{
		//    using (var db = new insuranceUhohEntities())
		//    {
		//        var finder = db.CustomerTables.Find(Id);
		//        string quote = finder.Quote.ToString();
		//        return View(quote);

		//    }

		//}


	}
}

//{
//	public class HomeController : Controller
//	{
//		public ActionResult Index()
//		{
//			return View();
//		}

//		public ActionResult About()
//		{
//			ViewBag.Message = "Your application description page.";

//			return View();
//		}

//		public ActionResult Contact()
//		{
//			ViewBag.Message = "Your contact page.";

//			return View();
//		}
//	}
//}