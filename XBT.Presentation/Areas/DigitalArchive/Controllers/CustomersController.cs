using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBT.Business;

namespace XBT.Presentation.Areas.DigitalArchive.Controllers
{
  /// <summary>
  /// Contains all the methods responsible for handling the customers
  /// </summary>
  public class CustomersController : Controller
  {

    private CustomerBL _customerBL;

    public CustomersController()
    {
      _customerBL = new CustomerBL();
    }

    /// =============================================================================
    /// GET: /DigitalArchive/Customers/GetAll
    /// =============================================================================
    /// <summary>
    /// Retrieves all the customers from the customers database via a third-party API
    /// </summary>
    /// <returns>list of all customers</returns>
    [HttpGet]
    public ActionResult FetchAll()
    {
      // TODO: Implement functionality for retrieving customers using the API

      return View();
    }

    /// =============================================================================
    /// GET: /DigitalArchive/Customers/Search
    /// =============================================================================
    /// <summary>
    /// Searches for a customer based on a search term (keyword)
    /// </summary>
    /// <returns>list of the top 10 matches</returns>
    [HttpGet]
    public JsonResult Search(string keyword = null)
    {
      // TODO: Implement functionality for searching for a customer
      var customers = _customerBL.SearchCustomer(keyword);

      var customerList = new Dictionary<string, string>();

      foreach (var customer in customers)
      {
        //customer only unique customers are added
        if (!customerList.ContainsKey(customer.CustomerNo.ToString()))
          customerList.Add(customer.CustomerNo.ToString(), customer.FirstName + " " + customer.LastName);
      }

      return Json(customerList.ToList(), JsonRequestBehavior.AllowGet);
    }

  }
}
