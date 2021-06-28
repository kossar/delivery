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
using TransportOffer = PublicApi.DTO.v1.TransportOffer;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// TransportOffers controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransportOffersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TransportOfferMapper _transportOfferMapper;

        /// <summary>
        /// TransportOffers controller constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map TransportOffer entity between BLL and PublicApi layers</param>
        public TransportOffersController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _transportOfferMapper = new TransportOfferMapper(mapper);
        }

        // GET: api/TransportOffers
        /// <summary>
        /// Get all TransportOffers
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.TransportOffer</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportOfferListItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TransportOfferListItem>>> GetTransportOffers()
        {
             var res = (await _bll.TransportOffers.GetAllAsync())
                 .Select(t => _transportOfferMapper.Map(t));
             var listItems = res.Select(x => TransportOfferMapper.MapToListItem(x));
            return Ok(listItems);

        }
        
        // GET: api/TransportOffers
        /// <summary>
        /// Get all User TransportOffers
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.TransportOffer</returns>
        [HttpGet("user")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportOffer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TransportOffer>>> GetUserTransportOffers()
        {
            var res = (await _bll.TransportOffers.GetAllAsync(User.GetUserId()!.Value))
                .Select(t => _transportOfferMapper.Map(t));
            var listItems = res.Select(TransportOfferMapper.MapToListItem);
            // return Ok((await _bll.TransportOffers.GetAllAsync(User.GetUserId()!.Value))
            //     .Select(t => _transportOfferMapper.Map(t)));
            return Ok(listItems);
        }

        // GET: api/TransportOffers/5
        /// <summary>
        /// Get all TransportOffers
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>List of TransportOffers</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportOffer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportOffer>> GetTransportOffer(Guid id)
        {
            
            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id);
            
            if (transportOffer == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            return _transportOfferMapper.Map(transportOffer)!;
        }

        // PUT: api/TransportOffers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update TransportOffer
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="transportOffer">PublicApi.DTO.v1.TransportOffer</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutTransportOffer(Guid id, PublicApi.DTO.v1.TransportOffer transportOffer)
        {
            if (id != transportOffer.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found."));
            }
            
            var bllTransportOffer = _transportOfferMapper.Map(transportOffer);
            if (bllTransportOffer!.Trailer != null)
            {
                bllTransportOffer.Trailer.AppUserId = User.GetUserId()!.Value;
            }

            bllTransportOffer.Vehicle!.AppUserId = User.GetUserId()!.Value;
            _bll.TransportOffers.Update(bllTransportOffer!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }   

        // POST: api/TransportOffers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new TransportOffer
        /// </summary>
        /// <param name="transportOffer">PublicApi.DTO.v1.TransportOfferAdd</param>
        /// <returns>PublicApi.DTO.v1.TransportOffer</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TransportOffer), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportOffer>> PostTransportOffer(PublicApi.DTO.v1.TransportOfferAdd transportOffer)
        {
            var bllTransportOffer = TransportOfferMapper.MapToBll(transportOffer);
            bllTransportOffer.AppUserId = User.GetUserId()!.Value;
            var addedTransportOffer = _bll.TransportOffers.Add(bllTransportOffer);
            await _bll.SaveChangesAsync();

            var returnTransportOffer = _transportOfferMapper.Map(addedTransportOffer);
            return CreatedAtAction("GetTransportOffer", new {id = returnTransportOffer!.Id}, returnTransportOffer);
        }

        // DELETE: api/TransportOffers/5
        /// <summary>
        /// Delete TransportOffer
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteTransportOffer(Guid id)
        {
            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id);
            if (transportOffer == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.TransportOffers.Remove(transportOffer, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TransportOfferExists(Guid id)
        {
            return await _bll.TransportOffers.ExistsAsync(id);
        }
    }
}