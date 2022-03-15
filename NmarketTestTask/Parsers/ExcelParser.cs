using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using NmarketTestTask.Models;

namespace NmarketTestTask.Parsers
{
    public class ExcelParser : IParser
    {
        public IList<House> GetHouses(string path)
        {
            IList<House> list = new List<House>();
            List<int> uniqueHouses = new List<int>();
            var workbook = new XLWorkbook(path);
            var rows = workbook.Worksheet(1).RangeUsed().RowsUsed();

            var cells = rows.Cells().Where(c => c.GetString().Contains("№")).ToList();
            var houses = rows.Cells().Where(c => c.GetString().Contains("Дом")).ToList();


            foreach (var houseName in houses)
            {
                House house = new House();
                house.Name = houseName.GetString();
                uniqueHouses.Add(houseName.Address.RowNumber);
                list.Add(house);

            }


            foreach (var cell in cells)
            {
                string flatNumber = cell.GetString().Replace("№", string.Empty);
                string flatPrice = cell.CellBelow().GetString();
                Flat flat = new Flat();
                flat.Number = flatNumber;
                flat.Price = flatPrice;
                for (int i = 0; i < uniqueHouses.Count; i++)
                {
                    try
                    {
                        if (cell.Address.RowNumber > uniqueHouses[i] && cell.Address.RowNumber < uniqueHouses[i + 1])
                        {
                            list[i].Flats.Add(flat);
                        }
                    }
                    catch (Exception)
                    {
                        list[i].Flats.Add(flat);
                    }
                    
                }
                
            }
            return list;
        }
    }
}
