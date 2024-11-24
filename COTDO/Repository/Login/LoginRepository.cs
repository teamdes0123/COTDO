using COTDO.Database;
using COTDO.Helpers;
using COTDO.Interfaces.Repository;
using COTDO.Models;
using COTDO.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace COTDO.Repository.Login
{
    public class LoginRepository
    {
        private readonly DbConcurso _dbConcurso;
        private readonly PasswordHelper _passwordHelper;

        public LoginRepository()
        {
            _dbConcurso = new DbConcurso();
            _passwordHelper = new PasswordHelper();
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
            User user = null;

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

        public async Task<bool> CreateUser(RegisterVM user, int? codCargo)
        {
            using (var con = new SqlConnection(_dbConcurso.ConnectionString))
            {
                try
                {
                    using (var sc = new SqlCommand("[dbo].[prc_Inserta_CandidatoTecnico]", con))
                    {
                        sc.CommandType = CommandType.StoredProcedure;
                        sc.Parameters.Add("Cedula", SqlDbType.Char, 13).Value = user.Cedula;
                        sc.Parameters.Add("CodCargo", SqlDbType.Int).Value = codCargo;
                        sc.Parameters.Add("Correo", SqlDbType.VarChar, 100).Value = user.Correo;
                        sc.Parameters.Add("Clave", SqlDbType.VarChar, 200).Value = _passwordHelper.HashPassword(user.Clave);

                        await con.OpenAsync();
                        int rowsAffected = await sc.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else 
                        { 
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<bool> IsExistsAccount(string correo)
        {
            using (var con = new SqlConnection(_dbConcurso.ConnectionString))
            {
                try
                {
                    using (var sc = new SqlCommand("dbo.prc_Valida_CandidatoTecnicoPorCorreo", con))
                    {
                        sc.CommandType = CommandType.StoredProcedure;
                        sc.Parameters.Add("Correo", SqlDbType.VarChar, 100).Value = correo;

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

        public async Task<User> GetCandidate(LoginVM vm)
        {
            User user = null;

            using (var con = new SqlConnection(_dbConcurso.ConnectionString))
            {
                try
                {
                    using (var sc = new SqlCommand("dbo.prc_Obtiene_CandidatoTecnico", con))
                    {
                        sc.CommandType = CommandType.StoredProcedure;
                        sc.Parameters.Add("Correo", SqlDbType.VarChar, 100).Value = vm.Username;

                        await con.OpenAsync();
                        using (var reader = await sc.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string storedHash = reader.GetString(reader.GetOrdinal("Hash"));

                                if (_passwordHelper.VerifyPassword(vm.Password, storedHash))
                                {
                                    user = new User
                                    {
                                        Cedula = reader.IsDBNull(reader.GetOrdinal("Cedula")) ? "" : reader.GetString(reader.GetOrdinal("Cedula")),
                                        Nombres = reader.IsDBNull(reader.GetOrdinal("Nombres")) ? "" : reader.GetString(reader.GetOrdinal("Nombres")),
                                        Apellido1 = reader.IsDBNull(reader.GetOrdinal("Apellido1")) ? "" : reader.GetString(reader.GetOrdinal("Apellido1")),
                                        Apellido2 = reader.IsDBNull(reader.GetOrdinal("Apellido2")) ? "" : reader.GetString(reader.GetOrdinal("Apellido2")),
                                        CodCargo = reader.IsDBNull(reader.GetOrdinal("CodCargo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CodCargo")),
                                        TiempoEnServicio = reader.IsDBNull(reader.GetOrdinal("TiempoServicio")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TiempoServicio"))
                                    };

                                }                              
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