using Microsoft.AspNetCore.Mvc;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    #region snippetErrorCode
    public enum ErrorCode
    {
        TodoItemNameAndNotesRequired,
        TodoItemIDInUse,
        RecordNotFound,
        CouldNotCreateItem,
        CouldNotUpdateItem,
        CouldNotDeleteItem
    }
    #endregion
    #region snippetDI
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        int errorPercent = 100;  // Er tallet 0 returneres altid HTTP 200, jo større værdi jo større chanche for fejl. Vælges 100 vil den hver gang returnere HTTP 500.

        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoRepository todoRepository, ILogger<TodoItemsController> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }
        #endregion

        #region snippet
        [HttpGet]
        public IActionResult List()
        {
            Random rnd = new Random();
            int rndInteger = rnd.Next(1, 101);
            if (rndInteger <= errorPercent)
            {
                _logger.LogWarning("---> StatusCode 500");
                return StatusCode(StatusCodes.Status500InternalServerError, new List<Task>());
            }
            else
            {
                _logger.LogWarning("---> StatusCode 200");
                return Ok(_todoRepository.All);
            }

            //return Ok(_todoRepository.All);
        }

        [HttpGet("{id}")]
        public IActionResult GetItemById(string id)
        {
            try
            {
                var item = _todoRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                return Ok(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
        }
        #endregion

        #region snippetCreate
        [HttpPost]
        public IActionResult Create([FromBody]TodoItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                bool itemExists = _todoRepository.DoesItemExist(item.ID!);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.TodoItemIDInUse.ToString());
                }
                _todoRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(item);
        }
        #endregion

        #region snippetEdit
        [HttpPut]
        public IActionResult Edit([FromBody] TodoItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                var existingItem = _todoRepository.Find(item.ID!);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _todoRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }
        #endregion
        
        #region snippetDelete
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _todoRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _todoRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
        #endregion
    }
}
