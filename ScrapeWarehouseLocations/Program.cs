using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace GetLocationCodes
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();

            // Load the source file from the cosco website that lists the provinces with warehouses
            var doc = new HtmlDocument();
            doc.Load("../../cosco-warehouse-locations.html");

            // This is the base url to get back a json string with warehosue locations - we just need to add the parentGeoNode number at the end
            var rootUrl = @"http://www.costco.com/AjaxWarehouseBrowseLookupView?&storeId=10301&catalogId=10051&langId=-1&parentGeoNode=";
            
            // We're going to store all the warehouse locations here
            var warehouseLocations = new List<LatLongObject>();

            // Iterate through each node that provides a code for a province
            foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='form-item statePanel']//option"))
            {
                // get the code for that province, query the api, and deserialize the response into our simplified class object 
                var parentGeoNode = node.Attributes[0].Value;
                var response = httpClient.GetStringAsync(rootUrl + parentGeoNode).Result;
                var result = JsonConvert.DeserializeObject<List<LatLongObject>>(response);

                // Add the result to our list
                warehouseLocations.AddRange(result);
            }

            // Write all responses to a file, so we can quickly deserialize them again (without the web call)
            // We'll use this to populate our database for visualize the locations with the google maps API
            var serializedString = JsonConvert.SerializeObject(warehouseLocations);
            using (var writer = new StreamWriter("../../warehouse-locations.json"))
            {
                writer.Write(serializedString);
            }
        }
    }
}
