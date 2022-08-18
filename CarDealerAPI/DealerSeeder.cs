using CarDealerAPI.Contexts;
using CarDealerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI
{
    public class DealerSeeder
    {
        private readonly DealerDbContext _dealerDbContext;

        public DealerSeeder(DealerDbContext dealerDbContext)
        {
            this._dealerDbContext = dealerDbContext;
        }
        public void Seed()
        {
            //add auto migration into server
            if (_dealerDbContext.Database.IsRelational())
            {
                if (_dealerDbContext.Database.CanConnect())
                {

                    if (!_dealerDbContext.Roles.Any())
                    {
                        var roles = GetRoles();
                        _dealerDbContext.Roles.AddRange(roles);
                        _dealerDbContext.SaveChanges();
                    }

                    if (!_dealerDbContext.Dealers.Any())
                    {
                        var dealers = GetDealers();
                        _dealerDbContext.Dealers.AddRange(dealers);
                        _dealerDbContext.SaveChanges();
                    }
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    NameRole ="User"
                },
                new Role()
                {
                    NameRole ="Dealer Manager"
                },
                new Role()
                {
                    NameRole ="Administrator"
                }

            };

            return roles;
        }

        private IEnumerable<Dealer> GetDealers()
        {
            var dealers = new List<Dealer>()
            {
                new Dealer()
                {
                    DealerName = "MAN Delar Poland",
                    Category = "TRUCK",
                    Description =
                           "The best Truck car",
                    ContactEmail = "contact@mannadarzyn.com",
                    TestDrive = true,
                    Cars = new List<Car>()
                        {
                            new Car()
                            {
                                NameMark = "MAN",
                                Model = "TGS",
                                Price = 225.00M,
                            },

                            new Car()
                            {
                                NameMark = "MAN",
                                Model = "TGS",
                                Price = 230.00M,
                            },
                        },
                    Address = new Address()
                    {
                        City = "Nadarzyn",
                        Country = "Poland",
                        Street = "Prosta 8",

                    }
                },
                new Dealer()
                {
                    DealerName = "Italian Speed",
                    Category = "Sports",
                    Description =
                        "If everything infurating you maybe time to buy a car",
                    ContactEmail = "contact@speedpoland.com",
                    TestDrive = true,
                    Cars = new List<Car>()
                    {
                        new Car()
                            {
                                NameMark = "Lamborgini",
                                Model = "Diablo",
                                Price = 450.00M
                            },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Country = "Poland",
                        Street = "Szewska 2",

                    }
                }
            };

            return dealers;
        }
    }
}
