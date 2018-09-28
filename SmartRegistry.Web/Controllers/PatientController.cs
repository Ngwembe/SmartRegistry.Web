using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRegistry.Web.Interfaces;
using SmartRegistry.Web.ViewModels;

namespace SmartRegistry.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientHandler _patientHandler;

        public PatientController(IPatientHandler patientHandler)
        {
            _patientHandler = patientHandler;
        }

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }
            
        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View(new PatientPayloadViewModel());
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientPayloadViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientHandler.AddPatient(model);

                    return RedirectToAction(nameof(Create));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Error","Please enter all the required fields");
                return View(model);
            }
        }

        // GET: Patient/Edit/5
        public async Task<ActionResult> Edit(int id, bool includeAllDetails = true)
        {
            try
            {
                var patient = await _patientHandler.GetPatient(id, includeAllDetails);

                if(patient == null) return RedirectToAction(nameof(Create));

                return View("Create", patient);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Create));
            }

        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PatientPayloadViewModel model)
        {
            try
            {
               var result = await _patientHandler.UpdatePatient(model);

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}