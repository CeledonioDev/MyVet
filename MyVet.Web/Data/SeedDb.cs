using MyVet.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task seedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPetTypesAsync();
            //await CheckServiceTypeAsync();
            await CheckOwnerAsync();
            await CheckPetsAsync();
            await CheckAgendasAsync();
        }

        private async Task CheckAgendasAsync()
        {
            if (!_context.Agendas.Any())
            {
                var initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                var finalDate = initialDate.AddYears(1);
                while(initialDate < finalDate)
                {
                    if(initialDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var finalDate2 = initialDate.AddHours(10);
                        while(initialDate < finalDate2)
                        {
                            _context.Agendas.Add(new Agenda
                            {
                                Date = initialDate.ToUniversalTime(),
                                IsAvailable = true
                            });

                            initialDate = initialDate.AddMinutes(30);
                        }

                        initialDate = initialDate.AddHours(14);
                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }

        private  async Task CheckPetsAsync()
        {
            var owner = _context.Owners.FirstOrDefault();
            var petType = _context.PetTypes.FirstOrDefault();
            if(!_context.Pets.Any())
            {
                AddPet("Otto", owner, petType, "shih tzu");
                AddPet("killer", owner, petType, "Dobermann");
                await _context.SaveChangesAsync();
            }
        }

        private void AddPet(string name, Owner owner, PetType petType, string race)
        {
            _context.Pets.Add(new Pet
            {
                Born = DateTime.Now.AddYears(-2),
                Name = name,
                Owner = owner,
                PetType = petType,
                Race = race
            });
        }

        private async Task CheckOwnerAsync()
        {
            if (!_context.Owners.Any())
            {
                AddOwner("89898989", "Leandro", "Celedonio", "234 3232", "310 322 3221", "Boca Chica");
                AddOwner("76555445", "Jose", "Cardona", "343 3226", "300 322 3221", "Calle 77 #22 21");
                AddOwner("54548789", "Maria", "López", "450 4332", "350 322 3221", "Carrera 56 #22 21");
                await _context.SaveChangesAsync();
            }
        }

        private void AddOwner(string document, string firstName, string lastName, string fixedPhone, string cellPhone, string address)
        {
            _context.Owners.Add(new Owner
            {
                Address = address,
                CellPhone = cellPhone,
                Document = document,
                FirstName = firstName,
                LastName = lastName

            });
        }

        //private async Task CheckServiceTypeAsync()
        //{
        //    throw new NotImplementedException();
        //}

        private async Task CheckPetTypesAsync()
        {
            if (!_context.PetTypes.Any())
            {
                _context.PetTypes.Add(new PetType { Name = "Dog" });
                _context.PetTypes.Add(new PetType { Name = "Cat" });
                _context.PetTypes.Add(new PetType { Name = "Turtle" });
                await _context.SaveChangesAsync();

            }
        }
    }
}
