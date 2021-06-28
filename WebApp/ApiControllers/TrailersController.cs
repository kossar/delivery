using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Trailers API controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TrailersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TrailerMapper _trailerMapper;

        /// <summary>
        /// Trailers API controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Trailer</param>
        public TrailersController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _trailerMapper = new TrailerMapper(mapper);
        }

        // GET: api/Trailers
        /// <summary>
        /// Gets Trailers That belongs to the user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Trailer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Trailer>>> GetTrailers()
        {
            return Ok((await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value))
                .Select(t => _trailerMapper.Map(t)));
        }

        // GET: api/Trailers/5
        /// <summary>
        /// Gets PublicApi.DTO.v1.Trailer by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Trailer</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Trailer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Trailer>> GetTrailer(Guid id)
        {
            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id);

            if (trailer == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnTrailer = _trailerMapper.Map(trailer)!;

            return returnTrailer;
        }

        // PUT: api/Trailers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update Trailer
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="trailer">PublicApi.DTO.v1.Trailer</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutTrailer(Guid id, PublicApi.DTO.v1.Trailer trailer)
        {
            if (id != trailer.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllTrailer = _trailerMapper.Map(trailer)!;
            _bll.Trailers.Update(bllTrailer);

            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Trailers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new Trailer
        /// </summary>
        /// <param name="trailer"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Trailer), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Trailer>> PostTrailer(PublicApi.DTO.v1.TrailerAdd trailer)
        {
            var bllTrailer = TrailerMapper.MapToBll(trailer);
            bllTrailer.AppUserId = User.GetUserId()!.Value;
            
            var addedTrailer = _bll.Trailers.Add(bllTrailer);
            await _bll.SaveChangesAsync();
            Console.WriteLine(addedTrailer.DimensionsId);
            Console.WriteLine(addedTrailer.Dimensions);

            var returnTrailer = _trailerMapper.Map(addedTrailer);
            Console.WriteLine(returnTrailer!.ToString());
            return CreatedAtAction("GetTrailer", new {id = returnTrailer!.Id}, returnTrailer);
        }

        // DELETE: api/Trailers/5
        /// <summary>
        /// Delete Trailer.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTrailer(Guid id)
        {
            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id);
            if (trailer == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Trailers.Remove(trailer);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}