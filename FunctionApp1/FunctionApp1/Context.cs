using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1 {
    internal class Context {
        private readonly string connectionString;
        private const string Query = "Select * from people";
        private const string QueryInsert = "Insert into people(FirstName, LastName) values (@Name, @Surname)";
        private const string QueryGet = "Select * from people where PersonId = @ID";
        private const string QueryDelete = "delete from people where PersonId = @ID";

        public Context(string connectionString) {
            this.connectionString = connectionString;
        }

        public IEnumerable<Person> GetPeople() {
            var people = new List<Person>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(Query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    people.Add(new Person {
                        PersonId = Convert.ToInt32(reader["PersonId"]),
                        FirstName = reader["FirstName"].ToString()
                    });
                }
                reader.Close();
            }

            return people;
        }

        public void InsertPerson(PersonInsert person) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(QueryInsert, connection);
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = person.FirstName;
                command.Parameters["@Surname"].Value = person.LastName;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeletePerson(int id) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(QueryDelete, connection);
                command.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                command.Parameters["@ID"].Value = id;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Person GetPerson(int id) {
            Person res = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(Query, connection);
                command.Parameters.Add("@ID", System.Data.SqlDbType.Int);
                command.Parameters["@ID"].Value = id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    res = new Person {
                        PersonId = Convert.ToInt32(reader["PersonId"]),
                        FirstName = reader["FirstName"].ToString()
                    };
                }
                reader.Close();
            }
            return res;
        }
    }
}
