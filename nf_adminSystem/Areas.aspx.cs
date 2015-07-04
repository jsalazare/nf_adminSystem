﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nf_adminSystem
{
    public partial class Inicio : System.Web.UI.Page
    {
        DataTable ins;
        PgConnector pg = PgConnector.getInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                clear();
            }
        }

        public void clear()
        {
            ins = pg.consultar("SELECT * FROM institution");
            DataTable insClon = ins.Copy();
            GridView1.DataSource = insClon;
            GridView1.DataBind();

            ViewState["tipoTabla"] = "institution";

            DropDownList1.DataSource = ins;
            DropDownList1.DataTextField = "name";
            DropDownList1.DataValueField = "iID";
            DropDownList1.DataBind();
            ListItem l = new ListItem();
            l.Text = "--Todas Las Instituciones--";
            l.Value = "0";
            DropDownList1.Items.Insert(0, l);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Add("Seleccione Institucion");
            DropDownList3.Items.Clear();
            DropDownList3.Items.Add("Seleccione Area");
        }


        public void cambio(DropDownList dwlSelect, string query)
        {
            string id = dwlSelect.SelectedValue;

            //Response.Write("<script>alert('"+query+"');</script>");
            DataTable dt = pg.consultar(query + id);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }


        public void cambio(DropDownList dwlSelect, DropDownList dwlFill, string query,string msg)
        {
            string id = dwlSelect.SelectedValue;

            //Response.Write("<script>alert('"+query+"');</script>");
            DataTable dt = pg.consultar(query + id);
            GridView1.DataSource = dt;
            GridView1.DataBind();


            dwlFill.DataSource = dt;
            dwlFill.DataTextField = "name";
            dwlFill.DataValueField = "iID";
            dwlFill.DataBind();

            ListItem l = new ListItem();
            l.Text = "--Todas Las " + msg + "--";
            l.Value = "0";
            dwlFill.Items.Insert(0, l);

        }

        


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue != "0")
            {
                cambio(
                   DropDownList1,
                   DropDownList2,
                   "select a.\"iID\",a.name,a.description from " +
                   "institution i, area a WHERE a.institution_id = i.\"iID\" AND i.\"iID\" = ",
                   "Areas"
                   );
                ViewState["tipoTabla"] = "area";
                DropDownList3.Items.Clear();
                DropDownList3.Items.Add("Seleccione Area");
            }
            else
            {
                clear();
            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue != "0")
            {
                cambio(
                   DropDownList2,
                   DropDownList3,
                   "SELECT s.\"iID\",s.name,s.description FROM institution i, area a, subarea s " +
                    "where a.institution_id = i.\"iID\" AND a.\"iID\" = s.area_id AND a.\"iID\" = ",
                     "SubAreas"
                   );
                ViewState["tipoTabla"] = "subarea";
            }
            else
            {
                cambio(
                   DropDownList1,
                   DropDownList2,
                   "select a.\"iID\",a.name,a.description from " +
                   "institution i, area a WHERE a.institution_id = i.\"iID\" AND i.\"iID\" = ",
                   "Areas"
                   );
                ViewState["tipoTabla"] = "area";

            }

            
        }

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList3.SelectedValue != "0")
            {
                cambio(DropDownList3,
                   "SELECT n.\"iID\",n.title,n.description,n.image,n.date " +
                   "FROM subarea s,notification n where s.\"iID\" = n.subarea_id AND s.\"iID\" = ");
                ViewState["tipoTabla"] = "notification";

            }
            else
            {
                cambio(
                   DropDownList2,
                   DropDownList3,
                   "SELECT s.\"iID\",s.name,s.description FROM institution i, area a, subarea s " +
                    "where a.institution_id = i.\"iID\" AND a.\"iID\" = s.area_id AND a.\"iID\" = ",
                     "SubAreas"
                   );
                ViewState["tipoTabla"] = "subarea";

            }
            
        }


        protected void newBtn_Click(object sender, EventArgs e)
        {

        }

        protected void editBtn_Click(object sender, EventArgs e)
        {

        }

        protected void eraseBtn_Click(object sender, EventArgs e)
        {

            string selectedID = GridView1.SelectedRow.Cells[0].Text;
            string tipoTabla = ViewState["tipoTabla"].ToString();
            string query = "DELETE FROM " + tipoTabla + " WHERE \"iID\"= " + selectedID;
            if (pg.modificar(query))
            {
                Response.Write("<script>alert('Restros Eliminados + " + query + "');</script>");
                switch (tipoTabla)
                {
                    case "notification":

                        cambio(DropDownList3,
                          "SELECT n.\"iID\",n.title,n.description,n.image,n.date " +
                          "FROM subarea s,notification n where s.\"iID\" = n.subarea_id AND s.\"iID\" = ");

                        break;

                    case "subarea":
                        cambio(
                           DropDownList2,
                           DropDownList3,
                           "SELECT s.\"iID\",s.name,s.description FROM institution i, area a, subarea s " +
                            "where a.institution_id = i.\"iID\" AND a.\"iID\" = s.area_id AND a.\"iID\" = ",
                             "SubAreas"
                           );

                        break;

                    case "area":
                        cambio(
                           DropDownList1,
                           DropDownList2,
                           "select a.\"iID\",a.name,a.description from " +
                           "institution i, area a WHERE a.institution_id = i.\"iID\" AND i.\"iID\" = ",
                           "Areas"
                           );
                        DropDownList3.Items.Clear();
                        DropDownList3.Items.Add("Seleccione Area");
                        break;

                    case "institution":
                        clear();
                        break;
                }
            }
            else
            {
                Response.Write("<script>alert('Restros No Eliminados');</script>");
            }

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click Para Seleccionar";
            }

            e.Row.Cells[0].Visible = false;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = GridView1.SelectedIndex;

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowIndex == GridView1.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click Para Seleccionar";
                }
            }
        }

     
        
    }
}