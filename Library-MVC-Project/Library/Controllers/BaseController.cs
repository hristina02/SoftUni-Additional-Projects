﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

        protected string? GetUserId()
        {
            string? id = null;

            if (User != null)
            {
                id=User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }
  

    }
}
