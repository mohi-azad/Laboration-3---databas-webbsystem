using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Channels;
using Laboration_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Laboration_3.Controllers
{
    public class TournamentController : Controller
    {


        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateTournament");
        }

        [HttpPost]
        public ActionResult Create(Tournament tournamentDetails)
        {
            string errormsg = "";
            TournamentMethods tournamentMethods = new TournamentMethods(); // skapa objekt
            int i = tournamentMethods.InsertTournament(tournamentDetails, out errormsg);
            if (i == 1)
            {
                return RedirectToAction("SelectTournaments");
            }
            ViewBag.error = errormsg;
            return View("CreateTournament", tournamentDetails);
        }

        public IActionResult SelectTournaments()
        {
            string errormsg = "";
            TournamentMethods tournamentMethods = new TournamentMethods();
            var tournaments = tournamentMethods.GetTournamentList(out errormsg); // behöver implementeras i DAL
            ViewBag.error = errormsg;
            return View(tournaments);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            string errormsg = "";
            TournamentMethods t= new TournamentMethods();
            var tournament= t.GetTournament(id, out errormsg);
            if (tournament == null)
            {
                return NotFound();
            }
            ViewBag.error=errormsg;
            return View("EditTournament", tournament);
        }

        [HttpPost]
        public IActionResult Edit(Tournament tournament)
        {
            string errormsg = "";
            TournamentMethods t= new TournamentMethods();
            int i= t.UpdateTournament(tournament, out errormsg);
            if (i == 1)
            {
                return RedirectToAction("SelectTournaments");
            }
            ViewBag.error = errormsg;
            return View("EditTournament", tournament);
        }

        // delete-metoden
        [HttpGet]
        public IActionResult Delete(int id)
        {
            string errormsg = "";
            TournamentMethods t=new TournamentMethods();
            var tournament=t.GetTournament(id,out errormsg);
            if(tournament == null)
            {
                return NotFound();
            }
            ViewBag.error=errormsg;
            return View("DeleteTournament", tournament);
        }

        [HttpPost]
        public IActionResult Deleted(int TournamentId)
        {
            string errormsg = "";
            TournamentMethods t = new TournamentMethods();
            int i = t.DeleteTournament(TournamentId, out errormsg);
            if (i == 1)
            {
                return RedirectToAction("SelectTournaments");
            }
            ViewBag.error = errormsg;
            var tournament = t.GetTournament(TournamentId, out errormsg);
            return View("DeleteTournament", tournament);
        }

    }
}


