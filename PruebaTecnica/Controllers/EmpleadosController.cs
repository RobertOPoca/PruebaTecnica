using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaTecnica.Controllers
{
    public class EmpleadosController : BaseController
    {
        private readonly DB_PruebaContext _context;

        public EmpleadosController(DB_PruebaContext context)
        {
            _context = context;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            var dB_PruebaContext = _context.Empleados.Include(e => e.IdAreaNavigation).Include(e => e.IdJefeNavigation);

            ViewData["IdArea"] = new SelectList(_context.Areas, "Nombre", "Nombre");
            return View(await dB_PruebaContext.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.IdAreaNavigation)
                .Include(e => e.IdJefeNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }
            var direccion = "";
            if (empleado.Foto != null)
            {
                direccion = "data:image/png;base64," + Convert.ToBase64String(empleado.Foto, 0, empleado.Foto.Length);
            }
            ViewData["Edad"] = DateTime.Today.Year - empleado.FechaNacimiento.Value.Year;
            ViewData["Ingreso"] = DateTime.Today.Year - empleado.FechaIngreso.Value.Year;
            ViewData["Imagen"] = direccion;
            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre");
            ViewData["IdJefe"] = new SelectList(_context.Empleados.Where(e=>e.IdJefe==null), "IdEmpleado", "NombreCompleto");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Empleado empleado, List<IFormFile> Foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Foto != null && Foto.Count > 0)
                    {

                        foreach (var item in Foto)
                        {
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await item.CopyToAsync(stream);
                                    empleado.Foto = stream.ToArray();
                                }
                            }
                        }
                    }
                    _context.Add(empleado);
                    await _context.SaveChangesAsync();

                    Alert("Guardado correctamente", "Éxito", Utils.TipoNotificacion.success);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {

            }
            
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre", empleado.IdArea);
            ViewData["IdJefe"] = new SelectList(_context.Empleados.Where(e => e.IdJefe == null), "IdEmpleado", "NombreCompleto");
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            var direccion = "";
            if (empleado.Foto != null)
            {
                direccion = "data:image/png;base64," + Convert.ToBase64String(empleado.Foto, 0, empleado.Foto.Length);
            }
            
            ViewData["Imagen"] = direccion;
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre", empleado.IdArea);
            ViewData["IdJefe"] = new SelectList(_context.Empleados.Where(e => e.IdJefe == null), "IdEmpleado", "NombreCompleto");
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado, List<IFormFile> Foto)
        {
            
            if (id != empleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Foto != null && Foto.Count>0)
                    {

                        foreach (var item in Foto)
                        {
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await item.CopyToAsync(stream);
                                    empleado.Foto = stream.ToArray();
                                }
                            }
                        }
                    }
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.IdEmpleado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre", empleado.IdArea);
            ViewData["IdJefe"] = new SelectList(_context.Empleados, "IdEmpleado", "NombreCompleto", empleado.IdJefe);
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.IdAreaNavigation)
                .Include(e => e.IdJefeNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                var empleado = await _context.Empleados.FindAsync(id);
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();

                Alert("Eliminado correctamente", "Éxito", Utils.TipoNotificacion.success);
            }
            catch
            {

                Alert("No se pudo eliminar", "Error", Utils.TipoNotificacion.error);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.IdEmpleado == id);
        }
    }
}
