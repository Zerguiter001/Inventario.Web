using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Inventario.Entities;

namespace Inventario.DataAccess.Repositories
{
    public class MovInventarioRepository
    {
        private readonly string connectionString;

        public MovInventarioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<MovInventario> Consultar(DateTime? fechaInicio, DateTime? fechaFin, string tipoMovimiento, string nroDocumento)
        {
            var lista = new List<MovInventario>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_CONSULTAR_MOV_INVENTARIO", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = (object)fechaInicio ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = (object)fechaFin ?? DBNull.Value;
                    cmd.Parameters.Add("@TipoMovimiento", SqlDbType.VarChar, 2).Value = string.IsNullOrWhiteSpace(tipoMovimiento) ? (object)DBNull.Value : tipoMovimiento.Trim();
                    cmd.Parameters.Add("@NroDocumento", SqlDbType.VarChar, 50).Value = string.IsNullOrWhiteSpace(nroDocumento) ? (object)DBNull.Value : nroDocumento.Trim();

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MovInventario
                            {
                                IdMovimiento = Convert.ToInt32(reader["ID_MOVIMIENTO"]),
                                CodCia = reader["COD_CIA"] != DBNull.Value ? reader["COD_CIA"].ToString() : null,
                                CompaniaVenta3 = reader["COMPANIA_VENTA_3"] != DBNull.Value ? reader["COMPANIA_VENTA_3"].ToString() : null,
                                AlmacenVenta = reader["ALMACEN_VENTA"] != DBNull.Value ? reader["ALMACEN_VENTA"].ToString() : null,
                                TipoMovimiento = reader["TIPO_MOVIMIENTO"] != DBNull.Value ? reader["TIPO_MOVIMIENTO"].ToString() : null,
                                TipoDocumento = reader["TIPO_DOCUMENTO"] != DBNull.Value ? reader["TIPO_DOCUMENTO"].ToString() : null,
                                NroDocumento = reader["NRO_DOCUMENTO"] != DBNull.Value ? reader["NRO_DOCUMENTO"].ToString() : null,
                                CodItem2 = reader["COD_ITEM_2"] != DBNull.Value ? reader["COD_ITEM_2"].ToString() : null,
                                Proveedor = reader["PROVEEDOR"] != DBNull.Value ? reader["PROVEEDOR"].ToString() : null,
                                AlmacenDestino = reader["ALMACEN_DESTINO"] != DBNull.Value ? reader["ALMACEN_DESTINO"].ToString() : null,
                                Cantidad = reader["CANTIDAD"] != DBNull.Value ? (int?)Convert.ToInt32(reader["CANTIDAD"]) : null,
                                DocRef1 = reader["DOC_REF_1"] != DBNull.Value ? reader["DOC_REF_1"].ToString() : null,
                                DocRef2 = reader["DOC_REF_2"] != DBNull.Value ? reader["DOC_REF_2"].ToString() : null,
                                DocRef3 = reader["DOC_REF_3"] != DBNull.Value ? reader["DOC_REF_3"].ToString() : null,
                                DocRef4 = reader["DOC_REF_4"] != DBNull.Value ? reader["DOC_REF_4"].ToString() : null,
                                DocRef5 = reader["DOC_REF_5"] != DBNull.Value ? reader["DOC_REF_5"].ToString() : null,
                                FechaTransaccion = reader["FECHA_TRANSACCION"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["FECHA_TRANSACCION"].ToString()) : null,
                                Estado = reader["ESTADO"] != DBNull.Value ? reader["ESTADO"].ToString() : null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consultar movimientos de inventario: {ex.Message}");
                return lista;
            }

            return lista;
        }

        public MovInventario ObtenerPorId(int idMovimiento)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_CONSULTAR_MOV_INVENTARIO_POR_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID_MOVIMIENTO", SqlDbType.Int).Value = idMovimiento;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MovInventario
                            {
                                IdMovimiento = reader.GetInt32(reader.GetOrdinal("ID_MOVIMIENTO")),
                                CodCia = reader["COD_CIA"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("COD_CIA")) : null,
                                CompaniaVenta3 = reader["COMPANIA_VENTA_3"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("COMPANIA_VENTA_3")) : null,
                                AlmacenVenta = reader["ALMACEN_VENTA"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ALMACEN_VENTA")) : null,
                                TipoMovimiento = reader["TIPO_MOVIMIENTO"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("TIPO_MOVIMIENTO")) : null,
                                TipoDocumento = reader["TIPO_DOCUMENTO"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("TIPO_DOCUMENTO")) : null,
                                NroDocumento = reader["NRO_DOCUMENTO"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO")) : null,
                                CodItem2 = reader["COD_ITEM_2"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("COD_ITEM_2")) : null,
                                Proveedor = reader["PROVEEDOR"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PROVEEDOR")) : null,
                                AlmacenDestino = reader["ALMACEN_DESTINO"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ALMACEN_DESTINO")) : null,
                                Cantidad = reader["CANTIDAD"] != DBNull.Value ? (int?)reader.GetInt32(reader.GetOrdinal("CANTIDAD")) : null,
                                DocRef1 = reader["DOC_REF_1"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("DOC_REF_1")) : null,
                                DocRef2 = reader["DOC_REF_2"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("DOC_REF_2")) : null,
                                DocRef3 = reader["DOC_REF_3"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("DOC_REF_3")) : null,
                                DocRef4 = reader["DOC_REF_4"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("DOC_REF_4")) : null,
                                DocRef5 = reader["DOC_REF_5"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("DOC_REF_5")) : null,
                                FechaTransaccion = reader["FECHA_TRANSACCION"] != DBNull.Value ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("FECHA_TRANSACCION")) : null,
                                Estado = reader["ESTADO"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ESTADO")) : null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el movimiento de inventario: {ex.Message}", ex);
            }
            return null;
        }

        public MovInventario Insertar(MovInventario movimiento)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(movimiento.CodCia) || movimiento.CodCia.Length > 5)
                    throw new ArgumentException("El código de compañía es obligatorio y debe tener máximo 5 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.CompaniaVenta3) || movimiento.CompaniaVenta3.Length > 5)
                    throw new ArgumentException("La compañía de venta es obligatoria y debe tener máximo 5 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.AlmacenVenta) || movimiento.AlmacenVenta.Length > 10)
                    throw new ArgumentException("El almacén de venta es obligatorio y debe tener máximo 10 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento) || !new[] { "01", "02", "03", "04", "05" }.Contains(movimiento.TipoMovimiento))
                    throw new ArgumentException("El tipo de movimiento es obligatorio y debe ser uno de: 01, 02, 03, 04, 05.");
                if (string.IsNullOrWhiteSpace(movimiento.TipoDocumento) || !new[] { "DN", "RU", "PA" }.Contains(movimiento.TipoDocumento))
                    throw new ArgumentException("El tipo de documento es obligatorio y debe ser uno de: DN, RU, PA.");
                if (string.IsNullOrWhiteSpace(movimiento.NroDocumento) || movimiento.NroDocumento.Length > 50)
                    throw new ArgumentException("El número de documento es obligatorio y debe tener máximo 50 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.CodItem2) || movimiento.CodItem2.Length > 50)
                    throw new ArgumentException("El código de ítem es obligatorio y debe tener máximo 50 caracteres.");
                if (movimiento.Cantidad.HasValue && movimiento.Cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero.");
                if (movimiento.Proveedor != null && movimiento.Proveedor.Length > 100)
                    throw new ArgumentException("El proveedor debe tener máximo 100 caracteres.");
                if (movimiento.AlmacenDestino != null && movimiento.AlmacenDestino.Length > 50)
                    throw new ArgumentException("El almacén destino debe tener máximo 50 caracteres.");
                if (movimiento.DocRef1 != null && movimiento.DocRef1.Length > 50)
                    throw new ArgumentException("El documento de referencia 1 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef2 != null && movimiento.DocRef2.Length > 50)
                    throw new ArgumentException("El documento de referencia 2 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef3 != null && movimiento.DocRef3.Length > 50)
                    throw new ArgumentException("El documento de referencia 3 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef4 != null && movimiento.DocRef4.Length > 50)
                    throw new ArgumentException("El documento de referencia 4 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef5 != null && movimiento.DocRef5.Length > 50)
                    throw new ArgumentException("El documento de referencia 5 debe tener máximo 50 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.Estado) || !new[] { "A", "I", "P" }.Contains(movimiento.Estado))
                    throw new ArgumentException("El estado debe ser uno de: A, I, P.");
                if (!movimiento.FechaTransaccion.HasValue)
                    movimiento.FechaTransaccion = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_INSERTAR_MOV_INVENTARIO", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@COD_CIA", SqlDbType.VarChar, 5).Value = movimiento.CodCia;
                    cmd.Parameters.Add("@COMPANIA_VENTA_3", SqlDbType.VarChar, 5).Value = movimiento.CompaniaVenta3;
                    cmd.Parameters.Add("@ALMACEN_VENTA", SqlDbType.VarChar, 10).Value = movimiento.AlmacenVenta;
                    cmd.Parameters.Add("@TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = movimiento.TipoMovimiento;
                    cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar, 2).Value = movimiento.TipoDocumento;
                    cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 50).Value = movimiento.NroDocumento;
                    cmd.Parameters.Add("@COD_ITEM_2", SqlDbType.VarChar, 50).Value = movimiento.CodItem2;
                    cmd.Parameters.Add("@PROVEEDOR", SqlDbType.VarChar, 100).Value = (object)movimiento.Proveedor ?? DBNull.Value;
                    cmd.Parameters.Add("@ALMACEN_DESTINO", SqlDbType.VarChar, 50).Value = (object)movimiento.AlmacenDestino ?? DBNull.Value;
                    cmd.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = (object)movimiento.Cantidad ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_1", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef1 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_2", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef2 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_3", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef3 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_4", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef4 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_5", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef5 ?? DBNull.Value;
                    cmd.Parameters.Add("@FECHA_TRANSACCION", SqlDbType.Date).Value = (object)movimiento.FechaTransaccion ?? DBNull.Value;
                    cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar, 1).Value = movimiento.Estado;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MovInventario
                            {
                                IdMovimiento = Convert.ToInt32(reader["ID_MOVIMIENTO"]),
                                CodCia = reader["COD_CIA"] != DBNull.Value ? reader["COD_CIA"].ToString() : null,
                                CompaniaVenta3 = reader["COMPANIA_VENTA_3"] != DBNull.Value ? reader["COMPANIA_VENTA_3"].ToString() : null,
                                AlmacenVenta = reader["ALMACEN_VENTA"] != DBNull.Value ? reader["ALMACEN_VENTA"].ToString() : null,
                                TipoMovimiento = reader["TIPO_MOVIMIENTO"] != DBNull.Value ? reader["TIPO_MOVIMIENTO"].ToString() : null,
                                TipoDocumento = reader["TIPO_DOCUMENTO"] != DBNull.Value ? reader["TIPO_DOCUMENTO"].ToString() : null,
                                NroDocumento = reader["NRO_DOCUMENTO"] != DBNull.Value ? reader["NRO_DOCUMENTO"].ToString() : null,
                                CodItem2 = reader["COD_ITEM_2"] != DBNull.Value ? reader["COD_ITEM_2"].ToString() : null,
                                Proveedor = reader["PROVEEDOR"] != DBNull.Value ? reader["PROVEEDOR"].ToString() : null,
                                AlmacenDestino = reader["ALMACEN_DESTINO"] != DBNull.Value ? reader["ALMACEN_DESTINO"].ToString() : null,
                                Cantidad = reader["CANTIDAD"] != DBNull.Value ? (int?)Convert.ToInt32(reader["CANTIDAD"]) : null,
                                DocRef1 = reader["DOC_REF_1"] != DBNull.Value ? reader["DOC_REF_1"].ToString() : null,
                                DocRef2 = reader["DOC_REF_2"] != DBNull.Value ? reader["DOC_REF_2"].ToString() : null,
                                DocRef3 = reader["DOC_REF_3"] != DBNull.Value ? reader["DOC_REF_3"].ToString() : null,
                                DocRef4 = reader["DOC_REF_4"] != DBNull.Value ? reader["DOC_REF_4"].ToString() : null,
                                DocRef5 = reader["DOC_REF_5"] != DBNull.Value ? reader["DOC_REF_5"].ToString() : null,
                                FechaTransaccion = reader["FECHA_TRANSACCION"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["FECHA_TRANSACCION"].ToString()) : null,
                                Estado = reader["ESTADO"] != DBNull.Value ? reader["ESTADO"].ToString() : null
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al insertar movimiento de inventario: {ex.Message}, Código: {ex.Number}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar movimiento de inventario: {ex.Message}", ex);
            }
            return null;
        }

        public MovInventario Actualizar(MovInventario movimiento)
        {
            try
            {
                // Validaciones
                if (movimiento.IdMovimiento <= 0)
                    throw new ArgumentException("El ID del movimiento es obligatorio y debe ser mayor que cero.");
                if (string.IsNullOrWhiteSpace(movimiento.CodCia) || movimiento.CodCia.Length > 5)
                    throw new ArgumentException("El código de compañía es obligatorio y debe tener máximo 5 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.CompaniaVenta3) || movimiento.CompaniaVenta3.Length > 5)
                    throw new ArgumentException("La compañía de venta es obligatoria y debe tener máximo 5 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.AlmacenVenta) || movimiento.AlmacenVenta.Length > 10)
                    throw new ArgumentException("El almacén de venta es obligatorio y debe tener máximo 10 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento) || !new[] { "01", "02", "03", "04", "05" }.Contains(movimiento.TipoMovimiento))
                    throw new ArgumentException("El tipo de movimiento es obligatorio y debe ser uno de: 01, 02, 03, 04, 05.");
                if (string.IsNullOrWhiteSpace(movimiento.TipoDocumento) || !new[] { "DN", "RU", "PA" }.Contains(movimiento.TipoDocumento))
                    throw new ArgumentException("El tipo de documento es obligatorio y debe ser uno de: DN, RU, PA.");
                if (string.IsNullOrWhiteSpace(movimiento.NroDocumento) || movimiento.NroDocumento.Length > 50)
                    throw new ArgumentException("El número de documento es obligatorio y debe tener máximo 50 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.CodItem2) || movimiento.CodItem2.Length > 50)
                    throw new ArgumentException("El código de ítem es obligatorio y debe tener máximo 50 caracteres.");
                if (movimiento.Cantidad.HasValue && movimiento.Cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero.");
                if (movimiento.Proveedor != null && movimiento.Proveedor.Length > 100)
                    throw new ArgumentException("El proveedor debe tener máximo 100 caracteres.");
                if (movimiento.AlmacenDestino != null && movimiento.AlmacenDestino.Length > 50)
                    throw new ArgumentException("El almacén destino debe tener máximo 50 caracteres.");
                if (movimiento.DocRef1 != null && movimiento.DocRef1.Length > 50)
                    throw new ArgumentException("El documento de referencia 1 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef2 != null && movimiento.DocRef2.Length > 50)
                    throw new ArgumentException("El documento de referencia 2 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef3 != null && movimiento.DocRef3.Length > 50)
                    throw new ArgumentException("El documento de referencia 3 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef4 != null && movimiento.DocRef4.Length > 50)
                    throw new ArgumentException("El documento de referencia 4 debe tener máximo 50 caracteres.");
                if (movimiento.DocRef5 != null && movimiento.DocRef5.Length > 50)
                    throw new ArgumentException("El documento de referencia 5 debe tener máximo 50 caracteres.");
                if (string.IsNullOrWhiteSpace(movimiento.Estado) || !new[] { "A", "I", "P" }.Contains(movimiento.Estado))
                    throw new ArgumentException("El estado debe ser uno de: A, I, P.");
                if (!movimiento.FechaTransaccion.HasValue)
                    movimiento.FechaTransaccion = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_MOV_INVENTARIO", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID_MOVIMIENTO", SqlDbType.Int).Value = movimiento.IdMovimiento;
                    cmd.Parameters.Add("@COD_CIA", SqlDbType.VarChar, 5).Value = movimiento.CodCia;
                    cmd.Parameters.Add("@COMPANIA_VENTA_3", SqlDbType.VarChar, 5).Value = movimiento.CompaniaVenta3;
                    cmd.Parameters.Add("@ALMACEN_VENTA", SqlDbType.VarChar, 10).Value = movimiento.AlmacenVenta;
                    cmd.Parameters.Add("@TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = movimiento.TipoMovimiento;
                    cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar, 2).Value = movimiento.TipoDocumento;
                    cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 50).Value = movimiento.NroDocumento;
                    cmd.Parameters.Add("@COD_ITEM_2", SqlDbType.VarChar, 50).Value = movimiento.CodItem2;
                    cmd.Parameters.Add("@PROVEEDOR", SqlDbType.VarChar, 100).Value = (object)movimiento.Proveedor ?? DBNull.Value;
                    cmd.Parameters.Add("@ALMACEN_DESTINO", SqlDbType.VarChar, 50).Value = (object)movimiento.AlmacenDestino ?? DBNull.Value;
                    cmd.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = (object)movimiento.Cantidad ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_1", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef1 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_2", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef2 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_3", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef3 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_4", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef4 ?? DBNull.Value;
                    cmd.Parameters.Add("@DOC_REF_5", SqlDbType.VarChar, 50).Value = (object)movimiento.DocRef5 ?? DBNull.Value;
                    cmd.Parameters.Add("@FECHA_TRANSACCION", SqlDbType.Date).Value = (object)movimiento.FechaTransaccion ?? DBNull.Value;
                    cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar, 1).Value = movimiento.Estado;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MovInventario
                            {
                                IdMovimiento = Convert.ToInt32(reader["ID_MOVIMIENTO"]),
                                CodCia = reader["COD_CIA"] != DBNull.Value ? reader["COD_CIA"].ToString() : null,
                                CompaniaVenta3 = reader["COMPANIA_VENTA_3"] != DBNull.Value ? reader["COMPANIA_VENTA_3"].ToString() : null,
                                AlmacenVenta = reader["ALMACEN_VENTA"] != DBNull.Value ? reader["ALMACEN_VENTA"].ToString() : null,
                                TipoMovimiento = reader["TIPO_MOVIMIENTO"] != DBNull.Value ? reader["TIPO_MOVIMIENTO"].ToString() : null,
                                TipoDocumento = reader["TIPO_DOCUMENTO"] != DBNull.Value ? reader["TIPO_DOCUMENTO"].ToString() : null,
                                NroDocumento = reader["NRO_DOCUMENTO"] != DBNull.Value ? reader["NRO_DOCUMENTO"].ToString() : null,
                                CodItem2 = reader["COD_ITEM_2"] != DBNull.Value ? reader["COD_ITEM_2"].ToString() : null,
                                Proveedor = reader["PROVEEDOR"] != DBNull.Value ? reader["PROVEEDOR"].ToString() : null,
                                AlmacenDestino = reader["ALMACEN_DESTINO"] != DBNull.Value ? reader["ALMACEN_DESTINO"].ToString() : null,
                                Cantidad = reader["CANTIDAD"] != DBNull.Value ? (int?)Convert.ToInt32(reader["CANTIDAD"]) : null,
                                DocRef1 = reader["DOC_REF_1"] != DBNull.Value ? reader["DOC_REF_1"].ToString() : null,
                                DocRef2 = reader["DOC_REF_2"] != DBNull.Value ? reader["DOC_REF_2"].ToString() : null,
                                DocRef3 = reader["DOC_REF_3"] != DBNull.Value ? reader["DOC_REF_3"].ToString() : null,
                                DocRef4 = reader["DOC_REF_4"] != DBNull.Value ? reader["DOC_REF_4"].ToString() : null,
                                DocRef5 = reader["DOC_REF_5"] != DBNull.Value ? reader["DOC_REF_5"].ToString() : null,
                                FechaTransaccion = reader["FECHA_TRANSACCION"] != DBNull.Value ? (DateTime?)DateTime.Parse(reader["FECHA_TRANSACCION"].ToString()) : null,
                                Estado = reader["ESTADO"] != DBNull.Value ? reader["ESTADO"].ToString() : null
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al actualizar movimiento de inventario: {ex.Message}, Código: {ex.Number}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar movimiento de inventario: {ex.Message}", ex);
            }
            return null;
        }

        public void Eliminar(int idMovimiento)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MOV_INVENTARIO", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID_MOVIMIENTO", SqlDbType.Int).Value = idMovimiento;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al eliminar movimiento de inventario: {ex.Message}, Código: {ex.Number}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar movimiento de inventario: {ex.Message}", ex);
            }
        }
    }
}