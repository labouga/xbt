using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using NUnit.Framework;
using XBT.Model;

namespace XBT.Data.Tests
{
    [TestFixture]
    class CustomerTest
    {
        [Test]
        public void AddNewCustomer()
        {
            var customer = new Customer()
            {
                CustomerNo = 235,
                FirstName = "Timo",
                LastName = "Lukyamuzi",
                DateOfBirth = DateTime.Now.ToLocalTime(),
                Email = "shaban@laboremus.no",
                Address = "Ebbs"
            };

            using (var uow = new XbtContext())
            {
                uow.DocumentOwners.Add(customer);
                try
                {
                    uow.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    //Retrieve validation errors
                    ex.EntityValidationErrors.ToList().ForEach
                    (
                        v =>
                        {
                            v.ValidationErrors.ToList().ForEach
                                (
                                    e =>
                                    {
                                        System.Diagnostics.Debug.WriteLine(e.ErrorMessage);
                                    }
                                );
                        }
                    );

                    //If we have reached here the test has failed
                    Assert.Fail("Test failed");
                }
            }

        }

        [Test]
        public void RetrieveCustomer()
        {
            using (var uow = new XbtContext())
            {
                var retrievedCustomer = from rc in uow.DocumentOwners.OfType<Customer>()
                                        select rc;
                var result = retrievedCustomer.ToList();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Should_Retreive_Documents_By_Customer()
        {
            using (var uow = new XbtContext())
            {
                var repo = new GenericRepository<DocumentOwner>(uow);
                var customer = repo.Get(includeProperties: "Documents").FirstOrDefault(d => d.Id == 1);

                try
                {
                    if (customer != null)
                    {
                        var documents = customer.Documents.ToList();
                        Assert.IsTrue(documents.Count() > 1);
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail("Customer has zero documents");
                }
            }
        }

        [Test]
        public void should_retrieve_documentOwner_of_type_customer()
        {
            using (var uow = new XbtContext())
            {
                var repo = new GenericRepository<DocumentOwner>(uow);
                var docOwner = repo.Get(includeProperties: "Documents").FirstOrDefault(d => d.Id == 1);
                try
                {
                    if (docOwner != null)
                    {
                        Assert.IsTrue(docOwner is Customer);
                    }
                }
                catch (Exception)
                {
                    Assert.Fail("Document Owner is not of type Customer");
                }
            }
        }
    }
}
