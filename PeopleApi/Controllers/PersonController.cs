
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleApi.Extensions;
using PeopleApi.Models;
using PeopleApi.Services;

namespace PersonApi
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Get all People stored in datastore
        /// </summary>
        /// <response code="200">Successfully returned a list of all persons in the data store</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<PersonResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok((await _personService.GetPeople()).Select(x => x.AsPersonResponse()));
        }

        /// <summary>
        /// Get a single stored person by their Id from the datastore
        /// </summary>
        /// <response code="200">The person was found and returned successfully</response>
        /// <response code="404">The person could not be found in the datastore</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var person = await _personService.GetPerson(id);
            if (person == null)
                return NotFound();

            return Ok(person.AsPersonResponse());
        }

        /// <summary>
        /// Create a new Person in the datastore
        /// </summary>
        /// <response code="201">The person was successfully created</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody]PersonRequest request)
        {
            var newPerson = await _personService.CreatePerson(request);
            return Created($"person/{newPerson.Id}", newPerson);
        }

        /// <summary>
        /// Update an existing Person in the datastore
        /// </summary>
        /// <response code="204">The Person was updated</response>
        /// <response code="404">The person to update was not found in the datastore</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody]PersonRequest request)
        {
            try
            {
                await _personService.UpdatePerson(id, request);
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a Person from the datastore
        /// </summary>
        /// <response code="204">Delete Operation completed</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _personService.DeletePerson(id);
            return NoContent();
        }
    }
}