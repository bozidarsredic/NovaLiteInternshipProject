using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using ToDoApi.Infrastructure;
using ToDoApi.Notes.Commands;
using ToDoApi.Notes.Models;
using ToDoApi.ToDoLists.Commands;
using ToDoApi.ToDoLists.Queries;
using ToDoCore;

namespace ToDoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/to-do-lists")]
    public class ToDoListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(IEnumerable<Response>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetToDoLists()
        {
            var owner = User.GetEmail();
            var result = await _mediator.Send(new GetToDoLists.Query
            {
                Owner = owner
            });
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(Response), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetToDoListById([FromRoute] Guid id)
        {
            var owner = User.GetEmail();
            var result = await _mediator.Send(new GetToDoListById.Query
            {
                Id = id,
                Owner = owner
            });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateToDoList([FromBody] UpdateToDoList.Command command)
        {
            var owner = User.GetEmail();
            command.Owner = owner;
            var result = await _mediator.Send(command);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id}/{position}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateToDoListPosition([FromRoute] Guid id, [FromRoute] int position)
        {
            var owner = User.GetEmail();

            try
            {
                var result = await _mediator.Send(new UpdateToDoListPosition.Command
                {
                    Id = id,
                    Position = position,
                    Owner = owner
                }); ;
                return Ok();

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ToDoList), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateToDoList([FromBody] CreateToDoList.Command command)
        {
            var owner = User.GetEmail();
            command.Owner = owner;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteToDoList([FromRoute] Guid id)
        {
            var owner = User.GetEmail();
            var result = await _mediator.Send(new DeleteToDoList.Command
            {
                Id = id,
                Owner = owner
            });
            return result ? Ok() : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string? title)
        {
            var owner = User.GetEmail();
            var result = await _mediator.Send(new SearchByTitle.Query
            {
                Title = title ?? string.Empty,
                Owner = owner
            });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}/notes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(Note), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotesByListId([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetNotesByListId.Query
            {
                Id = id
            });

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("{id}/notes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(Note), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNote([FromRoute] Guid id, [FromBody] NoteModel model)
        {
            var result = await _mediator.Send(new CreateNote.Command
            {
                Id = id,
                Model = model
            });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{listId}/notes/{noteId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid listId, [FromRoute] Guid noteId, [FromBody] NoteModel model)
        {
            var result = await _mediator.Send(new UpdateNote.Command
            {
                ToDoListId = listId,
                NotetId = noteId,
                Model = model
            });
            return result ? Ok() : NotFound();
        }

        [HttpPut("{listId}/{noteId}/{position}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateNotePosition([FromRoute] Guid listId, [FromRoute] Guid noteId, [FromRoute] int position)
        {
            try
            {
                var result = await _mediator.Send(new UpdateNotePosition.Command
                {
                    ListId = listId,
                    NoteId = noteId,
                    Position = position
                }); ;
                return Ok();

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{toDoListId}/notes/{noteId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid toDoListId, [FromRoute] Guid noteId)
        {
            var result = await _mediator.Send(new DeleteNote.Command
            {
                NoteId = noteId,
                ToDoListId = toDoListId
            });
            return result ? Ok() : NotFound();

        }

        [AllowAnonymous]
        [HttpGet("share/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(Response), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShareToDoListById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetShareToDoListById.Query
            {
                Id = id,
            });

            return result == null ? NotFound() : Ok(result);
        }

    }

}
