using AutoMapper;
using CarDealerAPI.Authorization;
using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.Exceptions;
using CarDealerAPI.Extensions;
using CarDealerAPI.Extensions.Pagination;
using CarDealerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CarDealerAPI.Services
{
    public class DealerService : IDealerService
    {
        private readonly DealerDbContext _dealerDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DealerService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        

        public DealerService(DealerDbContext dealerDbContext, IMapper mapper, ILogger<DealerService> logger, 
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            this._dealerDbContext = dealerDbContext;
            this._mapper = mapper;
            this._logger = logger;
            this._authorizationService = authorizationService;
            this._userContextService = userContextService;
        }

        public DealerReadDTO GetDealerById(int id)
        {
            var dealer = _dealerDbContext
                .Dealers
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            if (dealer == null) throw new NotFoundException("dealer not found");

            var result = _mapper.Map<DealerReadDTO>(dealer);

            return result;
        }
        public Paginator<DealerReadDTO> GetAllDealers(DealerQuerySearch query)
        {
            
            var dealers = _dealerDbContext
                .Dealers
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .Where(s => query.Search == null ||
                                    (s.DealerName.ToUpper().Contains(query.Search.ToUpper()) ||
                                    s.Description.ToUpper().Contains(query.Search.ToUpper())));

            dealers = SortQuery(query, dealers);

            var skipPages = ComputeSkip(query);
            var paginatedealers = dealers.Skip(skipPages)
                                .Take(query.PageSize)
                                .ToList();

            var totalItems = dealers.Count();

            var dealersDto = _mapper.Map<List<DealerReadDTO>>(paginatedealers);

            var result = new Paginator<DealerReadDTO>(dealersDto, totalItems, query.PageSize, query.PageNumber);

            return result;

        }



        public int CreateDealer(DealerCreateDTO createDto)
        {
            var dealer = _mapper.Map<Dealer>(createDto);


            ///////////////////////////////something wrong  not assign 

            dealer.CreatedById = _userContextService.GetUserId;
            _dealerDbContext.Add(dealer);
            _dealerDbContext.SaveChanges();

            return dealer.Id;
        }

        public void DeleteDealer(int id)
        {

            _logger.LogWarning($"Dealer with: {id} DELETE action invoke");
            _logger.LogError($"Dealer with: {id} DELETE action invoke uuuuuuuuuuuuu");

            var dealer = _dealerDbContext
                .Dealers
                .FirstOrDefault(r => r.Id == id);

            

            if (dealer == null) throw new NotFoundException("dealer not found");

            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, dealer, new ResouceOperationRequirement(ResouceOperation.Delete)).Result;

            if (!authResult.Succeeded)
            {
                throw new ForbiddenExc("Access denied");
            }
            
            _dealerDbContext.Dealers.Remove(dealer);
            _dealerDbContext.SaveChanges();



            _logger.LogInformation($"Deleted successfully");
        }

        public void UpdateDealer(DealerUpdateDTO dto, int id)
        {
            var dealer = _dealerDbContext
                .Dealers
                .FirstOrDefault(a => a.Id == id);

            if (dealer == null)
                throw new NotFoundException("dealer not found");

            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, dealer, new ResouceOperationRequirement(ResouceOperation.Update)).Result;

            if (!authResult.Succeeded)
            {
                throw new ForbiddenExc("Access denied");
            }

            dealer.DealerName = dto.DealerName;
            dealer.Description = dto.Description;
            dealer.TestDrive = dto.TestDrive;

            _dealerDbContext.SaveChanges(); 
        }

        private int ComputeSkip(DealerQuerySearch query)
        {
            return query.PageSize * query.PageNumber - query.PageSize;
        }

        private  IQueryable<Dealer> SortQuery(DealerQuerySearch query, IQueryable<Dealer> dealers)
        {
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelector = new Dictionary<string, Expression<Func<Dealer, object>>>
                {
                    {nameof(Dealer.DealerName), a=>a.DealerName },
                    {nameof(Dealer.Description), a=>a.Description },
                    {nameof(Dealer.Category), a=>a.Category },
                    {nameof(Dealer.Address.Country),a=>a.Address.Country  }
                };

                var selectedColumn = columnSelector[query.SortBy];

                dealers = query.DirectionSort == DirectionSort.Ascending
                ? dealers.OrderBy(selectedColumn)
                : dealers.OrderByDescending(selectedColumn);
            }

            return dealers;
        }
    }
}
