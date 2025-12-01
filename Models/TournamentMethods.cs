using System.Data;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.Data.SqlClient;
namespace Laboration_3.Models
{
    public class TournamentMethods
    {
        // konstruktor
        public TournamentMethods() { }

        // metod Create för att skapa en turnering
        public int InsertTournament(Tournament tournamentDetails, out string errormsg)
        {
            errormsg = "";
            SqlConnection sqlConnection = new SqlConnection();

            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
               "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            // parameter passning
            string sqlstring = "Insert Into Tournaments(Sort, T_date, Start_time) Values(@Sort, @T_date, @Start_time)";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Sort", tournamentDetails.Sort);
            sqlCommand.Parameters.AddWithValue("@T_date", tournamentDetails.T_date);
            sqlCommand.Parameters.AddWithValue("@Start_time", tournamentDetails.Start_time);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
        }


        // metod för att hämta turneringar
        public List<Tournament> GetTournamentList(out string errormsg)
        {
            errormsg = "";
            List<Tournament> tournaments = new List<Tournament>();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "SELECT * FROM Tournaments";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Tournament t = new Tournament();
                    t.TournamentId = (int)reader["TournamentId"];
                    t.Sort = reader["Sort"].ToString();
                    t.T_date = (DateTime)reader["T_date"];
                    t.Start_time = (TimeSpan)reader["Start_time"];
                    tournaments.Add(t);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                sqlConnection.Close();
            }

            return tournaments;
        }


        // metod för att hämta en turnering
        public Tournament GetTournament(int id, out string errormsg)
        {
            errormsg = "";
            Tournament tournament = new Tournament();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "SELECT * FROM Tournaments WHERE TournamentId = @Id"; ;
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    tournament = new Tournament
                    {
                        TournamentId = (int)reader["TournamentId"],
                        Sort = reader["Sort"].ToString(),
                        T_date = (DateTime)reader["T_date"],
                        Start_time = (TimeSpan)reader["Start_time"]
                    };
                }
                reader.Close();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
            return tournament;
        }

        // Update-metod för att uppdatera en turnering
        public int UpdateTournament(Tournament tournamentDetails, out string errormsg)
        {
            errormsg = "";
            Tournament tournament = new Tournament();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "UPDATE Tournaments SET Sort=@Sort, T_date=@T_date, Start_time=@Start_time WHERE TournamentId=@Id";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Sort", tournamentDetails.Sort);
            sqlCommand.Parameters.AddWithValue("@T_date", tournamentDetails.T_date);
            sqlCommand.Parameters.AddWithValue("@Start_time", tournamentDetails.Start_time);
            sqlCommand.Parameters.AddWithValue("@Id", tournamentDetails.TournamentId);

            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errormsg=e.Message;
                return 0;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // DELETE-metod för att ta bort en tournering
        public int DeleteTournament(int tournamentId, out string errormsg)
        {
            errormsg = "";
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "DELETE FROM Tournaments WHERE TournamentId=@TournamentId";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@TournamentId", tournamentId);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally {
                sqlConnection.Close(); 
            }
        }
    }
}
