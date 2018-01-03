using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace XBT.Business.Tests
{
  /// <summary>
  /// Test methods for the document archiving process in the DocumentArchivingProcess business class
  /// </summary>
  [TestFixture]
  public class DocumentArchivingTests
  {
    /// <summary>
    /// Test file retrieval functionality from the Windows file system. 
    /// </summary>
    [Test]
    public void GetFileNamesInDirectory()
    {
      var documentArchiveProcess = new DocumentArchivingProcess();

      //Use a docuemnt type that exists. The DocumentDirectory property for this document type will be used in the businessclass
      //to retreive file names for the specified directory
      const int documentTypeId = 1;

      //retrieve files in directory
      var fileQueue = documentArchiveProcess.GetFileNamesInDirectory(documentTypeId);
      
      //Check if there are files in the queue
      Assert.NotNull(fileQueue);

      for (int i = 1; i <= fileQueue.Count(); i++ )
      {
        //Get first items in queue and write to debug window
        var fileName = (string)fileQueue.Dequeue();
        System.Diagnostics.Debug.WriteLine(fileName);
      }
    }
  }
}
