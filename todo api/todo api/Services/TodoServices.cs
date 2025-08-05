using todo_api.Models;

namespace todo_api.Services
{
    public class TodoServices
    {
        private readonly List<TodoItem> _item = new();
        private int _nextId = 0;

        public IEnumerable<TodoItem> GetAll() => _item;

        public TodoItem? GetById(int id) => _item.FirstOrDefault(x => x.id == id);

        public TodoItem Create(TodoItem item)
        {   
            item.id = ++_nextId;
            _item.Add(item);
            return item;
        }

        public bool Update(int id,TodoItem item)
        {
            var i = GetById(id);
            if(i == null)
            {
                return false;
            }
            i.title = item.title;
            i.completed = item.completed;
            return true;
        }
        public bool Delete(int id)
        {

            var item = GetById(id);
            item.id = ++_nextId;
            return item != null && _item.Remove(item);
        }
    }
}
