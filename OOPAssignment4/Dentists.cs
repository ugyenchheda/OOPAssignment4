using System.Data;
using System.Data.SqlClient;

class Dentist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TelNum { get; set; }
}

class Dentists
{
    public List<Dentist> GetAllDentists(SqlConnection connection)
    {
        List<Dentist> dentists = new List<Dentist>();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Dentist", connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                dentists.Add(new Dentist
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    TelNum = reader.GetString(2)
                });
            }
        }

        return dentists;
    }

    public void InsertDentist(SqlConnection connection, Dentist dentist)
    {
        using (SqlCommand command = new SqlCommand("INSERT INTO Dentist (Name, TelNum) VALUES (@name, @telNum)", connection))
        {
            command.Parameters.AddWithValue("@name", dentist.Name);
            command.Parameters.AddWithValue("@telNum", dentist.TelNum);
            command.ExecuteNonQuery();
        }
    }

    public List<Dentist> FindDentist(SqlConnection connection, string name)
    {
        List<Dentist> dentists = new List<Dentist>();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Dentist WHERE Name LIKE '%' + @name + '%'", connection))
        {
            command.Parameters.AddWithValue("@name", name);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dentists.Add(new Dentist
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        TelNum = reader.GetString(2)
                    });
                }
            }
        }

        return dentists;
    }

    public void UpdateDentist(SqlConnection connection, Dentist dentist)
    {
        using (SqlCommand command = new SqlCommand("UPDATE Dentist SET Name = @name, TelNum = @telNum WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@name", dentist.Name);
            command.Parameters.AddWithValue("@telNum", dentist.TelNum);
            command.Parameters.AddWithValue("@id", dentist.Id);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteDentist(SqlConnection connection, string removeDentist)
    {
        using (SqlCommand command = new SqlCommand("DELETE FROM Dentist WHERE Name=@dName", connection))
        {
            command.Parameters.AddWithValue("@dName", removeDentist);
            command.ExecuteNonQuery();
        }
    }


}