using System;
using System.IO;
using System.Net;

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
            active = act;
            toBuy = buy;
            toSell = sell;
        }

        public int GetPrice(){
            var request = WebRequest.CreateHttp(url+active);
            using (var response = request.GetResponse()){
                var streamDados = response.GetResponseStream();
                var reader = new StreamReader(streamDados ?? throw new InvalidOperationException());
                object objResponse = reader.ReadToEnd();
                Console.WriteLine(objResponse.ToString());
                string[] parts = objResponse.ToString().Split(',');
                for(int index=0;index<parts.Length;index++){
                    string[] temp = parts[index].Split(':');
                    if(string.CompareOrdinal("\"price\"",temp[0])==0){
                        Console.WriteLine("Price: "+temp[1]);
                        float price = float.Parse(temp[1]);
                        if(price < toBuy)
                            return -1;
                        if(price > toSell)
                            return 1;
                    }
                }
                streamDados.Close();
                response.Close();
                return 0;
            }
        }
    }
}