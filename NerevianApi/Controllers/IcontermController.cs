using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using NerevianApi.Models.Incoterms;

namespace NerevianApi.Controllers
{
    [Route("api/incoterms")]
    [ApiController]
    public class IncotermController : ControllerBase
    {
        private readonly NerevianDbContext _context;

        public IncotermController(NerevianDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}/{newStepId}")]
        public async Task<IActionResult> UpdateIncotermStep(int id, int newStepId)
        {
            IActionResult response;
            var incoterm = await _context.Incoterm.FindAsync(id);

            if (incoterm == null)
            {
                response = NotFound(new { message = $"Incoterm with ID {id} not found." });
            }
            else
            {
                var stepExists = await _context.TrackingStep.AnyAsync(s => s.Id == newStepId);

                if (!stepExists)
                {
                    response = BadRequest(new { message = $"TrackingStep ID {newStepId} is invalid." });
                }
                else
                {
                    try
                    {
                        incoterm.TrackingStepId = newStepId;
                        incoterm.UpdatedAt = DateTime.Now;

                        await _context.SaveChangesAsync();

                        response = Ok(new
                        {
                            message = "Status updated successfully",
                            id = id,
                            newStepId = newStepId
                        });
                    }
                    catch (DbUpdateException)
                    {
                        response = StatusCode(500, "An error occurred while updating the database.");
                    }
                }
            }

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetIncotermById(int id)
        {
            ActionResult<object> response;

            var incoterm = await _context.Incoterm
                .Include(i => i.IncotermType)
                .Include(i => i.TrackingStep)
                .Where(i => i.Id == id)
                .Select(i => new {
                    i.Id,
                    i.Name,
                    Type = i.IncotermType.Name,
                    TrackingStep = i.TrackingStep.Name,
                    i.IncotermTypeId,
                    i.TrackingStepId,
                    i.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (incoterm == null)
            {
                response = NotFound(new { message = $"Incoterm with ID {id} not found." });
            }
            else
            {
                response = Ok(incoterm);
            }

            return response;
        }
    }
}