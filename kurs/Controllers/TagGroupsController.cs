using kurs.Attributes;
using Microsoft.AspNetCore.Mvc;
using kurs.Services;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Controllers
{
    public class TagGroupsController : EntityController<TagGroup, TagGroupForm>
    {
        public TagGroupsController(TagGroupManager manager) : base(manager)
        {
            _filters = new List<Func<List<TagGroup>, string, List<TagGroup>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList()
            };
        }
    }
}
