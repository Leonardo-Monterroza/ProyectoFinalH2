﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Security.Policy;
using System.Net.Sockets;
using System.Drawing;
using System.IO;

namespace ProyectoFinalH2
{
    internal class SqlComands
    {
        public void funConexion(string server, string database)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                con.Close();
            }
        }

        public int funLoginTest(string server, string database, string tPerson, string entradaNombe, string entradaContra)
        {
            int Bandera = 0;
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string Query2 = $"select * from {tPerson} where Nombre = '{entradaNombe}'";
                SqlCommand Coman2 = new SqlCommand(Query2, con);
                SqlDataReader RC2 = Coman2.ExecuteReader();
                if (RC2.Read())
                {
                    RC2.Close();
                    string Query3 = $"select * from {tPerson} where Contraseña = '{entradaContra}' and Nombre = '{entradaNombe}'";
                    SqlCommand Coman3 = new SqlCommand(Query3, con);
                    SqlDataReader RC3 = Coman3.ExecuteReader();
                    if (RC3.Read())
                    {
                        RC3.Close();
                        Bandera = 1;
                    }
                    else {RC3.Close();}
                }
                else {RC2.Close();}
                con.Close();
                return Bandera;
            }
        }



        public void funSingUp(string server, string database, string tClientes, string entradaNombe, string entradaContra)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string Query2 = $"insert into {tClientes} values('{entradaNombe}', '{entradaContra}',0)";
                SqlCommand Coman4 = new SqlCommand(Query2,con);
                int Value = Coman4.ExecuteNonQuery();
                if (Value > 0) { MessageBox.Show("User successfully registered",$"Hello! {entradaNombe}."); }
                else { MessageBox.Show("Error while registering, try again.", "Ups!."); }
                con.Close();
            }
        }

        public void funTableCars(string server, string database, DataGridView table)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string QuerYCars = "select	* from tCars where Disponible = 1";
                SqlDataAdapter tCars = new SqlDataAdapter(QuerYCars,con);
                DataTable dt = new DataTable();
                tCars.Fill(dt);
                table.DataSource = dt;
                con.Close();
            }
        }

        private Image GetCarImage(string server, string database, string placa)
        {
            using (SqlConnection con = new SqlConnection($"server={server};database={database};integrated security=true;"))
            {
                con.Open();

                string query = "SELECT Imagen FROM tCars WHERE Placa = @Placa";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Placa", placa);
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])result;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            return Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void funTableCarsHistory(string server, string database, string Placa, DataGridView table)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string QuerYCars = $"select	* from tArriendoReservas where ID_Carro = '{Placa}'";
                SqlDataAdapter tCars = new SqlDataAdapter(QuerYCars, con);
                DataTable dt = new DataTable();
                tCars.Fill(dt);
                table.DataSource = dt;
                con.Close();
            }
        }

        public void funTableCarsHistory2(string server, string database, int ID, DataGridView table)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string QuerYCars = $"select	* from tArriendoReservas where ID_Cliente = {ID}";
                SqlDataAdapter tCars = new SqlDataAdapter(QuerYCars, con);
                DataTable dt = new DataTable();
                tCars.Fill(dt);
                table.DataSource = dt;
                con.Close();
            }
        }

        public int funReturIDClient(string server, string database, string tClientes, string entradaNombe, string entradaContra)
        {
            int IDClient;
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string Query = $"select ID from {tClientes} where Contraseña = '{entradaContra}' and Nombre = '{entradaNombe}' ";
                SqlCommand Coman4 = new SqlCommand(Query, con);
                object Result = Coman4.ExecuteScalar();
                IDClient = Convert.ToInt32(Result);
                con.Close();
            }
            return IDClient;
        }

        public void funTArriReser(string server, string database, int IDCliente, string IDCarro, string Modo, DateTime FechaIn, DateTime FechaFin)
        {
            using (SqlConnection con = new SqlConnection($"server={server}; database={database}; integrated security=true;"))
            {
                con.Open();
                string query = "INSERT INTO tArriendoReservas (ID_Cliente, ID_Carro, Modo, FechaIn, FehcaFn) VALUES (@IDCliente, @IDCarro, @Modo, @FechaIn, @FechaFin)";
                SqlCommand sqlCommand = new SqlCommand(query, con);

                sqlCommand.Parameters.AddWithValue("@IDCliente", IDCliente);
                sqlCommand.Parameters.AddWithValue("@IDCarro", IDCarro);
                sqlCommand.Parameters.AddWithValue("@Modo", Modo);
                sqlCommand.Parameters.AddWithValue("@FechaIn", FechaIn);
                sqlCommand.Parameters.AddWithValue("@FechaFin", FechaFin);

                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public void funDispoCars(string server, string database, int value, string placaCar)
        {
            using (SqlConnection con = new SqlConnection($"server={server}; database={database}; integrated security=true;"))
            {
                con.Open();
                string Query = $"update tCars set Disponible = {value} where Placa = '{placaCar}'";
                SqlCommand sqlCarsDispo = new SqlCommand(Query, con);
                sqlCarsDispo.ExecuteNonQuery();
                con.Close();
            }
        }

        public int funSancion(string server, string database, int idClien)
        {
            using (SqlConnection con = new SqlConnection($"server={server}; database={database}; integrated security=true;"))
            {
                con.Open();
                string Query = $"select Sancion from tClientes where ID = '{idClien}'";
                SqlCommand sqlSancion = new SqlCommand(Query, con);
                object result = sqlSancion.ExecuteScalar();
                int bitReturn = Convert.ToInt32(result);
                con.Close();
                return bitReturn;
            }
        }

        public void funPaySancion(string server, string database, int idClien)
        {
            using (SqlConnection con = new SqlConnection($"server={server}; database={database}; integrated security=true;"))
            {
                con.Open();
                string Query = $"update tClientes set Sancion = 0 where ID = '{idClien}'";
                SqlCommand sqlSancion = new SqlCommand(Query, con);
                sqlSancion.ExecuteNonQuery();
                con.Close();

            }
        }

        public void funTableCarsAll(string server, string database, DataGridView table)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string QuerYCars = "select	* from tCars";
                SqlDataAdapter tCars = new SqlDataAdapter(QuerYCars, con);
                DataTable dt = new DataTable();
                tCars.Fill(dt);
                table.DataSource = dt;
                con.Close();
            }
        }

        public void funTableClientes(string server, string database, DataGridView table)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string QuerYCars = "select	* from tClientes";
                SqlDataAdapter tCars = new SqlDataAdapter(QuerYCars, con);
                DataTable dt = new DataTable();
                tCars.Fill(dt);
                table.DataSource = dt;
                con.Close();
            }
        }

        public void funEditPenality(string server, string database, int penality, int IDClient)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string Query = $"update tClientes set Sancion = {penality} where ID = {IDClient}";
                SqlCommand sqlSancion = new SqlCommand(Query, con);
                sqlSancion.ExecuteNonQuery();
                con.Close();
            }
        }

        public void funDeleteCar(string server, string database, string Placa)
        {
            using (SqlConnection con = new SqlConnection($"server = {server}; database = {database}; integrated security = true;"))
            {
                con.Open();
                string Query = $"delete from tCars where Placa = '{Placa}'";
                SqlCommand sqlSancion = new SqlCommand(Query, con);
                sqlSancion.ExecuteNonQuery();
                con.Close();
            }
        }

        public bool funAddCar(string server, string database, string Placa, string Modelo, string Tipo, int Dispo, string Color, int NPuertas, int Asientos, int Precio, byte[] carImageBytes)
        {
            using (SqlConnection con = new SqlConnection($"server={server};database={database};integrated security=true;"))
            {
                con.Open();

                // Verificar si la placa ya existe
                string Qur = "SELECT * FROM tCars WHERE Placa = @Placa";
                SqlCommand sqlS = new SqlCommand(Qur, con);
                sqlS.Parameters.AddWithValue("@Placa", Placa);
                SqlDataReader RC2 = sqlS.ExecuteReader();

                if (RC2.Read())
                {
                    RC2.Close();
                    con.Close();
                    return false;
                }
                else
                {
                    RC2.Close();

                    // Comando SQL para insertar los datos en la tabla tCars
                    string Query = "INSERT INTO tCars (Placa, Modelo, Tipo, Disponible, Color, N_Puertas, Asientos, Precio_Dia, Imagen) " +
                                   "VALUES (@Placa, @Modelo, @Tipo, @Disponible, @Color, @NPuertas, @Asientos, @Precio, @Imagen)";

                    using (SqlCommand sqlSancion = new SqlCommand(Query, con))
                    {
                        // Agregar parámetros con valores
                        sqlSancion.Parameters.AddWithValue("@Placa", Placa);
                        sqlSancion.Parameters.AddWithValue("@Modelo", Modelo);
                        sqlSancion.Parameters.AddWithValue("@Tipo", Tipo);
                        sqlSancion.Parameters.AddWithValue("@Disponible", Dispo);
                        sqlSancion.Parameters.AddWithValue("@Color", Color);
                        sqlSancion.Parameters.AddWithValue("@NPuertas", NPuertas);
                        sqlSancion.Parameters.AddWithValue("@Asientos", Asientos);
                        sqlSancion.Parameters.AddWithValue("@Precio", Precio);
                        sqlSancion.Parameters.AddWithValue("@Imagen", carImageBytes);

                        int rowsAffected = sqlSancion.ExecuteNonQuery();
                        con.Close();

                        return rowsAffected > 0;
                    }
                }
            }
        }


        public void funEditCar(string server, string database, string Placa, string Modelo, string Tipo, int Dispo, string Color, int NPuertas, int Asientos, int Precio, byte[] carImageBytes)
        {
            using (SqlConnection con = new SqlConnection($"server={server};database={database};integrated security=true;"))
            {
                con.Open();

                string Qur;
                if (carImageBytes != null)
                {
                    Qur = "UPDATE tCars SET Modelo = @Modelo, Tipo = @Tipo, Disponible = @Disponible, Color = @Color, N_Puertas = @NPuertas, Asientos = @Asientos, Precio_Dia = @Precio, Imagen = @Imagen WHERE Placa = @Placa";
                }
                else
                {
                    Qur = "UPDATE tCars SET Modelo = @Modelo, Tipo = @Tipo, Disponible = @Disponible, Color = @Color, N_Puertas = @NPuertas, Asientos = @Asientos, Precio_Dia = @Precio WHERE Placa = @Placa";
                }

                using (SqlCommand EditCar = new SqlCommand(Qur, con))
                {
                    EditCar.Parameters.AddWithValue("@Placa", Placa);
                    EditCar.Parameters.AddWithValue("@Modelo", Modelo);
                    EditCar.Parameters.AddWithValue("@Tipo", Tipo);
                    EditCar.Parameters.AddWithValue("@Disponible", Dispo);
                    EditCar.Parameters.AddWithValue("@Color", Color);
                    EditCar.Parameters.AddWithValue("@NPuertas", NPuertas);
                    EditCar.Parameters.AddWithValue("@Asientos", Asientos);
                    EditCar.Parameters.AddWithValue("@Precio", Precio);

                    if (carImageBytes != null)
                    {
                        EditCar.Parameters.AddWithValue("@Imagen", carImageBytes);
                    }

                    EditCar.ExecuteNonQuery();
                }

                con.Close();
            }
        }



    }
}
