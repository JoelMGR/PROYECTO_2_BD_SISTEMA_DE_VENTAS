using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_II_SistemaVentas
{
    class DB_CRUD
    {
        // Usando el string de conexion podemos abrir cualquier base de datos de sql server.
        public static SqlConnection openConnection()
        {
            try
            {
                string DataSource = "Data Source=DESKTOP-U1C5ALN;Initial Catalog=Banco;Integrated Security=True";
                SqlConnection connection = new SqlConnection(DataSource);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /*********************************************

        CREATE

        *********************************************/

        // Falta comentar *************************************************************************************
        public void addRowData(string table, List<string> data)
        {

        }

        /*********************************************

        READ

        *********************************************/

        // Esta funcion se encarga de obtener todas las tablas de la base de datos.
        // Retorna una lista de strings
        public List<string> getTables()
        {
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM Information_Schema.Tables", connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            List<string> tableNames = new List<string>();
            //Este ciclo es donde se consiguen los nombres de las tablas.
            while (reader.Read())
            {
                string tmpName = reader.GetString(2);
                tableNames.Add(tmpName);
            }
            connection.Close();
            return tableNames;
        }

        // Saca los datos de una tabla.
        // Se le debe proporcionar el nombre de la tabla y el nombre de la columna a elegir.
        public List<string> getTableData(string columnName, string table)
        {
            List<string> data = new List<string>();
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("SELECT " + columnName + " FROM " + table, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmpData = reader.GetString(0);
                data.Add(tmpData);
                reader.GetString(0);
            }
            connection.Close();
            return data;
        }

        // Esta funcion se encarga de obtener el nombre de las columnas de una tabla.
        // Ocupa el nombre de la tabla que se desea procesar.
        public List<string> getColumns(string tableName)
        {
            List<string> columns = new List<string>();
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
            SqlDataReader reader = command.ExecuteReader();
            int cont = 0;
            while (reader.VisibleFieldCount > cont)
            {
                string tmpName = reader.GetName(cont);
                columns.Add(tmpName);
                cont++;
            }
            connection.Close();
            return columns;
        }

        // ADAPTAR FUNCIÓN A LA PROGRA, DEBE SER MÁS GENÉRICA *********************************************
        /*
        public List<Relationship> GetForeignKeys()
        {
            List<Relationship> list = new List<Relationship>();
            SqlConnection connection = openConnection();
            string select = "SELECT * FROM Information_Schema.KEY_COLUMN_USAGE";
            SqlCommand command = new SqlCommand(select, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(2);
                if (name.Contains("FK_"))
                {
                    List<string> tables = this.getTables();
                    int pos = 0;
                    Relationship key = new Relationship();
                    while (tables.Count != pos)
                    {
                        if (name.EndsWith(tables.ElementAt(pos)))
                        {
                            key.SetPrimary(tables.ElementAt(pos));
                        }
                        else if (name.StartsWith("FK_" + tables.ElementAt(pos)))
                        {
                            key.SetForeign(tables.ElementAt(pos));
                        }
                        pos++;
                    }
                    List<string> Primary_Table = this.getColumns(key.GetPrimary());
                    List<string> Foreign_Table = this.getColumns(key.GetForeign());
                    foreach (string primary in Primary_Table)
                    {
                        int cont = 0;
                        while (Foreign_Table.Count != cont)
                        {
                            if (Foreign_Table.ElementAt(cont).Equals(primary))
                            {
                                key.SetColumn(primary);
                                break;
                            }
                            cont++;
                        }
                    }

                    list.Add(key);
                }
            }
            connection.Close();
            return list;
        }
        */
        // ADAPTAR FUNCIÓN A LA PROGRA, DEBE SER MÁS GENÉRICA *********************************************
        /*
        public string JoinAll()
        {
            List<Relationship> list = c.GetForeignKeys();
            string join = "";

            if (list.Count > 0)
            {
                Relationship key = list.ElementAt(0);
                List<string> visited = new List<string>();
                visited.Add(key.GetPrimary());
                visited.Add(key.GetForeign());

                join = join = key.GetPrimary() + " JOIN " + key.GetForeign() + " ON "
                + key.GetPrimary() + "." + key.GetColumn() + " = " + key.GetForeign() + "." +
                key.GetColumn();

                int times_left = list.Count - 1;
                Console.WriteLine("Total times left" + times_left);
                int pos = 0;

                while (times_left != 0)
                {

                    if (visited.Contains(list.ElementAt(pos).GetPrimary()) && !visited.Contains(list.ElementAt(pos).GetForeign()))
                    {
                        Console.WriteLine("ENTERED THE IF CONDITION");
                        key = list.ElementAt(pos);
                        times_left--;
                        visited.Add(list.ElementAt(pos).GetForeign());
                        join = join + " JOIN " + key.GetForeign() + " ON " + key.GetForeign() +
                        "." + key.GetColumn() + " = " + key.GetPrimary() + "." + key.GetColumn();
                    }
                    else if (!visited.Contains(list.ElementAt(pos).GetPrimary()) && visited.Contains(list.ElementAt(pos).GetForeign()))
                    {
                        Console.WriteLine("ENTERED THE ELSEIF CONDITION");
                        key = list.ElementAt(pos);
                        times_left--;
                        visited.Add(list.ElementAt(pos).GetPrimary());

                        join = join + " JOIN " + key.GetPrimary() + " ON " + key.GetForeign() +
                        "." + key.GetColumn() + " = " + key.GetPrimary() + "." + key.GetColumn();
                    }
                    if (pos == list.Count - 1)
                    {
                        pos = 0;
                    }
                    else
                    {
                        pos++;
                    }
                }
                return join;
            }
            return join;
        }
        */

        // ADAPTAR FUNCIÓN A LA PROGRA, DEBE SER MÁS GENÉRICA *********************************************
        /*
        public DataSet consult(Consult consult, List<Panel_filter> columns)
        {
            SqlConnection connection = Connection.openConnection();
            string select = "SELECT ";
            foreach (Panel_filter panel in columns)
            {
                select += panel.getColumnName();
                if (!columns.ElementAt(columns.Count - 1).getColumnName().Equals(panel.getColumnName()))
                {
                    select += ",";
                }
            }
            select += " FROM ";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(select + this.JoinAll() + " WHERE " + consult.getWhere(), connection);
            Console.WriteLine(select + this.JoinAll() + " WHERE " + consult.getWhere());

            Console.WriteLine("Where:" + consult.getWhere());

            DataSet data = new DataSet();
            consult.getConsult();
            dataAdapter.Fill(data, select + this.JoinAll() + " WHERE " + consult.getWhere());

            connection.Close();

            return data;
        }
        */

        /*********************************************

        UPDATE

        *********************************************/

        // Falta comentar *************************************************************************************
        public void updateRowData(string table, string key, List<string> data)
        {

        }

        /*********************************************

        DELETE

        *********************************************/

        // Falta comentar *************************************************************************************
        public void deleteRow(string table, string key)
        {

        }
    }
}
