using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Stock
{
    public class Api
    {
        private string active;
        private string url;
        private float toBuy;
        private float toSell;

        public Api(string act,float buy, float sell, string key){
            url = key;
            active = act.ToUpper();
            toBuy = buy;
            toSell = sell;
        }

        public int GetPrice(){
            var request = WebRequest.CreateHttp(url+active);
            using (var response = request.GetResponse()){
                var streamDados = response.GetResponseStream();
                var reader = new StreamReader(streamDados ?? throw new InvalidOperationException());
                string objResponse = reader.ReadToEnd();
                
                objResponse = objResponse.Replace(active, "active");
                Console.WriteLine(objResponse);
                var activeObj = JsonSerializer.Deserialize<ApiResponse>(objResponse);
                if (activeObj != null)
                {
                    double price = activeObj.results.active.price;
                    streamDados.Close();
                    response.Close();
                    if (price < toBuy)
                        return -1;
                    if (price > toSell)
                        return 1;
                }
                return 0;
            }
        }
    }
}