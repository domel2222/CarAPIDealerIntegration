using CarDealerAPI.DTOS;
using System.Collections.Generic;

namespace CarDealerAPI.Services
{
    public interface ICarService
    {
        int CreateNewCar(int dealerId, CarCreateDTO newCar);
        CarReadDTO GetCarById(int dealerId, int carId);
        List<CarReadDTO> GetAllCarForDealer(int dealerId);
        void DeleteAll(int dealerId);
    }
}