using System.Data;
using System.Data.SqlTypes;
using System.Linq.Expressions;
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
            sqlCommand.Parameters.Add("FirstName", System.Data.SqlDbType.NVarChar, 30).Value = memberDetails.FirstName;
            sqlCommand.Parameters.Add("LastName", System.Data.SqlDbType.NVarChar, 40).Value = memberDetails.LastName;
            sqlCommand.Parameters.Add("Email", System.Data.SqlDbType.NVarChar, 60).Value = memberDetails.Email;
            sqlCommand.Parameters.Add("Phone", System.Data.SqlDbType.NVarChar, 20).Value = memberDetails.Phone;
            sqlCommand.Parameters.Add("Age", System.Data.SqlDbType.Int, 3).Value = memberDetails.Age;
            sqlCommand.Parameters.Add("Score", System.Data.SqlDbType.Int, 15).Value = memberDetails.Score;


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
            string sqlstring = "Select * From Members Where MemberId =@memberId";
            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            sqlCommand.Parameters.Add("MemberId", SqlDbType.Int).Value = memberId;
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

    }
}
