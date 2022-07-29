using PicoyPlacaPredictor.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PicoyPlacaPredictor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public string Comprobar(Auto oAuto)
        {
            bool plateValid = false;
            bool dateValid = false;
            bool hourValid = false;

            string rpta = "<ul class='list-group'>"; 

            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values//Values
                             from error in state.Errors//Messages
                             select error.ErrorMessage).ToList();

                foreach (var item in query)
                {
                    rpta += "<li class='list-group-item'>" + item + "</li>";
                }

            }
            else
            {
                //Data asignation
                plateValid = PlateCheck(oAuto.placa);
                dateValid = DateCheck(oAuto.fecha);
                hourValid = HourCheck(oAuto.hora);
                DateTime dateValue; 

                if (plateValid && dateValid && hourValid)
                {
                    //date exists and is valid
                    if (DateTime.TryParse(oAuto.fecha, out dateValue))
                    {
                        DateTime dateFinal = DateTime.Parse(oAuto.fecha, new CultureInfo("es-ES"));
                        oAuto.fecha = dateFinal.DayOfWeek.ToString(); 
                        rpta += MobilityCheck(oAuto, dateValue);
                    }
                    else
                    {
                        rpta += "<li class='list-group-item'> No existe el día. Pruebe otro </li>";
                    }
                }
                else
                {
                    if (!plateValid) //Invalid plate format
                    {
                        rpta += "<li class='list-group-item'> Formato de placa: XXX-0000 </li>";
                    }
                    if (!dateValid) //Invalid Date format
                    {
                        rpta += "<li class='list-group-item'> Formato de fecha: dd/mm/yyyy </li>";
                    }
                    if (!hourValid) //Invalid hour format
                    {
                        rpta += "<li class='list-group-item'> Formato de hora: hh:mm </li>";
                    }
                }
            }
            rpta += "</ul>";
            return rpta; //Message with errors in format or information about mobility
        }

        public string MobilityCheck(Auto oAuto, DateTime dateValue)
        {
            string rpta = "";
            char lastDigit = oAuto.placa[oAuto.placa.Length - 1];//Laas digit of the plate

            // Days to compare in English and in Spanish to be visualized
            string day = dateValue.ToString("dddd", new CultureInfo("en-US"));
            string dia = dateValue.ToString("dddd", new CultureInfo("es-ES"));

            //Onject to compare between a time gap
            TimeSpan hour;
            TimeSpan.TryParse(oAuto.hora, out hour);


            if (((hour >= new TimeSpan(07, 00, 00) && hour <= new TimeSpan(9, 30, 0))
                || (hour >= new TimeSpan(16, 00, 00) && hour <= new TimeSpan(19, 30, 0)))
                && (day != "Sunday" || day != "Saturday"))
            {
                if ((lastDigit.Equals('1') || lastDigit.Equals('2'))
                && (day == "Monday"))
                {
                    rpta = "<li class='list-group-item'> SU AUTO NO PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy") + " EN ESTE HORARIO " + " (07:00-9:30am / 16:00-19:30) </li>";
                }
                else if ((lastDigit.Equals('3') || lastDigit.Equals('4'))
                && (day == "Tuesday"))
                {
                    rpta = "<li class='list-group-item'> SU AUTO NO PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy") + " EN ESTE HORARIO " + " (07:00-9:30am / 16:00-19:30) </li>";
                }
                else if ((lastDigit.Equals('5') || lastDigit.Equals('6'))
                && (day == "Wednesday"))
                {
                    rpta = "<li class='list-group-item'> SU AUTO NO PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy") + " EN ESTE HORARIO " + " (07:00-9:30am / 16:00-19:30) </li>";
                }
                else if ((lastDigit.Equals('7') || lastDigit.Equals('8'))
                && (day == "Thursday"))
                {
                    rpta = "<li class='list-group-item'> SU AUTO NO PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy") + " EN ESTE HORARIO " + " (07:00-9:30am / 16:00-19:30) </li>";
                }
                else if ((lastDigit.Equals('9') || lastDigit.Equals('0'))
                && (day == "Friday"))
                {
                    rpta = "<li class='list-group-item'> SU AUTO NO PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy") + " EN ESTE HORARIO " + " (07:00-9:30am / 16:00-19:30) </li>";
                }
                else
                {
                    rpta = "<li class='list-group-item'> PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                rpta = "<li class='list-group-item'> PUEDE SALIR HOY " + dia.ToUpper() + " " + dateValue.ToString("dd/MM/yyyy");
            }

            return rpta;
        }

        public bool PlateCheck(string plate)
        {
            // Valid plate format = XXX-0000
            // Admits 3 letters (Except D and F at de beggining) , and at least 3 numbers
            var validPlate = Regex.Match(plate, "^[^DFa-z0-9!\"#$%&'()*+,-./:;<=>?@\\][^_`{|}~][A-Z]{2}-[0-9]{3,4}$");

            return validPlate.Success;
        }

        public bool DateCheck(string date)
        {
            // Valid date format = dd/MM/yyyy            
            var validDate = Regex.Match(date, @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");

            return validDate.Success;
        }


        public bool HourCheck(string hour)
        {
            // 00:00 
            // 24 Hour format
            var validHour = Regex.Match(hour, "^([01][0-9]|2[0-3]):[0-5][0-9]$");

            return validHour.Success;
        }
    }
}