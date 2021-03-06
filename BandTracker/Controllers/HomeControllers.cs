using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;

namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
    // HOME AND MENU ROUTES
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet("/band")]
    public ActionResult Bands()
    {
      List<Band> allBands = Band.GetAll();
      return View(allBands);
    }
    [HttpGet("/venue")]
    public ActionResult Venues()
    {
      List<Venue> allVenues = Venue.GetAll();
      return View(allVenues);
    }

    //NEW BAND
    [HttpGet("/bands/new")]
    public ActionResult BandForm()
    {
      return View();
    }

    [HttpPost("/bands/new")]
    public ActionResult BandCreate()
    {
      Band newBand = new Band(Request.Form["band-name"]);
      newBand.Save();
      return View("Success");
    }

    //NEW VENUE
    [HttpGet("/venues/new")]
    public ActionResult VenueForm()
    {
      return View();
    }

    [HttpPost("/venues/new")]
    public ActionResult VenueCreate()
    {
      Venue newVenue = new Venue(Request.Form["venue-name"]);
      newVenue.Save();
      return View("Success");
    }

    //ONE BAND
    [HttpGet("/bands/{id}")]
    public ActionResult BandDetail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Band selectedBand = Band.Find(id);
      List<Venue> BandVenues = selectedBand.GetVenues();
      List<Venue> AllVenues = Venue.GetAll();
      model.Add("band", selectedBand);
      model.Add("bandVenues", BandVenues);
      model.Add("allVenues", AllVenues);
      return View("BandDetail", model);
    }

    //ONE VENUE
    [HttpGet("/venues/{id}")]
    public ActionResult VenueDetail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Venue SelectedVenue = Venue.Find(id);
      List<Band> VenueBands = SelectedVenue.GetBands();
      List<Band> AllBands = Band.GetAll();
      model.Add("venue", SelectedVenue);
      model.Add("venueBands", VenueBands);
      model.Add("allBands", AllBands);
      return View("VenueDetail", model);
    }

    //ADD BAND TO VENUE
    [HttpPost("/venues/{venueId}/bands/new")]
    public ActionResult VenueAddBand(int venueId)
    {
      Venue venue = Venue.Find(venueId);
      Band band = Band.Find(Int32.Parse(Request.Form["band-id"]));
      venue.AddBand(band);
      return View("Success");
    }

    //ADD VENUE TO BAND
    [HttpPost("/bands/{bandId}/venues/new")]
    public ActionResult BandAddVenue(int bandId)
    {
      Band band = Band.Find(bandId);
      Venue venue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
      band.AddVenue(venue);
      return View("Success");
    }

    //EDIT VENUE
    [HttpGet("/venues/{id}/edit")]
    public ActionResult VenueEdit(int id)
    {
      Venue thisVenue = Venue.Find(id);

      return View(thisVenue);
    }

    [HttpPost("/venues/{id}/edit")]
    public ActionResult VenueEditConfirm(int id)
    {
      Venue thisVenue = Venue.Find(id);
      thisVenue.UpdateName(Request.Form["new-name"]);

      return RedirectToAction("Index");
    }

    [HttpGet("/{name}/{id}/venue/delete")]
    public ActionResult VenueDelete(int id)
    {
      // Cuisine is selected as an object
      Venue thisVenue = Venue.Find(id);
      thisVenue.DeleteVenue();

      return RedirectToAction("Index");
    }
  }
}
