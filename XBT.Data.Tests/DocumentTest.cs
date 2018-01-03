using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XBT.Model;

namespace XBT.Data.Tests
{
    [TestFixture]
    public class DocumentTest
    {
        [TestFixtureSetUp]
        public void InitializeTest()
        {
            System.Data.Entity.Database.SetInitializer<XbtContext>(new XbtContextInitializer());
        }

        [Test]
        public void Should_Create_Document()
        {
            using (var uow = new XbtContext())
            {
                //retrieve document owner
                var docOwner = from rc in uow.DocumentOwners.OfType<Customer>()
                                        select rc;
                var result = docOwner.FirstOrDefault();
                
                //retrieve document type
                int doctypeId = 1;
                var repoDocType = new GenericRepository<DocumentType>(uow);
                var docType = repoDocType.GetById(doctypeId);

                var newDocument = new Document()
                {
                    Description = "New Doc Description",
                    SerialNo = "35-899-hello-world",
                    FileName = "New Document-hello-world",
                    DocumentType = docType,
                    DocumentOwner = result
                };

                uow.Documents.Add(newDocument);
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

                
                //Assert.IsNotNull(newDocument);

                //Retreive the inserted document using a new unit of work and repository 
                //to ensure values are retreived from database instead of in memory graph
                var uow1 = new XbtContext();
                var repo1 = new GenericRepository<Document>(uow1);

                var insertedDocument = repo1.GetById(newDocument.Id);
                Assert.NotNull(insertedDocument); 
            }
        }
    }
}
