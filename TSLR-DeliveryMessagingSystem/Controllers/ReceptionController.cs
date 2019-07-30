using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSLR_DeliveryMessagingSystem.Models;
using TSLR_DeliveryMessagingSystem.ViewModels;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TSLR_DeliveryMessagingSystem.Controllers
{
    public class ReceptionController : Controller
    {
        private TSLR_MasterEntities1 db = new TSLR_MasterEntities1();

        // GET: Reception
        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "Name")
            {
                return View(db.StudentDetails.Where(x => x.Contact_Number_ == search || search == null).ToList());
            }
            else
            {
                return View(db.StudentDetails.Where(x => x.Name.Contains(search) || search == null).ToList());
            }
        }

        public ActionResult Message(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetail student = db.StudentDetails.Find(id);
            if (student == null)

            {
                return HttpNotFound();
            }

            ReceptionVM receptionViewModel = new ReceptionVM();
            receptionViewModel.StudentDetail = db.StudentDetails.Include(x => x.Company).Include(x => x.Message).ToList().Single(x => x.Id == id);


            receptionViewModel.ListCompany = db.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.Company_Name,
                Selected = c.Company_Name.Equals("")
            });
            receptionViewModel.ListMessage = db.Messages.Select(s => new SelectListItem
            {
                Value = s.MessageId.ToString(),
                Text = s.Body,
                Selected = s.Body.Equals("")
            });
            if (receptionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(receptionViewModel);

        }

        // POST: Message
        // HELP: https://forums.asp.net/t/2150152.aspx?+Html+DropDownListFor+with+SelectList+method+on+update
        // The post back action needs to recreate them if its view requires them. Note: only model data mapped to a form element is included in the post back.Selects only post the selected values.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Message([Bind(Include = "StudentDetail,Company,Message,ListCompany,ListMessage,CompanyName,Body")] ReceptionVM messageDetail, int? id)
        {
            if (ModelState.IsValid)
            {                
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentDetail studentInDb = db.StudentDetails.Find(id);
                if (studentInDb == null)
                {
                    return HttpNotFound();
                }

                
                ReceptionVM messageCompilation = new ReceptionVM();
                messageCompilation.StudentDetail = db.StudentDetails.Single(x => x.Id == id);

                ReceptionVM messageCompilationCompany = new ReceptionVM();
                messageCompilationCompany.Company = db.Companies.Single(x => x.CompanyId == messageDetail.Company.CompanyId);

                ReceptionVM messageCompilationMessage = new ReceptionVM();
                messageCompilationMessage.Message = db.Messages.Single(x => x.MessageId == messageDetail.Message.MessageId);


                const string accountSid = "AC33abb4756a7d9ac7561a3ab45eb171bb";
                const string authToken = "a27427fc290424bb072ed27ece5e4ff3";

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    body: messageCompilationMessage.Message.Body,
                    from: new Twilio.Types.PhoneNumber("+447480802108"),
                    to: new Twilio.Types.PhoneNumber(messageCompilation.StudentDetail.Contact_Number_));

                return RedirectToAction("Index");
                }

            return View();
        }


        // GET: Reception/Create
        public ActionResult Create()
        {
            ViewBag.Building_Id = new SelectList(db.Buildings, "BuildingId", "Building_No");
            ViewBag.Company_ID = new SelectList(db.Companies, "CompanyId", "CompanyName");
            ViewBag.Message_ID = new SelectList(db.Messages, "MessageId", "Title");
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No");
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No");
            return View();
        }

        // POST: Reception/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Building_No,Room_No_,Name,Email_Address_,Contact_Number_,University_of_Study_,Course_,Student_ID_")] StudentDetail studentDetail)
        {
            if (ModelState.IsValid)
            {
                if (studentDetail.Id == 0)
                {
                    int dbStudentRowcount = db.StudentDetails.Count();
                    int studentIdCountAnd1 = dbStudentRowcount + 1;
                    studentDetail.Id = studentIdCountAnd1;


                    db.StudentDetails.Add(studentDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                db.StudentDetails.Add(studentDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        // GET: Reception/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetail studentDetail = db.StudentDetails.Find(id);
            if (studentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Building_Id = new SelectList(db.Buildings, "BuildingId", "Building_No", studentDetail.BuildingId);
            ViewBag.Company_ID = new SelectList(db.Companies, "CompanyId", "CompanyName", studentDetail.CompanyId);
            ViewBag.Message_ID = new SelectList(db.Messages, "MessageId", "Title", studentDetail.MessageId);
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No", studentDetail.Id);
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No", studentDetail.Id);
            return View(studentDetail);
        }

        // POST: Reception/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Building_No,Room_No_,Name,Email_Address_,Contact_Number_,University,Course_,Building_Id,Message_ID,Company_ID")] StudentDetail studentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Building_Id = new SelectList(db.Buildings, "BuildingId", "Building_No", studentDetail.BuildingId);
            ViewBag.Company_ID = new SelectList(db.Companies, "CompanyId", "CompanyName", studentDetail.CompanyId);
            ViewBag.Message_ID = new SelectList(db.Messages, "MessageId", "Title", studentDetail.MessageId);
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No", studentDetail.Id);
            ViewBag.StudentId = new SelectList(db.StudentDetails, "StudentId", "Building_No", studentDetail.Id);
            return View(studentDetail);
        }

        // GET: Reception/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetail studentDetail = db.StudentDetails.Find(id);
            if (studentDetail == null)
            {
                return HttpNotFound();
            }
            return View(studentDetail);
        }

        // POST: Reception/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentDetail studentDetail = db.StudentDetails.Find(id);
            db.StudentDetails.Remove(studentDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
