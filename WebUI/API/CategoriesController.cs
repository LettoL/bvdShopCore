﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresData;
using WebUI.Dtos;

namespace WebUI.API
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ShopContext _db;
        private readonly PostgresContext _postgresContext;

        public CategoriesController(ShopContext db, PostgresContext postgresContext)
        {
            _db = db;
            _postgresContext = postgresContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _db.Categories
                    .Select(x => new CategoryItemFilter()
                    {
                        Id = x.Id,
                        Title = x.Title
                    }).ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }
    }
}