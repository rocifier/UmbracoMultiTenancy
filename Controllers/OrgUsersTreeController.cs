using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using System.Net.Http.Formatting;
using MultiTenantedUsers.Implementation;

namespace MultiTenantedUsers.Controllers
{
    [PluginController("MultiTenantedUsers")]
    [Tree("orgusers", "orgusersTree", "Users")]
    public class OrgUsersTreeController : TreeController
    {
        private const string ROOT_NODE_ID = "-1";
        private const string ROOT_USERS_ID = "root-users";
        public IMultiTenancyRepository MultiTenancyRepository { get; set; }

        public OrgUsersTreeController()
        {
            MultiTenancyRepository = new MultiTenancyRepository();
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            int? orgNodeId = MultiTenancyRepository.GetOrgIdForUser(Security.GetUserId());
            if (orgNodeId == null)
            {
                throw new Exception("Current user does not have permissions to manage this organisation's users.");
            }

            var nodes = new TreeNodeCollection();
            if (id == ROOT_NODE_ID)
            {
                var usersRoot = CreateTreeNode(ROOT_USERS_ID, id, queryStrings, "Backoffice Users", "icon-user", true, "orgusers");
                nodes.Add(usersRoot);
            }
            else if (id == ROOT_USERS_ID)
            {
                var orgUsers = MultiTenancyRepository.GetOrganisationUsers(orgNodeId.Value).ToList();
                orgUsers.ForEach(user => 
                    nodes.Add(CreateTreeNode("u" + user.Id.ToString(), ROOT_USERS_ID, queryStrings, user.Name, "icon-user", false))
                );
            }

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            if (id == ROOT_NODE_ID)
            {

            }
            else if (id == ROOT_USERS_ID)
            {
                menu.Items.Add<ActionNew>("Add New Backoffice User");
                menu.DefaultMenuAlias = ActionNew.Instance.Alias;
            }
            else
            {
                menu.Items.Add<ActionDisable>("Disable");
                menu.DefaultMenuAlias = ActionDisable.Instance.Alias;

            }
            return menu;
        }
        
    }
}