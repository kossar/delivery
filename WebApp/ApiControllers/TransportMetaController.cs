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
    /// TransportMeta controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TransportMetaController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TransportMetaMapper _transportMetaMapper;

        /// <summary>
        /// TransportMeta controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        /// <param name="mapper">Mapper to map between BLL and PublicApi TransportMeta</param>
        public TransportMetaController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _transportMetaMapper = new TransportMetaMapper(mapper);
        }

        // GET: api/TransportMeta
        /// <summary>
        /// Get all TransportMeta objects
        /// </summary>
        /// <returns>List of PublicApi.DTO.v1.TransportMeta</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PublicApi.DTO.v1.TransportMeta>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TransportMeta>>> GetTransportMetas()
        {
            return Ok((await _bll.TransportMeta.GetAllAsync())
                .Select(t => _transportMetaMapper.Map(t)));
        }

        // GET: api/TransportMeta/5
        /// <summary>
        /// Get TransportMeta by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>PublicApi.DTO.v1.TransportMeta</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TransportMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportMeta>> GetTransportMeta(Guid id)
        {
            var transportMeta = await _bll.TransportMeta.FirstOrDefaultAsync(id);

            if (transportMeta == null)
            {
                return NotFound(new Message("Cannot find this id: " + id));
            }

            var returnTransportMeta = _transportMetaMapper.Map(transportMeta)!;
            return returnTransportMeta;
        }

        // PUT: api/TransportMeta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update TransportMeta
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="transportMeta"></param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTransportMeta(Guid id, PublicApi.DTO.v1.TransportMeta transportMeta)
        {
            if (id != transportMeta.Id)
            {
                return BadRequest(new Message("Cannot update. Id: " + id + " not found." ));
            }

            var bllTransportMeta = _transportMetaMapper.Map(transportMeta)!;
            _bll.TransportMeta.Update(bllTransportMeta);

            await _bll.SaveChangesAsync();
            

            return NoContent();
        }

        // POST: api/TransportMeta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Save new TransportMeta
        /// </summary>
        /// <param name="transportMeta">PublicApi.DTO.v1.TransportMetaAdd</param>
        /// <returns>PublicApi.DTO.v1.TransportMeta</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TransportMeta), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApi.DTO.v1.TransportMeta>> PostTransportMeta(PublicApi.DTO.v1.TransportMetaAdd transportMeta)
        {
            var bllTransportMeta = TransportMetaMapper.MapToBll(transportMeta);
            var addedTransportMeta = _bll.TransportMeta.Add(bllTransportMeta);
            await _bll.SaveChangesAsync();

            var returnTransportMeta = _transportMetaMapper.Map(addedTransportMeta);

            return CreatedAtAction("GetTransportMeta", new { id = returnTransportMeta!.Id }, returnTransportMeta);
        }

        // DELETE: api/TransportMeta/5
        /// <summary>
        /// Delete TransportMeta
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransportMeta(Guid id)
        {
            var transportMeta = await _bll.TransportMeta.FirstOrDefaultAsync(id);
            if (transportMeta == null)
            {
                return NotFound(new Message("Cant delete. Cannot find id: " + id));
            }

            _bll.TransportMeta.Remove(transportMeta);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
