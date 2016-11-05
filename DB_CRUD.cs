using Project_1_DataBase_1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;



namespace Proyecto_2.Properties
{
    public class DB_CRUD
    {
        // Usando el string de conexion podemos abrir cualquier base de datos de sql server.
        public static SqlConnection openConnection()
        {
            try
            {
                string DataSource = "Data Source=EQUIPO-JOSE;Initial Catalog=Data;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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

        // Recibe el nombre de la tabla y una lista de strings que son los datos a insertar dentro del SQL
        public void addRowData(string table, List<string> data)
        {
            //Verifica que la cantidad de datos introducidos sean las mismas que las que ocupa el SQL
            if (data.Count() == this.getColumns(table).Count)
            {
                //En caso de que ocurra una excepcion de tipo SQL es atrapado
                try
                {
                    //List<string> columns = this.getColumns(table);
                    SqlConnection connection = openConnection();
                    int pos = 0;
                    string values = " VALUES(";
                    while (data.Count != pos)
                    {
                        if (pos + 1 != data.Count)
                        {
                            values += data.ElementAt(pos) + ",";
                        }
                        else
                        {
                            values += data.ElementAt(pos) + ")";
                        }
                        pos++;
                    }
                    SqlCommand command = new SqlCommand("INSERT INTO " + table + values, connection);
                    Console.WriteLine("INSERT INTO " + table + values);
                    command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    Console.WriteLine("Ha ocurrido un error y no se pudo desarrollar la accion");
                }
            }
            //Aqui iria un messagebox 
            else
            {
                Console.WriteLine("Ha ocurrido un error y no se pudo desarrollar la accion");
            }
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

        public DataSet getSomeTableData(string columnName, string table, string condition)
        {
            List<string> data = new List<string>();
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM " + table + " WHERE "+ columnName + "="+ condition, connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            DataSet datatable = new DataSet();
            adapter.Fill(datatable);
            return datatable;
        }
        
        public List<string> getTableDataAsStrings(string columnName, string table, string condition,string operation)
        {
            List<string> data = new List<string>();
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM " + table + " WHERE " + columnName + operation + condition, connection);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int pos = 0;
            while (reader.FieldCount != data.Count)
            {
                data.Add((reader.GetValue(pos).ToString()));
                pos++;
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

        //Retorna todas las llaves foraneas de la base de datos
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


        //Retorna las llaves primarias de una tabla
        public List<string> getPrimaryKeys(string table)
        {
            List<string> primary = new List<string>();
            SqlConnection conexion = DB_CRUD.openConnection();
            string select = "SELECT * FROM Information_Schema.KEY_COLUMN_USAGE";
            SqlCommand comando = new SqlCommand(select, conexion);
            SqlDataReader leer = comando.ExecuteReader();
            while (leer.Read())
            {
                string nombre = leer.GetString(2);
                Console.WriteLine(nombre);
                if (nombre.Equals("PK_" + table))
                {
                    string dato = leer.GetString(6);
                    primary.Add(dato.ToString());
                }
            }
            return primary;
        }

        public string Join(string table1,string table2)
        {
            List<Relationship> foraneas = this.GetForeignKeys();
            int pos = 0;
            string join = "";
            while (pos != foraneas.Count)
            {
                if ((foraneas.ElementAt(pos).GetPrimary().Equals(table1) && foraneas.ElementAt(pos).GetForeign().Equals(table2)) || (foraneas.ElementAt(pos).GetPrimary().Equals(table2) && foraneas.ElementAt(pos).GetForeign().Equals(table1)))
                {
                    join = table1 + " JOIN " + table2;
                    join += " " + table1 + "." + foraneas.ElementAt(pos).GetColumn()+"=" + table2 + "." + foraneas.ElementAt(pos).GetColumn();
                    break;
                }
                pos++;
            }
            if (join.Equals(""))
            {
                return null;
            }
            else
            {
                return join;
            }
        }

        //Une todas las tabla de la base de datos
        public string JoinAll()
        {
            List<Relationship> list = this.GetForeignKeys();
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

        /*********************************************
            UPDATE
            *********************************************/

        // Recibe el nombre de la tabla(table), dos strings que forman la condicion del where(where y condition)
        // Una lista de las columnas que se van a modificar(columns) y una lista con los datos a actualizar(data)
        public void updateRowData(string table, string column, string condition, List<string> columns, List<string> data)
        {
            try
            {
                SqlConnection connection = openConnection();
                int pos = 0;
                string values = "";
                //Este while construye la seccion del sqlcommand del set
                while (data.Count != pos)
                {
                    if (pos + 1 != data.Count)
                    {
                        values += data.ElementAt(pos) + "=" + columns.ElementAt(pos) + ",";
                    }
                    else
                    {
                        values += data.ElementAt(pos) + "=" + columns.ElementAt(pos);
                    }
                    pos++;
                }
                SqlCommand command = new SqlCommand("UPDATE " + table + " SET " + values + " WHERE " + column + "=" + condition, connection);
                Console.WriteLine("UPDATE " + table + " SET " + values + " WHERE " + column + "=" + condition);
                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Console.WriteLine("Ha ocurrido un error y no se pudo desarrollar la accion");
            }
        }

    /*********************************************
    DELETE
    *********************************************/

    // Esta funcion borra una tupla de la base de datos
    // Recibe el nombre de la tabla y el condicional dividido en dos strings con where siendo la columna
    // y key el valor del dato
    public void deleteRow(string table, string where,string key)
        {
            SqlConnection connection = openConnection();
            SqlCommand command = new SqlCommand("DELETE FROM " + table +" WHERE " + where + "=" + key, connection);
            command.ExecuteNonQuery();
        }
    }
}
