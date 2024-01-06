using Microsoft.EntityFrameworkCore.ChangeTracking;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Services
{
    public class ProductManager : EntityManager<Product, ProductForm>
    {
        private readonly FileManager _fileManager;
        private readonly string _directoryName;

        public ProductManager(DatabaseContext context, FileManager fileManager) : base(context)
        {
            _fileManager = fileManager;
            _directoryName = "products";
        }

        public override Product CreateEntity(ProductForm form)
        {
            var entity = new Product
            {
                Name = form.Name,
                Description = form.Description,
                Price = form.Price,
                Amount = form.Amount,
                CategoryId = form.CategoryId,
                CompanyId = form.CompanyId
            };
            return entity;
        }

        public override ProductForm CreateForm(Product entity)
        {
            var form = new ProductForm
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Amount = entity.Amount,
                CategoryId = entity.CategoryId,
                CompanyId = entity.CompanyId,
                TagIds = entity.Tags.Select(x => x.TagId).ToList()
            };
            return form;
        }

        public override async Task<EntityEntry<Product>> AddAsync(ProductForm form)
        {
            var result = await base.AddAsync(form);

            _context.AddRange(form.TagIds.Select(tagId => new ProductTag
            {
                ProductId = result.Entity.Id,
                TagId = tagId
            }));
            _context.AddRange(form.Images.Select(image => new ProductImage
            {
                ProductId = result.Entity.Id,
                ImagePath = _fileManager.UploadFile(image, _directoryName)
            }));

            await _context.SaveChangesAsync();
            return result;
        }

        public override async Task Update(Product entity, ProductForm form)
        {
            entity.Name = form.Name;
            entity.Description = form.Description;
            entity.Price = form.Price;
            entity.Amount = form.Amount;
            entity.CategoryId = form.CategoryId;
            entity.CompanyId = form.CompanyId;

            _context.RemoveRange(entity.Tags);
            _context.AddRange(form.TagIds.Select(tagId => new ProductTag
            {
                ProductId = entity.Id,
                TagId = tagId
            }));

            if (form.Images != null)
            {
                _context.RemoveRange(entity.Images);
                _context.AddRange(form.Images.Select(image => new ProductImage
                {
                    ProductId = entity.Id,
                    ImagePath = _fileManager.UploadFile(image, _directoryName)
                }));
            }
            await _context.SaveChangesAsync();
        }
    }
}
