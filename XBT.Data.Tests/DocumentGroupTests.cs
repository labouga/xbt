using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XBT.Data;
using XBT.Model;

namespace XBT.Data.Tests
{
  [TestFixture]
  public class DocumentGroupTests
  {
    [TestFixtureSetUp]
    public void InitializeTest()
    {
      System.Data.Entity.Database.SetInitializer<XbtContext>(new XbtContextInitializer());
    }

    /// <summary>
    /// Test creation of a new document group and persist in a SQL database
    /// </summary>
    [Test]
    public void CreateDocumentGroup()
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
        var repo1 = new GenericRepository<DocumentGroup>(uow1);
        var savedDocumentGroup = repo1.GetById(newDocumentGroup.Id);

        //Establish if the saved document group has the same name as the saved one
        Assert.AreEqual(savedDocumentGroup.Name, newDocumentGroup.Name);
      };
    }

    /// <summary>
    /// Test modification and update of document group 
    /// </summary>
    [Test]
    public void UpdateDocumentGroup()
    {
      DocumentGroup existingDocumentGroup; 

      using (var uow = new XbtContext())
      {
        var repo = new GenericRepository<DocumentGroup>(uow);

        //[retrieve any existing document group]
        existingDocumentGroup = repo.Get().FirstOrDefault();

        //Check if there was any document group retreived
        Assert.IsNotNull(existingDocumentGroup);

        //edit an existing class
        existingDocumentGroup.Name = "Corporate Documents";
        existingDocumentGroup.Description = "Documents used by the corporate department";

        repo.Update(existingDocumentGroup);

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
      };

      //Retreive the modified document group using a new unit of work and repository 
      //to ensure values are retreived from database instead of in memory graph
      var uow1 = new XbtContext();
      var repo1 = new GenericRepository<DocumentGroup>(uow1);
      var modifiedDocumentGroup = repo1.GetById(existingDocumentGroup.Id);

      //Establish if the changes to the document group were pushed to the database
      Assert.AreEqual(modifiedDocumentGroup.Name, "Corporate Documents");
      Assert.AreEqual(modifiedDocumentGroup.Description, "Documents used by the corporate department");
    }
  }
}
