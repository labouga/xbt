using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBT.Data;
using XBT.Model;

namespace XBT.Business
{
    /// <summary>
    /// Contains methods for handling customer operations
    /// </summary>
    public class CustomerBL
    {
        private XbtContext uow;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerBL()
        {
            uow = new XbtContext();
        }

        /// <summary>
        /// Retrieves a customer by customer id
        /// </summary>
        /// <param name="customerId"></param>
        public Customer GetCustomer(int customerId = 0)
        {
            return uow.DocumentOwners.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);
        }

        /// <summary>
        /// Retrieves a list of customers whose name, number matches with the searchString
        /// </summary>
        /// <param name="searchString"></param>
        public IEnumerable<Customer> SearchCustomer(string searchString)
        {
            return uow.DocumentOwners.OfType<Customer>()
                .Where(
                    c => SqlFunctions.StringConvert((double)c.CustomerNo).Contains(searchString) || 
                    c.FirstName.ToLower().Contains(searchString.ToLower()) || 
                    c.LastName.ToLower().Contains(searchString.ToLower())
                ).ToList();
        }

      /// <summary>
      /// Returns documents belonging to a specific customer
      /// </summary>
      /// <param name="customerNumber"></param>
      /// <returns>List of documents </returns>
        public IEnumerable<Document> GetCustomerDocuments(int customerNumber = 0)
        {
            var repo = new GenericRepository<DocumentOwner>(uow);

            //Retrieve customer
            var customer = repo.Get(includeProperties: "Documents").FirstOrDefault(d => d.Id == customerNumber);

            //Return customer documents
            return customer.Documents.ToList();
        } 
    }
}
