using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Locations controller. API endpoints for PublicApi.DTO.v1.Locations Create, Read, Update and Delete. 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private PublicApi.DTO.v1.Mappers.LocationMapper _locationMapper;

        /// <summary>
        /// Locations controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Location</param>
        public LocationsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _locationMapper = new LocationMapper(mapper);
        }

        // GET: api/Locations
        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.Locations</returns>
        [HttpGet]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Location>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Location>>> GetLocations()
        {
            return Ok((await _bll.Locations.GetAllWithAdditionalDataCount())
                .Select(l => _locationMapper.Map(l)));
        }

        // GET: api/Locations/5
        /// <summary>
        /// Get one location.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Location</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Location), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Location>> GetLocation(Guid id)
        {
            var location = await _bll.Locations.FirstOrDefaultAsync(id);

            if (location == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            return _locationMapper.Map(location)!;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update location.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="location">PublicApi.DTO.v1.Location</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutLocation(Guid id, PublicApi.DTO.v1.Location location)
        {
            if (id != location.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllLocation = _locationMapper.Map(location)!;

            _bll.Locations.Update(bllLocation);

            
            await _bll.SaveChangesAsync();
           

            return NoContent();
        }

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Saves new location.
        /// </summary>
        /// <param name="location">PublicApi.DTO.v1.LocationAdd </param>
        /// <returns>PublicApi.DTO.v1.Location</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Location), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApi.DTO.v1.Location>> PostLocation(PublicApi.DTO.v1.LocationAdd location)
        {
            var bllLocation = LocationMapper.MapToBll(location);
            var addedLocation = _bll.Locations.Add(bllLocation);
            await _bll.SaveChangesAsync();
            var returnLocation = _locationMapper.Map(addedLocation);

            return CreatedAtAction("GetLocation", new { id = returnLocation!.Id }, returnLocation);
        }

        // DELETE: api/Locations/5
        /// <summary>
        /// Deletes location.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var location = await _bll.Locations.FirstOrDefaultAsync(id);
            if (location == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Locations.Remove(location);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
