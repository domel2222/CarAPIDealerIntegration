using AutoMapper;
using CarDealerAPI.DTOS;
using CarDealerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Profiles
{
    public class DealerProfile : Profile
    {
        public DealerProfile()
        {
            MapReadDtos();
            //MapUpdateDtos();
            MapCreateDtos();
        }


       private void MapReadDtos()
        {
            CreateMap<Dealer, DealerReadDTO>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.Country, c => c.MapFrom(s => s.Address.Country));

            CreateMap<Car, CarReadDTO>().ReverseMap();
        }

        private void MapCreateDtos()
        {
            CreateMap<DealerCreateDTO, Dealer>()
                .ForMember(r => r.Address, o => o.MapFrom(dto => new Address()
                { City = dto.City, Street = dto.Street, Country = dto.Country }));


            CreateMap<CarCreateDTO, Car>();
        }

        private void MapUpdateDtos()
        {

        }
    }
}
