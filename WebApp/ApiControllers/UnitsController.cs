using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using Unit = BLL.App.DTO.Unit;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Controller for PublicApiDTOv1.Unit GetAll, Get by Id, Put, Post, Delete
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UnitsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.UnitMapper _unitMapper;

        /// <summary>
        /// Units Controller constructctor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Unit</param>
        public UnitsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _unitMapper = new UnitMapper(mapper);
        }

        // GET: api/Units
        /// <summary>
        /// Get List of Units
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.Unit</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Unit>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Unit>>> GetUnits()
        {
            return Ok((await _bll.Units.GetAllAsync())
                .Select(u => _unitMapper.Map(u)));
        }

        // GET: api/Units/5
        /// <summary>
        /// Get Unit by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.Unit</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Unit), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublicApi.DTO.v1.Unit>> GetUnit(Guid id)
        {
            var unit = await _bll.Units.FirstOrDefaultAsync(id);

            if (unit == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnUnit = _unitMapper.Map(unit)!;

            return returnUnit;
        }

        // PUT: api/Units/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update Unit 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutUnit(Guid id, PublicApi.DTO.v1.Unit unit)
        {
            if (id != unit.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllUnit = _unitMapper.Map(unit)!;
            _bll.Units.Update(bllUnit);
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Units
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new Unit
        /// </summary>
        /// <param name="unit">PublicApi.DTO.v1.UnitAdd</param>
        /// <returns>PublicApi.DTO.v1.Unit</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Unit), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> PostUnit(PublicApi.DTO.v1.UnitAdd unit)
        {
            var bllUnit = UnitMapper.MapToBll(unit);
            var addedUnit = _bll.Units.Add(bllUnit);
            await _bll.SaveChangesAsync();

            var returnUnit = _unitMapper.Map(addedUnit);

            return CreatedAtAction("GetUnit", new {id = returnUnit!.Id}, returnUnit);
        }

        // DELETE: api/Units/5
        /// <summary>
        /// Delete Unit
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteUnit(Guid id)
        {
            var unit = await _bll.Units.FirstOrDefaultAsync(id);
            if (unit == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Units.Remove(unit);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}