using System;
using System.Collections.Generic;
using Inventario.DataAccess.Repositories;
using Inventario.Entities;

namespace Inventario.BusinessLogic.Services
{
    public class MovInventarioService
    {
        private readonly MovInventarioRepository _repository;

        public MovInventarioService(string connectionString)
        {
            _repository = new MovInventarioRepository(connectionString);
        }

        public List<MovInventario> Consultar(DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento, string nroDocumento)
        {
            return _repository.Consultar(fechaInicio, fechaFin, tipoMovimiento, nroDocumento);
        }

        public MovInventario ObtenerPorId(int idMovimiento)
        {
            return _repository.ObtenerPorId(idMovimiento);
        }

        public MovInventario Insertar(MovInventario movimiento)
        {
            try
            {
                return _repository.Insertar(movimiento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar movimiento de inventario: {ex.Message}", ex);
            }
        }

        public MovInventario Actualizar(MovInventario movimiento)
        {
            try
            {
                return _repository.Actualizar(movimiento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar movimiento de inventario: {ex.Message}", ex);
            }
        }

        public MovInventario Eliminar(int idMovimiento)
        {
            try
            {
                return _repository.Eliminar(idMovimiento);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar movimiento de inventario: {ex.Message}", ex);
            }
        }
    }
}