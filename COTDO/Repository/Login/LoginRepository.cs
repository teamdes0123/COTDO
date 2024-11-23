using COTDO.Database;
using COTDO.Interfaces.Repository;
using COTDO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace COTDO.Repository.Login
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DbConcurso _dbConcurso;

        public LoginRepository()
        {
            _dbConcurso = new DbConcurso();
        }

        public async Task<bool> IsValidUser(string cedula)
        {
            using (var con = new SqlConnection(_dbConcurso.ConnectionString))
            {
                try
                {
                    using (var sc = new SqlCommand("[dbo].[prc_Valida_PreCandidato]", con))
                    {
                        sc.CommandType = CommandType.StoredProcedure;
                        sc.Parameters.Add("Cedula", SqlDbType.Char, 13).Value = cedula;

                        await con.OpenAsync();
                        using (var reader = await sc.ExecuteReaderAsync())
                        {
                            return reader.HasRows;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<User> GetUserByCedula(string cedula)
        {
            var user = new User();

            using (var con = new SqlConnection(_dbConcurso.ConnectionString))
            {
                try
                {
                    using (var sc = new SqlCommand("[dbo].[prc_Obtiene_DatosUsuario]", con))
                    {
                        sc.CommandType = CommandType.StoredProcedure;
                        sc.Parameters.Add("Cedula", SqlDbType.Char, 13).Value = cedula;

                        await con.OpenAsync();
                        using (var reader = await sc.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    Cedula = reader.IsDBNull(reader.GetOrdinal("Cedula")) ? "" : reader.GetString(reader.GetOrdinal("Cedula")),
                                    Nombres = reader.IsDBNull(reader.GetOrdinal("Nombres")) ? "" : reader.GetString(reader.GetOrdinal("Nombres")),
                                    Apellido1 = reader.IsDBNull(reader.GetOrdinal("Apellido1")) ? "" : reader.GetString(reader.GetOrdinal("Apellido1")),
                                    Apellido2 = reader.IsDBNull(reader.GetOrdinal("Apellido2")) ? "" : reader.GetString(reader.GetOrdinal("Apellido2")),
                                    CodCargo = reader.IsDBNull(reader.GetOrdinal("CodCargo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CodCargo")),
                                    TiempoEnServicio = reader.IsDBNull(reader.GetOrdinal("TiempoServicio")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TiempoServicio")),
                                    FechaIngreso = reader.IsDBNull(reader.GetOrdinal("FechaIngreso")) ? "" : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")).ToString("dd-MM-yyyy"),
                                    IdPeriodo = reader.IsDBNull(reader.GetOrdinal("IdPeriodo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdPeriodo"))
                                };
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return user;
        }
    }
}