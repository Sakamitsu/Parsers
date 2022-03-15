using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NmarketTestTask.Models;

namespace NmarketTestTask.Parsers
{
    public class HtmlParser : IParser
    {
        public IList<House> GetHouses(string path)
        {
            IList<House> list = new List<House>();
            List<string> uniqueHouses = new List<string>();

            var doc = new HtmlDocument();
            doc.Load(path);
            var table = doc.DocumentNode.SelectSingleNode("//tbody");
            var rows = table.SelectNodes(".//tr");
            foreach (var row in rows)
            {
                string houseNumber = row.SelectSingleNode(".//td[@class='house']").InnerText;
                string flatNumber = row.SelectSingleNode(".//td[@class='number']").InnerText;
                string flatPrice = row.SelectSingleNode(".//td[@class='price']").InnerText;

                if (uniqueHouses.Distinct().ToList().Contains(houseNumber))
                {
                    foreach (var house in list)
                    {
                        if (house.Name == houseNumber)
                        {
                            Flat flat = new Flat();
                            flat.Number = flatNumber;
                            flat.Price = flatPrice;
                            house.Flats.Add(flat);
                        }
                    }
                }
                else
                {
                    House house = new House();
                    Flat flat = new Flat();
                    house.Name = houseNumber;
                    flat.Number = flatNumber;
                    flat.Price = flatPrice;
                    house.Flats.Add(flat);
                    list.Add(house);
                }
                uniqueHouses.Add(houseNumber);
            }
            return list;
        }
    }
}
