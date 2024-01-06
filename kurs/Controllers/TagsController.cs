using kurs.Attributes;
using Microsoft.AspNetCore.Mvc;
using kurs.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Controllers
{
    public class TagsController : EntityController<Tag, TagForm>
    {
        private readonly TagGroupManager _tagGroupManager;

        public TagsController(TagManager manager, TagGroupManager tagGroupManager) : base(manager)
        {
            _filters = new List<Func<List<Tag>, string, List<Tag>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.TagGroup.Name.ToLower().Contains(s.ToLower())).ToList(),
            };
            _tagGroupManager = tagGroupManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.TagGroups = _tagGroupManager.GetEntities();
        }
    }
}
