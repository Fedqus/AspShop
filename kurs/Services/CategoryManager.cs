using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class CategoryManager : EntityManager<Category, CategoryForm>
    {
        private readonly FileManager _fileManager;
        private readonly string _directoryName;

        public CategoryManager(DatabaseContext context, FileManager fileManager) : base(context)
        {
            _fileManager = fileManager;
            _directoryName = "categories";
        }

        public override Category CreateEntity(CategoryForm form)
        {
            var entity = new Category
            {
                Name = form.Name,
                ImagePath = _fileManager.UploadFile(form.Image, _directoryName)
            };
            return entity;
        }

        public override CategoryForm CreateForm(Category entity)
        {
            var form = new CategoryForm
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return form;
        }

        public override async Task Update(Category entity, CategoryForm form)
        {
            entity.Name = form.Name;
            if (form.Image != null)
            {
                entity.ImagePath = _fileManager.UploadFile(form.Image, _directoryName);
            }
            await _context.SaveChangesAsync();
        }
    }
}
