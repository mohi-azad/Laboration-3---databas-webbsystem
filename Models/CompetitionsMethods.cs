using Microsoft.Data.SqlClient;

namespace Laboration_3.Models
{
    public class CompetitionsMethods { 

        public CompetitionsMethods() { }

        // Add-metod för competition
        public int AddCompetition(int memberId, int tournamentId, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            errormsg = "";
            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlstring = "INSERT INTO Competitions(MemberId, TournamentId) VALUES(@MemberId, @TournamentId)";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MemberId", memberId);
            sqlCommand.Parameters.AddWithValue("@TournamentId", tournamentId);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
                return 0;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Remove-metod för competition
        public int RemoveCompetition(int memberId, int tournamentId, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            errormsg = "";
            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlstring = "DELETE FROM Competitions WHERE MemberId=@MemberId AND TournamentId=@TournamentId";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MemberId", @memberId);
            sqlCommand.Parameters.AddWithValue("@TournamentId", tournamentId);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
                return 0;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public List<CompetitionView> GetMemberToTournament(out string errormsg)
        {
            errormsg = "";
            var list= new List<CompetitionView>();
            SqlConnection sqlConnection = new SqlConnection();
            errormsg = "";
            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = @"
            SELECT 
                MC.MemberId,
                MC.TournamentId,
                M.FirstName + ' ' + M.LastName AS MemberName,
                T.Sort AS Sort,
                T.T_date,
                T.Start_time
                FROM Competitions MC
                JOIN Members M ON MC.MemberId = M.MemberId
                JOIN Tournaments T ON MC.TournamentId = T.TournamentId";
            SqlCommand sqlCommand = new SqlCommand( sqlstring, sqlConnection);
            try
            {
                sqlConnection.Open();
                SqlDataReader reader=sqlCommand.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(new CompetitionView
                    {
                        MemberId = (int)reader["MemberId"],
                        TournamentId = (int)reader["TournamentId"],
                        MemberName = reader["MemberName"].ToString(),
                        Sort = reader["Sort"].ToString(),
                        T_date = (DateTime)reader["T_date"],
                        Start_time = (TimeSpan)reader["Start_time"]
                    });
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
                return list;
            }
            finally
            {
                sqlConnection.Close();
            }
            return list;
        }


        /*
        // metod för att läsa alla competitions 
        public List<Competition> GetAllCompetitions(out string errormsg)
        {
            errormsg = "";
            var list = new List<Competition>();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            {
                string sqlstring = "SELECT MemberId, TournamentId FROM Competitions";

                SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);

                try
                {
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new Competition
                        {
                            MemberId = (int)reader["MemberId"],
                            TournamentId =(int)reader["TournamentId"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    errormsg = ex.Message;
                }
            }
            return list;
        }
        */

    }
}
