using Inventario.BusinessLogic.Services;
using Inventario.Entities;
using System;
using System.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Inventario.Web.Controllers
{
    public class MovInventarioController : Controller
    {
        private readonly MovInventarioService _service;

        public MovInventarioController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            _service = new MovInventarioService(connectionString);
        }

        public ActionResult Index(DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento, string nroDocumento)
        {
            try
            {
                var resultado = _service.Consultar(fechaInicio, fechaFin, tipoMovimiento, nroDocumento);

                if (Request.IsAjaxRequest())
                {
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-dd",
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    string json = JsonConvert.SerializeObject(resultado, settings);
                    return Content(json, "application/json");
                }

                ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
                ViewBag.TipoMovimiento = tipoMovimiento;
                ViewBag.NroDocumento = nroDocumento;

                return View(resultado);
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { error = $"Error al consultar movimientos: {ex.Message}" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.Error = $"Error al consultar movimientos: {ex.Message}";
                return View(new List<MovInventario>());
            }
        }

        [HttpPost]
        public ActionResult Create(MovInventario movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    movimiento.Estado = "A";
                    var resultado = _service.Insertar(movimiento);
                    if (resultado != null)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            DateFormatString = "yyyy-MM-dd",
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        string json = JsonConvert.SerializeObject(resultado, settings);
                        return Content(json, "application/json");
                    }
                    return Json(new { error = "No se pudo insertar el movimiento." });
                }
                return Json(new { error = "Los datos proporcionados no son válidos." });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error al crear el movimiento: {ex.Message}" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var movimiento = _service.ObtenerPorId(id);
                if (movimiento == null)
                {
                    return Json(new { error = "Movimiento no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd",
                    NullValueHandling = NullValueHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(movimiento, settings);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error al obtener el movimiento: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(MovInventario movimiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _service.Actualizar(movimiento);
                    if (resultado != null)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            DateFormatString = "yyyy-MM-dd",
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        string json = JsonConvert.SerializeObject(resultado, settings);
                        return Content(json, "application/json");
                    }
                    return Json(new { error = "No se pudo actualizar el movimiento." });
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { error = "Los datos proporcionados no son válidos.", details = errors });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error al actualizar el movimiento: {ex.Message}" });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var resultado = _service.Eliminar(id);
                if (resultado != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-dd",
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    string json = JsonConvert.SerializeObject(resultado, settings);
                    return Content(json, "application/json");
                }
                return Json(new { error = "No se pudo marcar el movimiento como inactivo." });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error al marcar el movimiento como inactivo: {ex.Message}" });
            }
        }
    }
}