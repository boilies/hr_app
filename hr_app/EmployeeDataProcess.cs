using HR_API.Models;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace hr_app
{
    internal class EmployeeDataProcess
    {
        public List<Entry> GetEmployees(string path)
        {
            List<Entry> employees = new List<Entry>();
            Double anualTaxToPay = 0;
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                bool firstline = true;
              
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    Entry entry = new Entry();
                 
                    if (!firstline)
                    {
                        entry.EmployeeID = 0;//Int32.Parse(fields[0].Trim());
                        entry.FirstName = fields[1].Trim();
                        entry.LastName = fields[2].Trim();
                        entry.BirthDate = DateOnly.Parse(fields[3].Trim());
                        entry.AnnualIncome =CalculateNet(Double.Parse(fields[4].Trim()));
                        anualTaxToPay += Double.Parse(fields[4].Trim()) - entry.AnnualIncome;
                        employees.Add(entry);
                    }
                    else { 
                        Console.WriteLine("Ignoring first line"); 
                        firstline = false; 
                        continue; }
                }
            }
            Console.WriteLine("Annual Tax To Pay for this csv: " + anualTaxToPay);
            return employees;
        }

        public Double CalculateNet(double brut) {
            if (brut <= 5000)
            {
                return brut;
            }
            else if (brut <= 20000)
            {
                return brut * (100- (int)TaxRate.TaxBandB) / 100;
            }
            else
            {
                return brut * (100 - (int)TaxRate.TaxBandC) / 100;
            }
        }
    }

    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.FromDateTime(reader.GetDateTime());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            var isoDate = value.ToString("O");
            writer.WriteStringValue(isoDate);
        }
    }
    enum TaxRate: int
    {
        TaxBandA = 0,
        TaxBandB = 20,  
        TaxBandC = 40
    }

}
