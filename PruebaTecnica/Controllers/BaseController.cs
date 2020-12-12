using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Utils;

namespace PruebaTecnica.Controllers
{
    public class BaseController : Controller
    {
        public void Alert(string mensaje,string titulo, TipoNotificacion notificationType)
        {
            var msg = new
            {
                message = mensaje,
                title = titulo,
                icon = notificationType.ToString(),
                type = notificationType.ToString(),
            };

           

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
