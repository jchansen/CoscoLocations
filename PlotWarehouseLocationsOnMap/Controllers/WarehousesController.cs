using LocationsOnMap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LocationsOnMap.Controllers
{
    public class WarehousesController : ApiController
    {
        // GET api/values
        public IEnumerable<LatLongObject> Get()
        {
            var jsonFileUrl = HttpContext.Current.Server.MapPath("~/App_Data/warehouse-locations.json");
            var file = new StreamReader(jsonFileUrl);
            var json = file.ReadToEnd();
            var warehouseLocations = JsonConvert.DeserializeObject<List<LatLongObject>>(json);
            return warehouseLocations;
        }
    }
}