using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {

            CarDealerContext context = new CarDealerContext();
            string inputJson = string.Empty;
            string output = string.Empty;

            //09.Import Suppliers
            //inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");
            //output = ImportSuppliers(context, inputJson);

            //10.Import Parts
            //inputJson = File.ReadAllText(@"../../../Datasets/parts.json");
            //output = ImportParts(context, inputJson);

            //11.Import Cars
            //inputJson = File.ReadAllText(@"../../../Datasets/cars.json");
            //output = ImportCars(context, inputJson);

            //12.Import Customers
            //inputJson = File.ReadAllText(@"../../../Datasets/customers.json");
            //output = ImportCustomers(context, inputJson);

            //13.Import Sales
            //inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
            //output = ImportSales(context, inputJson);

            //14.Export Ordered Customers
            //output = GetOrderedCustomers(context);

            //15.Export Cars From Make Toyota
           // output = GetCarsFromMakeToyota(context);

            //16.Export Local Suppliers
            //output = GetLocalSuppliers(context);

            //17.Export Cars With Their List Of Parts
            //output = GetCarsWithTheirListOfParts(context);

            //18.Export Total Sales By Customer
            //output = GetTotalSalesByCustomer(context);

            //19.Export Sales With Applied Discount
            output = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(output);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper=new Mapper(config);

            //Turning the json file to a DTO
            ImportSuppliersDTO[] suppliersDtos = JsonConvert.DeserializeObject<ImportSuppliersDTO[]>(inputJson);

            //Mapping the Users from their DTO
            Supplier[] suppliers = mapper.Map<Supplier[]>(suppliersDtos);

            //Adding the Suppliers
            context.Suppliers.AddRangeAsync(suppliers);
            context.SaveChanges();

            //Output
            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            //Turning the json file to a DTO
            ImportPartDto[] partsDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            //Mapping the Parts from their DTO
            ICollection<Part> parts = new List<Part>();

            foreach (var part in partsDtos)
            {
                if (context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    parts.Add(mapper.Map<Part>(part));
                }
            }

            //Adding the Parts
            context.Parts.AddRangeAsync(parts);
            context.SaveChanges();

            //Output
            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            //Turning the json file to a DTO
            ImportCarDTO[] carsDtos = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson);

            //Mapping the Cars from their DTOs
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in carsDtos)
            {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsIds)
                {
                    if (context.Parts.Any(p => p.Id == id))
                    {
                        currentCar.PartsCars.Add(new PartCar
                        {
                            PartId = id,
                        });
                    }
                }

                carsToAdd.Add(currentCar);
            }

            //Adding the Cars
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            //Output
            return $"Successfully imported {carsToAdd.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);


            //Turning the json file to a DTO
            ImportCustomerDTO[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDTO[]>(inputJson);

            //Mapping the Customers from their DTOs
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            //Adding the Customers
            context.Customers.AddRange(customers);
            context.SaveChanges();

            //Output
            return $"Successfully imported {customers.Length}.";
        }

        //13.Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);


            //Turning the json file to a DTO
            ImportSaleDto[] salesDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            //Mapping the Sales from their DTOs
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            //Adding the Sales
            context.Sales.AddRange(sales);
            context.SaveChanges();

            //Output
            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            //Finding the Customers
            var customers = context.Customers
                .AsNoTracking()
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                })
                .ToArray();

            //Output
            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .AsNoTracking()
                .Where(p=>p.Make=="Toyota")
                .OrderBy(p=>p.Make)
                .OrderByDescending (p=>p.TraveledDistance)
               .Select(c => new
                {
                   Id = c.Id,
                   Make = c.Make,
                   Model = c.Model,
                   TravelledDistance = c.TraveledDistance
               })
                .ToArray();


            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.Where(s => s.IsImporter == false)
                                             .Select(s => new
                                             {
                                                 Id = s.Id,
                                                 Name = s.Name,
                                                 PartsCount = s.Parts.Count()
                                             }).ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                  .Select(c => new
                   {
                      car = new
                      {
                          Make = c.Make,
                          Model = c.Model,
                          TravelledDistance = c.TraveledDistance
                      },
                      parts = c.PartsCars
                      .Select(pc => new
                      {
                          Name = pc.Part.Name,
                          Price = pc.Part.Price.ToString("f2")
                      }).ToList()

                  }).ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
               .Where(c => c.Sales.Count() > 0)
               .Select(c => new
               {
                   fullName = c.Name,
                   boughtCars = c.Sales.Count(),
                   spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price))
               })
               .OrderByDescending(x => x.spentMoney)
               .ThenByDescending(x => x.boughtCars)
               .ToList();

            //Output
            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //Finding the Sales
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    priceWithDiscount = ((s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100))).ToString("f2")
                })
                .ToArray();

            //Output
            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }

    }
}