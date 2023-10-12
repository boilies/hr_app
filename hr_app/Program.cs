// See https://aka.ms/new-console-template for more information
using HR_API.Models;
using hr_app;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;


HttpClient httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://localhost:7037");
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

EmployeeDataProcess process = new EmployeeDataProcess();
List<Entry> list = process.GetEmployees("employees.csv");
foreach(Entry entry in list) {
    Console.WriteLine(entry.FirstName+" "+ entry.AnnualIncome);

    HttpResponseMessage response1 = await httpClient.PostAsJsonAsync("api/entry", entry);
}

HttpResponseMessage response = await httpClient.GetAsync("api/entry");
Console.WriteLine(response.IsSuccessStatusCode);

   
