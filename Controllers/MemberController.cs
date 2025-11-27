using Microsoft.AspNetCore.Mvc;
// lägger till modellen
using Laboration_3.Models;
using AspNetCoreGeneratedDocument;
namespace Laboration_3.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult InsertMember()
        {
            MemberDetails memberDetails = new MemberDetails();
            MemberMethods memberMethods = new MemberMethods();
            int i = 0;
            string error = "";

            memberDetails.FirstName = "Mattias";
            memberDetails.LastName = "Andersson";
            memberDetails.Email = "mattias.andersson@gmail.com";
            memberDetails.Phone = "073 88 88 888";
            memberDetails.Age = 35;
            memberDetails.Score = 1800;

            i = memberMethods.InsertMember(memberDetails, out error);
            ViewBag.error = error;
            ViewBag.antal = i;
            return View();
        }

        // metod för att visa medlemmar
        public ActionResult SelectMembers()
        {
            List<MemberDetails> memberDetailsList = new List<MemberDetails>();
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            memberDetailsList = memberMethods.GetMemberDetailsList(out error);

            // ViewBag.antal = HttpContext.Session.GetString("antal");
            ViewBag.error = error;
            return View(memberDetailsList);
        }


        // metod för att visa en medlem
        [HttpGet]
        public ActionResult UpdateMember(int id)
        {
            MemberDetails memberDetails = new MemberDetails();
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            memberDetails = memberMethods.GetMemberDetails(id, out error);

            // ViewBag.antal = HttpContext.Session.GetString("antal");
            ViewBag.error = error;
            return View(memberDetails);
        }

        // metod för att uppdatera databasen
        [HttpPost]
        public ActionResult UpdateMember(MemberDetails memberDetails, int memberId)
        {
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            int Result = memberMethods.UpdateMemberDetails(memberDetails, memberId, out error);
            if(Result == 1)
            {
                return RedirectToAction("SelectMembers");
            }
            // ViewBag.antal = HttpContext.Session.GetString("antal");
            ViewBag.error = error;
            return View(memberDetails);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateMember");
        }
        [HttpPost]
        public IActionResult Create(MemberDetails memberDetails)
        {
            string errormsg = "";
            MemberMethods memberMethods = new MemberMethods();
            if (memberMethods.MemberExists(memberDetails.Email, memberDetails.Phone, out errormsg))
            {
                ViewBag.error = "A member with the same email or phone number already exists!";
                return View("CreateMember", memberDetails);
            }
            int result = memberMethods.InsertMember(memberDetails, out errormsg);
            if(result == 1)
            {
                return RedirectToAction("SelectMembers");
            }
            ViewBag.error = errormsg;
            return View("CreateMember", memberDetails);
        }

        // metod för att ta bort en medlem
        [HttpGet]
        public IActionResult Delete(int id)
        {
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            MemberDetails memberDetails = memberMethods.GetMemberDetails(id, out error);
            if(memberDetails == null)
            {
                ViewBag.error = "Member not found!";
                return RedirectToAction("SelectMembers");
            }
            return View("DeleteMember", memberDetails);
        }

        [HttpPost]
        public IActionResult Deleted(int MemberId)
        {
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            int i = memberMethods.DeleteMember(MemberId, out error);
            if(i > 0)
            {
                return RedirectToAction("SelectMembers");
            }
            else
            {
                ViewBag.error = error;
                MemberDetails memberDetails = memberMethods.GetMemberDetails(MemberId, out error);
                return View("DeleteMember", memberDetails);
            }
        }
    }
}
