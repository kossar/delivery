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
using PublicApi.DTO.v1.Enums;
using PublicApi.DTO.v1.Mappers;
using Transport = BLL.App.DTO.Transport;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Transports controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransportsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TransportMapper _transportMapper;

        /// <summary>
        /// Transports controller constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Transport</param>
        public TransportsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _transportMapper = new TransportMapper(mapper);
        }

        // GET: api/Transports
        /// <summary>
        /// Get all Transport that are connected to User
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.Transport</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Transport>>> GetTransports()
        {
            return Ok((await _bll.Transports.GetAllAsync(User.GetUserId()!.Value))
                .Select(t => _transportMapper.Map(t)));
        }

        /// <summary>
        /// Gets user transportneed requests, that are waiting for other user action
        /// </summary>
        /// <returns></returns>
        [HttpGet("PendingNeeds")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Transport>>> GetPendingTransportNeeds()
        {
            return Ok((await _bll.Transports.GetUserTransportNeedTransports(User.GetUserId()!.Value))
                .Select(t => _transportMapper.Map(t)));
        }
        
        /// <summary>
        /// Gets user transport offer requests, that are waiting for other user action
        /// </summary>
        /// <returns></returns>
        [HttpGet("PendingOffers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Transport>>> GetPendingTransportOffers()
        {
            return Ok((await _bll.Transports.GetUserTransportOfferTransports(User.GetUserId()!.Value))
                .Select(t => _transportMapper.Map(t)));
        }
        
        /// <summary>
        /// Gets transport need requests, that are waiting for current user action
        /// </summary>
        /// <returns></returns>
        [HttpGet("WaitingNeeds")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Transport>>> GetWaitingTrasportNeeds()
        {
            return Ok((await _bll.Transports.GetUserTransportNeedsWaitingForUserAction(User.GetUserId()!.Value))
                .Select(t => _transportMapper.Map(t)));
        }
        
        /// <summary>
        /// Gets transport offer requests, that are waiting for current user action
        /// </summary>
        /// <returns></returns>
        [HttpGet("WaitingOffers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Transport>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Transport>>> GetWaitingTrasportOffers()
        {
            return Ok((await _bll.Transports.GetUserTransportOffersWaitingForUserAction(User.GetUserId()!.Value))
                .Select(t => _transportMapper.Map(t)));
        }
        
        // GET: api/Transports/5
        /// <summary>
        /// Get Transport by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Transport</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Transport), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Transport>> GetTransport(Guid id)
        {
            var transport = await _bll.Transports.FirstOrDefaultAsync(id, User.GetUserId()!.Value);

            if (transport == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnTransport = _transportMapper.Map(transport)!;
            return returnTransport;
        }

        // PUT: api/Transports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update Transport
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="transport">PublicApi.DTO.v1.Transport</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutTransport(Guid id, PublicApi.DTO.v1.Transport transport)
        {
            if (id != transport.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllTransport = _transportMapper.Map(transport)!;
            _bll.Transports.Update(bllTransport);

            await _bll.SaveChangesAsync();


            return NoContent();
        }

        // POST: api/Transports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new Transport
        /// </summary>
        /// <param name="transport">PublicApi.DTO.v1.TransportAdd</param>
        /// <returns>PublicApi.DTO.v1.Transport</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Transport), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Transport>> PostTransport(PublicApi.DTO.v1.TransportAdd transport)
        {
            var bllTransport = TransportMapper.MapToBll(transport);
            var addedTransport = await _bll.Transports
                .InitialTransportAdd(bllTransport, bllTransport.TransportOfferId, bllTransport.TransportNeedId, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();

            var returnTransport = _transportMapper.Map(addedTransport);
            return CreatedAtAction("GetTransport", new {id = returnTransport!.Id}, returnTransport);
        }

        // DELETE: api/Transports/5
        /// <summary>
        /// Delete Transport
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTransport(Guid id)
        {
            var transport = await _bll.Transports.FirstOrDefaultAsync(id);
            if (transport == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Transports.Remove(transport);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TransportExists(Guid id)
        {
            return await _bll.Transports.ExistsAsync(id);
        }
    }
}