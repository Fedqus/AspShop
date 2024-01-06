using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class TagManager : EntityManager<Tag, TagForm>
    {
        public TagManager(DatabaseContext context) : base(context) { }

        public override Tag CreateEntity(TagForm form)
        {
            var entity = new Tag
            {
                Name = form.Name,
                TagGroupId = form.TagGroupId
            };
            return entity;
        }

        public override TagForm CreateForm(Tag entity)
        {
            var form = new TagForm
            {
                Id = entity.Id,
                Name = entity.Name,
                TagGroupId = entity.TagGroupId
            };
            return form;
        }

        public override async Task Update(Tag entity, TagForm form)
        {
            entity.Name = form.Name;
            entity.TagGroupId = form.TagGroupId;
            await _context.SaveChangesAsync();
        }
    }
}
