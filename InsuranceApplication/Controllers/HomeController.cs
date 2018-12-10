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



		//private CustomerInfo db = new CustomerInfo();
		private InsuranceEntities db = new InsuranceEntities();




		[HttpPost]
		public ActionResult CustomerInfo(string firstName, string lastName, string emailAddress, string CustomerDOB, int carYear,
										   string carMake, string carModel, bool dui, string speedingTicket, bool fullCoverage)

		{
			//Check for Blanks
			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(CustomerDOB.ToString()) ||
				string.IsNullOrEmpty(carYear.ToString()) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(speedingTicket.ToString()))
			{
				return View("~/Views/Shared/Error.cshtml");

			}
			else
			{
				using (InsuranceEntities db = new InsuranceEntities())
				{
					var customerInfo = new CustomerInfo();

					customerInfo.FirstName = firstName;
					customerInfo.LastName = lastName;
					customerInfo.EmailAddress = emailAddress;
					customerInfo.CustomerDOB = DateTime.Parse(CustomerDOB);
					customerInfo.CarYear = carYear;
					customerInfo.CarMake = carMake;
					carMake = carMake.ToLower();
					customerInfo.CarModel = carModel;
					carModel = carModel.ToLower();
					customerInfo.DUI = dui;
					customerInfo.SpeedingTicket = Convert.ToInt32(speedingTicket);
					customerInfo.FullCoverage = fullCoverage;


					DateTime grab = DateTime.Parse(CustomerDOB);
					int year = grab.Year;
					int age = DateTime.Now.Year - year;

					double quote;
					quote = 50.00;

					//Age
					if (age < 18)
					{
						quote = quote + 100.00;
					}
					else if (age < 25 || age > 100)
					{
						quote = quote + 25.00;
					}

					//Car Yr
					if (carYear == 1999 || carYear > 2015)
					{
						quote = quote + 25.00;
					}

					//Check Car Model for Porsche 
					if (carMake == "porsche")
					{
						quote = quote + 25.00;
						if (carModel == "911 carrera")
						{
							quote = quote + 25.00;
						}
					}

					//Check for Speeding Tickets	
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

					//Full Coverage Check
					if (fullCoverage == true)
					{
						quote = quote + (quote * .50);
					}

					customerInfo.Quote = quote;

					db.CustomerInfoes.Add(customerInfo);
					db.SaveChanges();

					int userId = customerInfo.Id;

					return RedirectToAction("Quote", new { Id = userId });
				}
			}
		}
	}
}

