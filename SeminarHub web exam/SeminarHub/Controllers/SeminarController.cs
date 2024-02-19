using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeminarHub.Data;
using SeminarHub.Models;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext data;

        public  SeminarController( SeminarHubDbContext context )
        {
            data = context;
        }
        public async Task<IActionResult> All()
        {
            var seminars = await data.Seminars
                .AsNoTracking()
                .Select(s => new SeminarInfoViewModel(

                    s.Id,
                    s.Topic,
                    s.Lecturer,
                    s.Category.Name,
                    s.DateAndTime,
                    s.Organizer.UserName

                 )).ToListAsync();


            return View(seminars);
        }
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var e = await data.Seminars
                .Where(e => e.Id == id)
                .Include(e => e.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (e == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!e.SeminarsParticipants.Any(p => p.ParticipantId == userId))
            {
                e.SeminarsParticipants.Add(new SeminarParticipant()
                {
                    SeminarId = e.Id,
                    ParticipantId = userId
                });

                await data.SaveChangesAsync();
                return RedirectToAction(nameof(Joined));
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await data.SeminarParticipants
                .Where(ep => ep.ParticipantId == userId)
                .AsNoTracking()
                .Select(s => new SeminarInfoViewModel(
                   s.Seminar.Id,
                   s.Seminar.Topic,
                   s.Seminar.Lecturer,
                   s.Seminar.Category.Name,
                   s.Seminar.DateAndTime,
                   s.Seminar.Organizer.UserName

                    ))
                .ToListAsync();

            return View(model);
        }
        
        [HttpGet]
        public  async Task<IActionResult> Delete(int id)
        {
            var s = await data.Seminars
               .FindAsync(id);

            if (s == null)
            {
                return BadRequest();
            }

            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new DeleteSeminarViewModel()
            {

                Id=s.Id,
                Topic = s.Topic, 
                DateAndTime = s.DateAndTime.ToString(DataValidationConstants.DateFormat),
        


            };

         

            return View(model);


            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { 
                var currentSeminar =  await data.Seminars
                    .Where(e => e.Id == id)
                    .FirstOrDefaultAsync();

                var seminarParticipants = await data.SeminarParticipants
                    .Where(ep => ep.SeminarId == id)
                    .ToListAsync();


               if (currentSeminar == null)
               {
                return BadRequest();
               }

                if (currentSeminar.OrganizerId != GetUserId())
                {
                    return Unauthorized();
                }
               
                if (seminarParticipants != null && seminarParticipants.Any())
                {
                    data.SeminarParticipants.RemoveRange(seminarParticipants);
                }

                data.Seminars.Remove(currentSeminar);
                await data.SaveChangesAsync();

                return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new SeminarFormViewModel();
            model.Categories = await GetCategories();

            return View(model);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {
            DateTime dateAndTime = DateTime.Now;
           
            if (!DateTime.TryParseExact(
                model.DateAndTime,
                DataValidationConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateAndTime))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date! Format must be: {DataValidationConstants.DateFormat}");
            }

            

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            var entity = new Seminar()
            {
                Topic = model.Topic,
                Lecturer= model.Lecturer,
                Details= model.Details,
                OrganizerId=GetUserId(),
                DateAndTime=dateAndTime,
                Duration=model.Duration,
                CategoryId=model.CategoryId,    

            };

            await data.Seminars.AddAsync(entity);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var s = await data.Seminars
                .FindAsync(id);

            if (s == null)
            {
                return BadRequest();
            }

            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new SeminarFormViewModel()
            {
                Topic=s.Topic,
                Lecturer=s.Lecturer,
                Details=s.Details,
                DateAndTime=s.DateAndTime.ToString(DataValidationConstants.DateFormat),
                Duration=s.Duration,
                CategoryId=s.CategoryId,



            };

            model.Categories = await GetCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormViewModel model, int id)
        {
            DateTime dateAndTime = DateTime.Now;

            var s = await data.Seminars
                .FindAsync(id);

            if (s == null)
            {
                return BadRequest();
            }

            if (s.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(
                model.DateAndTime,
                DataValidationConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateAndTime))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date! Format must be: {DataValidationConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            s.Topic= model.Topic;
            s.DateAndTime = dateAndTime;
            s.Duration = model.Duration;
            s.Details=model.Details;
            s.CategoryId = model.CategoryId;
            s.Lecturer= model.Lecturer;
            

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await data.Seminars
                .Where(s => s.Id == id)
                .AsNoTracking()
                .Select(s => new SeminarDetailsViewModel(

                    s.Id,
                    s.Topic,
                    s.Lecturer,
                    s.Category.Name,
                    s.Details,
                    s.DateAndTime,
                    s.Organizer.UserName,
                    s.Duration

                 ))
                 .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public async Task<IActionResult> Leave(int id)
        {
            var s= await data.Seminars
                .Where(s => s.Id == id)
                .Include(s =>s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (s == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var sp = s.SeminarsParticipants
                .FirstOrDefault(ep => ep.ParticipantId == userId);

            if (sp == null)
            {
                return BadRequest();
            }

            s.SeminarsParticipants.Remove(sp);

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return await data.Categories    
                .AsNoTracking()
                .Select(t => new CategoryViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }
    }
}
