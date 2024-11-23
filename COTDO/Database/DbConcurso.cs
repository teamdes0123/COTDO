using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace COTDO.Database
{
    public class DbConcurso
    {
        public string ConnectionString { get; }

        public DbConcurso()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["dbConcursoFocalizado2023BKL"].ConnectionString;
        }
    }
}