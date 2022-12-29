using System.Data;
using Microsoft.AspNetCore.Mvc;
using myapi.Dtos;
using myapi.Entities;
using myapi.Extensions;
using myapi.Repositories;

namespace myapi.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemRepository _itemRepository;

        public ItemsController(ILogger<ItemsController> logger, IItemRepository itemRepository)
        {
            _logger = logger;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            var items = await _itemRepository.GetItemsAsync();
            return items.Select(item => item.AsDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id)
        {
            var item = await _itemRepository.GetItemAsync(id);
            return item != null ? Ok(item.AsDto()) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto createItem)
        {

            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = createItem.Name,
                Price = createItem.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await _itemRepository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());


        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto updateItem)
        {

            var exisitngItem = await _itemRepository.GetItemAsync(id);

            if (exisitngItem == null)
            {
                return NotFound();
            }

            var updatedItemDto = exisitngItem with
            {
                Name = updateItem.Name,
                Price = updateItem.Price,
            };

            await _itemRepository.UpdateItemAsync(updatedItemDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {

            var exisitngItem = await _itemRepository.GetItemAsync(id);

            if (exisitngItem == null)
            {
                return NotFound();
            }

            await _itemRepository.DeleteAsync(id);

            return NoContent();

        }

    }
}