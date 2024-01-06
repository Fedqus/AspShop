using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class CompanyManager : EntityManager<Company, CompanyForm>
    {
        private readonly FileManager _fileManager;
        private readonly string _directoryName;

        public CompanyManager(DatabaseContext context, FileManager fileManager) : base(context)
        {
            _fileManager = fileManager;
            _directoryName = "companies";
        }

        public override Company CreateEntity(CompanyForm form)
        {
            var entity = new Company
            {
                Name = form.Name,
                ImagePath = _fileManager.UploadFile(form.Image, _directoryName)
            };
            return entity;
        }

        public override CompanyForm CreateForm(Company entity)
        {
            var form = new CompanyForm
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return form;
        }

        public override async Task Update(Company entity, CompanyForm form)
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
