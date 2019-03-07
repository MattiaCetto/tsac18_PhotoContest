using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using tsac18_CettolinPhotoContest.data.Models;

namespace tsac18_CettolinPhotoContest.data
{
    public class DataAccess : IDataAccess
    {
        readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        void IDataAccess.Vote(int id_p, string id_u, int v)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO tsac18cettolin.vote( id_photo,  id_user, vote) VALUES (@id_p1, @id_u1, @v1);";
                connection.Execute(query, new { v1 = v, id_p1 = id_p, id_u1 = id_u });
            }
        }

        void IDataAccess.LoadImageS3(string url, string UserId) //ins database
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "INSERT INTO tsac18cettolin.photo( url, id_user, nvotes, sumvotes) VALUES( @u, @user, @n1, @n2); ";
                connection.Execute(query, new { u = url, user = UserId, n1=0, n2=0 });
            }
        }

        IEnumerable<Photo> IDataAccess.GetPhoto(string UserId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT p.*, v.vote as vote, v.id_user as id_user FROM tsac18cettolin.photo as p FULL JOIN tsac18cettolin.vote as v ON (v.id_photo = p.id) WHERE v.id_user IS NULL OR v.id_user = user; ";
                IEnumerable<Photo> help = connection.Query<Photo>(query, new { user = UserId});
                foreach(var item in help)
                {
                    if ((item.Rate>0)||(item.Id_User2==null))
                        item.Voted = true;
                    else
                        item.Voted = false;
                }
                return help;
            }
        }


        
    }
}
