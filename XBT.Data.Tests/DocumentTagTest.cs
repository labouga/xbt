using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XBT.Model;

namespace XBT.Data.Tests
{
    /// <summary>
    /// Summary of document tag tests
    /// </summary>

    [TestFixture]
    public class DocumentTagTest
    {
        [TestFixtureSetUp]
        public void InitializeTest()
        {
            System.Data.Entity.Database.SetInitializer<XbtContext>(new XbtContextInitializer());
        }

        [Test]
        public void CreateDocumentTag()
        {

            DocumentTag newDocTag = new DocumentTag();

            using (var uow = new XbtContext())
            {
                var repoDocGroup = new GenericRepository<DocumentGroup>(uow);

                var newDocumentGroup = new DocumentGroup()
                {
                    Name = "Credit Documents",
                    Description = "Documents used in the credit department"
                };

                repoDocGroup.Insert(newDocumentGroup);

                //Retrieve Inserted document 

                var insertedDocumentGroup = repoDocGroup.GetById(newDocumentGroup.Id);

                var repoDocType = new GenericRepository<DocumentType>(uow);

                var newDocumentType = new DocumentType()
                {
                    Name = "Document Type 3",
                    Description = "Document type description 3",
                    DocumentGroup = insertedDocumentGroup,
                    DocumentDirectory = @"\TestDocuments",
                };

                repoDocType.Insert(newDocumentType);

                var repoDocTag = new GenericRepository<DocumentTag>(uow);

                var insertedDocumentType = repoDocType.GetById(newDocumentType.Id);

                newDocTag.Name = "New Doc Tag";
                newDocTag.Description = "New Doc Tag Description";
                newDocTag.DocumentTypes = new List<DocumentType>() { insertedDocumentType };

                repoDocTag.Insert(newDocTag);
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

            //Retreive the modified document tag using a new unit of work and repository 
            //to ensure values are retreived from database instead of in memory graph
            var uowDoctag = new XbtContext();

            var repoExistingDocTag = new GenericRepository<DocumentTag>(uowDoctag);
            var retrievedDocumentTag = repoExistingDocTag.Get().FirstOrDefault();

            Assert.AreEqual(retrievedDocumentTag.Name, newDocTag.Name);
        }

        [Test]
        public void OneDocumentTagWithManyDocumentTypes()
        {
            DocumentType existingdocumentType;
            DocumentTag existingdocumentTag;
       
            using (var uow = new XbtContext())
            {
                //var repoDocType = new GenericRepository<DocumentType>(uow);
                //[retrieve any existing document type]

                existingdocumentType = uow.DocumentTypes.Include("DocumentTags").FirstOrDefault();
                Assert.IsTrue(existingdocumentType.DocumentTags.Count()>1,"Document Type should atleast have more than one tag");

                existingdocumentTag = uow.DocumentTags.Include("DocumentTypes").FirstOrDefault();
                Assert.IsTrue(existingdocumentTag.DocumentTypes.Count() > 1, "Document Tag should atleast have more than one type");
              }
        }

        [Test]
        public void Should_Retrieve_DocTags_By_DocTypeId()
        {
            int docTypeId = 1;

            using (var uow = new XbtContext())
            {
                //Retreive document type by id
                var docType = uow.DocumentTypes.Include("DocumentTags").SingleOrDefault(d => d.Id == docTypeId);

                //retrieve doc tags related to document type retrieved
                var docTags = docType.DocumentTags.ToList();

                //Assert
                Assert.IsTrue(docTags.Count()>2);

            }
        }

    }
}
