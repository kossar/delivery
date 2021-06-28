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
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Controlelr for dimensions.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DimensionsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        private PublicApi.DTO.v1.Mappers.DimensionsMapper _dimensionsMapper;

        /// <summary>
        /// Dimensions controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi Dimensions</param>
        public DimensionsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _dimensionsMapper = new DimensionsMapper(mapper);
        }

        // GET: api/Dimensions
        /// <summary>
        /// Gets all dimensions.
        /// </summary>
        /// <returns>List of </returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.Dimensions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Dimensions>>> GetDimensions()
        {
            return Ok((await _bll.Dimensions.GetAllAsync())
                .Select(d => _dimensionsMapper.Map(d)));
        }

        // GET: api/Dimensions/5
        /// <summary>
        /// Gets one PublicApi.DTO.v1.Dimensions object by id.
        /// </summary>
        /// <param name="id">GUID of PublicApi.DTO.v1.Dimensions</param>
        /// <returns>PublicApi.DTO.v1.Dimensions object</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Dimensions), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Dimensions>> GetDimensions(Guid id)
        {
            var dimensions = _dimensionsMapper.Map(await _bll.Dimensions.FirstOrDefaultAsync(id));

            if (dimensions == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }
            return dimensions;  
        }

        // PUT: api/Dimensions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update existing dimensions.
        /// </summary>
        /// <param name="id">GUID of existing dimensions.</param>
        /// <param name="dimensions">New PublicApi.DTO.v1.Dimensions object</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Dimensions), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDimensions(Guid id, PublicApi.DTO.v1.Dimensions dimensions)
        {
            if (id != dimensions.Id)
            {
                return BadRequest(new Message("Cannot update. Ids are not matching."));
            }
            
            var bllDimensions = _dimensionsMapper.Map(dimensions)!;
            _bll.Dimensions.Update(bllDimensions);


            await _bll.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Dimensions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Saves new PublicApi.DTO.v1.Dimensions object.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns>PublicApi.DTO.v1.Dimensions</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Dimensions), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApi.DTO.v1.Dimensions>> PostDimensions([FromBody] PublicApi.DTO.v1.DimensionsAdd dimensions)
        {
            var bllDimensions = DimensionsMapper.MapToBll(dimensions);
            var addedDimensions = _bll.Dimensions.Add(bllDimensions);
            await _bll.SaveChangesAsync();

            var returnDimensions = _dimensionsMapper.Map(addedDimensions);

            return CreatedAtAction("GetDimensions", new {id = returnDimensions!.Id}, returnDimensions);
        }

        // DELETE: api/Dimensions/5
        /// <summary>
        /// Deletes dimensions by id.
        /// </summary>
        /// <param name="id">GUID of dimensions object.</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDimensions(Guid id)
        {
            var dimensions = await _bll.Dimensions.FirstOrDefaultAsync(id);
            if (dimensions == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.Dimensions.Remove(dimensions);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}