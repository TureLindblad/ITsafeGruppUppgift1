using Microsoft.Data.Sqlite;
using System.Data;

namespace Lektion2Grupp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DbHandler.InitDB();
            UpdateGridView();
        }

        private void UpdateGridView()
        {
            string query = "SELECT * FROM EncryptedStrings;";
            dataGridView1.DataSource = DbHandler.GetTable(query);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            Tuple<string, int> cryptAndSalt = Crypto.Encrypt(s);

            /*textBox3.Text = cryptAndSalt.Item1;*/
            string saveToDBquery = $@"
                    INSERT INTO EncryptedStrings (Encryption, Salt) VALUES
                    ('{cryptAndSalt.Item1}', {cryptAndSalt.Item2});";
            DbHandler.ExecuteQuery(saveToDBquery);
            UpdateGridView();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow data = dataGridView1.Rows[e.RowIndex];

                var cellValue = data.Cells[0].Value.ToString();

                /*textBox3.Text = cellValue;*/
                string id = cellValue;

                string getByID = $@"
                    SELECT * FROM EncryptedStrings
                    WHERE Id={id};";
                DataTable dataTable = DbHandler.GetTable(getByID);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];

                    string encryption = row["Encryption"].ToString();
                    int saltLength = Int32.Parse(row["Salt"].ToString());

                    string decrypt = Crypto.Decrypt(encryption, saltLength);

                    textBox2.Text = decrypt;
                    UpdateGridView();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
