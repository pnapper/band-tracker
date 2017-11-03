using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Bands
  {
    private string _bandName;
    private int _id;
    public Bands(string bandName, int id = 0)
    {
      _bandName = bandName;
      _id = id;
    }
  }
}
