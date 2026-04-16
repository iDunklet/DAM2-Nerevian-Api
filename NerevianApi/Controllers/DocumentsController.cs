using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using NerevianApi.Models.Documents;


namespace NerevianApi.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly NerevianDbContext _context;

        public DocumentsController(NerevianDbContext context)
        {
            _context = context;
        }

        public class DocumentUploadDto
        {
            public string fileName { get; set; }
            public string originalName { get; set; }
            public int typeId { get; set; }
            public string path { get; set; }
            public string weight { get; set; }
            public int userId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentUploadDto dto)
        {
            try
            {

                var user = await _context.Users.FindAsync(dto.userId);
                if (user == null)
                {
                    return NotFound(new { message = "No se ha encontrado el usuario :(" });
                }

                var docType = await _context.DocumentTypes.FindAsync(dto.typeId);
                if (docType == null)
                {
                    return NotFound(new { message = "No se ha encontrado el tipo de documento :(" });
                }

              
                var newDoc = new Documents
                {
                    fileName = dto.fileName,
                    originalName = dto.originalName,
                    type = docType,
                    path = dto.path,
                    weight = dto.weight,
                    realesedBy = user,
                    RealesedDate = DateTime.UtcNow
                };

                _context.Documents.Add(newDoc);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Documento registrado correctamente :)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Se ha petado al guardar el documento",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("latest/{typeId}")]
        public async Task<IActionResult> GetLatestDocument(int typeId)
        {
            try
            {
                var latestDoc = await _context.Documents
                    .Include(d => d.type)
                    .Where(d => d.type.id == typeId)
                    .OrderByDescending(d => d.id)
                    .Select(d => new {
                        fileName = d.fileName,
                        originalName = d.originalName
                    })
                    .FirstOrDefaultAsync();

                if (latestDoc == null)
                {
                    return NotFound(new { message = "No se ha encontrado el documento :(" });
                }

                return Ok(latestDoc);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Se ha petado al buscar el documento",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}