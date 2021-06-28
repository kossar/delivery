using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using PublicApi.DTO.v1.Enums;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// TransportNeeds controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransportNeedsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TransportNeedMapper _transportNeedMapper;

        /// <summary>
        /// TransportNeeds controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi TransportNeed</param>
        public TransportNeedsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _transportNeedMapper = new TransportNeedMapper(mapper);
        }

        // GET: api/TransportNeeds
        /// <summary>
        /// Gets all TransportNeeds that contains User id.
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.TransportNeed</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportNeed>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TransportNeed>>> GetTransportNeeds()
        {
            return Ok((await _bll.TransportNeeds.GetAllAsync())
                .Select(n => _transportNeedMapper.Map(n)));
        }
        
        // GET: api/TransportNeeds
        /// <summary>
        /// Gets all TransportNeeds that contains User id.
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.TransportNeed</returns>
        [HttpGet("user")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportNeed>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TransportNeed>>> GetUserTransportNeeds()
        {
            return Ok((await _bll.TransportNeeds.GetAllAsync(User.GetUserId()!.Value))
                .Select(n => _transportNeedMapper.Map(n)));
        }

        // GET: api/TransportNeeds/5
        /// <summary>
        /// Gets TransportNeed by ID
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.TransportNeed</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TransportNeed), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportNeed>> GetTransportNeed(Guid id)
        {
            var bllTransportNeed = await _bll.TransportNeeds.GetWithParcelIds(id, true);

            if (bllTransportNeed == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnTransportNeed = _transportNeedMapper.Map(bllTransportNeed)!;
            return returnTransportNeed;
        }

        // PUT: api/TransportNeeds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update TransportNeed.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="transportNeed">PublicApi.DTO.v1.TransportNeed</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutTransportNeed(Guid id, PublicApi.DTO.v1.TransportNeed transportNeed)
        {
            if (id != transportNeed.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllTransportNeed = _transportNeedMapper.Map(transportNeed)!;
            _bll.TransportNeeds.Update(bllTransportNeed);
            await _bll.SaveChangesAsync();
 

            return NoContent();
        }

        // POST: api/TransportNeeds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Saves new TransportNeed.
        /// </summary>
        /// <param name="transportNeed"></param>
        /// <returns>PublicApi.DTO.v1.TransportNeed</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TransportNeed), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportNeed>> PostTransportNeed(PublicApi.DTO.v1.TransportNeedAdd transportNeed)
        {
            var bllTransportNeed = TransportNeedMapper.MapToBll(transportNeed);
            bllTransportNeed.AppUserId = User.GetUserId()!.Value;
            var addedTransportNeed = _bll.TransportNeeds.Add(bllTransportNeed);
            await _bll.SaveChangesAsync();

            var returnTransportNeed = _transportNeedMapper.Map(addedTransportNeed);

            return CreatedAtAction("GetTransportNeed", new { id = returnTransportNeed!.Id }, returnTransportNeed);
        }

        // DELETE: api/TransportNeeds/5
        /// <summary>
        /// Deletes TransportNeed
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTransportNeed(Guid id)
        {
            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(id);
            if (transportNeed == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.TransportNeeds.Remove(transportNeed, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
