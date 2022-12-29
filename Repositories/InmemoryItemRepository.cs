using myapi.Entities;

namespace myapi.Repositories
{
    public class InMemoryItemRepository : IItemRepository
    {
        public List<Item> items = new List<Item>() {
            new Item { Id= Guid.NewGuid(), Name="Alpha", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
             new Item { Id= Guid.NewGuid(), Name="Beta", Price = 20, CreatedDate = DateTimeOffset.UtcNow},
              new Item { Id= Guid.NewGuid(), Name="Gamma", Price = 30, CreatedDate = DateTimeOffset.UtcNow}

        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.Run(() => items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var lists = await Task.Run(() => items);
            var item = lists.Where(x => x.Id == id).FirstOrDefault();
            return item;
        }

        public async Task CreateItemAsync(Item item)
        {
            await Task.Run(() => items.Add(item));
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = await Task.Run(() => items.FindIndex(x => x.Id == item.Id));
            items[index] = item;
        }

        public async Task DeleteAsync(Guid id)
        {
            var index = await Task.Run(() => items.FindIndex(x => x.Id == id));
            items.RemoveAt(index);
        }


    }
}