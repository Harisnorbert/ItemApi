using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ListManagerApi.Data;
using ListManagerApi.Models;
using System.Diagnostics;

namespace ListManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        return await _context.Items.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> UpdateItems([FromBody] List<Item> items)
    {
        var existingIds = items.Where(i => i.Id != 0).Select(i => i.Id).ToList();
        var toDelete = _context.Items.Where(i => !existingIds.Contains(i.Id));
        _context.Items.RemoveRange(toDelete);

        foreach (var item in items)
        {
            if (item.Id == 0)
            {
                _context.Items.Add(item);
            }
            else
            {
                var existingItem = await _context.Items.FindAsync(item.Id);
                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.Price = item.Price;
                    existingItem.Quantity = item.Quantity;
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(items);
    }
}