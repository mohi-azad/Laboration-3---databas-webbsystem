using System.Data;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.Data.SqlClient;
namespace Laboration_3.Models
{
    public class MemberMethods
    {
        // konstruktor
        public MemberMethods() { }

        // publika metoder

        // Insert-metod för att mata in data i databasen
        public int InsertMember(MemberDetails memberDetails, out string errormsg)
        {
           
            // skapa ett connection-objekt för att ansluta mot SQL-server
            SqlConnection sqlConnection = new SqlConnection();

            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            // parameter passning
            string sqlstring = "Insert Into Members(FirstName, LastName, Email, Phone, Age, Score) Values(@FirstName, @LastName, @Email, @Phone, @Age, @Score)";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 30).Value = memberDetails.FirstName;
            sqlCommand.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 40).Value = memberDetails.LastName;
            sqlCommand.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 60).Value = memberDetails.Email;
            sqlCommand.Parameters.Add("@Phone", System.Data.SqlDbType.NVarChar, 20).Value = memberDetails.Phone;
            sqlCommand.Parameters.Add("@Age", System.Data.SqlDbType.Int, 3).Value = memberDetails.Age;
            sqlCommand.Parameters.Add("@Score", System.Data.SqlDbType.Int, 15).Value = memberDetails.Score;


            try
            {
                sqlConnection.Open();
                int i = 0;
                i = sqlCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Insert command faild";
                }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Select-metod för att hämta och visa data på databasen
        public List<MemberDetails> GetMemberDetailsList(out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();

            string sqlstring = "Select * From Members";

            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            // skapa ett connection-objekt för att ansluta mot SQL-server
            //string sqlstring = "SELECT * FROM Members";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            List<MemberDetails> memberDetailsList = new List<MemberDetails>();

            try
            {
                sqlConnection.Open(); 
                    // lägger till en tabell i dataobjektet och fyller det med data
                sqlDataAdapter.Fill(dataSet, "Members");
                int i = 0;
                int count = 0;
                count = dataSet.Tables["Members"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        // lägger till data från dataset och fyller listobjekt
                        MemberDetails memberDetails = new MemberDetails();
                        memberDetails.MemberId = Convert.ToUInt16(dataSet.Tables["Members"].Rows[i]["MemberId"]);
                        memberDetails.FirstName = dataSet.Tables["Members"].Rows[i]["FirstName"].ToString();
                        memberDetails.LastName = dataSet.Tables["Members"].Rows[i]["LastName"].ToString();
                        memberDetails.Email = dataSet.Tables["Members"].Rows[i]["Email"].ToString();
                        memberDetails.Phone = dataSet.Tables["Members"].Rows[i]["Phone"].ToString();
                        memberDetails.Age = Convert.ToUInt16(dataSet.Tables["Members"].Rows[i]["Age"]);
                        memberDetails.Score = Convert.ToUInt16(dataSet.Tables["Members"].Rows[i]["Score"]);

                        i++;
                        memberDetailsList.Add(memberDetails);
                    }
                    errormsg = "";
                    return memberDetailsList;
                }
                else
                {
                    errormsg = "No member details is fetched!";
                    return memberDetailsList;
                }
                
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Select-metod för att hämta och visa en medlem
        public MemberDetails GetMemberDetails(int memberId, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            // Skapa koppling mot lokal instans av databas
            sqlConnection.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlstring = "Select * From Members Where MemberId =@id";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.Add("id", SqlDbType.Int).Value = memberId;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();

            try
            {
                sqlConnection.Open();
                sqlDataAdapter.Fill(dataSet, "Members");
                int count = 0;
                count = dataSet.Tables["Members"].Rows.Count;
                MemberDetails memberDetails =new MemberDetails();
                if (count == 1)
                {
                    memberDetails.MemberId = Convert.ToUInt16(dataSet.Tables["Members"].Rows[0]["MemberId"]);
                    memberDetails.FirstName = dataSet.Tables["Members"].Rows[0]["FirstName"].ToString();
                    memberDetails.LastName = dataSet.Tables["Members"].Rows[0]["LastName"].ToString();
                    memberDetails.Email = dataSet.Tables["Members"].Rows[0]["Email"].ToString();
                    memberDetails.Phone = dataSet.Tables["Members"].Rows[0]["Phone"].ToString();
                    memberDetails.Age = Convert.ToUInt16(dataSet.Tables["Members"].Rows[0]["Age"]);
                    memberDetails.Score = Convert.ToUInt16(dataSet.Tables["Members"].Rows[0]["Score"]);
                    errormsg = "";
                    return memberDetails;
                }
                else
                {
                    errormsg = "No member details is fetched!";
                    return memberDetails;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        
        // Update-mtod för att uppdatera databas
        public int UpdateMemberDetails(MemberDetails memberDetails, int memberId, out string errormsg)
        {
            // skapa ett connection-objekt för att ansluta mot sql server
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "UPDATE Members SET FirstName =@FirstName, LastName =@LastName, Email=@Email, Phone =@Phone, Age =@Age, Score =@Score Where MemberId=@MemberId";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@MemberId", memberId);
            sqlCommand.Parameters.AddWithValue("@FirstName", memberDetails.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", memberDetails.LastName);
            sqlCommand.Parameters.AddWithValue("@Email", memberDetails.Email);
            sqlCommand.Parameters.AddWithValue("@Phone", memberDetails.Phone);
            sqlCommand.Parameters.AddWithValue("@Age", memberDetails.Age);
            sqlCommand.Parameters.AddWithValue("@Score", memberDetails.Score);
            
            try
            {
                sqlConnection.Open();
                int i = sqlCommand.ExecuteNonQuery();
                if (i > 0)
                {
                    errormsg = "";
                    return i;
                }
                else
                {
                    errormsg = "No member details updated!";
                    return 0;
                }
                
            }
            catch (Exception e)
            {
                errormsg = e.Message; 
                return 0;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // en method för att undvika dubbletter vid insert
        public bool MemberExists(string email, string phone, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlstring = "SELECT COUNT (*) FROM Members Where Email =@Email OR Phone =@Phone";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            sqlCommand.Parameters.AddWithValue("@Phone", phone);

            try
            {
                sqlConnection.Open();
                int count = (int)sqlCommand.ExecuteScalar();
                errormsg = "";
                return count > 0;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return true;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // metoden DELETE för att ta bort en member från databasen
        public int DeleteMember(int memberId, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                 "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlstring = "DELETE From Members Where MemberId = @MemberId";
            SqlCommand sqlCommand = new SqlCommand( sqlstring, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MemberId", memberId);

            try
            {
                sqlConnection.Open();
                int i = sqlCommand.ExecuteNonQuery();
                if(i > 0)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "No member deleted!";
                    return i;
                }
                return i;
            }
            catch(Exception e) 
            {
                errormsg = e.Message;
                return 0;
            }
            finally { 
                sqlConnection.Close(); 
            }
        }

        // sökmetod som kan söka fram en medlem
        public List<MemberDetails> SearchMembers(string keyword, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            List<MemberDetails> members = new List<MemberDetails>();

            errormsg = "";
            

            string sqlstring = @"SELECT * FROM Members WHERE FirstName LIKE @Search OR LastName LIKE @Search OR MemberId LIKE @Search OR Phone LIKE @Search";
            SqlCommand cmd = new SqlCommand(sqlstring, sqlConnection);
            cmd.Parameters.AddWithValue("@Search", "%" + keyword + "%");

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                // gå igenom alla medlemmar som matchar sökningen
                while (reader.Read())
                {
                    MemberDetails m = new MemberDetails();
                    m.MemberId = (int)reader["MemberId"];
                    m.FirstName = reader["FirstName"].ToString();
                    m.LastName = reader["LastName"].ToString();
                    m.Email = reader["Email"].ToString();
                    m.Phone = reader["Phone"].ToString();
                    m.Age = (int)reader["Age"];
                    m.Score = (int)reader["Score"];

                    members.Add(m);
                }
                reader.Close();
            }
            catch(Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
            return members;
        }

        // metod för att filtrera medlemmar
        public List<MemberDetails> FilterMembers(string firstName, string lastName, int? minAge, int? maxAge, int? minScore, int? maxScore, out string errormsg)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString =
                  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BiljardKlubb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            List<MemberDetails> filtMembers = new List<MemberDetails>();
            errormsg = "";
            string sqlstring = "SELECT * FROM Members WHERE 1=1";
            if (!string.IsNullOrEmpty(firstName)) sqlstring += " AND FirstName LIKE @FirstName";
            if (!string.IsNullOrEmpty(lastName)) sqlstring += " AND LastName LIKE @LastName";
            if (minAge.HasValue)sqlstring += " AND Age >= @MinAge";
            if (maxAge.HasValue)sqlstring += " AND Age <= @MaxAge";
            if (minScore.HasValue)sqlstring += " AND Score >= @MinScore";
            if (maxScore.HasValue) sqlstring += " AND Score <= @MaxScore";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);

            if (!string.IsNullOrEmpty(firstName)) sqlCommand.Parameters.AddWithValue("@FirstName", "%" + firstName + "%");
            if (!string.IsNullOrEmpty(lastName)) sqlCommand.Parameters.AddWithValue("@LastName", "%" + lastName + "%");
            if (minAge.HasValue) sqlCommand.Parameters.AddWithValue("@MinAge", minAge.Value);
            if (maxAge.HasValue) sqlCommand.Parameters.AddWithValue("@MaxAge", maxAge.Value);
            if (minScore.HasValue) sqlCommand.Parameters.AddWithValue("@MinScore", minScore.Value);
            if (maxScore.HasValue) sqlCommand.Parameters.AddWithValue("@MaxScore", maxScore.Value);

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    MemberDetails m = new MemberDetails();
                    m.MemberId = (int)reader["MemberId"];
                    m.FirstName = reader["FirstName"].ToString();
                    m.LastName= reader["LastName"].ToString();
                    m.Email = reader["Email"].ToString();
                    m.Phone = reader["Phone"].ToString();
                    m.Age= (int)reader["Age"];
                    m.Score = (int)reader["Score"];
                    filtMembers.Add(m);
                }
                reader.Close();
            }
            catch(Exception ex) 
            {
                errormsg = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
            }
            return filtMembers;
        }
    }
}
