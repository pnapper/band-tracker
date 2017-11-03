using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Band
  {
    private string _bandName;
    private int _id;
    public Band(string bandName, int id = 0)
    {
      _bandName = bandName;
      _id = id;
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = (this.GetId() == newBand.GetId());
        bool nameEquality = (this.GetBandName() == newBand.GetBandName());
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetBandName()
    {
      return _bandName;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        Band newBand = new Band(bandName, bandId);
        allBands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
