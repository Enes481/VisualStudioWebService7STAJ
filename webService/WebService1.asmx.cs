using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;





namespace webService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

   



    public class WebService1 :System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string /*List<Class1>*/ getPersonel()
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();

            //Class1 sınıfından bir nesne oluşturalım.
            Class1 product = new Class1();
            //Gelen veriler birden fazla olacağı için liste oluşturalım
            List<Class1> productlist = new List<Class1>();
            //web1.config içerisinden veritabanı bağlantı stringimizi çekiyoruz.
            String cs = System.Configuration.ConfigurationManager.ConnectionStrings["Productdb"].ConnectionString;
            //sql bağlantı nesnesi oluşturalım.
            SqlConnection con = new SqlConnection(cs);
            //sql sorgumuzu oluşturalım
            SqlCommand cmd = new SqlCommand("SELECT * FROM Calisanlar", con);
            //bağlantıyı başlatalım
            con.Open();
            //Bağlantımız içerisindeki verileri okumaya başlayalım
            SqlDataReader dr = cmd.ExecuteReader();
            //okunan verileri tek tek dolaşalım
            while (dr.Read())
            {
                //okunan değerleri product nesnesine atayalım
                product.Ad = dr["Ad"].ToString();
                product.Soyad = dr["Soyad"].ToString();
                product.personelNo = dr["personelNo"].ToString();

                //okuduğumuz her bir veriyi listemiz içerisine atayalım
                productlist.Add(product);
                product = new Class1();
            }

            con.Close();
            dr.Close();


            /* var JsonData = new
             {
                 productlist = productlist
             };*/
            //Listemizi return edelim
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            var jsonPerson = JsonConvert.SerializeObject(productlist);
            return jsonPerson; //productlist; //  HttpContext.Current.Response.Write(ser.Serialize(JsonData));   //

        }

        [WebMethod]

        public Class1 addProduct(String Name,String LastName,String ID)
        {
            //product isminde Class1 den bir nesne oluşturalım
            Class1 product = new Class1();
            //web1.config içerisinden veritabanı bağlantı stringimizi çekiyoruz.
            String cs = System.Configuration.ConfigurationManager.ConnectionStrings["Productdb"].ConnectionString;
            using(SqlConnection con = new SqlConnection(cs))
            {
                
                //sql sorgumuzu oluşturalım
                using (SqlCommand cmd = new SqlCommand("INSERT INTO calisanlar (Ad,Soyad,PersonelNo) VALUES(@Name,@LastName,@ID)"))
                {
                    
                    //parametrelerimizi ekleyelim
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    
                    cmd.Connection = con;
                    //Bağlantıyı açalım
                    con.Open();
                    //Açılan bağlantıdan verileri veritabanına ekleyelim
                    cmd.ExecuteNonQuery();
                    con.Close();

                }

            }
            
            //eklenene productı return edelim
            return product;
        }
        
       
        
    }

   
    
}
