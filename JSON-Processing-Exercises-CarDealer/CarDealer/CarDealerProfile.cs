using AutoMapper;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //09.Import Suppliers
            CreateMap<ImportSuppliersDTO, Supplier>();

            //10.Import Parts
            CreateMap<ImportPartDto, Part>();

            //11.Import Cars
           CreateMap<ImportCarDTO, Car>();

            //12.Import Customers
           CreateMap<ImportCustomerDTO, Customer>();

            //13.Import Sales
            CreateMap<ImportSaleDto, Sale>();
        }
    }
}
