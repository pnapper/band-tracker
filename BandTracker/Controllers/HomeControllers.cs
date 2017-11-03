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
  }
}
