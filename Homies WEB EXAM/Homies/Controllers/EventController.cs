﻿using Homies.Data.Models;
using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Homies.Controllers
{
    [Authorize]

    public class EventController : Controller
    {
        private readonly HomiesDbContext data;

        public EventController(HomiesDbContext context) 
        {
            data = context;    
        }
        public async Task<IActionResult> All()
        {
           var events= await data.Events
            .AsNoTracking()
           .Select(e => new EventInfoViewModel
           ( e.Id,
             e.Name,
             e.Start,
             e.Type.Name,
             e.Organizer.UserName
           ))
          .ToListAsync();

            return View(events);
        }

        [HttpPost]

        public async Task<IActionResult> Join(int id)
        {
            var e = await data.Events
                        .Where(e => e.Id == id)
                        .Include(e => e.EventsParticipants)
                        .FirstOrDefaultAsync();

            if (e == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!e.EventsParticipants.Any(p=>p.HelperId==userId))
            {
                e.EventsParticipants.Add(new EventParticipant()
                {
                    EventId = e.Id,
                    HelperId = userId

                });

            }

            await data.SaveChangesAsync();

            return RedirectToAction("Joined");

        }

        [HttpGet]

        public async Task<IActionResult>Joined(int id)
        {
            string userId = GetUserId();

            var model = await data.EventParticipants
                            .Where(ep => ep.HelperId == userId)
                            .AsNoTracking()
                            .Select(ep => new EventInfoViewModel(
                               ep.EventId,
                               ep.Event.Name,
                               ep.Event.Start,
                               ep.Event.Type.Name,
                               ep.Event.Organizer.UserName

                             ))
                            .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult>Leave(int id)
        {
            var e = await data.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if(e== null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var ep=e.EventsParticipants.FirstOrDefault(ep => ep.HelperId == userId);

            if(ep==null)
            {
                return BadRequest();
            }
            e.EventsParticipants.Remove(ep);

            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        private string GetUserId() 
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value??string.Empty;
        }

        private async Task<IEnumerable<TypeViewModel>> GetTypes()
        {
            return await data.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        [HttpGet]

        public async Task<IActionResult>Add()
        {
            var model = new EventFormViewModel();
            model.Types=await GetTypes();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var e = await data.Events
                .FindAsync(id);

            if (e == null)
            {
                return BadRequest();
            }

            if (e.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new EventFormViewModel()
            {
                Description = e.Description,
                Name = e.Name,
                End = e.End.ToString(DataConstants.DateFormat),
                Start = e.Start.ToString(DataConstants.DateFormat),
                TypeId = e.TypeId
            };

            model.Types = await GetTypes();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model, int id)
        {
            var e = await data.Events
                .FindAsync(id);

            if (e == null)
            {
                return BadRequest();
            }

            if (e.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                model.Start,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState
                    .AddModelError(nameof(model.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
                model.End,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState
                    .AddModelError(nameof(model.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Types = await GetTypes();

                return View(model);
            }

            e.Start = start;
            e.End = end;
            e.Description = model.Description;
            e.Name = model.Name;
            e.TypeId = model.TypeId;

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormViewModel model)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                model.Start,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState
                    .AddModelError(nameof(model.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
                model.End,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState
                    .AddModelError(nameof(model.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Types = await GetTypes();

                return View(model);
            }

            var entity = new Event()
            {
                CreatedOn = DateTime.Now,
                Description = model.Description,
                Name = model.Name,
                OrganizerId = GetUserId(),
                TypeId = model.TypeId,
                Start = start,
                End = end
            };

            await data.Events.AddAsync(entity);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //Finding the Event and it's Participants
            var currentEvent = await data.Events
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            var eventParticipants = await data.EventParticipants
                .Where(ep => ep.EventId == id)
                .ToListAsync();

            //Checking if the Event exists
            if (currentEvent == null)
            {
                return BadRequest();
            }
            //Checking if the User that wants to Delete the Event is not it's creator
            if (currentEvent.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }
            //Checking if there are any participants of this event and removing them
            if (eventParticipants != null && eventParticipants.Any())
            {
                data.EventParticipants.RemoveRange(eventParticipants);
            }

            //Removing the Event and redirecting the User
            data.Events.Remove(currentEvent);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        public async Task<IActionResult> Details(int id)
        {
            var model = await data.Events
                .Where(e => e.Id == id)
                .AsNoTracking()
                .Select(e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    CreatedOn = e.CreatedOn.ToString(DataConstants.DateFormat),
                    Description = e.Description,
                    End = e.End.ToString(DataConstants.DateFormat),
                    Name = e.Name,
                    Organiser = e.Organizer.UserName,
                    Start = e.Start.ToString(DataConstants.DateFormat),
                    Type = e.Type.Name
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

    }
}
