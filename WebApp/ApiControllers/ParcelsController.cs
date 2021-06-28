using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Parcels controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.ParcelMapper _parcelMapper;

        /// <summary>
        /// Parcels controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Parcel</param>
        public ParcelsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _parcelMapper = new ParcelMapper(mapper);
        }

        // GET: api/Parcels
        /// <summary>
        /// Get all Parcels.
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.Parcel</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Parcel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Parcel>>> GetParcels()
        {
            return Ok((await _bll.Parcels.GetAllAsync(User.GetUserId()!.Value))
                .Select(ParcelMapper.MapToPublicApi));
        }

        // GET: api/Parcels/5
        /// <summary>
        /// Get Parcel
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Parcel</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Parcel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<PublicApi.DTO.v1.Parcel>> GetParcel(Guid id)
        {
            var parcel = await _bll.Parcels.FirstOrDefaultAsync(id);

            if (parcel == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnParcel = ParcelMapper.MapToPublicApi(parcel)!; 
            return returnParcel;
        }

        // PUT: api/Parcels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update Parcel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parcel"></param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutParcel(Guid id, PublicApi.DTO.v1.Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllParcel = _parcelMapper.Map(parcel)!;
            _bll.Parcels.Update(bllParcel);
            
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Parcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new parcel.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Parcel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Parcel>> PostParcel(PublicApi.DTO.v1.ParcelAdd parcel)
        {
            var bllParcel = ParcelMapper.MapToBll(parcel);
            
            var addedParcel = _bll.Parcels.Add(bllParcel);
            await _bll.SaveChangesAsync();
            var returnParcel = _parcelMapper.Map(addedParcel);
            return CreatedAtAction("GetParcel", new { id = returnParcel!.Id }, returnParcel);
        }

        // DELETE: api/Parcels/5
        /// <summary>
        /// Delete parcel.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParcel(Guid id)
        {
            var parcel = await _bll.Parcels.FirstOrDefaultAsync(id, User.GetUserId()!.Value);
            if (parcel == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Parcels.Remove(parcel);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
