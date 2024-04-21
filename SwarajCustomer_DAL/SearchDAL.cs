using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace SwarajCustomer_DAL
{
    public class SearchDAL : ISearchDAL
    {
        Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public SearchDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<SearchRes> GetSearchResult(double latitude, double longitude, string text)
        {
            List<SearchRes> searchList = new List<SearchRes>();

            DbParam[] param = new DbParam[3];
            param[0] = new DbParam("@latitude", latitude.ToString(), SqlDbType.VarChar);
            param[1] = new DbParam("@longitude", longitude.ToString(), SqlDbType.VarChar);
            param[2] = new DbParam("@text", text, SqlDbType.VarChar);
            DataSet ds = Db.GetDataSet("usp_get_search_results", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        SearchRes search = new SearchRes();
                        search.ID = Db.ToInteger(row["main_product_id"]);
                        search.Name = Db.ToString(row["name"]);
                        search.Description = Db.ToString(row["description"]);

                        if (Db.ToInteger(row["category_id"]) == Convert.ToInt32(PujaCategory.PUJA))
                        {
                            search.Type = "PUJA";
                        }
                        if (Db.ToInteger(row["category_id"]) == Convert.ToInt32(PujaCategory.PATH))
                        {
                            search.Type = "PATH";
                        }
                        if (Db.ToInteger(row["category_id"]) == Convert.ToInt32(PujaCategory.CORP))
                        {
                            search.Type = "CORP";
                        }
                        searchList.Add(search);
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 1)
            {
                string img = string.Empty;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        SearchRes search = new SearchRes();
                        search.ID = Db.ToInteger(row["adm_user_id"]);
                        search.Name = Db.ToString(row["first_name"]) + " " + Db.ToString(row["last_name"]);
                        search.ImageName = Db.ToString(row["ImageName"]);
                        if (!string.IsNullOrEmpty(search.ImageName))
                            search.ImageName = string.Format(CommonMethods.CustomerRetPath + search.ImageName);
                        else
                            search.ImageName = CommonMethods.CustomerIcon;

                        if (Db.ToInteger(row["user_type_id"]) == Convert.ToInt32(Roles.AST))
                        {
                            search.Type = "AST";
                        }
                        if (Db.ToInteger(row["user_type_id"]) == Convert.ToInt32(Roles.PRHT))
                        {
                            search.Type = "PRHT";
                        }
                        searchList.Add(search);
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 2)
            {
                string img = string.Empty;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        SearchRes search = new SearchRes();
                        search.ID = Db.ToInteger(row["mst_services_id"]);
                        search.Name = Db.ToString(row["name"]);
                        search.Description = Db.ToString(row["description"]);
                        search.Type = "SRV";
                        searchList.Add(search);
                    }
                }
            }

            return searchList;
        }

    }
}
