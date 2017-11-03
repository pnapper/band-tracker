using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Tests
{

  [TestClass]

  public class BandTests : IDisposable
  {
    public BandTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfNamesAreTheSame_Band()
    {
      // Arrange, Act
      Band firstBand = new Band("Depeche Mode");
      Band secondBand = new Band("Depeche Mode");

      // Assert
      Assert.AreEqual(firstBand, secondBand);
    }

    [TestMethod]
    public void Save_SavesToDatabase_BandList()
    {
      //Arrange
      Band testBand = new Band("Depeche Mode");

      //Act
      testBand.Save();
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Band testBand = new Band("Depeche Mode");
      testBand.Save();

      //Act
      Band savedBand = Band.GetAll()[0];

      int result = savedBand.GetId();
      int testId = testBand.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsBandInDatabase_Band()
    {
      //Arrange
      Band testBand = new Band("Depeche Mode", 1);
      testBand.Save();

      //Act
      Band result = Band.Find(testBand.GetId());

      //Assert
      Assert.AreEqual(testBand, result);
    }
  }
}
