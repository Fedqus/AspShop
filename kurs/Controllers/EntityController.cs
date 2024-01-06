using kurs.Attributes;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;

namespace kurs.Controllers
{
    public class EntityController<TEntity, TForm> : Controller where TEntity : class where TForm : class
    {
        protected readonly EntityManager<TEntity, TForm> _manager;
        protected List<Func<List<TEntity>, string, List<TEntity>>>? _filters;
        public EntityController(EntityManager<TEntity, TForm> manager)
        {
            _manager = manager;
        }
        [AjaxOnly]
        public virtual async Task<IActionResult> Table(int? field, string? search)
        {
            var entities = await _manager.GetEntitiesAsync();
            if (_filters != null && !string.IsNullOrEmpty(search))
            {
                try
                {
                    entities = _filters[field ?? 0].Invoke(entities, search);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return View(entities);
        }
        //[AjaxOnly]
        public virtual IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public virtual async Task<IActionResult> Add(TForm form)
        {
            if (ModelState.IsValid)
            {
                await _manager.AddAsync(form);
            }
            return View();
        }
        [AjaxOnly]
        public virtual async Task<IActionResult> Edit(int? id)
        {
            var entity = await _manager.GetEntityAsync(id);
            if (entity != null)
            {
                var form = _manager.CreateForm(entity);
                return View(form);
            }
            return BadRequest();
        }
        [HttpPost]
        public virtual async Task<IActionResult> Edit(TForm form, int? id)
        {
            if (ModelState.IsValid)
            {
                var entity = await _manager.GetEntityAsync(id);
                if (entity != null)
                {
                    await _manager.Update(entity, form);
                }
            }
            return View(form);
        }

        public virtual async Task<IActionResult> Delete(int? id)
        {
            var entity = await _manager.GetEntityAsync(id);
            if (entity != null)
            {
                await _manager.RemoveAsync(entity);
                return Ok();
            }
            return BadRequest();
        }
    }
}
