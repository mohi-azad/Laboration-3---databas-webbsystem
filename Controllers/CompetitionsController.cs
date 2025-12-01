using Laboration_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Laboration_3.Controllers
{
    public class CompetitionController : Controller
    {
        public IActionResult Index()
        {
            string errormsg;
            CompetitionsMethods compMethods = new CompetitionsMethods();
            var competitions = compMethods.GetMemberToTournament(out errormsg);
            ViewBag.error = errormsg;
            return View("~/Views/Competitions/Index.cshtml", competitions);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("~/Views/Competitions/Add.cshtml"); 
        }


        // POST: Lägg till medlem i turnering
        [HttpPost]
        public IActionResult Add(int memberId, int tournamentId)
        {
            string errormsg = "";
            CompetitionsMethods compMethods = new CompetitionsMethods();

            int result = compMethods.AddCompetition(memberId, tournamentId, out errormsg);

            if (result > 0)
                return RedirectToAction("Index");

            ViewBag.error = errormsg;
            return View("~/Views/Competitions/Add.cshtml");
        }

        // POST: Ta bort medlem från turnering
        [HttpPost]
        public IActionResult Remove(int memberId, int tournamentId)
        {
            string errormsg = "";
            CompetitionsMethods compMethods = new CompetitionsMethods();

            int result = compMethods.RemoveCompetition(memberId, tournamentId, out errormsg);

            if (result > 0)
                return RedirectToAction("Index");

            ViewBag.error = errormsg;
            return RedirectToAction("Index");
        }
    }
}
