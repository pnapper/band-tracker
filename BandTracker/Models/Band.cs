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

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (name) VALUES (@name);";

      MySqlParameter bandName = new MySqlParameter();
      bandName.ParameterName = "@name";
      bandName.Value = this._bandName;
      cmd.Parameters.Add(bandName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Band Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int bandId = 0;
      string bandName = "";

      while (rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
      }

      Band newBand= new Band(bandName, bandId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newBand;
    }

    public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);";

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = newVenue.GetId();
      cmd.Parameters.Add(venue_id);

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = _id;
      cmd.Parameters.Add(band_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Venue> GetVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venue_id FROM bands_venues WHERE band_id = @bandId;";

      MySqlParameter bandIdParameter = new MySqlParameter();
      bandIdParameter.ParameterName = "@bandId";
      bandIdParameter.Value = _id;
      cmd.Parameters.Add(bandIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> venueIds = new List<int> {};
      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        venueIds.Add(venueId);
      }
      rdr.Dispose();

      List<Venue> venues = new List<Venue> {};
      foreach (int venueId in venueIds)
      {
        var venueQuery = conn.CreateCommand() as MySqlCommand;
        venueQuery.CommandText = @"SELECT * FROM venues WHERE id = @VenueId;";

        MySqlParameter venueIdParameter = new MySqlParameter();
        venueIdParameter.ParameterName = "@VenueId";
        venueIdParameter.Value = venueId;
        venueQuery.Parameters.Add(venueIdParameter);

        var venueQueryRdr = venueQuery.ExecuteReader() as MySqlDataReader;
        while(venueQueryRdr.Read())
        {
          int thisVenueId = venueQueryRdr.GetInt32(0);
          string venueName = venueQueryRdr.GetString(1);
          Venue foundVenue = new Venue(venueName, thisVenueId);
          venues.Add(foundVenue);
        }
        venueQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return venues;
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
