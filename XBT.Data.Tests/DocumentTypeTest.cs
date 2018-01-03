using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XBT.Model;

namespace XBT.Data.Tests
{
    /// <summary>
    /// Summarises Create, Read and Update tests on the DocumentType model
    /// </summary>
    [TestFixture]
    public class DocumentTypeTest
    {
        [TestFixtureSetUp]
        public void InitializeTest()
        {
            System.Data.Entity.Database.SetInitializer<XbtContext>(new XbtContextInitializer());
        }

        [Test]
        public void CreateDocumentType()
        {
            using (var uow = new XbtContext())
            {
                var repo = new GenericRepository<DocumentGroup>(uow);
                //Create new document group
                var newDocumentGroup = new DocumentGroup()
                {
                    Name = "Credit Documents",
                    Description = "Documents used in the credit department"
                };

                repo.Insert(newDocumentGroup);

                //Retrieve Inserted document 
                var insertedDocumentGroup = repo.GetById(newDocumentGroup.Id);

                var repoDocType = new GenericRepository<DocumentType>(uow);

                //Create new document type
                var newDocumentType = new DocumentType()
                {
                    Name = "Document Type",
                    Description = "Document type description",
                    DocumentDirectory = @"\TestDocuments",
                    DocumentGroup = insertedDocumentGroup
                };

                repoDocType.Insert(newDocumentType);

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

                //Retrieve saved document using a new unit of work and repository
                //to ensure values are retreived from database instead of in memory graph
                var uow1 = new XbtContext();
                var repo1 = new GenericRepository<DocumentType>(uow1);
                var savedDocumentType = repo1.GetById(newDocumentType.Id);

                Assert.AreEqual(newDocumentType.Name,savedDocumentType.Name, "Unexpected Document Type");
            }
        }

        /// <summary>
        /// Summarises Updates on document type models
        /// </summary>

        [Test]
        public void UpdateDocumentType()
        {
            DocumentType existingdocumentType;

            using (var uow = new XbtContext())
            {
                var repo = new GenericRepository<DocumentType>(uow);

                //[retrieve any existing document type]
                existingdocumentType = repo.Get().FirstOrDefault();

                //Check if there was any document type retreived
                Assert.IsNotNull(existingdocumentType);

                //Create new document group

                var repoDoc = new GenericRepository<DocumentGroup>(uow);
                var newDocumentGroup = new DocumentGroup()
                {
                    Name = "updates Credit Documents",
                    Description = "updated Documents used in the credit department"
                };

                repoDoc.Insert(newDocumentGroup);
                uow.SaveChanges();
                Assert.IsNotNull(newDocumentGroup);

                //edit an existing document type properties
                existingdocumentType.Name = "New document type name";
                existingdocumentType.Description = "New document type description";
                existingdocumentType.DocumentGroup = newDocumentGroup;

                repo.Update(existingdocumentType);

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

            //Retreive the modified document type using a new unit of work and repository 
            //to ensure values are retreived from database instead of in memory graph
            var uow1 = new XbtContext();

            var repo1 = new GenericRepository<DocumentType>(uow1);

            var modifiedDocumentType = repo1.Get(d => d.Id == existingdocumentType.Id,
                includeProperties: "DocumentGroup").FirstOrDefault();

            Assert.NotNull(modifiedDocumentType); 

            Assert.AreEqual(modifiedDocumentType.Name, existingdocumentType.Name);
            Assert.AreEqual(modifiedDocumentType.Description, existingdocumentType.Description);
            Assert.AreEqual(modifiedDocumentType.DocumentGroup.Id, existingdocumentType.DocumentGroup.Id);
        }
    }
}
