using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CoreWebApp.Controllers
{
    public class ItemController : Controller
    {
        private string v1;
        //string v2 = _iConfig.GetValue<string>("DBConnection:ConnectionString");

        public ItemController(IConfiguration iConfig)
        {
            v1 = iConfig.GetSection("DBConnection").GetSection("ConnectionString").Value;
        }

        [HttpGet]
        public List<Item> Get()
        {
            var db = new SqlConnection(v1);

            var sql = "select * from Item";
            List<Item> items = db.Query<Item>(sql).ToList();

            db.Close();
            return items;
        }

        [HttpPost]
        public List<Item> Save(Item item)
        {
            var db = new SqlConnection(v1);

            ////var sql = "select * from Item";
            ////List<Item> items = db.Query<Item>("Crud_Item", CommandType.StoredProcedure).ToList();

            ////db.Close();
            ////return items;

            db.Open();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", item.Id);
            param.Add("@Name", item.Name);
            param.Add("@Price", item.Price);
            param.Add("@Oper", item.Oper);
            List<Item> items = db.Query<Item>("Crud_Item", param, null, false, null, CommandType.StoredProcedure).ToList();
            db.Close();
            return items;
        }
    }
}