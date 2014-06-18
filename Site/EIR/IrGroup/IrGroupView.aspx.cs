﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Site.EIR.IrGroup
{
    public partial class IrGroupView : Core.SitePage
    {
        private int irGroupId;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            irGroupId = Convert.ToInt32(Session["irGroupId"]);

            var urlGroup = Campus.SDK.Client.BuildUrl("IrGroup", "GetIrGroupData", new { SessionId, irGroupId });
            var resultGroup = CampusClient.Get(urlGroup);
            var innerGroup = JsonConvert.DeserializeObject(resultGroup.Data.ToString());
            group_name.InnerText = innerGroup["Name"];

            var content = GetGroupContentByID(irGroupId);
            foreach (var ir in content)
            {
                LinkButtonsRendering(ir);
            }

        }

        private List<JObject> GetGroupContentByID(int irGroupId)
        {
            var client = new Campus.SDK.Client();
            var url = Campus.SDK.Client.BuildUrl("IrGroup", "GetIrGroupContent", new { SessionId, irGroupId });
            var result = client.Get(url);

            var inner = JsonConvert.DeserializeObject(result.Data.ToString());
            if (inner != null)
            {
                var items = (inner as IEnumerable<Object>);
                return items.Cast<JObject>().ToList();
            }
            return new List<JObject>();
        }

        private void LinkButtonsRendering(JObject group)
        {
            var groupLink = new LinkButton();
            var mainDiv = new HtmlGenericControl("div");
            var nameShort = new HtmlGenericControl("h5");
            var nameFull = new HtmlGenericControl("h6");
            var description = new HtmlGenericControl("p");


            groupLink.PostBackUrl = Request.Url.AbsolutePath;
            groupLink.Attributes.Add("class", "irLink");
            groupLink.Attributes.Add("IrId", group["IrId"].ToString());

            mainDiv.Attributes.Add("id", "irGroupMainBlock");
            mainDiv.Attributes.Add("class", ".form-inline");

            nameShort.Attributes.Add("id", "irGroupName");
            nameShort.Attributes.Add("class", "text-primary");


            description.Attributes.Add("class", "irGroupDescription");
            nameFull.Attributes.Add("class", "irGroupDescription");


            nameShort.InnerText = group["NameShort"].ToString();
            nameFull.InnerText = group["NameFull"].ToString();
            description.InnerText = group["Description"].ToString();

            mainDiv.Controls.Add(nameShort);
            mainDiv.Controls.Add(nameFull);
            mainDiv.Controls.Add(description);
            groupLink.Controls.Add(mainDiv);

            LinkContainer.Controls.Add(groupLink);
        }

        protected void EditGroup_Click(object sender, EventArgs e)
        {
            //Session.Add("irGroupId", irGroupId);
            Response.Redirect("NewIrGroup.aspx?type=edit");
        }

        protected void AddIr_Click(object sender, EventArgs e)
        {
            //Session.Add("irGroupId", irGroupId);
            Response.Redirect("/EIR/Search.aspx?type=group");
        }

    }

}