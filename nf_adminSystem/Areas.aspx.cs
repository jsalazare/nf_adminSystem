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
        public string bodyText = "";
        public string headerText = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HiddenField1.Value = "123";
            if (!IsPostBack)
            {
                clear();
                int userlevel = Convert.ToInt32(Session["userlevel"]);
                string institution_id = Convert.ToString(Session["institution_id"]);
                for (int i = 1; i <= userlevel; i++)
                {
                    switch (userlevel)
                    {
                        case 1:

                            DropDownList1.SelectedValue = institution_id;
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
                            DropDownList1.Enabled = false;
                            break;

                        case 2:
                            DropDownList1.SelectedValue = institution_id;
                            DropDownList1.Enabled = false;
                            string usernfid = Convert.ToString(Session["iID"]);
                            DataTable res = pg.consultar("SELECT area_id FROM user_area WHERE usernf_id = " + usernfid);
                            Label2.Text = Convert.ToString(res.Rows[0][0]);
                            DropDownList2.SelectedValue = "" + 6;
                            DropDownList2.Enabled = false;

                            break;

                        case 3:
                            break;
                    }
                }
               

            }
           
        }

        public void clear()
        {
            GridView1.SelectedIndex = -1;
            ins = pg.consultar("SELECT * FROM institution");
            DataTable insClon = ins.Copy();
            GridView1.DataSource = ins;
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

            if (GridView1.Rows.Count == 0)
            {
                Label2.Text = "No Existen Registros.";
                Label2.Font.Size = FontUnit.Larger;
            }
            else
            {
                Label2.Text = "";
                Label2.Font.Size = FontUnit.Medium;
            }
        }


        public void cambio(DropDownList dwlSelect, string query)
        {

            GridView1.SelectedIndex = -1;
            string id = dwlSelect.SelectedValue;

            //Response.Write("<script>alert('"+query+"');</script>");
            DataTable dt = pg.consultar(query + id);
            GridView1.DataSource = dt;
            GridView1.DataBind();

            if (GridView1.Rows.Count == 0)
            {
                Label2.Text = "No Existen Registros.";
                Label2.Font.Size = FontUnit.Larger;
            }
            else
            {
                Label2.Text = "";
                Label2.Font.Size = FontUnit.Medium;
            }
        }


        public void cambio(DropDownList dwlSelect, DropDownList dwlFill, string query,string msg)

        {
            GridView1.SelectedIndex = -1;
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

            if (GridView1.Rows.Count == 0)
            {
                Label2.Text = "No Existen Registros.";
                Label2.Font.Size = FontUnit.Larger;
            }
            else
            {
                Label2.Text = "";
                Label2.Font.Size = FontUnit.Medium;
            }

        }

        public void update() {
            string tipoTabla = ViewState["tipoTabla"].ToString();
            switch (tipoTabla)
            {
                case "notification":

                    cambio(DropDownList3,
                      "SELECT n.\"iID\",n.title,n.description,n.image,n.date,n.url " +
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

        public void msgPopUp(string header, string body)
        {
            bodyText = body;
            headerText = header;
            ModalPopupExtender2.Show();

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
                   "SELECT s.\"iID\",s.name,s.description FROM area a, subarea s " +
                    "where a.\"iID\" = s.area_id AND a.\"iID\" = ",
                     "SubAreas"
                   );
                ViewState["tipoTabla"] = "subarea";
                Label2.Text = DropDownList2.SelectedValue;
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
                   "SELECT n.\"iID\",n.title,n.description,n.image,n.date,n.url " +
                   "FROM subarea s,notification n where s.\"iID\" = n.subarea_id AND s.\"iID\" = ");
                ViewState["tipoTabla"] = "notification";

            }
            else
            {
                cambio(
                   DropDownList2,
                   DropDownList3,
                   "SELECT s.\"iID\",s.name,s.description FROM area a, subarea s " +
                    "where  a.\"iID\" = s.area_id AND a.\"iID\" = ",
                     "SubAreas"
                   );
                ViewState["tipoTabla"] = "subarea";

            }
            
        }


        protected void newBtn_Click(object sender, EventArgs e)
        {
           
            string  tabla = Convert.ToString(ViewState["tipoTabla"]);
            switch (tabla)
            {
                case "institution":
                    submitEditarInstitution.Text = "Crear";
                    nameIns.Text = "";
                    descriptionIns.Text = "";
                    imageIns.Text = "";
                    mpeInstitution.Show();
                    break;

                case "area":
                    submitEditarAreaYSubArea.Text = "Crear";
                    lblHeadrEditarCrear.Text = "Area";
                    nameAreaYsub.Text = "";
                    desAreaYsub.Text = "";
                    mpuAreaYSubArea.Show();
                    break;

                case "subarea":
                    submitEditarAreaYSubArea.Text = "Crear";
                    lblHeadrEditarCrear.Text = "SubArea";
                    nameAreaYsub.Text = "";
                    desAreaYsub.Text = "";
                    mpuAreaYSubArea.Show();
                    break;

                case "notification":
                    submitEditarNotification.Text = "Crear";
                    titleNoti.Text = "";
                    desNotifi.Text = "";
                    imageNoti.Text = "";
                    urlNoti.Text = "";
                    mpeNotification.Show();
                    break;
            }
            ViewState["editarCrear"] = "crear";
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            string tabla = "";
            DataTable dt;
            string selectedId;

            if (GridView1.SelectedIndex == -1)
	        {
		        msgPopUp("Error","Seleccione un elemento.");
                ModalPopupExtender2.Show();
	        }
            else
	        {
                ViewState["editarCrear"] = "editar";
                selectedId = GridView1.SelectedRow.Cells[0].Text;
                tabla = Convert.ToString(ViewState["tipoTabla"]);
                switch (tabla)
                {
                    case "institution":
                        dt = pg.consultar("SELECT * FROM " + tabla + " WHERE \"iID\" = " + selectedId);
                        nameIns.Text = Convert.ToString(dt.Rows[0][1]);
                        descriptionIns.Text = Convert.ToString(dt.Rows[0][2]);
                        imageIns.Text = Convert.ToString(dt.Rows[0][3]);
                        submitEditarInstitution.Text = "Editar";
                        mpeInstitution.Show();
                        break;

                    case "area":
                        dt = pg.consultar("SELECT * FROM " + tabla + " WHERE \"iID\" = " + selectedId);
                        nameAreaYsub.Text = Convert.ToString(dt.Rows[0][1]);
                        desAreaYsub.Text = Convert.ToString(dt.Rows[0][2]);
                        lblHeadrEditarCrear.Text = "Area";
                        submitEditarAreaYSubArea.Text = "Editar";
                        mpuAreaYSubArea.Show();
                        break;

                    case "subarea":
                        dt = pg.consultar("SELECT * FROM " + tabla + " WHERE \"iID\" = " + selectedId);
                        nameAreaYsub.Text = Convert.ToString(dt.Rows[0][1]);
                        desAreaYsub.Text = Convert.ToString(dt.Rows[0][2]);
                        lblHeadrEditarCrear.Text = "SubArea";
                        submitEditarAreaYSubArea.Text = "Editar";
                        mpuAreaYSubArea.Show();
                        break;

                    case "notification":
                        dt = pg.consultar("SELECT * FROM " + tabla + " WHERE \"iID\" = " + selectedId);
                        titleNoti.Text = Convert.ToString(dt.Rows[0][1]);
                        desNotifi.Text = Convert.ToString(dt.Rows[0][2]);
                        imageNoti.Text = Convert.ToString(dt.Rows[0][5]);
                        urlNoti.Text = Convert.ToString(dt.Rows[0][6]);
                        submitEditarNotification.Text = "Editar";
                        mpeNotification.Show();
                        break;
                }
	        }
            
            //Label2.Text = GridView1.SelectedRow.Cells[0].Text;
            
        }

        protected void submitEditarInstitution_Click(object sender, EventArgs e)
        {
            string tabla = Convert.ToString(ViewState["tipoTabla"]);
            string editarOCrear = Convert.ToString(ViewState["editarCrear"]);
            string campo1;
            string campo2;
            string campo3;
            string campo4;
            string campo5;

            switch (editarOCrear)
            {
                case "editar":
                        string selectedId =  GridView1.SelectedRow.Cells[0].Text;
                      
                        switch (tabla)
                        {
                            case "institution":
                                campo1 = nameIns.Text;
                                campo2 = descriptionIns.Text;
                                campo3 = imageIns.Text;
                                pg.modificar("UPDATE " + tabla + " SET name = '" + campo1 + "' ,description = '" + campo2 + "',image = '" + campo3 + "' WHERE \"iID\" = " + selectedId);
                                update();
                                mpeInstitution.Hide();
                                break;

                            case "area":
                                campo1 = nameAreaYsub.Text;
                                campo2 = desAreaYsub.Text;
                                pg.modificar("UPDATE " + tabla + " SET name = '" + campo1 + "' ,description = '" + campo2 + "' WHERE \"iID\" = " + selectedId);
                                update();
                                mpuAreaYSubArea.Hide();
                                break;

                            case "subarea":
                                campo1 = nameAreaYsub.Text;
                                campo2 = desAreaYsub.Text;
                                pg.modificar("UPDATE " + tabla + " SET name = '" + campo1 + "' ,description = '" + campo2 + "' WHERE \"iID\" = " + selectedId);
                                update();
                                mpuAreaYSubArea.Hide();
                                break;

                            case "notification":
                                campo1 = titleNoti.Text;
                                campo2 = desNotifi.Text;
                                DateTime dt = DateTime.Now;
                                campo4 = imageNoti.Text;
                                campo5 = urlNoti.Text;
                                pg.modificar("UPDATE " + tabla + " SET title = '" + campo1 + "' ," +
                                                "description = '" + campo2 + "' ,date = '" + dt.ToString("yyyy-MM-dd") + "' ," +
                                                "image = '" + campo4 + "',url = '" + campo5 + "' WHERE \"iID\" = " + selectedId);
                                update();
                                mpeNotification.Hide();
                                break;
                        }
                    break;

                case "crear":
                    switch (tabla)
                    {
                        case "institution":
                            
                                campo1 = nameIns.Text;
                                campo2 = descriptionIns.Text;
                                campo3 = imageIns.Text;
                                pg.modificar("INSERT INTO " + tabla + " (name, description, image) VALUES ( '" + campo1 + "', '" + campo2 + "', '" + campo3 + "');");
                                update();
                                mpeInstitution.Hide();
                            
                            
                            break;

                        case "area":
                            campo1 = nameAreaYsub.Text;
                            campo2 = desAreaYsub.Text;
                            pg.modificar("INSERT INTO " + tabla + " (name, description, institution_id) VALUES ( '" + campo1 + "', '" + campo2 + "', '" + DropDownList1.SelectedValue + "');");
                            update();
                            mpuAreaYSubArea.Hide();
                            break;

                        case "subarea":
                            campo1 = nameAreaYsub.Text;
                            campo2 = desAreaYsub.Text;
                            pg.modificar("INSERT INTO " + tabla + " (name, description, area_id) VALUES ( '" + campo1 + "', '" + campo2 + "', '" + DropDownList2.SelectedValue + "');");
                            update();
                            mpuAreaYSubArea.Hide();
                            break;

                        case "notification":
                            campo1 = titleNoti.Text;
                            campo2 = desNotifi.Text;
                            DateTime dt = DateTime.Now;
                            campo4 = imageNoti.Text;
                            campo5 = urlNoti.Text;
                            pg.modificar("INSERT INTO " + tabla + " (title, description, date, subarea_id, image, url) VALUES "+
                                        "( '" + campo1 + "', '" + campo2 + "', '" + dt.ToString("yyyy-MM-dd") + "', '" + DropDownList3.SelectedValue + "', '" + campo4 + "', '" + campo5 + "');");
                            update();
                            mpeNotification.Hide();
                            break;
                    }

                break;
            }


            
           
            
        }

        protected void eraseBtn_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedIndex == -1)
            {
                msgPopUp("Error","Seleccione un elemento.");
                ModalPopupExtender2.Show();
            }
            else
            {
               //msgPopUp("Error", GridView1.SelectedIndex.ToString());
                Label1.Text = "";
                ModalPopupExtender1.Show();
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            
            string securityPass = securityPassword.Text;
            if (securityPass == "123")
            {
                
                string selectedID = GridView1.SelectedRow.Cells[0].Text;
                string tipoTabla = ViewState["tipoTabla"].ToString();
                string query = "DELETE FROM " + tipoTabla + " WHERE \"iID\"= " + selectedID;
                if (pg.modificar(query))
                {
                    //Response.Write("<script>alert('Restros Eliminados - " + query + "');</script>");
                    ModalPopupExtender1.Hide();
                    update();
                }
                else
                {
                    Response.Write("<script>alert('Restros No Eliminados');</script>");
                }
            }
            else
            {
                Label1.Text = "Contraseña Incorrecta";
                Label1.ForeColor = Color.Red;
            }
           
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
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

        protected void btnEditarInsCancelar_Click(object sender, EventArgs e)
        {
            mpeInstitution.Hide();
        }

        protected void btnEditarAreaSubCancelar_Click(object sender, EventArgs e)
        {
            mpuAreaYSubArea.Hide();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void btnEditarNotificationCancelar_Click(object sender, EventArgs e)
        {
            mpeNotification.Hide();
        }

        

        

     
        
    }
}