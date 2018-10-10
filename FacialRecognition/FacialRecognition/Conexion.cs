using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace FacialRecognition
{
    public class Conexion
    {
        SqlConnection cn;
        SqlCommand cmd;
        SqlDataReader dr;

        // private static string cadenaConexion = @"Data Source=sharkwilliams\sqlexpress;Initial Catalog=iscavDB;User ID=sa;Password=mipassword";

        public Conexion()
        {
            try
            {
                cn = new SqlConnection(@"Data Source=localhost;Initial Catalog=iscavDB2;Trusted_Connection =True")/*User ID=sa;Password=mipassword*/;
                cn.Open();

                MessageBox.Show("conectado a ISCAV");
            }
            catch (Exception ex)
            {

                MessageBox.Show(" no conectado" + ex.ToString());
            }


        }

        public string Insertar(string nombre, string faceurl)
        {
            string salida = "se inserto";
            try
            {
                //cmd = new SqlCommand("insert into TBFaces(nombre,faceurl,userid) values('"+nombre+"',' " +faceurl+ "','" +userid+"') ",cn);
                //cmd.ExecuteNonQuery();
                //MessageBox.Show("Se inserto");

                cmd = new SqlCommand("insert into TBFACE(nombre,faceurl) values(@nombre,@faceurl)", cn);
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar);
                cmd.Parameters.Add("@faceurl", SqlDbType.VarChar);
                //cmd.Parameters.Add("@faceid",SqlDbType.Int);


                cmd.Parameters["@nombre"].Value = nombre;
                //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //faceurl.Image.Save(ms,System.Drawing.Imaging.ImageFormat.Bmp);

                cmd.Parameters["@faceurl"].Value = faceurl; //ms.GetBuffer();
                                                            // cmd.Parameters["@faceid"].Value = faceid;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se Agrego a usuario" + ' '+ nombre);

            }
            catch (Exception ex)
            {

                salida = "no se conecto" + ex.ToString();
                MessageBox.Show("Fallo" + ex);
                cn.Close();

            }
            return salida;
        }



        public int validaPersona(int id)
        {
            //string salida = "se inserto";

            int contador = 0;
            try
            {
                cmd = new SqlCommand("select * from TBFace where Id =" + id + "", cn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    contador++;
                }
                dr.Close();



            }
            catch (Exception ex)
            {

                MessageBox.Show("No existen registros de esta persona " + ex.ToString());
            }
            return contador;
        }


        public string InsertarHoradetect(string nombre, DateTime horadetectada)
        {
            string salida = "se inserto hora";
            try
            {
                //cmd = new SqlCommand("insert into TBFaces(nombre,faceurl,userid) values('"+nombre+"',' " +faceurl+ "','" +userid+"') ",cn);
                //cmd.ExecuteNonQuery();
                //MessageBox.Show("Se inserto");

                cmd = new SqlCommand("insert into REGISTRO(nombre,horadetectada) values(@nombre,@horadetectada", cn);
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar);
                cmd.Parameters.Add("@horadetectada", SqlDbType.DateTime);

                //cmd.Parameters.Add("@userid",SqlDbType.Int);



                //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //faceurl.Image.Save(ms,System.Drawing.Imaging.ImageFormat.Bmp);


                cmd.ExecuteNonQuery();
                MessageBox.Show("Se inserto tiempo");

            }
            catch (Exception ex)
            {

                salida = "no se conecto" + ex.ToString();
                MessageBox.Show("Fallo" + ex);
                cn.Close();

            }
            return salida;
        }

    }

}