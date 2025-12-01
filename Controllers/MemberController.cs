using Microsoft.AspNetCore.Mvc;
// lägger till modellen
using Laboration_3.Models;
using AspNetCoreGeneratedDocument;
namespace Laboration_3.Controllers
{
    public class MemberController : Controller
    {
        /*
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
            return View("CreateMember");
        }
        */

        // metod för att visa medlemmar
        public ActionResult SelectMembers(string search, string sortField, string sortOrder)
        {
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            List<MemberDetails> memberList;
            if (!string.IsNullOrEmpty(search))
            {
                memberList = memberMethods.SearchMembers(search, out error);
                ViewBag.search = search;
                if(memberList.Count == 0)
                {
                    ViewBag.msg = "No member found!";
                }
            }
            else
            {
                memberList = memberMethods.GetMemberDetailsList(out error);
            }

            //sorteringshantering
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField)
                {
                    case "FirstName":
                        memberList = sortOrder == "DESC"  
                        ? memberList.OrderByDescending(m => m.FirstName).ToList()
                        : memberList.OrderBy(m => m.FirstName).ToList();
                        break;

                    case "Age":
                        memberList = sortOrder == "DESC"
                            ? memberList.OrderByDescending(m => m.Age).ToList()
                            : memberList.OrderBy(m => m.Age).ToList();
                        break;

                    case "Score":
                        memberList = sortOrder == "DESC" 
                            ? memberList.OrderByDescending(m => m.Score).ToList()
                            : memberList.OrderBy(m => m.Score).ToList();
                        break;
                }
            }
            ViewBag.sortField = sortField;
            ViewBag.sortOrder = sortOrder;
            ViewBag.error = error;
            return View(memberList);
        }


        // metod för att visa en medlem
        [HttpGet]
        public ActionResult UpdateMember(int id)
        {
            MemberDetails memberDetails = new MemberDetails();
            MemberMethods memberMethods = new MemberMethods();
            string error = "";
            memberDetails = memberMethods.GetMemberDetails(id, out error);
            ViewBag.error = error;
            return View(memberDetails);
        }

        // metod för att uppdatera databasen
        [HttpPost]
        public IActionResult UpdateMember(MemberDetails memberDetails, int memberId)
        {
            // modellvalidering
            if (!ModelState.IsValid)
            {
                return View(memberDetails);
            }
            MemberMethods memberMethods = new MemberMethods();
            string errormsg = "";

            // uppdatera medlemmen
            int result = memberMethods.UpdateMemberDetails(memberDetails, memberId, out errormsg);

            if (result == 1)
            {
                return RedirectToAction("SelectMembers");
            }
            ViewBag.error = errormsg;
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
            // 1. Modellvalidering
            if (!ModelState.IsValid)
            {
                return View(memberDetails);
            }

            MemberMethods memberMethods = new MemberMethods();
            string errormsg = "";

            // 2. Check om Email/Phone redan finns
            if (memberMethods.MemberExists(memberDetails.Email, memberDetails.Phone, out errormsg))
            {
                ViewBag.error = "A member with the same email or phone number already exists!";
                return View(memberDetails);
            }

            // 3. Insert
            int result = memberMethods.InsertMember(memberDetails, out errormsg);

            if (result == 1)
            {
                return RedirectToAction("SelectMembers");
            }

            // 4. Om något gick fel
            ViewBag.error = errormsg;
            return View(memberDetails);
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

        // metod för filtrera member
        [HttpGet]
        public IActionResult FilterMembers(string firstName, string lastName, int? minAge, int? maxAge, int? minScore, int? maxScore)
        {
            MemberMethods memberMethods = new MemberMethods();
            string errormsg;
            var filtered = memberMethods.FilterMembers(firstName, lastName, minAge, maxAge, minScore, maxScore, out errormsg);
            ViewBag.error = errormsg;
            return View("FilterMembers", filtered);
        }

        // visa filtervyn
        [HttpGet]
        public IActionResult FilterView()
        {
            return View("FilterMembers");
        }
    }
}
