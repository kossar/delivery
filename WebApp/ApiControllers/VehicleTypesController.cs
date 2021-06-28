using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
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
    /// VehicleTypes API controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.VehicleTypeMapper _vehicleTypeMapper;

        /// <summary>
        /// VehicleTypes controller constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi VehicleType</param>
        public VehicleTypesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _vehicleTypeMapper = new VehicleTypeMapper(mapper);
        }

        // GET: api/VehicleTypes
        /// <summary>
        /// Get all VehicleTypes
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.VehicleType</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.VehicleType>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.VehicleType>>> GetVehicleTypes()
        {
            return Ok((await _bll.VehicleTypes.GetAllAsync())
                .Select(v => _vehicleTypeMapper.Map(v)));
        }

        // GET: api/VehicleTypes/5
        /// <summary>
        /// Get VehicleTYpe by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.VehicleType</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.VehicleType), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublicApi.DTO.v1.VehicleType>> GetVehicleType(Guid id)
        {
            var vehicleType = await _bll.VehicleTypes.FirstOrDefaultAsync(id);

            if (vehicleType == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnVehicleType = _vehicleTypeMapper.Map(vehicleType)!;
            return returnVehicleType;
        }

        // PUT: api/VehicleTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update VehicleType
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vehicleType">PublicApi.DTO.v1.VehicleType</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutVehicleType(Guid id, PublicApi.DTO.v1.VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found."));
            }

            var bllVehicleType = _vehicleTypeMapper.Map(vehicleType)!;
            _bll.VehicleTypes.Update(bllVehicleType);
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/VehicleTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new VehicleType
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.VehicleType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PublicApi.DTO.v1.VehicleType>> PostVehicleType(
            PublicApi.DTO.v1.VehicleTypeAdd vehicleType)
        {
            var bllVehicleType = VehicleTypeMapper.MapToBll(vehicleType);
            
            var addedVehicleType = _bll.VehicleTypes.Add(bllVehicleType);
            await _bll.SaveChangesAsync();

            var returnVehicleType = _vehicleTypeMapper.Map(addedVehicleType);

            return CreatedAtAction("GetVehicleType", new {id = returnVehicleType!.Id}, returnVehicleType);
        }

        // DELETE: api/VehicleTypes/5
        /// <summary>
        /// Delete VehicleType
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteVehicleType(Guid id)
        {
            var vehicleType = await _bll.VehicleTypes.FirstOrDefaultAsync(id);
            if (vehicleType == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.VehicleTypes.Remove(vehicleType);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}