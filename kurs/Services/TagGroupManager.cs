using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class TagGroupManager : EntityManager<TagGroup, TagGroupForm>
    {
        public TagGroupManager(DatabaseContext context) : base(context) { }

        public override TagGroup CreateEntity(TagGroupForm form)
        {
            var entity = new TagGroup
            {
                Name = form.Name
            };
            return entity;
        }

        public override TagGroupForm CreateForm(TagGroup entity)
        {
            var form = new TagGroupForm
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return form;
        }

        public override async Task Update(TagGroup entity, TagGroupForm form)
        {
            entity.Name = form.Name;
            await _context.SaveChangesAsync();
        }
    }
}
