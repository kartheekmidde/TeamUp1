using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using TeamUp1.Models;

namespace TeamUp1.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Events
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Events.ToList().Where(x => x.eventDate >= DateTime.Today));
        }

        // GET: Events/Details/5
        [Authorize]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = db.Events.Find(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        // GET: Events/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,player,eventName,eventDate,fromTime,toTime,address,latitude,longitude")] Event @event)
        {
            @event.player = User.Identity.Name;
            
            if (ModelState.IsValid)
            {
                
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
                //return Json(@event, JsonRequestBehavior.AllowGet);
                //return Json(@event);
            }
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("Index");
            //return View(@event);
        }

        // POST: Events/SignUp
        [HttpPost]
        [Authorize]
        public ActionResult SignUp([Bind(Include = "Id,player,eventName,eventDate,fromTime,toTime,address,latitude,longitude")] Event @event)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return Json(@event, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        public ActionResult deleteItem(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Request.IsAjaxRequest())
            {
                Event @event = db.Events.Find(id);
                db.Events.Remove(@event);
                db.SaveChanges();
                return Json(@event, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //if (ModelState.IsValid)
            //{
            //    db.Events.Add(@event);
            //    db.SaveChanges();
            //    return Json(@event, JsonRequestBehavior.AllowGet);
            //}
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        // GET: Events/Edit/5

        //[Authorize]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = db.Events.Find(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Edit([Bind(Include = "Id,player,eventName,eventDate")] Event @event)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(@event).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(@event);
        //}

        // GET: Events/Delete/5
        //[Authorize]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = db.Events.Find(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        // POST: Events/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Event @event = db.Events.Find(id);
        //    db.Events.Remove(@event);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public async Task<bool> getWeather()
        {
            string webURL = "http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + "37.83" + "&lon=" + "-84.51" + "&APPID=a4c778d444cd694c5d72899a151dae29" + "&mode=xml";

            var xml = await new WebClient().DownloadStringTaskAsync(new Uri(webURL));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string sDate = doc.DocumentElement.SelectSingleNode("forecast/time[@day = '2016-11-29']").Attributes["day"].Value;


            string avgTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["day"].Value;
            string minTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["min"].Value;
            string maxTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["max"].Value;
            string clouds = doc.DocumentElement.SelectSingleNode("forecast/time/clouds").Attributes["value"].Value;
            string wind = doc.DocumentElement.SelectSingleNode("forecast/time/windSpeed").Attributes["name"].Value;
            string windSpeed = doc.DocumentElement.SelectSingleNode("forecast/time/windSpeed").Attributes["mps"].Value;
            double sTemp = double.Parse(avgTemp) - 273.16;
            double mnTemp = double.Parse(minTemp) - 273.16;
            double mxTemp = double.Parse(maxTemp) - 273.16;
            String text1 = sTemp.ToString("N2") + " Celsius";
            String text2 = mnTemp.ToString("N2") + " Celsius";
            String text3 = mxTemp.ToString("N2") + " Celsius";


            Console.WriteLine("Average Temperature: " + text1 + " Minimum Temperature: " + text2 + " Maximum Temperature: " + text3);
            Console.WriteLine("Sky: " + clouds);
            Console.WriteLine("There is a " + wind.ToLower() + " with a speed of " + windSpeed + " miles per hour");

            return true;
        }
    }
}
