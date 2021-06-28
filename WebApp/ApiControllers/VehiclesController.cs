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
    /// Vehicle API controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VehiclesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.VehicleMapper _vehicleMapper;

        /// <summary>
        /// Vehicle controller constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Vehicle</param>
        public VehiclesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _vehicleMapper = new VehicleMapper(mapper);
        }

        // GET: api/Vehicles
        /// <summary>
        /// Get Vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Vehicle>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Vehicle>>> GetVehicles()
        {
            Console.WriteLine("APi call");
            return Ok((await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value))
                .Select(v => _vehicleMapper.Map(v)));
        }

        // GET: api/Vehicles/5
        /// <summary>
        /// Get Vehicle by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Vehicle</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Vehicle), StatusCodes.Status201Created)]
        public async Task<ActionResult<PublicApi.DTO.v1.Vehicle>> GetVehicle(Guid id)
        {
            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id);

            if (vehicle == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnVehicle = _vehicleMapper.Map(vehicle)!;
            return returnVehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update Vehicle
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vehicle">PublicApi.DTO.v1.Vehicle</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutVehicle(Guid id, PublicApi.DTO.v1.Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllVehicle = _vehicleMapper.Map(vehicle)!;
            _bll.Vehicles.Update(bllVehicle);

            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new Vehicle
        /// </summary>
        /// <param name="vehicle">PublicApi.DTO.v1.VehicleAdd</param>
        /// <returns>PublicApi.DTO.v1.Vehicle</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Vehicle), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Vehicle>> PostVehicle([FromBody] PublicApi.DTO.v1.VehicleAdd vehicle)
        {
            var bllVehicle = VehicleMapper.MapToBll(vehicle);
            bllVehicle.AppUserId = User.GetUserId()!.Value;
            
            var addedVehicle = _bll.Vehicles.Add(bllVehicle);
            await _bll.SaveChangesAsync();

            var vt = await _bll.VehicleTypes.FirstOrDefaultAsync(addedVehicle.VehicleTypeId);

            var returnVehicle = _vehicleMapper.Map(addedVehicle);
            
            return CreatedAtAction("GetVehicle", new { id = returnVehicle!.Id }, returnVehicle);
        }

        // DELETE: api/Vehicles/5
        /// <summary>
        /// Delete Vehicle
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicApi.DTO.v1.Vehicle))]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicApi.DTO.v1.Vehicle>> DeleteVehicle(Guid id)
        {
            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id);
            if (vehicle == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            var deletedVehicle = await _bll.Vehicles.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            var vt = await _bll.VehicleTypes.FirstOrDefaultAsync(deletedVehicle.VehicleTypeId);

            var returnVehicle = _vehicleMapper.Map(deletedVehicle);
            
            return Ok(returnVehicle);
        }
    }
}
