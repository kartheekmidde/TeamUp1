using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace TeamUp1.Models
{
    [CustomValidation(typeof(Event), "fromTimeLessThanToTime")]
    public class Event
    {
        [Display(Name = "Event ID")]
        public int Id { get; set; }

        [Display(Name = "Player")]
        public string player { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public eventName eventName { get; set; }

        [Required]
        [Display(Name = "Event date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [CustomValidation(typeof(Event), "eventDateInFuture")]
        public DateTime eventDate { get; set; }

        public static ValidationResult eventDateInFuture(DateTime eventDate, ValidationContext context)
        {
            if (eventDate < DateTime.Today)
            {
                return new ValidationResult("Event cannot be in the past");
            }
            return ValidationResult.Success;
        }
        [Required]
        [Display(Name = "From Time")]
        [DataType(DataType.Time)]
        public DateTime fromTime { get; set; }

        [Required]
        [Display(Name = "To Time")]
        [DataType(DataType.Time)]
        public DateTime toTime { get; set; }

        public static ValidationResult fromTimeLessThanToTime(Event eve, ValidationContext context)
        {

            if (eve.fromTime >= eve.toTime)
            {
                return new ValidationResult("From time should be less than to time.");
            }
            return ValidationResult.Success;
        }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public string latitude { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public string longitude { get; set; }

        public Event()
        {
            //this.player = User.Identity.Name;
        }

        public String getWeather(String wlat, String wlong, DateTime wdate)
        {
            if (wlat == null || wlong == null)
            {
                return "Data not availale";
            } else
            { 
            string webURL = "http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + wlat + "&lon=" + wlong + "&APPID=942bb387f9ff98f3f210e4896f2c7694&mode=xml";
            //string webURL = "http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + wlat + "&lon=" + wlong + "&APPID=a4c778d444cd694c5d72899a151dae29" + "&mode=xml";
            var xml = new WebClient().DownloadString(new Uri(webURL));
            DateTime d = new DateTime(wdate.Year, wdate.Month, wdate.Day);
            string cvrtDate = d.ToString("yyyy-MM-dd");
            Console.WriteLine("Param date: " + cvrtDate);
            //string date = "2016-11-28";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string sDate = doc.DocumentElement.SelectSingleNode("forecast/time[@day='" + cvrtDate + "']").Attributes["day"].Value;

            if (sDate == null)
            {
                return "Data not availale";
            }
            else
            {
                string avgTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["day"].Value;
                string minTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["min"].Value;
                string maxTemp = doc.DocumentElement.SelectSingleNode("forecast/time/temperature").Attributes["max"].Value;
                string clouds = doc.DocumentElement.SelectSingleNode("forecast/time/clouds").Attributes["value"].Value;
                string wind = doc.DocumentElement.SelectSingleNode("forecast/time/windSpeed").Attributes["name"].Value;
                string windSpeed = doc.DocumentElement.SelectSingleNode("forecast/time/windSpeed").Attributes["mps"].Value;
                double sTemp = double.Parse(avgTemp) - 273.16;
                double mnTemp = double.Parse(minTemp) - 273.16;
                double mxTemp = double.Parse(maxTemp) - 273.16;
                String text1 = sTemp.ToString("N2") + " °C";
                String text2 = mnTemp.ToString("N2") + " °C";
                String text3 = mxTemp.ToString("N2") + " °C";

                String returnValue = text1;
                return returnValue;
            }
        }
      }
    }

    public enum eventName : int
    {
        Badminton,
        Baseball,
        Basketball,
        BeachVolleyball,
        Boxing,
        Chess,
        Cricket,
        Football,
        Golf,
        Hockey,
        IceHockey,
        IceSkating,
        SkyDiving,
        Snooker,
        Soccer,
        Swimming,
        TableTennis,
        Tennis,
        Volleyball
    }

    


}